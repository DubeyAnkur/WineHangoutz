using System;
using System.Net;
using System.Drawing;
using Foundation;
using UIKit;
using ImageIO;
using System.IO;

namespace WineHangoutz
{
	public static class BlobWrapper
	{
		static NSCache wineBottles;
		static BlobWrapper()
		{
			wineBottles = new NSCache();
		}

		public static UIImage GetImageBitmapFromWineId(string wineId)
		{
			NSObject btl = wineBottles.ObjectForKey(NSObject.FromObject(wineId));

			if (btl != null)
				return (UIImage)btl;
			string url = "https://icsintegration.blob.core.windows.net/bottleimages/" + wineId + ".jpg";
			NSUrl imageURL = new NSUrl(url);
			NSData imageData = NSData.FromUrl(imageURL);

			if (imageData == null)
				return null;

			UIImage img = UIImage.LoadFromData(imageData);
			wineBottles.SetObjectforKey(img, NSObject.FromObject(wineId));

			return img;
		}



		public static void CachedImagePhysically(NSData image, string wineId)
		{
			var documents = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
			var cache = Path.Combine(documents, "..", "Library", "Caches", "WineHangoutz");
			//var tmp = Path.Combine(documents, "..", "WineHangoutz");
			var filename = Path.Combine(cache, wineId + ".jpg");

			byte[] dataBytes = new byte[image.Length];

			System.Runtime.InteropServices.Marshal.Copy(image.Bytes, dataBytes, 0, Convert.ToInt32(image.Length));

			File.WriteAllBytes(filename, dataBytes);
		}

		public static NSData ReadPhysicalCache(string wineId)
		{
			var documents = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
			var cache = Path.Combine(documents, "..", "Library", "Caches", "WineHangoutz");
			//var tmp = Path.Combine(documents, "..", "WineHangoutz");
			var filename = Path.Combine(cache, wineId + ".jpg");

			byte[] dataBytes = File.ReadAllBytes(filename);
			return NSData.FromArray(dataBytes);
		}
	}
}
