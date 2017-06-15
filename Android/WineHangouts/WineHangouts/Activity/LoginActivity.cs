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

namespace WineHangouts

{
	[Activity(Label = "@string/ApplicationName", MainLauncher = true, Icon = "@drawable/logo5")]
	public class LoginActivity : Activity

	{
		public string otp = "";
		private int screenid = 1;

		private Context myContext;
		public string gplaystatus = "";
		ServiceWrapper svc = new ServiceWrapper();
		CustomerResponse authen = new CustomerResponse();
		protected override void OnCreate(Bundle savedInstanceState)
		{
			Stopwatch st = new Stopwatch();
			st.Start();
			//for direct login
			//CurrentUser.SaveUserName("user","3");
			base.OnCreate(savedInstanceState);

			SetContentView(Resource.Layout.login);
			Button login = FindViewById<Button>(Resource.Id.btnLoginLL);
			Button Resend = FindViewById<Button>(Resource.Id.btnLoginRs);
			///Resend.Visibility = ViewStates.Invisible;
			EditText CardID = FindViewById<EditText>(Resource.Id.txtUsername);
			EditText UserEmail = FindViewById<EditText>(Resource.Id.TxtUserEmail);
			Boolean isValidEmail(String email)
			{
				return Android.Util.Patterns.EmailAddress.Matcher(email).Matches();
			}
			ServiceWrapper svc = new ServiceWrapper();
			var TaskA = new System.Threading.Tasks.Task(() => {
				BlobWrapper.DownloadImages(Convert.ToInt32(CurrentUser.getUserId()));
			});
			TaskA.Start();

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
				// Do nothing
				int i = 0;
			}
			else
			{
				//string ap = "Logged In with userid" + CurrentUser.GetMailId();
				//LoggingClass.UploadErrorLogs();	
				//            LoggingClass.LogInfoEx(ap,screenid);
				//LoggingClass.LogInfoEx("User entering into Tab activity", screenid);

				Intent intent = new Intent(this, typeof(TabActivity));
				ProgressIndicator.Show(this);
				StartActivity(intent);

				//            SendRegistrationToAppServer(CurrentUser.getToken());
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

			//DeleteReview1 dr = new DeleteReview1();
			//dr.Show(((Activity)myContext).FragmentManager, "");




			//ShowExitDialog();
			Resend.Click += delegate
			{
				CurrentUser.SaveUserName("Guest", null);
				Intent intent = new Intent(this, typeof(TabActivity));
				ProgressIndicator.Show(this);
				string i = CurrentUser.getUserId();
				StartActivity(intent);

			};

			login.Click += async delegate
			{
				ProgressIndicator.Show(this);
				var DeviceD = Android.OS.Build.Serial;
				if (UserEmail.Text == "")
				{
					AlertDialog.Builder aler = new AlertDialog.Builder(this, Resource.Style.MyDialogTheme);
					aler.SetTitle("Sorry");
					aler.SetMessage("Please enter email id please");
					aler.SetNegativeButton("Ok", delegate { ProgressIndicator.Hide(); });
					Dialog dialog = aler.Create();
					dialog.Show();

					return;
				}
				else if (CardID.Text == "")
				{
					AlertDialog.Builder aler = new AlertDialog.Builder(this, Resource.Style.MyDialogTheme);
					aler.SetTitle("Sorry");
					aler.SetMessage("Please enter CardID please");
					aler.SetNegativeButton("Ok", delegate { ProgressIndicator.Hide(); });
					Dialog dialog = aler.Create();
					dialog.Show();
					return;
				}

				else if (isValidEmail(UserEmail.Text) == true)
				{
					CurrentUser.SaveMailId(UserEmail.Text);

					try
					{

						authen = await svc.AuthencateUser(CardID.Text, UserEmail.Text, DeviceID);
						if (authen.customer != null && authen.customer.CustomerID != 0)
						{

							CurrentUser.SaveUserName("user", authen.customer.CustomerID.ToString());
							SendRegistrationToAppServer(CurrentUser.getToken());

							ProgressIndicator.Show(this);
							Intent intentq = new Intent(this, typeof(ProfileActivity));
							LoggingClass.LogInfoEx("User verified and Logging" + "---->" + CurrentUser.GetMailId(), screenid);
							StartActivity(intentq);






						}

						else
						{
							AlertDialog.Builder aler = new AlertDialog.Builder(this, Resource.Style.MyDialogTheme);
							aler.SetTitle("Sorry");
							string mess = authen.ErrorDescription;
							aler.SetMessage(mess);
							aler.SetNegativeButton("Ok", delegate { ProgressIndicator.Hide(); });
							Dialog dialog1 = aler.Create();
							dialog1.Show();

						};











						//CurrentUser.SaveUserName("user", authen.customer.CustomerID.ToString());
						//SendRegistrationToAppServer(CurrentUser.getToken());



						//Intent intent = new Intent(this, typeof(TabActivity));
						//LoggingClass.LogInfoEx("User verified and Logging" + "---->" + CurrentUser.GetMailId(), screenid);
						//StartActivity(intent);

						LoggingClass.LogInfoEx("user---->" + UserEmail.Text, screenid);
						//EmailVerification();

					}


					catch (Exception exception)
					{
						if (exception.Message.ToString() == "One or more errors occurred.")
						{

							AlertDialog.Builder aler = new AlertDialog.Builder(this, Resource.Style.MyDialogTheme);
							aler.SetTitle("Sorry");
							aler.SetMessage("Please check your internet connection");
							aler.SetNegativeButton("Ok", delegate { ProgressIndicator.Hide(); });
							Dialog dialog2 = aler.Create();
							dialog2.Show();
						}
						else
						{
							AlertDialog.Builder aler = new AlertDialog.Builder(this, Resource.Style.MyDialogTheme);
							aler.SetTitle("Sorry");
							aler.SetMessage("We're under maintanence");
							aler.SetNegativeButton("Ok", delegate { ProgressIndicator.Hide(); });
							Dialog dialog3 = aler.Create();
							dialog3.Show();

						}
						ProgressIndicator.Hide();
					}

					//SendSmsgs(txtUserNumber.Text);
					//var intent = new Intent(this, typeof(VerificationActivity));
					////var intent = new Intent(this, typeof(TabActivity));
					//intent.PutExtra("otp", otp);
					//intent.PutExtra("username", username.Text);
					//StartActivity(intent);

				}
				else
				{
					AlertDialog.Builder aler = new AlertDialog.Builder(this, Resource.Style.MyDialogTheme);
					aler.SetTitle("Sorry");
					aler.SetMessage("Please enter valid EmailID please");
					//aler.SetBackgroundDrawable(new ColorDrawable(Android.Graphics.Color.Purple));
					aler.SetNegativeButton("Ok", delegate { ProgressIndicator.Hide(); });
					Dialog dialog = aler.Create();

					dialog.Show();
					return;

				}

				//CustomerResponse authen = new CustomerResponse();
				//try
				//{
				//    authen = svc.AuthencateUser(username.Text).Result;
				//    if (authen.customer != null && authen.customer.CustomerID != 0)
				//    {
				//        CurrentUser.SaveUserName(username.Text, authen.customer.CustomerID.ToString());
				//        Intent intent = new Intent(this, typeof(TabActivity));
				//        StartActivity(intent);

				//    }
				//    else
				//    {
				//        AlertDialog.Builder aler = new AlertDialog.Builder(this);
				//        aler.SetTitle("Sorry");
				//        aler.SetMessage("Incorrect Details");
				//        aler.SetNegativeButton("Ok", delegate { });
				//        Dialog dialog = aler.Create();
				//        dialog.Show();
				//    };
				//}
				//catch(Exception exception)
				//{
				//    if (exception.Message.ToString() == "One or more errors occurred.")
				//    {
				//        AlertDialog.Builder aler = new AlertDialog.Builder(this);
				//        aler.SetTitle("Sorry");
				//        aler.SetMessage("Please check your internet connection");
				//        aler.SetNegativeButton("Ok", delegate { });
				//        Dialog dialog = aler.Create();
				//        dialog.Show();
				//    }
				//    else {
				//        AlertDialog.Builder aler = new AlertDialog.Builder(this);
				//        aler.SetTitle("Sorry");
				//        aler.SetMessage("We're under maintanence");
				//        aler.SetNegativeButton("Ok", delegate { });
				//        Dialog dialog = aler.Create();
				//        dialog.Show();

				//    }

				//}

				ProgressIndicator.Hide();

				//Resend.Visibility = ViewStates.Visible;
				st.Stop();
				LoggingClass.LogTime("login activity", st.Elapsed.TotalSeconds.ToString());
			};




		}

