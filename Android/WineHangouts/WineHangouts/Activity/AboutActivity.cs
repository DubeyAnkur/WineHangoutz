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
using AppseeAnalytics.Android;

namespace WineHangouts
{
    [Activity(Label = "About Us")]
    public class AboutActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.AboutLayout);
            Appsee.StartScreen("About");
            // Create your application here
        }
    }
}