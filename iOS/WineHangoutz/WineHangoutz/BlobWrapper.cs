using System;
using System.Net;
using System.Drawing;
using Foundation;
using UIKit;
using ImageIO;

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



		public static void CachedImages()
		{
			//var webImage = new Image { Aspect = Aspect.AspectFit };

			//webImage.Source = new UriImageSource
			//{
			//	Uri = new Uri("https://xamarin.com/content/images/pages/forms/example-app.png"),
			//	CachingEnabled = true,
			//	CacheValidity = new TimeSpan(5, 0, 0, 0)
			//};


			//foreach (var item in myItems)
			//{
			//	if (!wineImages.ContainsKey(item.WineId))
			//	{
			//		var imageBitmap = GetImageBitmapFromUrl("https://icsintegration.blob.core.windows.net/bottleimages/" + item.WineId + ".jpg");
			//		wineImages.Add(item.WineId, imageBitmap);
			//	}
			//}
		}
	}
}
