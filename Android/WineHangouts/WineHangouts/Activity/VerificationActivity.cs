using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Hangout;
using Hangout.Models;

namespace WineHangouts
{
    [Activity(Label = "Verification", ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    public class VerificationActivity : Activity
    {
        ServiceWrapper sc = new ServiceWrapper();
        private int screenid = 21;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            //base.OnCreate(savedInstanceState);
            //SetContentView(Resource.Layout.EmailVerificationLayout);
            //EmailVerification();
            ////string Sentotp = Intent.GetStringExtra("otp");//receiving otp from previous activity
            ////string username = Intent.GetStringExtra("username");//receiving username from previous activity
            ////EditText txtreceivedOtp = FindViewById<EditText>(Resource.Id.txtOtp);
            //Button btnVerification = FindViewById<Button>(Resource.Id.btnVerify);
            //Button btnResendMail = FindViewById<Button>(Resource.Id.btnResendMail);
            //EditText editEmail = FindViewById<EditText>(Resource.Id.txtEmail);
            //editEmail.Text = CurrentUser.GetCardNumber();//GetMailId();

            //btnResendMail.Click += async delegate
            //{
            //    CurrentUser.SaveCardNumber(editEmail.Text);
            //    await sc.AuthencateUser1(CurrentUser.GetCardNumber());
            //};
            //btnVerification.Click += delegate
            //{
            //    EmailVerification();
            //};

        }
        public async void EmailVerification()
        {
            DeviceToken DO = new DeviceToken();
            try
            {
                DO = await sc.CheckMail(CurrentUser.getUserId());
            }
            catch(Exception exe)
            {
                LoggingClass.LogError(exe.Message, screenid, exe.StackTrace.ToString());
            }
            if (DO.VerificationStatus == 1)
            {
                Intent intent = new Intent(this, typeof(TabActivity));
                StartActivity(intent);
            }
            else
            {
                AlertDialog.Builder alert = new AlertDialog.Builder(this);
                alert.SetTitle("Sorry");
                alert.SetMessage("Please verify your mail id");
                alert.SetNegativeButton("Ok", delegate { });
                Dialog dialog = alert.Create();
                dialog.Show();
            }

        }

        public void OtpVerification(string sentOtp, string receivedOtp, string username)
        {
            //starting of otp verification code
            if (sentOtp == receivedOtp)
            {
                AlertDialog.Builder alert = new AlertDialog.Builder(this);
                alert.SetTitle("Successfully your logged in");
                alert.SetMessage("Thank You");
                alert.SetNegativeButton("Ok", delegate { });
                Dialog dialog = alert.Create();
                dialog.Show();
                CustomerResponse authen = new CustomerResponse();
                ServiceWrapper svc = new ServiceWrapper();
                try
                {
                   /// authen = svc.AuthencateUser(username).Result;
                    if (authen.customer != null && authen.customer.CustomerID != 0)
                    {
                        CurrentUser.SaveUserName(username, authen.customer.CustomerID.ToString());
                        Intent intent = new Intent(this, typeof(TabActivity));
                        StartActivity(intent);

                    }
                    else
                    {
                        AlertDialog.Builder aler = new AlertDialog.Builder(this);
                        aler.SetTitle("Sorry");
                        aler.SetMessage("You entered wrong ");
                        aler.SetNegativeButton("Ok", delegate { });
                        Dialog dialog1 = aler.Create();
                        dialog1.Show();
                    };
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
            }
            else
            {
                AlertDialog.Builder aler = new AlertDialog.Builder(this);
                aler.SetTitle("Incorrect Otp");
                aler.SetMessage("Please Check Again");
                aler.SetNegativeButton("Ok", delegate { });
                Dialog dialog = aler.Create();
                dialog.Show();
            }
            //Ending of otp verification code
        }
    }
}