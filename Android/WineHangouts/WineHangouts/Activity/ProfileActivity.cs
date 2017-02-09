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
            
            SetContentView(Resource.Layout.Profile);
            ActionBar.SetHomeButtonEnabled(true);
            ActionBar.SetDisplayHomeAsUpEnabled(true);
            int userId = Convert.ToInt32(CurrentUser.getUserId()); ;
            ServiceWrapper sw = new ServiceWrapper();
            var output = sw.GetCustomerDetails(userId).Result;

            ImageView propicimage = FindViewById<ImageView>(Resource.Id.propicview);
            var imageBitmap = GetImageBitmapFromUrl("https://icsintegration.blob.core.windows.net/profileimages/"+ userId+".jpg");
            propicimage.SetImageBitmap(imageBitmap);
            //propicimage.SetImageResource(Resource.Drawable.user);
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
            string phno1 = output.customer.PhoneNumber;
            string phno2 = output.customer.Phone2;
            if (phno1 != null)
            {
                Mobilenumber.Text = phno1;
            }
            else
            Mobilenumber.Text = phno2;
            EditText Email = FindViewById<EditText>(Resource.Id.txtEmail);
            Email.Text = output.customer.Email;
            EditText Address = FindViewById<EditText>(Resource.Id.txtAddress);
            string Addres2 = output.customer.Address2;
            string Addres1 = output.customer.Address1;           
            Address.Text=string.Concat(Addres1, Addres2);
            EditText City = FindViewById<EditText>(Resource.Id.txtCity);
            City.Text = output.customer.City;
            EditText State = FindViewById<EditText>(Resource.Id.txtState);
            State.Text = output.customer.State;
          
            Button updatebtn = FindViewById<Button>(Resource.Id.UpdateButton);
         
            //updatebtn.SetScaleType(ImageView.ScaleType.CenterCrop);
            updatebtn.Click += async delegate
            {
                Customer customer = new Customer();
                customer.FirstName = Firstname.Text;
                customer.LastName = Lastname.Text;
                customer.PhoneNumber = Mobilenumber.Text;
                customer.Address1 = Address.Text;
                customer.Email = Email.Text;
                customer.CustomerID = userId;
                customer.State = State.Text;
                customer.City = City.Text;
                var x=await sw.UpdateCustomer(customer);
                if (x == 1)
                {
                    Toast.MakeText(this, "Thank you your profile is Updated", ToastLength.Short).Show();
                }
            };
        }

        private Bitmap GetImageBitmapFromUrl(string v)
        {
            Bitmap imageBitmap = null;

            using (var webClient = new WebClient())
            {
                var imageBytes = webClient.DownloadData(v);
                if (imageBytes != null && imageBytes.Length > 0)
                {
                    imageBitmap = BitmapFactory.DecodeByteArray(imageBytes, 0, imageBytes.Length);
                }
            }

            return imageBitmap;
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            if (item.ItemId == Android.Resource.Id.Home)
            {
                base.OnBackPressed();
                return false;
            }
            return base.OnOptionsItemSelected(item);
        }

    }
   
}