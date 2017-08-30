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
using Android.Util;
using System.Diagnostics;

namespace WineHangouts
{
	
    public static class App
    {
        public static Java.IO.File _file;
        public static Java.IO.File _dir;
        public static Bitmap bitmap;

    }

    [Activity(Label = "@string/ApplicationName", MainLauncher = false, Theme = "@android:style/Theme.Dialog", ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    public class ProfilePicturePickDialog : Activity
    {
		
        //private ImageView _imageView;
        public string path;
        private int screenid = 14;
        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
			
            base.OnActivityResult(requestCode, resultCode, data);
            if (resultCode.ToString() == "Canceled")
            {
                LoggingClass.LogInfo("Cancelled from camera",screenid);
                Intent intent = new Intent(this, typeof(ProfileActivity));
                StartActivity(intent);
            }
            else
            {

                // Make it available in the gallery
                try
                {
                    Intent mediaScanIntent = new Intent(Intent.ActionMediaScannerScanFile);
                    Uri contentUri = Uri.FromFile(App._file);
                    mediaScanIntent.SetData(contentUri);
                    SendBroadcast(mediaScanIntent);

                }
                catch (Exception exe)
                {
                    LoggingClass.LogError(exe.Message, screenid, exe.StackTrace.ToString());
                    Intent intent1 = new Intent(this, typeof(ProfileActivity));
                    StartActivity(intent1);
                }
                Resize();
                UploadProfilePic(path);
                Intent intent = new Intent(this, typeof(ProfileActivity));
                StartActivity(intent);
                GC.Collect();
				//LoggingClass.LogTime("profile  piccture ",st.Elapsed.TotalSeconds.ToString());
            }
        }

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            Window.RequestFeature(WindowFeatures.NoTitle);
            SetContentView(Resource.Layout.ProfilePickLayout);
			LoggingClass.LogInfo("Entered into ProfilePicturePickDialog", screenid);
            if (IsThereAnAppToTakePictures())
            {
                CreateDirectoryForPictures();
                ImageButton BtnCamera = FindViewById<ImageButton>(Resource.Id.btnCamera);
				LoggingClass.LogInfo("clicked on camera", screenid);
				// _imageView = FindViewById<ImageView>(Resource.Id.imageView1);
				BtnCamera.Click += TakeAPicture;
            }
            ImageButton btnGallery = FindViewById<ImageButton>(Resource.Id.imgbtnGallery);
            btnGallery.Click += delegate
            {
                LoggingClass.LogInfo("Clicked on gallery picking ",screenid);
                Intent intent = new Intent(this, typeof(ProfilePictureGallery));
                StartActivity(intent);
            };
            

        }



        public string CreateDirectoryForPictures()
        {
            App._dir = new Java.IO.File(Environment.GetExternalStoragePublicDirectory("WineHangouts"), "winehangouts/wineimages");

            if (!App._dir.Exists())
            {
                App._dir.Mkdirs();
            }
            path = App._dir.ToString();

            String NOMEDIA = ".nomedia";
            App._file = new Java.IO.File(path, NOMEDIA);
            if (!App._file.Exists())
            {
                App._file.CreateNewFile();
            }

            return path;
        }

        private bool IsThereAnAppToTakePictures()
        {
            Intent intent = new Intent(MediaStore.ActionImageCapture);
            IList<ResolveInfo> availableActivities =
                PackageManager.QueryIntentActivities(intent, PackageInfoFlags.MatchDefaultOnly);
            return availableActivities != null && availableActivities.Count > 0;
        }

        public void Resize()
        {
            try
            {
                Bitmap propic = BitmapFactory.DecodeFile(path);
                ProfileActivity pa = new ProfileActivity();
                Bitmap resized = pa.ResizeAndRotate(propic, 400, 400);

                var filePath = System.IO.Path.Combine(path);
                var stream = new FileStream(filePath, FileMode.Create);
                resized.Compress(Bitmap.CompressFormat.Jpeg, 100, stream);
                stream.Close();
                propic.Dispose();
                resized.Dispose();
            }
            catch (Exception exe)
            {
                LoggingClass.LogError(exe.Message, screenid, exe.StackTrace.ToString());
            }

        }

        private void TakeAPicture(object sender, EventArgs eventArgs)
        {
			
            Intent intent = new Intent(MediaStore.ActionImageCapture);
            App._file = new Java.IO.File(App._dir, String.Format(Convert.ToInt32(CurrentUser.getUserId()) + ".jpg", Guid.NewGuid()));
            path += "/" + CurrentUser.getUserId() + ".jpg";
            intent.PutExtra(MediaStore.ExtraOutput, Uri.FromFile(App._file));
            StartActivityForResult(intent, 0);
		
			LoggingClass.LogTime("Tak pic","time");
        }

        public async void UploadProfilePic(string path)
        {
			
            StorageCredentials sc = new StorageCredentials("icsintegration", "+7UyQSwTkIfrL1BvEbw5+GF2Pcqh3Fsmkyj/cEqvMbZlFJ5rBuUgPiRR2yTR75s2Xkw5Hh9scRbIrb68GRCIXA==");
            CloudStorageAccount storageaccount = new CloudStorageAccount(sc, true);
            CloudBlobClient blobClient = storageaccount.CreateCloudBlobClient();
            CloudBlobContainer container = blobClient.GetContainerReference("profileimages");
            await container.CreateIfNotExistsAsync();
            CloudBlockBlob blob = container.GetBlockBlobReference(CurrentUser.getUserId() + ".jpg"); 
            LoggingClass.LogInfo("Updated profile picture",screenid);
            using (var fs = System.IO.File.Open(path, System.IO.FileMode.Open, System.IO.FileAccess.Read, System.IO.FileShare.None))
            {
                await blob.UploadFromStreamAsync(fs);
                LoggingClass.LogInfo("Profile picture uploaded into blob",screenid);
            }
			//st.Stop();
			//LoggingClass.LogTime("Upload profile pic ", st.Elapsed.TotalSeconds.ToString());
        }
    }

}
