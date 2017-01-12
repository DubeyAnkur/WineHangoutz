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

namespace WineHangouts

{
    [Activity(Label = "WineHangouts",MainLauncher =true)]
    public class LoginActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.login);
            Button login = FindViewById<Button>(Resource.Id.btnLoginLL);
            Button cancel = FindViewById<Button>(Resource.Id.btnClearLL);
            EditText username = FindViewById<EditText>(Resource.Id.txtUsername);
            EditText password = FindViewById<EditText>(Resource.Id.txtPassword);

            ISharedPreferences pref = Application.Context.GetSharedPreferences("userInfo", FileCreationMode.Private);
            ISharedPreferencesEditor edit = pref.Edit();
            edit.PutString("user", username.Text);
            edit.PutString("pass", password.Text);
            edit.Apply();

            username.Text = CurrentUser.getUserName();

            //string value1 = pref.GetString("user", null);
            //username.Text = value1;

            //edit.Commit();

            login.Click += delegate {
                CurrentUser.SaveUserName(username.Text, password.Text);

                string cmp = CurrentUser.getPassword();

                if (password.Text == cmp)



                {
                    Intent intent = new Intent(this, typeof(Activity1));
                    StartActivity(intent);
                }
                else
                {
                    Intent intent = new Intent(this, typeof(LoginActivity));
                    StartActivity(intent);

                }


            };
            // Create your application here
        }
    }
}