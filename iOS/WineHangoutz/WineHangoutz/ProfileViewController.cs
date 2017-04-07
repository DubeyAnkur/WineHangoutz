using System;
using UIKit;
using Hangout.Models;
using Foundation;
using AVFoundation;
using BigTed;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using CoreGraphics;
using System.Net;
using System.Drawing;
using ImageIO;
using System.IO;
using System.Threading.Tasks;


namespace WineHangoutz
{
	public partial class ProfileViewController : UIViewController
	{
		UINavigationController NavCtrl;
		UIImagePickerController imagePicker;
		//static NSCache ProfileImages;
		public ProfileViewController(UINavigationController navCtrl) : base("ProfileViewController", null)
		{
			NavCtrl = navCtrl;
		}

		public override void ViewDidLoad()
		{
			//AboutController1.ViewDidLoad(base);

			ServiceWrapper sw = new ServiceWrapper();
			var cRes = sw.GetCustomerDetails(CurrentUser.RetreiveUserId()).Result;
			txtFirstName.Text = cRes.customer.FirstName;
			txtLastName.Text = cRes.customer.LastName;
			txtCity.Text = cRes.customer.City;
			txtEmail.Text = cRes.customer.Email;
			txtPhone.Text = cRes.customer.PhoneNumber;
			txtAddress.Text = cRes.customer.Address1 + cRes.customer.Address2;
			txtState.Text = cRes.customer.State;
			//imgProfile.Image = new UIImage("user.png");


			imgEmail.Image = new UIImage("mail.png");

			imgAddr.Image = new UIImage("add.png");

			imgCity.Image = new UIImage("City1.png");

			imgState.Image = new UIImage("state.png");

			imgPhone.Image = new UIImage("phone1.png");
	
			//imgPhone

			//var imageBitmap = GetImageBitmapFromUrl("https://icsintegration.blob.core.windows.net/profileimages/" + userId + ".jpg");
			//if (imageBitmap == null)
			//{
			//	propicimage.SetImageResource(Resource.Drawable.user);
			//}
			//else
			//	propicimage.SetImageBitmap(imageBitmap);


			UIImage prpicImage = GetImageBitmapFromUrl(CurrentUser.RetreiveUserId());
			if (prpicImage != null)
			{
				imgProfile.Image = prpicImage;
			}
			else
			{
				imgProfile.Image=new UIImage("user1.png");
			}

			btnUpdate.TouchDown += (sender, e) =>
			{
				BTProgressHUD.Show("Updating profile..."); //show spinner + text
			};

			btnUpdate.TouchUpInside += async (sender, e) =>
			{
				Customer cust = new Customer();
				cust.CustomerID = CurrentUser.RetreiveUserId();
				cust.Address1 = txtAddress.Text;
				cust.FirstName = txtFirstName.Text;
				cust.LastName = txtLastName.Text;
				cust.City = txtCity.Text;
				cust.Email = txtCity.Text;
				cust.Email = txtEmail.Text;
				cust.PhoneNumber = txtPhone.Text;
				cust.State = txtState.Text;

				await sw.UpdateCustomer(cust);
				BTProgressHUD.ShowSuccessWithStatus("Profile Updated.");
			};


			btnEdit.TouchUpInside += (sender, e) =>
			{
				IsCameraAuthorized();
				imagePicker = new UIImagePickerController();
				imagePicker.SourceType = UIImagePickerControllerSourceType.PhotoLibrary;
				imagePicker.MediaTypes = UIImagePickerController.AvailableMediaTypes(UIImagePickerControllerSourceType.PhotoLibrary);

				imagePicker.FinishedPickingMedia += Handle_FinishedPickingMedia;
				imagePicker.Canceled += Handle_Canceled;
				NavCtrl.PresentModalViewController(imagePicker, true);
				if (IsCameraAuthorized())
				{
					this.PresentModalViewController(imagePicker, false);
				}
			};
		}

