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
using Hangout.Models;

namespace WineHangouts
{ 
    [Activity(Label = "Profile")]
    public class ProfileActivity : Activity
    {
       protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            //ActionBar.NavigationMode = ActionBarNavigationMode.Tabs;
            SetContentView(Resource.Layout.Profile);
            int userId = Convert.ToInt32(CurrentUser.getUserId()); ;
            ServiceWrapper sw = new ServiceWrapper();
            var output = sw.GetCustomerDetails(userId).Result;

            ImageView propicimage = FindViewById<ImageView>(Resource.Id.propicview);
            ///var imageBitmap = GetImageBitmapFromUrl("http://xamarin.com/resources/design/home/devices.png");
            //propicimage.SetImageBitmap(imageBitmap);
            propicimage.SetImageResource(Resource.Drawable.user);
            ImageButton changepropic = FindViewById<ImageButton>(Resource.Id.btnChangePropic);
            changepropic.SetImageResource(Resource.Drawable.dpreplacer);
            changepropic.SetScaleType(ImageView.ScaleType.CenterCrop);
            changepropic.Click += delegate
            {
                Intent intent = new Intent(this, typeof(ProfilePicturePickDialog));
                StartActivity(intent);
            };

            EditText Firstname = FindViewById<EditText>(Resource.Id.txtFirstName);
            Firstname.Text = output.customer.FirstName;
            EditText Lastname = FindViewById<EditText>(Resource.Id.txtLastName);
            Lastname.Text = output.customer.LastName;
            EditText Mobilenumber = FindViewById<EditText>(Resource.Id.txtMobileNumber);
            Mobilenumber.Text = output.customer.PhoneNumber;
            EditText Email = FindViewById<EditText>(Resource.Id.txtEmail);
            Email.Text = output.customer.Email;
            EditText Addres = FindViewById<EditText>(Resource.Id.txtAddress);
            string Addres2 = output.customer.Address2;
            string Addres1 = output.customer.Address1;           
            Addres.Text=string.Concat(Addres1, Addres2);
        }
        
    }
   
}