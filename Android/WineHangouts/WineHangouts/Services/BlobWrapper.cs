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