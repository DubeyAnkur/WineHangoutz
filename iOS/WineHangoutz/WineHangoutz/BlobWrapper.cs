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
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace WineHangoutz
{
	public static class BlobWrapper
	{
		static NSCache wineBottles;
		static NSCache profilePics;

		static BlobWrapper()
		{
			wineBottles = new NSCache();
			profilePics = new NSCache();
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
		public static UIImage GetProfileImageforUser(int userid)
		{
			//String usid =Convert.ToString(CurrentUser.RetreiveUserId());
			//NSObject pro = ProfileImages.ObjectForKey(NSObject.FromObject(usid));

			NSObject profile = profilePics.ObjectForKey(NSObject.FromObject(userid));

			if (profile != null)
				return (UIImage)profile;
			
			NSData imgData = null;
			UIImage img = null;

			try
			{
				string url = "https://icsintegration.blob.core.windows.net/profileimages/" + userid + ".jpg";
				NSUrl imageURL = new NSUrl(url);
				imgData = NSData.FromUrl(imageURL);
				img = UIImage.LoadFromData(imgData);
				profilePics.SetObjectforKey(img, NSObject.FromObject(userid));

			}
			catch (Exception)
			{
				return null;
			}

			return img;
		}
		public static async Task UploadProfilePic(byte[] myByteArray, int i)
		{

			StorageCredentials sc = new StorageCredentials("icsintegration", "+7UyQSwTkIfrL1BvEbw5+GF2Pcqh3Fsmkyj/cEqvMbZlFJ5rBuUgPiRR2yTR75s2Xkw5Hh9scRbIrb68GRCIXA==");
			CloudStorageAccount storageaccount = new CloudStorageAccount(sc, true);
			CloudBlobClient blobClient = storageaccount.CreateCloudBlobClient();
			CloudBlobContainer container = blobClient.GetContainerReference("profileimages");

			await container.CreateIfNotExistsAsync();
			//string[] FileEntries = App.System.IO._dir.GetFiles(path);


			//foreach (string FilePath in FileEntries)
			//{
			//    string key = System.IO.Path.GetFileName(path);//.GetFileName(FilePath);
			CloudBlockBlob blob = container.GetBlockBlobReference(CurrentUser.RetreiveUserId() + ".jpg"); //(path);










			//using (var fs = System.IO.File.Open(myByteArray, System.IO.FileMode.Open, System.IO.FileAccess.Read, System.IO.FileShare.None))
			//{

			await blob.UploadFromByteArrayAsync(myByteArray, 0, i);//  .UploadFromFileAsync(path);

			//}
			//}
			// await container=



		}
		public static byte[] ResizeImageIOS(byte[] imageData, float width, float height)
		{
			// Load the bitmap
			UIImage originalImage1 = ImageFromByteArray(imageData);
			//
			var Hoehe = originalImage1.Size.Height;
			var Breite = originalImage1.Size.Width;
			//
			nfloat ZielHoehe = 0;
			nfloat ZielBreite = 0;
			//

			if (Hoehe > Breite) // Höhe (71 für Avatar) ist Master
			{
				ZielHoehe = height;
				nfloat teiler = Hoehe / height;
				ZielBreite = Breite / teiler;
			}
			else // Breite (61 for Avatar) ist Master
			{
				ZielBreite = width;
				nfloat teiler = Breite / width;
				ZielHoehe = Hoehe / teiler;
			}
			//
			width = (float)ZielBreite;
			height = (float)ZielHoehe;
			//
			UIGraphics.BeginImageContext(new SizeF(width, height));
			originalImage1.Draw(new RectangleF(0, 0, width, height));
			var resizedImage = UIGraphics.GetImageFromCurrentImageContext();
			UIGraphics.EndImageContext();
			//
			var bytesImagen = resizedImage.AsJPEG().ToArray();
			resizedImage.Dispose();
			return bytesImagen;
		}

		static UIImage ImageFromByteArray(byte[] imageData)
		{
			{
				if (imageData == null)
				{
					return null;
				}
				//
				UIKit.UIImage image;
				try
				{
					image = new UIKit.UIImage(Foundation.NSData.FromArray(imageData));
				}
				catch (Exception e)
				{
					Console.WriteLine("Image load failed: " + e.Message);
					return null;
				}
				return image;
			}
		}
	}
}