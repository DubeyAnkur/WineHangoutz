using System;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;
using Android.Graphics;
using Android.Util;
using Hangout.Models;
//using System.Drawing.Drawing2D;

namespace WineHangouts
{
    [Activity(Label = "My Profile")]
    public class ProfileActivity : Activity, IPopupParent
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.Profile);
            try
            {
                LoggingClass.LogInfo("Entered into Profile");
                ActionBar.SetHomeButtonEnabled(true);
                ActionBar.SetDisplayHomeAsUpEnabled(true);
                int userId = Convert.ToInt32(CurrentUser.getUserId());
                ServiceWrapper sw = new ServiceWrapper();
                var output = sw.GetCustomerDetails(userId).Result;
                ImageView propicimage = FindViewById<ImageView>(Resource.Id.propicview);
                ProfilePicturePickDialog pppd = new ProfilePicturePickDialog();
                string path = pppd.CreateDirectoryForPictures();
                //string path = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);

                var filePath = System.IO.Path.Combine(path + "/" + userId + ".jpg");
                if (System.IO.File.Exists(filePath))
                {

                    Bitmap imageBitmap = BitmapFactory.DecodeFile(filePath);
                    if (imageBitmap == null)
                    {
                        propicimage.SetImageResource(Resource.Drawable.user1);
                        //propicimage.SetImageBitmap(imageBitmap);
                    }
                    else
                    {
                        propicimage.SetImageBitmap(imageBitmap);
                    }
                }
                else
                {
                    Bitmap imageBitmap = BlobWrapper.ProfileImages(userId);
                    if (imageBitmap == null)
                    {
                        propicimage.SetImageResource(Resource.Drawable.user1);
                    }
                    else
                    {
                        propicimage.SetImageBitmap(imageBitmap);
                    }
                }

                ImageButton changepropic = FindViewById<ImageButton>(Resource.Id.btnChangePropic);

                //changepropic.SetImageResource(Resource.Drawable.dpreplacer);
                //changepropic.SetScaleType(ImageView.ScaleType.CenterCrop);
                changepropic.Click += delegate
                {
                    LoggingClass.LogInfo("Clicked on change profile activity");
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
                Address.Text = string.Concat(Addres1, Addres2);
                EditText City = FindViewById<EditText>(Resource.Id.txtCity);
                City.Text = output.customer.City;
                EditText State = FindViewById<EditText>(Resource.Id.txtState);
                State.Text = output.customer.State;

                Button updatebtn = FindViewById<Button>(Resource.Id.UpdateButton);

                //updatebtn.SetScaleType(ImageView.ScaleType.CenterCrop);
                updatebtn.Click += async delegate
                {
                    Customer customer = new Customer()
                    {
                        FirstName = Firstname.Text,
                        LastName = Lastname.Text,
                        PhoneNumber = Mobilenumber.Text,
                        Address1 = Address.Text,
                        Email = Email.Text,
                        CustomerID = userId,
                        State = State.Text,
                        City = City.Text
                    };
                    LoggingClass.LogInfo("Clicked on update info");
                    var x = await sw.UpdateCustomer(customer);
                    if (x == 1)
                    {
                        Toast.MakeText(this, "Thank you your profile is Updated", ToastLength.Short).Show();
                    }
                };


            }
            catch (Exception exe)
            {
                LoggingClass.LogError(exe.Message + "In profile activity");
                AlertDialog.Builder aler = new AlertDialog.Builder(this);
                aler.SetTitle("Sorry");
                aler.SetMessage("We're under maintainence");
                aler.SetNegativeButton("Ok", delegate { });
                Dialog dialog = aler.Create();
                dialog.Show();
            }
            ProgressIndicator.Hide();
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

        public void RefreshParent()
        {
            ServiceWrapper svc = new ServiceWrapper();
            int userId = Convert.ToInt32(CurrentUser.getUserId());
            var output = svc.GetCustomerDetails(userId).Result;
            Android.Graphics.Bitmap imageBitmap = BlobWrapper.ProfileImages(userId);
        }

        public Bitmap ResizeAndRotate(Bitmap image, int width, int height)
        {
            var matrix = new Matrix();
            var scaleWidth = ((float)width) / image.Width;
            var scaleHeight = ((float)height) / image.Height;
            matrix.PostRotate(270);
            matrix.PreScale(scaleWidth, scaleHeight);
            return Bitmap.CreateBitmap(image, 0, 0, image.Width, image.Height, matrix, true);
        }
        public Bitmap Resize(Bitmap image, int width, int height)
        {
            var matrix = new Matrix();
            var scaleWidth = ((float)width) / image.Width;
            var scaleHeight = ((float)height) / image.Height;
            matrix.PreScale(scaleWidth, scaleHeight);
            return Bitmap.CreateBitmap(image, 0, 0, image.Width, image.Height, matrix, true);
        }

    }

}