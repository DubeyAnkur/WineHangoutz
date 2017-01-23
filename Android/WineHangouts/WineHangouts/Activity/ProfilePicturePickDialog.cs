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
        
        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);

            // Make it available in the gallery

            Intent mediaScanIntent = new Intent(Intent.ActionMediaScannerScanFile);
            Uri contentUri = Uri.FromFile(App._file);
            mediaScanIntent.SetData(contentUri);
            SendBroadcast(mediaScanIntent);

            // Display in ImageView. We will resize the bitmap to fit the display
            // Loading the full sized image will consume to much memory 
            // and cause the application to crash.

            int height = Resources.DisplayMetrics.HeightPixels;
            int width = _imageView.Height ;
            App.bitmap = App._file.Path.LoadAndResizeBitmap (width, height);
            if (App.bitmap != null) {
                _imageView.SetImageBitmap (App.bitmap);
                App.bitmap = null;
            }

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

                ImageButton button = FindViewById<ImageButton>(Resource.Id.btnCamera);
                button.SetImageResource(Resource.Drawable.camera);
                button.SetScaleType(ImageView.ScaleType.CenterCrop);
                _imageView = FindViewById<ImageView>(Resource.Id.imageView1);
                button.Click += TakeAPicture;
            }
            ImageButton btnGallery = FindViewById<ImageButton>(Resource.Id.btnGallery);
            btnGallery.SetImageResource(Resource.Drawable.Gallery);
            btnGallery.SetScaleType(ImageView.ScaleType.CenterCrop);
            btnGallery.Click += delegate {
                var imageIntent = new Intent();
                imageIntent.SetType("image/*");
                imageIntent.SetAction(Intent.ActionGetContent);
                StartActivityForResult(
                    Intent.CreateChooser(imageIntent, "Select photo"), 0);
            };
          
        }


        private void CreateDirectoryForPictures()
        {
            App._dir = new File(
                Environment.GetExternalStoragePublicDirectory(
                    Environment.DirectoryPictures), "WineHangoutsdp's");
            if (!App._dir.Exists())
            {
                App._dir.Mkdirs();
            }
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

            App._file = new File(App._dir, String.Format("WineHangouts_profile{0}.jpg", Guid.NewGuid()));

            intent.PutExtra(MediaStore.ExtraOutput, Uri.FromFile(App._file));

            StartActivityForResult(intent, 0);
        }
    }
}
