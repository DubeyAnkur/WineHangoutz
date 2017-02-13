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
            ImageHelper im = new ImageHelper();
            Bitmap imageBitmap = im.GetImageBitmapFromUrl(uri.ToString());
            return imageBitmap;
        }
        public Bitmap ProfileImages(int userid)
        {
            var uri = new Uri(ServiceURL + "profileimages/" + userid + ".jpg");
            ImageHelper im = new ImageHelper();
            Bitmap imageBitmap = im.GetImageBitmapFromUrl(uri.ToString());
            return imageBitmap;
        }


    }
}