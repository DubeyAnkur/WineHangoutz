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
namespace WineHangouts

{
    [Activity(Label = "@string/ApplicationName", MainLauncher =true, Icon ="@drawable/logo5")]
    public class LoginActivity : Activity

    {
        public string otp = "";
        private int screenid = 1;
        public string gplaystatus = "";
        ServiceWrapper svc = new ServiceWrapper();
        CustomerResponse authen = new CustomerResponse();
        protected override void OnCreate(Bundle savedInstanceState)
        {
			Stopwatch st = new Stopwatch();
			st.Start();
			base.OnCreate(savedInstanceState);
            
            SetContentView(Resource.Layout.login);
            Button login = FindViewById<Button>(Resource.Id.btnLoginLL);
			Button Resend = FindViewById<Button>(Resource.Id.btnLoginRs);
			Resend.Visibility = ViewStates.Invisible;
			//EditText UserName = FindViewById<EditText>(Resource.Id.txtUsername);
			EditText UserEmail = FindViewById<EditText>(Resource.Id.TxtUserEmail);
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
                CurrentUser.getUserName() == "" )
            {
				// Do nothing
				int i = 0;
            }
            else
            {
				string ap = "Logged In with userid" + CurrentUser.GetMailId();
				LoggingClass.UploadErrorLogs();	
                LoggingClass.LogInfoEx(ap,screenid);
				LoggingClass.LogInfoEx("User entering into Tab activity", screenid);
				
				Intent intent = new Intent(this, typeof(TabActivity));
			
				StartActivity(intent);
                SendRegistrationToAppServer(CurrentUser.getToken());
            }

			

			login.Click += async delegate
            {
				ProgressIndicator.Show(this);
				
				if (UserEmail.Text == "")
                {
                    AlertDialog.Builder aler = new AlertDialog.Builder(this);
                    aler.SetTitle("Sorry");
                    aler.SetMessage("Please enter email id please");
                    aler.SetNegativeButton("Ok", delegate { });
                    Dialog dialog = aler.Create();
                    dialog.Show();
                    return;
                }
                else
                {
                    CurrentUser.SaveMailId(UserEmail.Text);
                    
                    try
                    {
                        await svc.AuthencateUser1(CurrentUser.GetMailId());
						LoggingClass.LogInfoEx("user---->" + UserEmail.Text, screenid);
						EmailVerification();
						
					}
					

					catch (Exception exception)
                    {
                        if (exception.Message.ToString() == "One or more errors occurred.")
                        {
                            
                            AlertDialog.Builder aler = new AlertDialog.Builder(this);
                            aler.SetTitle("Sorry");
                            aler.SetMessage("Please check your internet connection");
                            aler.SetNegativeButton("Ok", delegate { });
                            Dialog dialog2 = aler.Create();
                            dialog2.Show();
                        }
                        else
                        {
                            AlertDialog.Builder aler = new AlertDialog.Builder(this);
                            aler.SetTitle("Sorry");
                            aler.SetMessage("We're under maintanence");
                            aler.SetNegativeButton("Ok", delegate { });
                            Dialog dialog3 = aler.Create();
                            dialog3.Show();

                        }
                    }

                   //SendSmsgs(txtUserNumber.Text);
                    //var intent = new Intent(this, typeof(VerificationActivity));
                    ////var intent = new Intent(this, typeof(TabActivity));
                    //intent.PutExtra("otp", otp);
                    //intent.PutExtra("username", username.Text);
                    //StartActivity(intent);

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

				Resend.Visibility = ViewStates.Visible;
				st.Stop();
				LoggingClass.LogTime("login activity",st.Elapsed.TotalSeconds.ToString());
			};

			

		}
        public async void SendRegistrationToAppServer(string token)
        {
            TokenModel _token = new TokenModel()
            {
                User_id = Convert.ToInt32(CurrentUser.getUserId()),
                DeviceToken = token,
                DeviceType = 1
            };
           
            LoggingClass.LogInfoEx("Token sent to db",screenid);
            int x = await svc.InsertUpdateToken1(_token);
           
        }
        public async void EmailVerification()
        {
            DeviceToken DO = new DeviceToken();
            authen = svc.AuthencateUser1(CurrentUser.GetMailId()).Result;
		
			try
            {
                DO = await svc.CheckMail(authen.customer.CustomerID.ToString());

                if (DO.VerificationStatus == 1)
                {
                    if (authen.customer != null && authen.customer.CustomerID != 0)
                    {
						//ProgressIndicator.Hide();
						CurrentUser.SaveUserName("user", authen.customer.CustomerID.ToString());
                        SendRegistrationToAppServer(CurrentUser.getToken());

					
						
						Intent intent = new Intent(this, typeof(TabActivity));
						LoggingClass.LogInfoEx("User verified and Logging" + "---->" + CurrentUser.GetMailId(), screenid);
						StartActivity(intent);
						
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
                    aler.SetMessage("If you're verified by mail click on verify");
                    aler.SetNegativeButton("Verify", delegate
                    {
                        EmailVerification();
                    });
                    aler.SetPositiveButton("Resend mail", async delegate
                    {
                        await svc.AuthencateUser1(CurrentUser.GetMailId());
                    });
                    //Dialog dialog = aler.Create();
                    //dialog.Show();
                }
            }
            catch (Exception exe)
            {
                LoggingClass.LogError(exe.Message, screenid, exe.StackTrace.ToString());
            }

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
                    
                    gplaystatus= GoogleApiAvailability.Instance.GetErrorString(resultCode);
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
			LoggingClass.LogInfo("OnPause state in Login activity" , screenid);

		}

		protected override void OnResume()
		{
			base.OnResume();
			LoggingClass.LogInfo("OnResume state in Login activity" , screenid);
		}
	}
}