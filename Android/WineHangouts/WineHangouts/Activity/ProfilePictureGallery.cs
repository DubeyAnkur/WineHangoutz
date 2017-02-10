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
                var imageView =
                    FindViewById<ImageView>(Resource.Id.imageView1);
                imageView.SetImageURI(data.Data);
                

                
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
