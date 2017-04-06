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
using Android.Media;
using System.Threading;
using System.Drawing;
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
                    if(imageBitmap==null)
                    {
                        propicimage.SetImageResource(Resource.Drawable.user);
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
                        propicimage.SetImageResource(Resource.Drawable.user);
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
                    Intent intent = new Intent(this, typeof(ProfilePicturePickDialog));
                    StartActivity(intent);
                };

                //Button Btnlogout = FindViewById<Button>(Resource.Id.button1);
                //Btnlogout.Click += delegate
                //{

                //    Intent intent = new Intent(this, typeof(LoginActivity));
                //    StartActivity(intent);
                //};

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
                    Customer customer = new Customer();
                    customer.FirstName = Firstname.Text;
                    customer.LastName = Lastname.Text;
                    customer.PhoneNumber = Mobilenumber.Text;
                    customer.Address1 = Address.Text;
                    customer.Email = Email.Text;
                    customer.CustomerID = userId;
                    customer.State = State.Text;
                    customer.City = City.Text;
                    var x = await sw.UpdateCustomer(customer);
                    if (x == 1)
                    {
                        Toast.MakeText(this, "Thank you your profile is Updated", ToastLength.Short).Show();
                    }
                };
               
            }
            catch (Exception exe)
            {
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

        public Bitmap resizeAndRotate(Bitmap image, int width, int height)
        {
            var matrix = new Matrix();
            var scaleWidth = ((float)width) / image.Width;
            var scaleHeight = ((float)height) / image.Height;
            matrix.PostRotate(90);
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


        //public void  ResizeImage(Image image, int width, int height, int desiredWidth, int desiredHeight)
        //{
        //    //float ratio = ((float)240) / height;
        //    //ratio = ratio / 2;
        //    float nPercent = 0;
        //    float nPercentW = 0;
        //    float nPercentH = 0;

        //    nPercentW = ((float)desiredWidth / (float)width);
        //    nPercentH = ((float)desiredHeight / (float)height);

        //    if (nPercentH < nPercentW)
        //        nPercent = nPercentH;
        //    else
        //        nPercent = nPercentW;
        //    float ratio = nPercent;
        //    var destRect = new System.Drawing.Rectangle(0, 0, Convert.ToInt32(width * ratio), Convert.ToInt32(height * ratio));
        //    var destImage = new System.Drawing.biBitmap(Convert.ToInt32(width * ratio), Convert.ToInt32(height * ratio));

        //    destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

        //    using (var graphics = Graphics.FromImage(destImage))
        //    {
        //        //graphics.CompositingMode = CompositingMode.SourceCopy;
        //        //graphics.CompositingQuality = CompositingQuality.HighSpeed;
        //        //graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
        //        //graphics.SmoothingMode = SmoothingMode.HighQuality;
        //        //graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

        //        using (var wrapMode = new ImageAttributes())
        //        {
        //            wrapMode.SetWrapMode(WrapMode.TileFlipXY);
        //            graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
        //        }
        //    }


        //}
    }

}