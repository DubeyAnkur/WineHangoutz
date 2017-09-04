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
        private static int screenid = 20;
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
        public static Bitmap Bottleimages(string WineBarcode,int storeid)
        {
			Bitmap imageBitmap = null ;
            ProfilePicturePickDialog pppd = new ProfilePicturePickDialog();
            Path = pppd.CreateDirectoryForPictures();
            //string path = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);

            if (wineImages.Contains(WineBarcode))
            {
				
				return (Bitmap)wineImages[WineBarcode];
            }

            var filePath = System.IO.Path.Combine(Path + "/" +WineBarcode);
			if (System.IO.File.Exists(filePath))
			{

				imageBitmap = BitmapFactory.DecodeFile(filePath);
				wineImages.Add( WineBarcode, imageBitmap);

			}
			else if (storeid == 1)
			{


				var uri = new Uri(ServiceURL + "barcodepp/" + WineBarcode );
				LoggingClass.LogInfo(" WineImage is dosent Existing Download"+ WineBarcode + "Wall", screenid);

				imageBitmap = GetImageBitmapFromUrl(uri.ToString());
				wineImages.Add( WineBarcode, imageBitmap);

			}
			else
			{
				var uri = new Uri(ServiceURL + "barcodepp/" + WineBarcode );
				LoggingClass.LogInfo(" WineImage is dosent Existing Download" + WineBarcode + " PP", screenid);

				imageBitmap = GetImageBitmapFromUrl(uri.ToString());
				wineImages.Add(WineBarcode, imageBitmap);
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

            App._dir = new Java.IO.File(Android.OS.Environment.GetExternalStoragePublicDirectory("WineHangouts"), "winehangouts/wineimages");

            if (!App._dir.Exists())
            {
                App._dir.Mkdirs();
            }
            string path = App._dir.ToString();
            //ProfilePicturePickDialog pppd = new ProfilePicturePickDialog();
            //string path = pppd.CreateDirectoryForPictures();
            //string path = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
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
                catch (Exception exe)
                {
                    LoggingClass.LogError(exe.Message, screenid, exe.StackTrace.ToString());
                }
            }

            for (int j = 1; j < storeid; j++)
            {
                var output = sw.GetItemList(j, userid).Result;
                List<Item> x = output.ItemList.ToList();
                int y = x.Count;
                for (int i = 0; i < y; i++)
                {
                    bool ispresent = di.GetFiles(x[i].SmallImageUrl + ".").Any();
					if (!ispresent)
					{
						if (j == 1)
						{
							var uri = new Uri(ServiceURL + "barcodepp/" + x[i].SmallImageUrl);
							Bitmap bm = GetImageBitmapFromUrl(uri.ToString());
							try
							{
								var filePath = System.IO.Path.Combine(path + "/"+x[i].SmallImageUrl);
								var stream = new FileStream(filePath, FileMode.Create);
								bm.Compress(Bitmap.CompressFormat.Webp, 100, stream);
								stream.Close();
							}

							catch (Exception e)
							{
								string Exe = e.ToString();
							}
						}
						else if (j == 2)
						{
							var uri = new Uri(ServiceURL + "barcodepp/" + x[i].SmallImageUrl);
							Bitmap bm = GetImageBitmapFromUrl(uri.ToString());
							try
							{
								var filePath = System.IO.Path.Combine(path + "/"+ x[i].SmallImageUrl);
								var stream = new FileStream(filePath, FileMode.Create);
								bm.Compress(Bitmap.CompressFormat.Webp, 100, stream);
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