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
    [Activity(Label = "Verification")]
    public class VerificationActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.OtpReceiverLayout);
            string Sentotp = Intent.GetStringExtra("otp");
            string username = Intent.GetStringExtra("username");
            EditText receivedOtp = FindViewById<EditText>(Resource.Id.txtOtp);
            Button btnVerification = FindViewById<Button>(Resource.Id.btnVerify);
            btnVerification.Click += delegate
            {
                if (Sentotp == receivedOtp.Text)
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
                        authen = svc.AuthencateUser(username).Result;
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
            };
            
        }

        private void BtnVerification_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}