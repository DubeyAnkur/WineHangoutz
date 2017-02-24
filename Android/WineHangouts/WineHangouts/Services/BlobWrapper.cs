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

namespace WineHangouts
{
    public class BlobWrapper
    {
        HttpClient client;
        public BlobWrapper()
        {
            client = new HttpClient();
            //client.MaxResponseContentBufferSize = 256000;
        }

        public string ServiceURL
        {
            get
            {

                string host = "https://icsintegration.blob.core.windows.net/";
                return host;

            }

        }
        public Bitmap Bottleimages(int wineid)
        {
            var uri = new Uri(ServiceURL + "bottleimages/" + wineid + ".jpg");
            Bitmap imageBitmap = GetImageBitmapFromUrl(uri.ToString());
            return imageBitmap;
        }
        public Bitmap ProfileImages(int userid)
        {
            var uri = new Uri(ServiceURL + "profileimages/" + userid + ".jpg");
            Bitmap imageBitmap = GetImageBitmapFromUrl(uri.ToString());
            return imageBitmap;
        }

        public void DownloadImages(int userid)
        {
            BlobWrapper bvb = new BlobWrapper();
            ServiceWrapper sw = new ServiceWrapper();
            ProfilePicturePickDialog pppd = new ProfilePicturePickDialog();

            string path = pppd.CreateDirectoryForPictures();
            int storeid = 3;
            DirectoryInfo di = new DirectoryInfo(path);

            bool isthere = di.GetFiles(userid + ".jpg").Any();
            if (!isthere)
            {
                var uri = new Uri(ServiceURL + "profileimages/" + userid + ".jpg");
                Bitmap bm = bvb.GetImageBitmapFromUrl(uri.ToString());
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
                        Bitmap bm = bvb.GetImageBitmapFromUrl(uri.ToString());
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
        public Bitmap GetImageBitmapFromUrl(string url)
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