		//private static void BrandAlertDialog(Dialog dialog)
		//{
		//	try
		//	{
		//		var resources = dialog.Context.Resources;
		//		var color = dialog.Context.Resources.GetColor(Android.Graphics.Color.Purple);
		//		//var background = dialog.Context.Resources.GetColor(Resource.Color.dialog_background);

		//		var alertTitleId = resources.GetIdentifier("alertTitle", "id", "android");
		//		var alertTitle = (TextView)dialog.Window.DecorView.FindViewById(alertTitleId);
		//		alertTitle.SetTextColor(color); // change title text color

		//		var titleDividerId = resources.GetIdentifier("titleDivider", "id", "android");
		//		var titleDivider = dialog.Window.DecorView.FindViewById(titleDividerId);
		//		//titleDivider.SetBackgroundColor(background); // change divider color
		//	}
		//	catch
		//	{
		//		//Can't change dialog brand color
		//	}
		//}
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
		//     public async void EmailVerification()
		//     {
		//         DeviceToken DO = new DeviceToken();
		//         authen = svc.AuthencateUser1(CurrentUser.GetMailId()).Result;

		//try
		//         {
		//             DO = await svc.CheckMail(authen.customer.CustomerID.ToString());

		//             if (DO.VerificationStatus == 1)
		//             {
		//                 if (authen.customer != null && authen.customer.CustomerID != 0)
		//                 {
		//			//ProgressIndicator.Hide();
		//			CurrentUser.SaveUserName("user", authen.customer.CustomerID.ToString());
		//                     SendRegistrationToAppServer(CurrentUser.getToken());



