using System;
using System.Net;
using System.Drawing;
using Foundation;
using UIKit;
using ImageIO;
using System.IO;
using Hangout.Models;
using System.Threading.Tasks;
using CoreGraphics;

namespace WineHangoutz
{
	public static class BlobWrapper
	{
		static NSCache wineBottles;
		static BlobWrapper()
		{
			wineBottles = new NSCache();
		}

		public static UIImage GetResizedImage(string wineId, CGRect bounds)
		{
			UIImage image = BlobWrapper.GetImageBitmapFromWineId(wineId);
			if (image != null)
			{
				nfloat boxHeight = bounds.Height;
				nfloat imgHeight = image.Size.Height;
				nfloat ratio = boxHeight / imgHeight;
				//if (ratio < 1)
				{
					CGSize newSize = new CGSize(image.Size.Width * ratio, image.Size.Height * ratio);
					image = image.Scale(newSize);
				}
				return image;
			}
			else
				return null;
		}
		public static UIImage GetImageBitmapFromWineId(string wineId)
		{
			NSObject btl = wineBottles.ObjectForKey(NSObject.FromObject(wineId));

			if (btl != null)
				return (UIImage)btl;
			
			NSData imageData = ReadPhysicalCache(wineId);
			if (imageData == null)
			{
				string url = "https://icsintegration.blob.core.windows.net/bottleimages/" + wineId + ".jpg";
				NSUrl imageURL = new NSUrl(url);
				imageData = NSData.FromUrl(imageURL);
				CachedImagePhysically(imageData, wineId);
			}
			if (imageData == null)
				return null;

			UIImage img = UIImage.LoadFromData(imageData);
			wineBottles.SetObjectforKey(img, NSObject.FromObject(wineId));

			return img;
		}

		public static void CachedImagePhysically(NSData image, string wineId)
		{
			try
			{
				var cache = Path.Combine("Library/Caches/", "WineHangoutz");
				var filename = Path.Combine(cache, wineId + ".jpg");

				byte[] dataBytes = new byte[image.Length];

				System.Runtime.InteropServices.Marshal.Copy(image.Bytes, dataBytes, 0, Convert.ToInt32(image.Length));
				if (!Directory.Exists(cache))
				{ 
					Directory.CreateDirectory(cache);
				}
				File.WriteAllBytes(filename, dataBytes);
			}
			catch (Exception)
			{
				//ignore the error. Download it next time.
			}
		}

		public static NSData ReadPhysicalCache(string wineId)
		{
			try
			{
				var cache = Path.Combine("Library/Caches/", "WineHangoutz");

				var filename = Path.Combine(cache, wineId + ".jpg");

				byte[] dataBytes = File.ReadAllBytes(filename);
				return NSData.FromArray(dataBytes);

			}
			catch (Exception)
			{
				return null;
			}
		}

		public static void DownloadAllImages()
		{
			Task.Factory.StartNew(() =>
			{
				ServiceWrapper svc = new ServiceWrapper();
				ItemListResponse myData = svc.GetItemList(1, CurrentUser.RetreiveUserId()).Result;

				foreach (var wine in myData.ItemList)
				{
					GetImageBitmapFromWineId(wine.WineId.ToString());
				}

				myData = svc.GetItemList(2, CurrentUser.RetreiveUserId()).Result;

				foreach (var wine in myData.ItemList)
				{
					GetImageBitmapFromWineId(wine.WineId.ToString());
				}

			});
		}
	}
}