		void Handle_Canceled(object sender, EventArgs e)
		{
			imagePicker.DismissModalViewController(true);
		}

		public static UIImage GetImageBitmapFromUrl(int userid)
		{
			//String usid =Convert.ToString(CurrentUser.RetreiveUserId());
			//NSObject pro = ProfileImages.ObjectForKey(NSObject.FromObject(usid));
			NSData imgData = null;
			UIImage img = null;
			//if (pro != null)
			//	return (UIImage)pro;
			try
			{
				string url = "https://icsintegration.blob.core.windows.net/profileimages/" + userid + ".jpg";
					NSUrl imageURL = new NSUrl(url);
					imgData = NSData.FromUrl(imageURL);
					img = UIImage.LoadFromData(imgData);
				//ProfileImages.SetObjectforKey(img, NSObject.FromObject(usid));
				//BlobWrapper bvb = new BlobWrapper();
				//CachedImagePhysically(imgData, usid);
				
			}

			catch (Exception)
			{
				return null;
			}

			return img;
		}

		//public static void CachedImagePhysically(NSData image, string userId)
		//{
		//	try
		//	{
		//		var cache = Path.Combine("Library/Caches/", "WineHangoutz");
		//		var filename = Path.Combine(cache, userId + ".jpg");

		//		byte[] dataBytes = new byte[image.Length];

		//		System.Runtime.InteropServices.Marshal.Copy(image.Bytes, dataBytes, 0, Convert.ToInt32(image.Length));
		//		if (!Directory.Exists(cache))
		//		{
		//			Directory.CreateDirectory(cache);
		//		}
		//		File.WriteAllBytes(filename, dataBytes);
		//	}
		//	catch (Exception)
		//	{
		//		//ignore the error. Download it next time.
		//	}
		//}

		protected async void Handle_FinishedPickingMedia(object sender, UIImagePickerMediaPickedEventArgs e)
		{
			// determine what was selected, video or image
			bool isImage = false;
			switch (e.Info[UIImagePickerController.MediaType].ToString())
			{
				case "public.image":
					Console.WriteLine("Image selected");
					isImage = true;
					break;
				case "public.video":
					Console.WriteLine("Video selected");
					break;
			}

			// get common info (shared between images and video)
			NSUrl referenceURL = e.Info[new NSString("UIImagePickerControllerReferenceUrl")] as NSUrl;
			if (referenceURL != null)
				Console.WriteLine("Url:" + referenceURL.ToString());

			// if it was an image, get the other image info
			if (isImage)
			{
				// get the original image

				UIImage originalImage = e.Info[UIImagePickerController.OriginalImage] as UIImage;
				if (originalImage != null)
				{
					// do something with the image

					Console.WriteLine("got the original image");
					imgProfile.Image = originalImage; // display
					using (NSData imagedata = originalImage.AsJPEG())
					{
						byte[] myByteArray = new byte[imagedata.Length];
						System.Runtime.InteropServices.Marshal.Copy(imagedata.Bytes,
																	myByteArray, 0, Convert.ToInt32(imagedata.Length));


						byte[] img =BlobWrapper.ResizeImageIOS(myByteArray, 250, 300);


						int i = img.Length;
						await BlobWrapper.UploadProfilePic(img,i);
					}


					//NetStandard.Library 1.6.0 is recommended else app will flicker.
				}
			}
			else
			{ // if it's a video
			  // get video url
				NSUrl mediaURL = e.Info[UIImagePickerController.MediaURL] as NSUrl;
				if (mediaURL != null)
				{
					Console.WriteLine(mediaURL.ToString());
				}
			}
			// dismiss the picker
			imagePicker.DismissModalViewController(true);
		}
		public override void DidReceiveMemoryWarning()
		{
			base.DidReceiveMemoryWarning();
			// Release any cached data, images, etc that aren't in use.
		}



