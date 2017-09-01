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
using Android.Graphics;

namespace WineHangouts

{

    [Activity(Label = "@string/ApplicationName", MainLauncher = true, Icon = "@drawable/logo5", ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    public class LoginActivity : Activity
    {
        public string otp = "";
        private int screenid = 1;
        public Button BtnLogin;
        public Button BtnResend;
        public Button BtnContinue;
        public Button BtnUpdateEmail;
        public string gplaystatus = "";
        public TextView TxtScanresult;

        ServiceWrapper svc = new ServiceWrapper();
        CustomerResponse AuthServ = new CustomerResponse();
        protected override void OnCreate(Bundle savedInstanceState)
        {
            CheckInternetConnection();
            Stopwatch st = new Stopwatch();
            st.Start();
            //for direct login
           CurrentUser.SaveUserName("Mohana Android","48732");
            //Preinfo("7207589007");
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
                        LoggingClass.LogInfo("User Tried to login with " + result.Text, screenid);
                        Preinfo(result.Text);
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
                //await svc.InsertUpdateGuest(CurrentUser.getAuthToken());
                CurrentUser.SaveUserName("Guest", null);
                Intent intent = new Intent(this, typeof(TabActivity));
                ProgressIndicator.Show(this);
                LoggingClass.LogInfo("User Tried to login with Guest Login ", screenid);
                StartActivity(intent);
                await svc.InsertUpdateGuest("Didn't get the token");
            };
            TxtScanresult = FindViewById<TextView>(Resource.Id.txtScanresult);
            BtnLogin = FindViewById<Button>(Resource.Id.btnLogin);
            BtnResend = FindViewById<Button>(Resource.Id.btnResend);
            BtnContinue = FindViewById<Button>(Resource.Id.btnContinue);
            BtnUpdateEmail = FindViewById<Button>(Resource.Id.btnUpdateEmail);
            BtnResend.Visibility = ViewStates.Invisible;
            BtnLogin.Visibility = ViewStates.Invisible;
            BtnContinue.Visibility = ViewStates.Invisible;
            BtnUpdateEmail.Visibility = ViewStates.Invisible;
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
                SendRegistrationToAppServer(CurrentUser.getDeviceToken());
                if (CurrentUser.GetCardNumber() != null)
                {
                    Preinfo(CurrentUser.GetCardNumber());
                }
            }
            else if(CurrentUser.getUserName()=="Guest")
            {

            }
            else
            {
                int storename =Convert.ToInt32(CurrentUser.GetPrefered());
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
        public async void Preinfo(string CardNumber)
        {
            AndHUD.Shared.Show(this, "Please Wait...", Convert.ToInt32(MaskType.Clear));
            try
            {
                BtnLogin.Visibility = ViewStates.Invisible;
                BtnResend.Visibility = ViewStates.Invisible;
                AuthServ = await svc.AuthencateUser("test", CardNumber, CurrentUser.GetDeviceID());
                LoggingClass.LogInfo("User Tried to login with " + CardNumber, screenid);
                if (CardNumber != null)
                {
                    CurrentUser.SaveCardNumber(CardNumber);
                }
                if (AuthServ != null)
                {
                    if (AuthServ.customer.Email != "" && AuthServ.customer.Email != null)
                    {
                        TxtScanresult.Text = AuthServ.ErrorDescription;//" Hi " + authen.customer.FirstName + authen.customer.LastName + ",\n We have sent an email at  " + authen.customer.Email + ".\n Please verify email to continue login. \n If you have not received email Click Resend Email.\n To get Email Id changed, contact store.";
                        TxtScanresult.SetTextColor(Android.Graphics.Color.Black);
                        BtnContinue.Visibility = ViewStates.Visible;
                        BtnUpdateEmail.Visibility = ViewStates.Visible;
                        BtnContinue.Click += async delegate
                        {
                            AndHUD.Shared.Show(this, " Please Wait...", Convert.ToInt32(MaskType.Clear));
                            AuthServ = await svc.ContinueService(AuthServ);
                            ShowInfo(AuthServ);
                            AndHUD.Shared.Dismiss();
                        };
                        BtnUpdateEmail.Click += delegate
                        {
                            BtnUpdateEmail_Click("please enter your new e-mail id.");  
                        };    
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
            catch (Exception exe)
            {

            }
        }
        private void BtnUpdateEmail_Click(string Message)
        {
            try
            {
                AlertDialog.Builder aler = new AlertDialog.Builder(this, Resource.Style.MyDialogTheme);
                LoggingClass.LogInfo("Entered Incorrect Details", screenid);
                aler.SetTitle(Message);
                EditText txtEmail = new EditText(this);
                txtEmail.SetTextColor(Color.Black);
                txtEmail.FocusableInTouchMode = true;
                aler.SetView(txtEmail);
                aler.SetPositiveButton("Update", async delegate
                {


                    if (txtEmail.Text == null && txtEmail.Text == "")
                    {
                        //BTProgressHUD.ShowErrorWithStatus("Email is invalid",3000);
                        BtnUpdateEmail_Click("Entered email id is invalid,Please enter again");
                    }
                    else if (txtEmail.Text.Contains("@") != true && txtEmail.Text.Contains(".") != true)
                    {
                        BtnUpdateEmail_Click("Entered email id is invalid,Please enter again");
                    }
                    else
                    {
                        AndHUD.Shared.Show(this, "Updating...Please Wait...", Convert.ToInt32(MaskType.Clear));
                        //BTProgressHUD.ShowSuccessWithStatus("We're sending mail to the updated mail");
                        //CurrentUser.PutEmail(txtEmail.Text);
                        AuthServ = await svc.UpdateMail(txtEmail.Text, AuthServ.customer.CustomerID.ToString());
                        ShowInfo(AuthServ);
                        AndHUD.Shared.Dismiss();

                        //AndHUD.Shared.ShowSuccess(Parent, "Updated!", MaskType.Clear, TimeSpan.FromSeconds(2));
                    }
                });
                aler.SetNegativeButton("Cancel", (senderAlert, args) =>
                {
                    Toast.MakeText(this, "Cancelled!", ToastLength.Short).Show();
                });
                Dialog dialog = aler.Create();
                dialog.Show();
            }
            catch (Exception ex) { }
            
        }
        public async void ShowInfo(CustomerResponse AuthServ)
        {
            AndHUD.Shared.Show(this, "Please Wait...", Convert.ToInt32(MaskType.Clear));
            try
            {
                //    AuthServ = await svc.AuthencateUser("test", Cardnumber, CurrentUser.GetDeviceID());
                //    LoggingClass.LogInfo("User Tried to login with " + Cardnumber , screenid);
                   //if (Cardnumber != null)
                   // { CurrentUser.SaveCardNumber(Cardnumber); }

                //    //if (authen.customer.CustomerID != 0)
                //if (AuthServ != null)
                //{
                    if (AuthServ.customer.Email != "" && AuthServ.customer.Email != null)
                    {
                        TxtScanresult.Text = AuthServ.ErrorDescription;// " Hi " + AuthServ.customer.FirstName + authen.customer.LastName + ",\n We have sent an email at  " + authen.customer.Email + ".\n Please verify email to continue login. \n If you have not received email Click Resend Email.\n To get Email Id changed, contact store.";

                        TxtScanresult.SetTextColor(Android.Graphics.Color.Black);
                        BtnResend.Visibility = ViewStates.Visible;
                        BtnLogin.Visibility = ViewStates.Visible;
                        BtnContinue.Visibility = ViewStates.Invisible;
                        BtnUpdateEmail.Visibility = ViewStates.Invisible;
                        BtnResend.Click += async delegate
                        {
                            try
                            {
                                AndHUD.Shared.Show(this, "Sending verification email to " + AuthServ.customer.Email, Convert.ToInt32(MaskType.Clear));
                                 LoggingClass.LogInfo("Resend email " + AuthServ.customer.Email, screenid);
                                    await svc.ResendEMail(CurrentUser.GetCardNumber());
                                    AndHUD.Shared.ShowSuccess(this, "Sent", MaskType.Clear, TimeSpan.FromSeconds(2));
                                AndHUD.Shared.Dismiss();
                            }
                            catch (Exception ex)
                            {
                            }
                        };
                        BtnLogin.Click += delegate
                        {
                            //ProgressIndicator.Show(this);

                            LoggingClass.LogInfo("Clicked on Login " + AuthServ.customer.CardNumber, screenid);
                            AndHUD.Shared.Show(this, "Checking Email Verification", Convert.ToInt32(MaskType.Clear));
                            EmailVerification();
                        };
                    }
                    else
                    {
                        TxtScanresult.Text = AuthServ.ErrorDescription;
                        TxtScanresult.SetTextColor(Android.Graphics.Color.Red);
                    }
                //}
                //else
                //{
                //    TxtScanresult.Text = "Sorry. Your Card number is not matching our records.\n Please re-scan Or Try app as Guest Log In.";
                //    BtnResend.Visibility = ViewStates.Invisible;
                //    BtnLogin.Visibility = ViewStates.Invisible;
                //    TxtScanresult.SetTextColor(Android.Graphics.Color.Red);
                //    AndHUD.Shared.Dismiss();
                //}
                AndHUD.Shared.Dismiss();
            }
            catch (Exception ex)
            {
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
            AuthServ = await svc.AuthencateUser("test", CurrentUser.GetCardNumber(), CurrentUser.GetDeviceID());
            DeviceToken DO = new DeviceToken();
            try
            {
                DO = await svc.CheckMail(AuthServ.customer.CustomerID.ToString());

                if (DO.VerificationStatus == 1)
                {
                    if (AuthServ.customer != null && AuthServ.customer.CustomerID != 0)
                    {
                        LoggingClass.LogInfo("The User logged in with user id: " + CurrentUser.getUserId(), screenid);
                        CurrentUser.SaveUserName(AuthServ.customer.FirstName + AuthServ.customer.LastName, AuthServ.customer.CustomerID.ToString());
                        SendRegistrationToAppServer(CurrentUser.getDeviceToken());
                        CurrentUser.SavePrefered(AuthServ.customer.PreferredStore);
                        int storename = AuthServ.customer.PreferredStore;
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
                        AlertDialog.Builder aler = new AlertDialog.Builder(this, Resource.Style.MyDialogTheme);
                        aler.SetTitle("Sorry");
                        aler.SetMessage("You entered wrong details or authentication failed");
                        aler.SetNegativeButton("Ok", delegate { });
                        Dialog dialog1 = aler.Create();
                        dialog1.Show();
                        //   AndHUD.Shared.ShowErrorWithStatus(this, "You entered wrong details or authentication failed", MaskType.Clear, TimeSpan.FromSeconds(2));

                    };
                }
                else
                {
                    AlertDialog.Builder aler = new AlertDialog.Builder(this, Resource.Style.MyDialogTheme);
                    //aler.SetTitle("Sorry");
                    aler.SetMessage("Your email is not verified. please check email and verify.");
                    aler.SetNegativeButton("Ok", delegate { });
                    Dialog dialog = aler.Create();
                    dialog.Show();
                    //  AndHUD.Shared.ShowErrorWithStatus(this, "Your email is not verified plesase check email and verify.", MaskType.Clear, TimeSpan.FromSeconds(2));
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
        //private void SendSmsgs(string userNumber)
        //{
        //	otp = RandomString(4);
        //	int otpcount = otp.Count();
        //	SmsManager.Default.SendTextMessage(userNumber.ToString(), null, "Your winehangouts Otp is:" + otp, null, null);
        //	//otps.Add(otp);
        //	//string httpreq="http://bhashsms.com/api/sendmsg.php?user=success&pass=********&sender=WineHangouts&phone=" + userNumber + "&text=" + otp + "&priority=dnd&stype=unicode";
        //}
        //private System.Random random = new System.Random();
        //public string RandomString(int length)
        //{
        //	const string chars = "0123456789";
        //	return new string(Enumerable.Repeat(chars, length)
        //	  .Select(s => s[random.Next(s.Length)]).ToArray());
        //}
        //protected override void OnPause()
        //{
        //	base.OnPause();
        //	LoggingClass.LogInfo("OnPause state in Login activity", screenid);
        //}
        //protected override void OnResume()
        //{
        //	base.OnResume();
        //	LoggingClass.LogInfo("OnResume state in Login activity", screenid);
        //}
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