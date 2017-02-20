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
            var uri = new Uri(ServiceURL + "bottleimages/"+wineid+".jpg");
            Bitmap imageBitmap = GetImageBitmapFromUrl(uri.ToString());
            return imageBitmap;
        }
        public Bitmap ProfileImages(int userid)
        {
            var uri = new Uri(ServiceURL + "profileimages/" + userid + ".jpg");
               Bitmap imageBitmap = GetImageBitmapFromUrl(uri.ToString());
            return imageBitmap;
        }

        public void Downloads(int userid,int storeid)
        {
            BlobWrapper bvb = new BlobWrapper();
            ServiceWrapper sw = new ServiceWrapper();
            var output = sw.GetItemList(storeid, userid).Result;
            List<Item> x=output.ItemList.ToList();
            var uri = new Uri(ServiceURL + "bottleimages/" + x[1].WineId + ".jpg");
            Bitmap bm = bvb.GetImageBitmapFromUrl("uri");
            ProfilePicturePickDialog pppd = new ProfilePicturePickDialog();
            try
            {
                var sdCardPath = Android.OS.Environment.ExternalStorageDirectory.AbsolutePath;
                var filePath = System.IO.Path.Combine(sdCardPath, x[1].WineId + ".jpg");
                var stream = new FileStream(filePath, FileMode.Create);
                bm.Compress(Bitmap.CompressFormat.Png, 100, stream);
                stream.Close();
            }
            catch (Exception e)
            {
                string Exe=e.ToString();
            }
        }       

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