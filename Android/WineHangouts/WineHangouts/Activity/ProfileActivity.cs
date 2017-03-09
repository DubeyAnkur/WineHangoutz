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

namespace WineHangouts
{
    [Activity(Label = "Wine Hangouts User Profile")]
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
                string path = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);

                var filePath = System.IO.Path.Combine(path + "/" + userId + ".jpg");
                if (System.IO.File.Exists(filePath))
                {
                    Bitmap imageBitmap = BitmapFactory.DecodeFile(filePath);
                    propicimage.SetImageBitmap(imageBitmap);
                }
                else
                {
                    Bitmap imageBitmap = BlobWrapper.ProfileImages(userId);
                    propicimage.SetImageBitmap(imageBitmap);
                }

                ImageButton changepropic = FindViewById<ImageButton>(Resource.Id.btnChangePropic);

                changepropic.SetImageResource(Resource.Drawable.dpreplacer);
                changepropic.SetScaleType(ImageView.ScaleType.CenterCrop);
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

            Bitmap imageBitmap = BlobWrapper.ProfileImages(userId);
        }
        //public void ResizeImage(string sourceFile, string targetFile, float maxWidth, float maxHeight)
        //{
        //    if (!Java.IO.Exists(targetFile) && File.Exists(sourceFile))
        //    {
        //        // First decode with inJustDecodeBounds=true to check dimensions
        //        var options = new BitmapFactory.Options()
        //        {
        //            InJustDecodeBounds = false,
        //            InPurgeable = true,
        //        };

        //        using (var image = BitmapFactory.DecodeFile(sourceFile, options))
        //        {
        //            if (image != null)
        //            {
        //                var sourceSize = new Size((int)image.GetBitmapInfo().Height, (int)image.GetBitmapInfo().Width);

        //                var maxResizeFactor = Math.Min(maxWidth / sourceSize.Width, maxHeight / sourceSize.Height);

        //                string targetDir = System.IO.Path.GetDirectoryName(targetFile);
        //                if (!Directory.Exists(targetDir))
        //                    Directory.CreateDirectory(targetDir);

        //                if (maxResizeFactor > 0.9)
        //                {
        //                    File.Copy(sourceFile, targetFile);
        //                }
        //                else
        //                {
        //                    var width = (int)(maxResizeFactor * sourceSize.Width);
        //                    var height = (int)(maxResizeFactor * sourceSize.Height);

        //                    using (var bitmapScaled = Bitmap.CreateScaledBitmap(image, height, width, true))
        //                    {
        //                        using (Stream outStream = File.Create(targetFile))
        //                        {
        //                            if (targetFile.ToLower().EndsWith("png"))
        //                                bitmapScaled.Compress(Bitmap.CompressFormat.Png, 100, outStream);
        //                            else
        //                                bitmapScaled.Compress(Bitmap.CompressFormat.Jpeg, 95, outStream);
        //                        }
        //                        bitmapScaled.Recycle();
        //                    }
        //                }

        //                image.Recycle();
        //            }
        //            else
        //                Log.E("Image scaling failed: " + sourceFile);
        //        }
        //    }
        //}
    }

}