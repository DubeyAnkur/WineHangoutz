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
		private int screenid = 8;
		UINavigationController NavCtrl;
		UIImagePickerController imagePicker;
		//static NSCache ProfileImages;
		public ProfileViewController(UINavigationController navCtrl) : base("ProfileViewController", null)
		{
			NavCtrl = navCtrl;
		}

		public override void ViewDidLoad()
		{
			try
			{

				//AboutController1.ViewDidLoad(base);
				LoggingClass.LogInfo("Entered into Profile View", screenid);
				//LoggingClass.UploadErrorLogs();

				//imgProfile.Image = new UIImage("Images/loading.gif");
				DownloadAsync();
				ServiceWrapper sw = new ServiceWrapper();
				var cRes = sw.GetCustomerDetails(CurrentUser.RetreiveUserId()).Result;
				txtFirstName.Text = cRes.customer.FirstName;
				txtLastName.Text = cRes.customer.LastName;
				txtCity.Text = cRes.customer.City;
				txtEmail.Text = cRes.customer.Email;
				txtPhone.Text = cRes.customer.PhoneNumber;
				txtAddress.Text = cRes.customer.Address1 + cRes.customer.Address2;
				txtState.Text = cRes.customer.State;

				txtState.ShouldReturn += (TextField) =>
			  {
				  ((UITextField)TextField).ResignFirstResponder();
				  return true;
			  };
				txtFirstName.ShouldReturn += (TextField) =>
			  {
				  ((UITextField)TextField).ResignFirstResponder();
				  return true;
			  };
				txtLastName.ShouldReturn += (TextField) =>
			  {
				  ((UITextField)TextField).ResignFirstResponder();
				  return true;
			  };

				txtCity.ShouldReturn += (TextField) =>
			  {
				  ((UITextField)TextField).ResignFirstResponder();
				  return true;
			  };


				txtEmail.ShouldReturn += (TextField) =>
			  {
				  ((UITextField)TextField).ResignFirstResponder();
				  return true;
			  };
				txtPhone.ShouldReturn += (TextField) =>
			  {
				  ((UITextField)TextField).ResignFirstResponder();
				  return true;
			  };
				txtAddress.ShouldReturn += (TextField) =>
			  {
				  ((UITextField)TextField).ResignFirstResponder();
				  return true;
			  };



				//imgProfile.Image = new UIImage("user.png");


				imgEmail.Image = new UIImage("mail.png");

				imgAddr.Image = new UIImage("add.png");

				imgCity.Image = new UIImage("City1.png");

				imgState.Image = new UIImage("state.png");

				imgPhone.Image = new UIImage("phone1.png");


				//UIImage prpicImage = GetImageBitmapFromUrl(CurrentUser.RetreiveUserId());
				//if (prpicImage != null)
				//{
				//	imgProfile.Image = prpicImage;
				//}
				//else
				//{
				//	imgProfile.Image = new UIImage("user1.png");
				//}

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
					LoggingClass.LogInfo("Update button into Profile View", screenid);


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
			catch (Exception ex)
			{
				LoggingClass.LogError(ex.ToString(), screenid, ex.StackTrace);
			}
		}

		void Handle_Canceled(object sender, EventArgs e)
		{
			imagePicker.DismissModalViewController(true);
		}

		protected async void Handle_FinishedPickingMedia(object sender, UIImagePickerMediaPickedEventArgs e)
		{
			try
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


							byte[] img = BlobWrapper.ResizeImageIOS(myByteArray, 250, 300);


							int i = img.Length;
							await BlobWrapper.UploadProfilePic(img, i);
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
			catch (Exception ex)
			{
				LoggingClass.LogError(ex.ToString(), screenid, ex.StackTrace);
			}
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
		public async void DownloadAsync()
		{
			NSData HighImgData = null;
			//UIImage HighresImg = null;
			try
			{
				imgProfile.Image = new UIImage("Images/loadin.png");
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
			}
			WebClient webClient = new WebClient();
			//string url = "http://www.my-hd-wallpapers.com/wall/1405244488_moutain-reflect-on-a-lake_800.jpg";
			string url = "https://icsintegration.blob.core.windows.net/profileimages/"+CurrentUser.RetreiveUserId()+".jpg";
			byte[] imageBytes = null;
			try
			{
				imageBytes = await webClient.DownloadDataTaskAsync(url);
				HighImgData = NSData.FromStream(new MemoryStream(imageBytes));
			}
			catch (TaskCanceledException)
			{
				//this.progressLayout.Visibility = ViewStates.Gone;
				return;
			}
			catch (Exception exe)
			{
				LoggingClass.LogError(exe.Message, screenid, exe.StackTrace.ToString());
			}

			//HighresImg  =UIImage.LoadFromData(HighImgData);
			try
			{
				if (HighImgData != null)
				{
					imgProfile.Image = UIImage.LoadFromData(HighImgData);
				}
				else
				{
					imgProfile.Image = new UIImage("user1.png");
				}
			}
			catch (Exception Ex)
			{
				LoggingClass.LogError(Ex.Message, screenid, Ex.StackTrace.ToString());
			}
		}

	}
}


