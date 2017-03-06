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

namespace WineHangouts

{
    [Activity(Label = "@string/ApplicationName", MainLauncher =true)]
    public class LoginActivity : Activity

    {
        
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            
            SetContentView(Resource.Layout.login);
           
            Button login = FindViewById<Button>(Resource.Id.btnLoginLL);
            
            EditText username = FindViewById<EditText>(Resource.Id.txtUsername);
            
            ServiceWrapper svc = new ServiceWrapper();
            BlobWrapper bvb = new BlobWrapper();
            //new Thread(new ThreadStart(delegate
            //{
            //    RunOnUiThread(() => bvb.DownloadImages(Convert.ToInt32(CurrentUser.getUserId())));
            //})).Start();

            //bvb.DownloadImages(Convert.ToInt32(CurrentUser.getUserId()));
            var TaskA = new Task(() => {
                bvb.DownloadImages(Convert.ToInt32(CurrentUser.getUserId()));
            });
            TaskA.Start();


            if (CurrentUser.getUserName() == null || CurrentUser.getUserName() == "")
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
                if(username.Text=="")
                {
                    AlertDialog.Builder aler = new AlertDialog.Builder(this);
                    aler.SetTitle("Sorry");
                    aler.SetMessage("Enter user name");
                    aler.SetNegativeButton("Ok", delegate { });
                    Dialog dialog = aler.Create();
                    dialog.Show();
                    return;

                }
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
                        aler.SetMessage("Incorrect Details");
                        aler.SetNegativeButton("Ok", delegate { });
                        Dialog dialog = aler.Create();
                        dialog.Show();
                    };
                }
                catch(Exception exception)
                {
                    AlertDialog.Builder aler = new AlertDialog.Builder(this);
                    aler.SetTitle("Sorry");
                    aler.SetMessage("We're under maintainence");
                    aler.SetNegativeButton("Ok", delegate { });
                    Dialog dialog = aler.Create();
                    dialog.Show();
                    
                }
             

            };                 

            
        }
    }
}