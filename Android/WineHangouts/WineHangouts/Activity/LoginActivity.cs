using System;
using System.Linq;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using Hangout.Models;
using Android.Telephony;
using Android.Gms.Common;
using Android.Views;
using System.Diagnostics;
using Java.Util;
using Android.Graphics.Drawables;
using ZXing.Mobile;
using AndroidHUD;
using System.Net;

namespace WineHangouts

{

	[Activity(Label = "@string/ApplicationName", MainLauncher = true, Icon = "@drawable/logo5", ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
	public class LoginActivity : Activity
	{
		public string otp = "";
		private int screenid = 1;
		public Button BtnLogin;
		public Button BtnResend;
		public string gplaystatus = "";
		public TextView TxtScanresult;

		ServiceWrapper svc = new ServiceWrapper();
		CustomerResponse authen = new CustomerResponse();
		protected override void OnCreate(Bundle savedInstanceState)
		{
			CheckInternetConnection();
			Stopwatch st = new Stopwatch();
			st.Start();
			//for direct login
			//CurrentUser.SaveUserName("Lokesh Android","1");
			//ShowInfo(8019808172.ToString());
			base.OnCreate(savedInstanceState);
			SetContentView(Resource.Layout.login);
			var TaskA = new System.Threading.Tasks.Task(() =>
			{
				BlobWrapper.DownloadImages(Convert.ToInt32(CurrentUser.getUserId()));
			});
			TaskA.Start();
			ImageButton BtnScanner = FindViewById<ImageButton>(Resource.Id.btnScanner);
			Button BtnGuestLogin = FindViewById<Button>(Resource.Id.btnGuestLogin);
            LoggingClass.LogInfo("Opened the app", screenid);
            BtnScanner.Click += async delegate
			{
				try
				{
					MobileBarcodeScanner.Initialize(Application);
					var scanner = new ZXing.Mobile.MobileBarcodeScanner();
					scanner.UseCustomOverlay = false;
					var result = await scanner.Scan();
					if (result.Text != null)
					{
						LoggingClass.LogInfo("User Tried to login with " + result.Text , screenid);
						ShowInfo(result.Text);
						CurrentUser.SaveCardNumber(result.Text);
					}
				}
				catch (Exception exe)
				{
					LoggingClass.LogError(exe.Message, screenid, exe.StackTrace);
				}
			};
			BtnGuestLogin.Click += async delegate
			{
				//await svc.InsertUpdateGuest(CurrentUser.getToken());
				CurrentUser.SaveUserName("Guest", null);
				Intent intent = new Intent(this, typeof(TabActivity));
				ProgressIndicator.Show(this);
                LoggingClass.LogInfo("User Tried to login with Guest Login " , screenid);
                StartActivity(intent);
				await svc.InsertUpdateGuest("Didn't get the token");
			};
			TxtScanresult = FindViewById<TextView>(Resource.Id.txtScanresult);
			BtnLogin = FindViewById<Button>(Resource.Id.btnLogin);
			BtnResend = FindViewById<Button>(Resource.Id.btnResend);
			BtnResend.Visibility = ViewStates.Invisible;
			BtnLogin.Visibility = ViewStates.Invisible;
			if (IsPlayServicesAvailable())
			{
				var TaskB = new System.Threading.Tasks.Task(() =>
				{
					var intent = new Intent(this, typeof(RegistrationIntentService));
					StartService(intent);
				});
				TaskB.Start();
			}
			if (CurrentUser.getUserName() == null ||
				CurrentUser.getUserName() == "")
			{
                	SendRegistrationToAppServer(CurrentUser.getToken());
				if (CurrentUser.GetCardNumber() != null)
				{
					ShowInfo(CurrentUser.GetCardNumber());
				}
			}
			else
			{
				int storename = Convert.ToInt32(CurrentUser.GetPrefered());
				if (storename == 1)
				{
					Intent intent = new Intent(this, typeof(GridViewActivity));
					intent.PutExtra("MyData", "Wall Store");
					ProgressIndicator.Show(this);
					StartActivity(intent);
				}
				else if (storename == 2)
				{
					Intent intent = new Intent(this, typeof(GridViewActivity));
					intent.PutExtra("MyData", "Point Pleasant Store");
					ProgressIndicator.Show(this);
					StartActivity(intent);
				}
				else
				{
					Intent intent = new Intent(this, typeof(TabActivity));
					ProgressIndicator.Show(this);
					StartActivity(intent);
				}
                //cFPtPgzgg1o,APA91bFt7Tp1mb1dthhHQVqDkbQHUapp - O1xrk9rKiLdLbJGJltq32smRjNE1lihbnN7sf5j8Fnjii2 - o_MlDap - GB - IyqXBtZkJc4h0XxZzWFGRtOmyT9VUEig58T5fuFi9ifboBxij


            }
			var telephonyDeviceID = string.Empty;
			var telephonySIMSerialNumber = string.Empty;
			TelephonyManager telephonyManager = (TelephonyManager)this.ApplicationContext.GetSystemService(Context.TelephonyService);
			if (telephonyManager != null)
			{
				if (!string.IsNullOrEmpty(telephonyManager.DeviceId))
					telephonyDeviceID = telephonyManager.DeviceId;
				if (!string.IsNullOrEmpty(telephonyManager.SimSerialNumber))
					telephonySIMSerialNumber = telephonyManager.SimSerialNumber;
			}
			var androidID = Android.Provider.Settings.Secure.GetString(this.ApplicationContext.ContentResolver, Android.Provider.Settings.Secure.AndroidId);
			var deviceUuid = new UUID(androidID.GetHashCode(), ((long)telephonyDeviceID.GetHashCode() << 32) | telephonySIMSerialNumber.GetHashCode());
			var DeviceID = deviceUuid.ToString();
			CurrentUser.SaveDeviceID(DeviceID);
		}
		public async void ShowInfo(string Cardnumber)
        {
            AndHUD.Shared.Show(this, "Please Wait", Convert.ToInt32(MaskType.Clear));


            try
            {
                authen = await svc.AuthencateUser("test", Cardnumber, CurrentUser.GetDeviceID());
                LoggingClass.LogInfo("User Tried to login with " + Cardnumber , screenid);
                if (Cardnumber != null)
                { CurrentUser.SaveCardNumber(Cardnumber); }

                //if (authen.customer.CustomerID != 0)
                if (authen != null)
                {
                    if (authen.customer.Email != "" && authen.customer.Email != null)
                    {
                        TxtScanresult.Text = " Hi " + authen.customer.FirstName + authen.customer.LastName + ",\n We have sent an email at  " + authen.customer.Email + ".\n Please verify email to continue login. \n If you have not received email Click Resend Email.\n To get Email Id changed, contact store.";

                        TxtScanresult.SetTextColor(Android.Graphics.Color.Black);
                        BtnResend.Visibility = ViewStates.Visible;
                        BtnLogin.Visibility = ViewStates.Visible;
                        BtnResend.Click += async delegate
                        {
                            //ProgressIndicator.Show(this);
                            try
                            {
                                AndHUD.Shared.Show(this, "Sending verification email to" + authen.customer.Email, Convert.ToInt32(MaskType.Clear));

                                //BTProgressHUD.Show("Sending verification email to" + authen.customer.Email);
                                if (Cardnumber != null)
                                {
                                    LoggingClass.LogInfo("Resend email " + authen.customer.Email, screenid);
                                    await svc.ResendEMail(Cardnumber);

                                }
                                else
                                {
                                    await svc.ResendEMail(CurrentUser.GetCardNumber());

                                }
                                //BTProgressHUD.ShowSuccessWithStatus("Sent");
                                AndHUD.Shared.Dismiss();
                                AndHUD.Shared.ShowSuccess(this, "Sent", MaskType.Clear, TimeSpan.FromSeconds(2));
                            }
                            catch (Exception ex) { }
                           
                            //ProgressIndicator.Hide();
                        };
                        BtnLogin.Click += delegate
                        {
                            //ProgressIndicator.Show(this);

                            LoggingClass.LogInfo("Clicked on Login "+Cardnumber, screenid);
                            AndHUD.Shared.Show(this, "Checking Email Verification", Convert.ToInt32(MaskType.Clear));
                                EmailVerification();
                          
                            
                        };

                    }
                    else
                    {
                        TxtScanresult.Text = authen.ErrorDescription;
                        TxtScanresult.SetTextColor(Android.Graphics.Color.Red);
                    }
                }
                else
                {
                    TxtScanresult.Text = "Sorry. Your Card number is not matching our records.\n Please re-scan Or Try app as Guest Log In.";
                    BtnResend.Visibility = ViewStates.Invisible;
                    BtnLogin.Visibility = ViewStates.Invisible;
                    TxtScanresult.SetTextColor(Android.Graphics.Color.Red);
                    AndHUD.Shared.Dismiss();
                }
                AndHUD.Shared.Dismiss();
            }
            catch(Exception ex ) {
                LoggingClass.LogError(ex.Message, screenid, ex.StackTrace);

            }
            AndHUD.Shared.Dismiss();
        }
		Boolean isValidEmail(String email)
		{
			return Android.Util.Patterns.EmailAddress.Matcher(email).Matches();
		}
		public async void SendRegistrationToAppServer(string token)
		{
			TokenModel _token = new TokenModel()
			{
				User_id = Convert.ToInt32(CurrentUser.getUserId()),
				DeviceToken = token,
				DeviceType = 1
			};

			LoggingClass.LogInfoEx("Token sent to db", screenid);
			int x = await svc.InsertUpdateToken1(_token);

		}
		public async void EmailVerification()
        {
            AndHUD.Shared.Show(this, "Checking Email Verification", Convert.ToInt32(MaskType.Clear));
            authen = await svc.AuthencateUser("test", CurrentUser.GetCardNumber(), CurrentUser.GetDeviceID());
			DeviceToken DO = new DeviceToken();
			try
			{
				DO = await svc.CheckMail(authen.customer.CustomerID.ToString());

				if (DO.VerificationStatus == 1)
				{
					if (authen.customer != null && authen.customer.CustomerID != 0)
					{
                        LoggingClass.LogInfo("The User logged in with user id: " + CurrentUser.getUserId(), screenid);
                        CurrentUser.SaveUserName(authen.customer.FirstName + authen.customer.LastName, authen.customer.CustomerID.ToString());
						SendRegistrationToAppServer(CurrentUser.getToken());
						int storename = Convert.ToInt32(CurrentUser.GetPrefered());
						if (storename == 1)
						{
							Intent intent = new Intent(this, typeof(GridViewActivity));
							intent.PutExtra("MyData", "Wall Store");
							ProgressIndicator.Show(this);

							StartActivity(intent);
						}
						else if (storename == 2)
						{
							Intent intent = new Intent(this, typeof(GridViewActivity));
							intent.PutExtra("MyData", "Point Pleasant Store");

							ProgressIndicator.Show(this);
							StartActivity(intent);
						}
						else
						{
							Intent intent = new Intent(this, typeof(TabActivity));
							ProgressIndicator.Show(this);
							StartActivity(intent);
						}
						LoggingClass.LogInfoEx("User verified and Logging" + "---->" + CurrentUser.GetCardNumber(), screenid);
                        AndHUD.Shared.Dismiss();
                        AndHUD.Shared.ShowSuccess(Parent, "Success!", MaskType.Clear, TimeSpan.FromSeconds(2));
                    }
                    else
					{
                        AlertDialog.Builder aler = new AlertDialog.Builder(this);
                        aler.SetTitle("Sorry");
                        aler.SetMessage("You entered wrong details or authentication failed");
                        aler.SetNegativeButton("Ok", delegate { });
                        Dialog dialog1 = aler.Create();
                        dialog1.Show();
                      };
				}
				else
				{
                    AlertDialog.Builder aler = new AlertDialog.Builder(this);
                    aler.SetTitle("Sorry");
                    aler.SetMessage("Your email is not verified plesase check email and verify.");
                    aler.SetNegativeButton("Ok", delegate { });
                    Dialog dialog = aler.Create();
                    dialog.Show();
                   }
                //ProgressIndicator.Hide();
                AndHUD.Shared.Dismiss();
               
            }

			catch (Exception exe)
			{
				LoggingClass.LogError(exe.Message, screenid, exe.StackTrace.ToString());
			}
            AndHUD.Shared.Dismiss();
        }
		private bool IsPlayServicesAvailable()
		{
			int resultCode = GoogleApiAvailability.Instance.IsGooglePlayServicesAvailable(this);
			if (resultCode != ConnectionResult.Success)
			{
				// Google Play Service check failed - display the error to the user:
				if (GoogleApiAvailability.Instance.IsUserResolvableError(resultCode))
				{
					// Give the user a chance to download the APK:

					gplaystatus = GoogleApiAvailability.Instance.GetErrorString(resultCode);
				}
				else
				{
					gplaystatus = "Sorry, this device is not supported";
					AlertDialog.Builder aler = new AlertDialog.Builder(this);
					aler.SetTitle("Sorry");
					aler.SetMessage(gplaystatus);
					aler.SetNegativeButton("Ok", delegate { });
					Dialog dialog3 = aler.Create();
					dialog3.Show();
					Finish();
				}
				return false;
			}
			else
			{
				gplaystatus = "Google Play Services is available.";
				return true;
			}
		}
		private void SendSmsgs(string userNumber)
		{
			otp = RandomString(4);
			int otpcount = otp.Count();
			SmsManager.Default.SendTextMessage(userNumber.ToString(), null, "Your winehangouts Otp is:" + otp, null, null);
			//otps.Add(otp);
			//string httpreq="http://bhashsms.com/api/sendmsg.php?user=success&pass=********&sender=WineHangouts&phone=" + userNumber + "&text=" + otp + "&priority=dnd&stype=unicode";
		}
		private System.Random random = new System.Random();
		public string RandomString(int length)
		{
			const string chars = "0123456789";
			return new string(Enumerable.Repeat(chars, length)
			  .Select(s => s[random.Next(s.Length)]).ToArray());
		}
		protected override void OnPause()
		{
			base.OnPause();
			LoggingClass.LogInfo("OnPause state in Login activity", screenid);
		}
		protected override void OnResume()
		{
			base.OnResume();
			LoggingClass.LogInfo("OnResume state in Login activity", screenid);
		}
		public bool CheckInternetConnection()
		{

			string CheckUrl = "http://google.com";

			try
			{
				HttpWebRequest iNetRequest = (HttpWebRequest)WebRequest.Create(CheckUrl);

				iNetRequest.Timeout = 5000;

				WebResponse iNetResponse = iNetRequest.GetResponse();

				// Console.WriteLine ("...connection established..." + iNetRequest.ToString ());
				iNetResponse.Close();

				return true;

			}
			catch (WebException ex)
			{
				AlertDialog.Builder aler = new AlertDialog.Builder(this, Resource.Style.MyDialogTheme);
				aler.SetTitle("Sorry");
				LoggingClass.LogInfo("Please check your internet connection", screenid);
				aler.SetMessage("Please check your internet connection");
				aler.SetNegativeButton("Ok", delegate { });
				Dialog dialog = aler.Create();
				dialog.Show();
				return false;
			}
		}

	}
}