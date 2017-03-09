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
using System.Net.Http;
using Android.Graphics;
using System.Net;
using System.IO;
using Hangout.Models;
using System.Collections;

namespace WineHangouts
{
    public static class BlobWrapper
    {
        static HttpClient client;
        static Hashtable wineImages;
        static string Path;
        static BlobWrapper()
        {
            client = new HttpClient();
            client.MaxResponseContentBufferSize = 256000;
            wineImages = new Hashtable();
           
        }

        public static string ServiceURL
        {
            get
            {
                string host = "https://icsintegration.blob.core.windows.net/";
                return host;
            }

        }
        public static Bitmap Bottleimages(int wineid)
        {
            Bitmap imageBitmap;
            ProfilePicturePickDialog pppd = new ProfilePicturePickDialog();
            Path = pppd.CreateDirectoryForPictures();
            //string path = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);

            if (wineImages.Contains(wineid))
            {
                return (Bitmap)wineImages[wineid];
            }

            var filePath = System.IO.Path.Combine(Path + "/" + wineid + ".jpg");
            if (System.IO.File.Exists(filePath))
            {
                imageBitmap = BitmapFactory.DecodeFile(filePath);
                wineImages.Add(wineid, imageBitmap);
            }
            else
            {
                var uri = new Uri(ServiceURL + "bottleimages/" + wineid + ".jpg");
                imageBitmap = GetImageBitmapFromUrl(uri.ToString());
                wineImages.Add(wineid, imageBitmap);
            }
            return imageBitmap;
        }
        public static Bitmap ProfileImages(int userid)
        {
            var uri = new Uri(ServiceURL + "profileimages/" + userid + ".jpg");
            Bitmap imageBitmap = GetImageBitmapFromUrl(uri.ToString());
            return imageBitmap;
        }
        public static void DownloadImages(int userid)
        {
            ServiceWrapper sw = new ServiceWrapper();
            //    ProfilePicturePickDialog pppd = new ProfilePicturePickDialog();

            //App._dir = new Java.IO.File(Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryPictures), "winehangouts/wineimages");

            //if (!App._dir.Exists())
            //{
            //    App._dir.Mkdirs();
            //}
            //string path = App._dir.ToString();

            //string path = pppd.CreateDirectoryForPictures();
            string path = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            int storeid = 3;
            DirectoryInfo di = new DirectoryInfo(path);

            bool isthere = di.GetFiles(userid + ".jpg").Any();
            if (!isthere)
            {
                var uri = new Uri(ServiceURL + "profileimages/" + userid + ".jpg");
                Bitmap bm = GetImageBitmapFromUrl(uri.ToString());
                try
                {
                    var filePath = System.IO.Path.Combine(path + "/" + userid + ".jpg");
                    var stream = new FileStream(filePath, FileMode.Create);
                    bm.Compress(Bitmap.CompressFormat.Jpeg, 100, stream);
                    stream.Close();
                }
                catch (Exception e)
                {
                    string Exe = e.ToString();
                }
            }

            for (int j = 1; j < storeid; j++)
            {
                var output = sw.GetItemList(j, userid).Result;
                List<Item> x = output.ItemList.ToList();
                int y = x.Count;
                for (int i = 0; i < y; i++)
                {
                    bool ispresent = di.GetFiles(x[i].WineId + ".jpg").Any();
                    if (!ispresent)
                    {
                        var uri = new Uri(ServiceURL + "bottleimages/" + x[i].WineId + ".jpg");
                        Bitmap bm = GetImageBitmapFromUrl(uri.ToString());
                        try
                        {
                            var filePath = System.IO.Path.Combine(path + "/" + x[i].WineId + ".jpg");
                            var stream = new FileStream(filePath, FileMode.Create);
                            bm.Compress(Bitmap.CompressFormat.Jpeg, 100, stream);
                            stream.Close();
                        }
                        catch (Exception e)
                        {
                            string Exe = e.ToString();
                        }
                    }

                }
            }
        }

        //public void DownloadProfileImages(int userid)
        //{
        //    BlobWrapper bvb = new BlobWrapper();
        //    ServiceWrapper sw = new ServiceWrapper();
        //    ProfilePicturePickDialog pppd = new ProfilePicturePickDialog();
        //    string path = pppd.CreateDirectoryForPictures();
        //    DirectoryInfo di = new DirectoryInfo(path);
        //    bool ispresent = di.GetFiles(userid + ".jpg").Any();
        //    if (!ispresent)
        //    {
        //        var uri = new Uri(ServiceURL + "profileimages/" +userid+ ".jpg");
        //        Bitmap bm = bvb.GetImageBitmapFromUrl(uri.ToString());
        //        try
        //        {
        //            var filePath = System.IO.Path.Combine(path + "/" +userid + ".jpg");
        //            var stream = new FileStream(filePath, FileMode.Create);
        //            bm.Compress(Bitmap.CompressFormat.Jpeg, 100, stream);
        //            stream.Close();
        //        }
        //        catch (Exception e)
        //        {
        //            string Exe = e.ToString();
        //        }
        //    }

        //}
        //async void downloadAsync(object sender, System.EventArgs ea) {
        //}






        public static Bitmap GetImageBitmapFromUrl(string url)
        {
            Bitmap imageBitmap = null;
            try
            {

                using (var webClient = new WebClient())
                {
                    var imageBytes = webClient.DownloadData(url);
                    if (imageBytes != null && imageBytes.Length > 0)
                    {
                        imageBitmap = BitmapFactory.DecodeByteArray(imageBytes, 0, imageBytes.Length);

                    }
                }

            }
            catch (Exception)
            {
                return null;
            }

            return imageBitmap;
        }


    }
}