		public bool IsCameraAuthorized()
		{
			AVAuthorizationStatus authStatus = AVCaptureDevice.GetAuthorizationStatus(AVMediaType.Video);
			if (authStatus == AVAuthorizationStatus.Authorized)
			{
				// do your logic
				return true;
			}
			else if (authStatus == AVAuthorizationStatus.Denied)
			{
				// denied
				return false;
			}
			else if (authStatus == AVAuthorizationStatus.Restricted)
			{
				// restricted, normally won't happen
				return false;
			}
			else if (authStatus == AVAuthorizationStatus.NotDetermined)
			{
				// not determined?!
				return false;
			}
			else
			{
				return false;
				// impossible, unknown authorization status
			}
		}
		//public async void UploadProfilePic(byte[] myByteArray,int i)
		//{

		//	StorageCredentials sc = new StorageCredentials("icsintegration", "+7UyQSwTkIfrL1BvEbw5+GF2Pcqh3Fsmkyj/cEqvMbZlFJ5rBuUgPiRR2yTR75s2Xkw5Hh9scRbIrb68GRCIXA==");
		//	CloudStorageAccount storageaccount = new CloudStorageAccount(sc, true);
		//	CloudBlobClient blobClient = storageaccount.CreateCloudBlobClient();
		//	CloudBlobContainer container = blobClient.GetContainerReference("profileimages");

		//	await container.CreateIfNotExistsAsync();
		//	//string[] FileEntries = App.System.IO._dir.GetFiles(path);


		//	//foreach (string FilePath in FileEntries)
		//	//{
		//	//    string key = System.IO.Path.GetFileName(path);//.GetFileName(FilePath);
		//	CloudBlockBlob blob = container.GetBlockBlobReference( CurrentUser.RetreiveUserId()+ ".jpg"); //(path);










		//	//using (var fs = System.IO.File.Open(myByteArray, System.IO.FileMode.Open, System.IO.FileAccess.Read, System.IO.FileShare.None))
		//	//{

		//	await blob.UploadFromByteArrayAsync(myByteArray,0,i) ;//  .UploadFromFileAsync(path);

		//	//}
		//	//}
		//	// await container=



		//}
		//public static byte[] ResizeImageIOS(byte[] imageData, float width, float height)
		//{
		//	// Load the bitmap
		//	UIImage originalImage1 = ImageFromByteArray(imageData);
		//	//
		//	var Hoehe = originalImage1.Size.Height;
		//	var Breite = originalImage1.Size.Width;
		//	//
		//	nfloat ZielHoehe = 0;
		//	nfloat ZielBreite = 0;
		//	//

		//	if (Hoehe > Breite) // Höhe (71 für Avatar) ist Master
		//	{
		//		ZielHoehe = height;
		//		nfloat teiler = Hoehe / height;
		//		ZielBreite = Breite / teiler;
		//	}
		//	else // Breite (61 for Avatar) ist Master
		//	{
		//		ZielBreite = width;
		//		nfloat teiler = Breite / width;
		//		ZielHoehe = Hoehe / teiler;
		//	}
		//	//
		//	width = (float)ZielBreite;
		//	height = (float)ZielHoehe;
		//	//
		//	UIGraphics.BeginImageContext(new SizeF(width, height));
		//	originalImage1.Draw(new RectangleF(0, 0, width, height));
		//	var resizedImage = UIGraphics.GetImageFromCurrentImageContext();
		//	UIGraphics.EndImageContext();
		//	//
		//	var bytesImagen = resizedImage.AsJPEG().ToArray();
		//	resizedImage.Dispose();
		//	return bytesImagen;
		//}

		//static UIImage ImageFromByteArray(byte[] imageData)
		//{
		//	{
		//		if (imageData == null)
		//		{
		//			return null;
		//		}
		//		//
		//		UIKit.UIImage image;
		//		try
		//		{
		//			image = new UIKit.UIImage(Foundation.NSData.FromArray(imageData));
		//		}
		//		catch (Exception e)
		//		{
		//			Console.WriteLine("Image load failed: " + e.Message);
		//			return null;
		//		}
		//		return image;
		//	}
		//}
}
}