		//			Intent intent = new Intent(this, typeof(TabActivity));
		//			LoggingClass.LogInfoEx("User verified and Logging" + "---->" + CurrentUser.GetMailId(), screenid);
		//			StartActivity(intent);

		//		}

		//                 else
		//                 {
		//                     AlertDialog.Builder aler = new AlertDialog.Builder(this);
		//                     aler.SetTitle("Sorry");
		//                     aler.SetMessage("You entered wrong details or authentication failed");
		//                     aler.SetNegativeButton("Ok", delegate { });
		//                     Dialog dialog1 = aler.Create();
		//                     dialog1.Show();
		//                 };

		//	}
		//             else
		//             {
		//                 AlertDialog.Builder aler = new AlertDialog.Builder(this);
		//                 aler.SetTitle("Sorry");
		//                 aler.SetMessage("If you're verified by mail click on verify");
		//                 aler.SetNegativeButton("Verify", delegate
		//                 {
		//                     EmailVerification();
		//                 });
		//                 aler.SetPositiveButton("Resend mail", async delegate
		//                 {
		//                     await svc.AuthencateUser1(CurrentUser.GetMailId());
		//                 });
		//                 //Dialog dialog = aler.Create();
		//                 //dialog.Show();
		//             }
		//         }
		//         catch (Exception exe)
		//         {
		//             LoggingClass.LogError(exe.Message, screenid, exe.StackTrace.ToString());
		//         }

		//     }
		private void ShowExitDialog()
		{
			AlertDialog.Builder aler = new AlertDialog.Builder(this);
			aler.SetTitle("Sorry");
			string mess = authen.ErrorDescription;
			aler.SetMessage(mess);
			aler.SetNegativeButton("Ok", delegate { });
			Dialog dialog1 = aler.Create();
			dialog1.Show();
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
	}
}