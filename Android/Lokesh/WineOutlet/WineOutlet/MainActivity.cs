using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Webkit;
using Android.OS;

namespace WineOutlet
{
    [Activity(Label = "WineOutlet", MainLauncher = true, Icon = "@drawable/logo")]
    public class MainActivity : Activity
    {
        //int count = 1;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);
            WebView localWebView = FindViewById<WebView>(Resource.Id.LocalWebView1);
            localWebView.SetWebViewClient(new WebViewClient()); // stops request going to Web Browser
            localWebView.LoadUrl("http://www.wineoutlet.com");
            localWebView.Settings.JavaScriptEnabled = true;
            localWebView.Settings.BuiltInZoomControls = true;
            localWebView.Settings.SetSupportZoom(true);
            localWebView.ScrollBarStyle = ScrollbarStyles.OutsideOverlay;
            localWebView.ScrollBarStyle = ScrollbarStyles.OutsideOverlay;

        }
    }
}

