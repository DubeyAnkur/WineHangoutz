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
using Hangout.Models;
using System.Net;
using System.Threading.Tasks;
using System.IO;
using Android.Graphics;

namespace WineHangouts
{
    [Activity(Label = "Testing App", MainLauncher =true, Icon = "@drawable/icon")]
    public class TestingActivity : Activity
    {
        Button downloadButton;
        WebClient webClient;
        ImageView imageView;
        LinearLayout progressLayout;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Dummy);

            Button btnGallery = FindViewById<Button>(Resource.Id.btnTest);

            btnGallery.Click += delegate
            {
                Notification.Builder builder = new Notification.Builder(this)
                .SetContentTitle("hi Notification")
                .SetContentText("https://developer.xamarin.com/guides/android/application_fundamentals/notifications/remote-notifications-with-gcm/")
                .SetSmallIcon(Resource.Drawable.user1);
                Notification notification = builder.Build();
                NotificationManager notificationManager =
                GetSystemService(Context.NotificationService) as NotificationManager;
                const int notificationId = 0;
                notificationManager.Notify(notificationId, notification);
                //Intent intent = new Intent(this, typeof(ProfilePictureGallery));
                //StartActivity(intent);
            };



            ////AsyncDownload asn = new AsyncDownload();
            //ImageView imageView = FindViewById<ImageView>(Resource.Id.imageView1);
            //LinearLayout progressLayout = FindViewById<LinearLayout>(Resource.Id.progressLayout);
            //progressLayout.Visibility = ViewStates.Gone;
            //Button downloadButton = FindViewById<Button>(Resource.Id.downloadButton);
            //downloadButton.Click += downloadAsync;

            //async void downloadAsync(object sender, System.EventArgs ea)
            //{
            //    webClient = new WebClient();
            //    var url = new Uri("https://icsintegration.blob.core.windows.net/bottleimagesdetails/198.jpg");
            //    byte[] imageBytes = null;
            //    progressLayout.Visibility = ViewStates.Visible;
            //    try
            //    {
            //        imageBytes = await webClient.DownloadDataTaskAsync(url);
            //    }
            //    catch (TaskCanceledException)
            //    {
            //        this.progressLayout.Visibility = ViewStates.Gone;
            //        return;
            //    }
            //    catch (Exception exe)
            //    {
            //        progressLayout.Visibility = ViewStates.Gone;
            //        downloadButton.Click += downloadAsync;
            //        downloadButton.Text = "Download Image";
            //        return;
            //    }

            //    try
            //    {
            //        string documentsPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            //        string localFilename = "Wine.png";
            //        string localPath = System.IO.Path.Combine(documentsPath, localFilename);

            //        FileStream fs = new FileStream(localPath, FileMode.OpenOrCreate);
            //        await fs.WriteAsync(imageBytes, 0, imageBytes.Length);
            //        Console.WriteLine("Saving image in local path: " + localPath);
            //        fs.Close();

            //        BitmapFactory.Options options = new BitmapFactory.Options();
            //        options.InJustDecodeBounds = true;
            //        await BitmapFactory.DecodeFileAsync(localPath, options);


            //    //options.InSampleSize = options.OutWidth > options.OutHeight ? options.OutHeight / imageView.Height : options.OutWidth / imageView.Width;
            //    //options.InJustDecodeBounds = false;

            //    Bitmap bitmap = await BitmapFactory.DecodeFileAsync(localPath);
            //    imageView.SetImageBitmap(bitmap);
            //    }
            //    catch (Exception e)
            //    {


            //    }

            //    progressLayout.Visibility = ViewStates.Gone;
            //    downloadButton.Click += downloadAsync;
            //    downloadButton.Text = "Download Image";
            //}

            //    public static async Task<bool> SaveCache(Stream data, string id)
            //{
            //    try
            //    {
            //        //cache folder in local storage
            //        IFolder rootFolder = FileSystem.Current.LocalStorage;
            //        var folder = await rootFolder.CreateFolderAsync("Cache",
            //            CreationCollisionOption.OpenIfExists);
            //        //save cached data
            //        IFile file = await folder.CreateFileAsync(id, CreationCollisionOption.ReplaceExisting);
            //        byte[] buffer = new byte[data.Length];
            //        data.Read(buffer, 0, buffer.Length);
            //        using (Stream stream = await file.OpenAsync(FileAccess.ReadAndWrite))
            //        {
            //            stream.Write(buffer, 0, buffer.Length);
            //        }
            //        return true;
            //    }
            //    catch
            //    {
            //        //some logging
            //        return false;
            //    }
            //}

            //public static async Task<Stream> LoadCache(string id)
            //{
            //    //cache folder in local storage
            //    IFolder rootFolder = FileSystem.Current.LocalStorage;
            //    var folder = await rootFolder.CreateFolderAsync("Cache",
            //        CreationCollisionOption.OpenIfExists);

            //    var isExists = await folder.CheckExistsAsync(id);

            //    if (isExists == ExistenceCheckResult.FileExists)
            //    {
            //        //file exists - load it from cache
            //        IFile file = await folder.GetFileAsync(id);
            //        return await file.OpenAsync(FileAccess.Read);
            //    }
            //}
            //    return null;
            //}
        }
    }
}