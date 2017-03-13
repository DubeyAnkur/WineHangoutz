using System;
using System.Collections.Generic;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Graphics;
using Android.OS;
using Android.Provider;
using Android.Widget;
using Java.IO;
using Environment = Android.OS.Environment;
using Uri = Android.Net.Uri;
using Android.Views;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage.Auth;
using System.IO;
using Android.Database;

namespace WineHangouts
{


   

    [Activity(Label = "@string/ApplicationName", MainLauncher = false, Theme = "@android:style/Theme.Dialog")]
    public class ProfilePictureGallery : Activity
    {

        private ImageView _imageView;
        public string path;
        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);

            if (resultCode == Result.Ok)
            {
                //var imageView =
                //    FindViewById<ImageView>(Resource.Id.imageView1);
                //imageView.SetImageURI(data.Data);

                // new FileInfo(path).Directory.FullName
              string Path=GetRealPathFromURI(data.Data);


                Bitmap propic = BitmapFactory.DecodeFile(Path);

                ProfilePicturePickDialog pppd = new ProfilePicturePickDialog();
                string dir_path=pppd.CreateDirectoryForPictures();
                dir_path = dir_path+"/"+Convert.ToInt32(CurrentUser.getUserId()) + ".jpg";
                ProfileActivity pa = new ProfileActivity();
                Bitmap resized = pa.Resize(propic, 450, 450);
                try
                {
                    var filePath = System.IO.Path.Combine(dir_path);
                    var stream = new FileStream(filePath, FileMode.Create);
                    resized.Compress(Bitmap.CompressFormat.Jpeg, 100, stream);
                    stream.Close();
                    Toast.MakeText(this, "Thank you,We will update your profile picture as soon as possible", ToastLength.Short).Show();
                    Toast.MakeText(this, "Please touch anywhere to exit this dialog.", ToastLength.Short).Show();
                    pppd.UploadProfilePic(filePath);
                }
                catch (Exception ex)
                {

                }


            }


        }

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            Window.RequestFeature(WindowFeatures.NoTitle);
            SetContentView(Resource.Layout.ProfilePickLayout);

            var imageIntent = new Intent();
            imageIntent.SetType("image/*");
            imageIntent.SetAction(Intent.ActionGetContent);
            StartActivityForResult(
                Intent.CreateChooser(imageIntent, "Select photo"), 0);
         


        }
        public string GetRealPathFromURI(Uri contentURI)
        {
            ICursor cursor = ContentResolver.Query(contentURI, null, null, null, null);
            cursor.MoveToFirst();
            string documentId = cursor.GetString(0);
            documentId = documentId.Split(':')[1];
            cursor.Close();

            cursor = ContentResolver.Query(
            Android.Provider.MediaStore.Images.Media.ExternalContentUri,
            null, MediaStore.Images.Media.InterfaceConsts.Id + " = ? ", new[] { documentId }, null);
            cursor.MoveToFirst();
            string path = cursor.GetString(cursor.GetColumnIndex(MediaStore.Images.Media.InterfaceConsts.Data));
            cursor.Close();

            return path;
        }

        


    }
   


}
