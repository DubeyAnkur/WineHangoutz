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
using System.Threading;
using System.Threading.Tasks;
using Hangout.Models;
using Android.Telephony;

namespace WineHangouts

{
    [Activity(Label = "@string/ApplicationName", MainLauncher =false, Icon ="@drawable/logo5")]
    public class LoginActivity : Activity

    {
        public string otp = "";
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            
            SetContentView(Resource.Layout.login);
           
            Button login = FindViewById<Button>(Resource.Id.btnLoginLL);
            
            EditText username = FindViewById<EditText>(Resource.Id.txtUsername);
            EditText txtUserNumber = FindViewById<EditText>(Resource.Id.MobileNumber);
            
            ServiceWrapper svc = new ServiceWrapper();
            //new Thread(new ThreadStart(delegate
            //{
            //    RunOnUiThread(() => bvb.DownloadImages(Convert.ToInt32(CurrentUser.getUserId())));
            //})).Start();

            //bvb.DownloadImages(Convert.ToInt32(CurrentUser.getUserId()));
            var TaskA = new Task(() => {
                BlobWrapper.DownloadImages(Convert.ToInt32(CurrentUser.getUserId()));
            });
            TaskA.Start();


            if (CurrentUser.getUserName() == null ||
                CurrentUser.getUserName() == "" )
            {
                // Do nothing
            }
            else
            {

                Intent intent = new Intent(this, typeof(TabActivity));
                StartActivity(intent);

            }



            login.Click += delegate
            {
                //1. Call Auth service and check for this user, it returns one.
                //2. If it returns 1 save Username and go to Tab Activity.
                //3. Else Show message, incorrect username.
                //
                if (username.Text == "" )//|| txtUserNumber.Text == "")
                {
                    AlertDialog.Builder aler = new AlertDialog.Builder(this);
                    aler.SetTitle("Sorry");
                    aler.SetMessage("Enter proper details");
                    aler.SetNegativeButton("Ok", delegate { });
                    Dialog dialog = aler.Create();
                    dialog.Show();
                    return;

                }
                else
                {
                    CustomerResponse authen = new CustomerResponse();
                    try
                    {
                        authen = svc.AuthencateUser(username.Text).Result;
                        if (authen.customer != null && authen.customer.CustomerID != 0)
                        {
                            CurrentUser.SaveUserName(username.Text, authen.customer.CustomerID.ToString());
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
             

            };                 

            
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
    }
}