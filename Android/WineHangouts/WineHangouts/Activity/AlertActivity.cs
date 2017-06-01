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
    [Activity(Label = "Alert")]
    public class AlertActivity : Activity
    {
		public int screenid = 25;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);          

            // Create your application here
        }
        public void IncorrectDetailsAlert()
        {
            AlertDialog.Builder aler = new AlertDialog.Builder(this);
			LoggingClass.LogInfo("Entered Incorrect Details", screenid);
			aler.SetTitle("Sorry");
            aler.SetMessage("Incorrect Details");
            aler.SetNegativeButton("Ok", delegate { });
            Dialog dialog = aler.Create();
            dialog.Show();
        }
        public void ThankuYouAlert()
        {
            AlertDialog.Builder alert = new AlertDialog.Builder(this);
			LoggingClass.LogInfo("Successfully you logged in", screenid);
			alert.SetTitle("Successfully you logged in");
            alert.SetMessage("Thank You");
            alert.SetNegativeButton("Ok", delegate { });
            Dialog dialog = alert.Create();
            dialog.Show();
        }
        public void IncorrectUserNameAlert()
        {
            AlertDialog.Builder aler = new AlertDialog.Builder(this);
            aler.SetTitle("Sorry");
            aler.SetMessage("Please Enter Correct username");
            aler.SetNegativeButton("Ok", delegate { });
            Dialog dialog = aler.Create();
            dialog.Show();
        }
        public void CheckInternetAlert()
        {
            AlertDialog.Builder aler = new AlertDialog.Builder(this);
            aler.SetTitle("Sorry");
			LoggingClass.LogInfo("Please check your internet connection", screenid);
			aler.SetMessage("Please check your internet connection");
            aler.SetNegativeButton("Ok", delegate { });
            Dialog dialog = aler.Create();
            dialog.Show();
        }
        public void UndermaintenenceAlert()
        {
            AlertDialog.Builder aler = new AlertDialog.Builder(this);
            aler.SetTitle("Sorry");
			LoggingClass.LogInfo("We're under maintanence", screenid);
			aler.SetMessage("We're under maintanence");
            aler.SetNegativeButton("Ok", delegate { });
            Dialog dialog = aler.Create();
            dialog.Show();
        }
    }
}