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

namespace WineHangouts
{
 

    public static class App {
        public static File _file;
        public static File _dir;     
        public static Bitmap bitmap;
       
    }
   
    [Activity(Label = "@string/ApplicationName", MainLauncher = false,Theme = "@android:style/Theme.Dialog")]
    public class ProfilePicturePickDialog : Activity
    {
       
        private ImageView _imageView;
        public string path;
        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);

            // Make it available in the gallery

            Intent mediaScanIntent = new Intent(Intent.ActionMediaScannerScanFile);
            Uri contentUri = Uri.FromFile(App._file);
            mediaScanIntent.SetData(contentUri);
            SendBroadcast(mediaScanIntent);
            Toast.MakeText(this, "Thank you,We will update your profile picture as soon as possible", ToastLength.Short).Show();
            Toast.MakeText(this, "Please touch anywhere to exit this dialog.", ToastLength.Short).Show();
            // Display in ImageView. We will resize the bitmap to fit the display
            // Loading the full sized image will consume to much memory 
            // and cause the application to crash.

            //int height = Resources.DisplayMetrics.HeightPixels;
            //    int width = _imageView.Height ;
            //    App.bitmap = App._file.Path.LoadAndResizeBitmap (width, height);
            //    if (App.bitmap != null) {
            //        _imageView.SetImageBitmap (App.bitmap);
            //        App.bitmap = null;
            //    }
            UploadProfilePic(path);
            // Dispose of the Java side bitmap.
            GC.Collect();
        }

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            Window.RequestFeature(WindowFeatures.NoTitle);
            SetContentView(Resource.Layout.ProfilePickLayout);

            if (IsThereAnAppToTakePictures())
            {
                CreateDirectoryForPictures();

                ImageButton BtnCamera = FindViewById<ImageButton>(Resource.Id.btnCamera);
               
               
               // _imageView = FindViewById<ImageView>(Resource.Id.imageView1);
                BtnCamera.Click += TakeAPicture;
            }
            //ImageButton btnGallery = FindViewById<ImageButton>(Resource.Id.btnGallery);
           
            //btnGallery.Click += delegate {
            //    Intent intent = new Intent(this, typeof(ProfilePictureGallery));
            //    StartActivity(intent);
            //};


        }



        private void CreateDirectoryForPictures()
        {
            App._dir = new File(
                Environment.GetExternalStoragePublicDirectory(
                    Environment.DirectoryPictures), "pics");
            
            if (!App._dir.Exists())
            {
                App._dir.Mkdirs();
            }
            path = App._dir.ToString();
        }

        private bool IsThereAnAppToTakePictures()
        {
            Intent intent = new Intent(MediaStore.ActionImageCapture);
            IList<ResolveInfo> availableActivities = 
                PackageManager.QueryIntentActivities(intent, PackageInfoFlags.MatchDefaultOnly);
            return availableActivities != null && availableActivities.Count > 0;
        }

        private void TakeAPicture(object sender, EventArgs eventArgs)
        {
            Intent intent = new Intent(MediaStore.ActionImageCapture);

            
            App._file = new File(App._dir, String.Format(Convert.ToInt32(CurrentUser.getUserId())+".jpg", Guid.NewGuid()));
           path += "/"+CurrentUser.getUserId()+".jpg";
            


            intent.PutExtra(MediaStore.ExtraOutput, Uri.FromFile(App._file));
            
            StartActivityForResult(intent, 0);
        }

        private async void UploadProfilePic(string path)
        {

            StorageCredentials sc = new StorageCredentials("icsintegration", "+7UyQSwTkIfrL1BvEbw5+GF2Pcqh3Fsmkyj/cEqvMbZlFJ5rBuUgPiRR2yTR75s2Xkw5Hh9scRbIrb68GRCIXA==");
            CloudStorageAccount storageaccount = new CloudStorageAccount(sc, true);
            CloudBlobClient blobClient = storageaccount.CreateCloudBlobClient();
            CloudBlobContainer container = blobClient.GetContainerReference("profileimages");

            await container.CreateIfNotExistsAsync();
            //string[] FileEntries = App.System.IO._dir.GetFiles(path);

            //foreach (string FilePath in FileEntries)
            //{
            //    string key = System.IO.Path.GetFileName(path);//.GetFileName(FilePath);
            CloudBlockBlob blob = container.GetBlockBlobReference(CurrentUser.getUserId() + ".jpg"); //(path);
           
        
            using (var fs = System.IO.File.Open(path, System.IO.FileMode.Open, System.IO.FileAccess.Read, System.IO.FileShare.None))
            {
                
               await blob.UploadFromStreamAsync(fs);//  .UploadFromFileAsync(path);
                
            }
                //}
                // await container=



            }
        }

}
