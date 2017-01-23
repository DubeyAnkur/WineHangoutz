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
using Java.Net;
using Android.Graphics;
using Java.IO;
using Android.Graphics.Drawables;
using Android.Util;
using System.Net;
using System.IO;


namespace WineHangouts
{ 
    [Activity(Label = "ProfileActivity")]
    public class ProfileActivity : Activity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            //ActionBar.NavigationMode = ActionBarNavigationMode.Tabs;
            SetContentView(Resource.Layout.Profile);
            
            ImageView propicimage = FindViewById<ImageView>(Resource.Id.propicview);
            ///var imageBitmap = GetImageBitmapFromUrl("http://xamarin.com/resources/design/home/devices.png");
            //propicimage.SetImageBitmap(imageBitmap);
            propicimage.SetImageResource(Resource.Drawable.user);
            TextView Username = FindViewById<TextView>(Resource.Id.txtUsername);
            ImageButton changepropic = FindViewById<ImageButton>(Resource.Id.btnChangePropic);
            changepropic.SetImageResource(Resource.Drawable.dpreplacer);
            changepropic.SetScaleType(ImageView.ScaleType.CenterCrop);
            changepropic.Click += delegate
            {
                Intent intent = new Intent(this, typeof(ProfilePicturePickDialog));
                StartActivity(intent);
            };
            TextView username = FindViewById<TextView>(Resource.Id.txtUsername);
            String user = "Development Savvyitsol";
            username.Text = user;
            //ActionBar.Tab tab = ActionBar.NewTab();

            //tab.SetIcon(Resource.Drawable.user);
            //tab.TabSelected += (sender, args) =>
            //{
            //    // Do something when tab is selected
            //};
            //ActionBar.AddTab(tab);

            //tab = ActionBar.NewTab();
            ////tab.SetText(Resources.GetString(Resource.String.tab2_text));
            //tab.SetIcon(Resource.Drawable.user);
            //tab.TabSelected += (sender, args) => {
            //    // Do something when tab is selected
            //};
            //ActionBar.AddTab(tab);

            // Create your application here
        }

       
    }
   
}