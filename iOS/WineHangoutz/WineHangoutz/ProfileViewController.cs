using System;
using UIKit;
using Hangout.Models;
using Foundation;
using AVFoundation;
using BigTed;

namespace WineHangoutz
{
	public partial class ProfileViewController : UIViewController
	{
		UINavigationController NavCtrl;
		UIImagePickerController imagePicker;
		public ProfileViewController(UINavigationController navCtrl) : base("ProfileViewController", null)
		{
			NavCtrl = navCtrl;
		}

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

			ServiceWrapper sw = new ServiceWrapper();
			var cRes = sw.GetCustomerDetails(CurrentUser.RetreiveUserId()).Result;
			txtFirstName.Text = cRes.customer.FirstName;
			txtLastName.Text = cRes.customer.LastName;
			txtCity.Text = cRes.customer.City;
			txtEmail.Text = cRes.customer.Email;
			txtPhone.Text = cRes.customer.PhoneNumber;
			txtAddress.Text = cRes.customer.Address1 + cRes.customer.Address2;
			txtState.Text = cRes.customer.State;

			imgProfile.Image = new UIImage("user.png");
			imgEmail.Image = new UIImage("mail.png");
			imgAddr.Image = new UIImage("ic_action_place.png");
			imgCity.Image = new UIImage("city.png");
			imgState.Image = new UIImage("state.png");
			imgPhone.Image = new UIImage("phone.png");
			//imgPhone

			//var imageBitmap = GetImageBitmapFromUrl("https://icsintegration.blob.core.windows.net/profileimages/" + userId + ".jpg");
			//if (imageBitmap == null)
			//{
			//	propicimage.SetImageResource(Resource.Drawable.user);
			//}
			//else
			//	propicimage.SetImageBitmap(imageBitmap);

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
				try
				{
					IsCameraAuthorized();
					imagePicker = new UIImagePickerController();
					imagePicker.SourceType = UIImagePickerControllerSourceType.PhotoLibrary;
					imagePicker.MediaTypes = UIImagePickerController.AvailableMediaTypes(UIImagePickerControllerSourceType.PhotoLibrary);

					imagePicker.FinishedPickingMedia += Handle_FinishedPickingMedia;
					imagePicker.Canceled += Handle_Canceled;

					if (IsCameraAuthorized())
					{
						NavCtrl.PresentModalViewController(imagePicker, true);
						//this.PresentModalViewController(imagePicker, false);
					}
				}
				catch (Exception ex)
				{
					string s = ex.Message;
					s = s + "";
				}
			};
		}

		void Handle_Canceled(object sender, EventArgs e)
		{
			imagePicker.DismissModalViewController(true);
		}
		protected void Handle_FinishedPickingMedia(object sender, UIImagePickerMediaPickedEventArgs e)
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
						//UploadProfilePic(myByteArray);

					}

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
				AVCaptureDevice.RequestAccessForMediaTypeAsync(AVMediaType.Video);
				return false;
			}
			else
			{
				return false;
				// impossible, unknown authorization status
			}
		}
		//public async void UploadProfilePic(byte[] bytesOfPic)
		//{

			//StorageCredentials sc = new StorageCredentials("icsintegration", "+7UyQSwTkIfrL1BvEbw5+GF2Pcqh3Fsmkyj/cEqvMbZlFJ5rBuUgPiRR2yTR75s2Xkw5Hh9scRbIrb68GRCIXA==");
			//CloudStorageAccount storageaccount = new CloudStorageAccount(sc, true);
			//CloudBlobClient blobClient = storageaccount.CreateCloudBlobClient();
			//CloudBlobContainer container = blobClient.GetContainerReference("profileimages");

			//await container.CreateIfNotExistsAsync();
			//CloudBlockBlob blob = container.GetBlockBlobReference(CurrentUser.RetreiveUserId() + ".jpg"); //(path);

			//Stream stream = new MemoryStream(bytesOfPic);
			//await blob.UploadFromStreamAsync(stream);//  .UploadFromFileAsync(path);
		//}
	}
}

