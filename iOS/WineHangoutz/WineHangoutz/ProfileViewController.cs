﻿using System;

using UIKit;
using Hangout.Models;
using Foundation;

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


			//var imageBitmap = GetImageBitmapFromUrl("https://icsintegration.blob.core.windows.net/profileimages/" + userId + ".jpg");
			//if (imageBitmap == null)
			//{
			//	propicimage.SetImageResource(Resource.Drawable.user);
			//}
			//else
			//	propicimage.SetImageBitmap(imageBitmap);
			
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
			};

			btnEdit.TouchUpInside += (sender, e) =>
		    {
			   imagePicker = new UIImagePickerController();
			   imagePicker.SourceType = UIImagePickerControllerSourceType.PhotoLibrary;
			   imagePicker.MediaTypes = UIImagePickerController.AvailableMediaTypes(UIImagePickerControllerSourceType.PhotoLibrary);

				imagePicker.FinishedPickingMedia += Handle_FinishedPickingMedia;
				imagePicker.Canceled += Handle_Canceled;
			    NavCtrl.PresentModalViewController(imagePicker, true);
				this.PresentModalViewController(imagePicker, false);
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
				}
			}
			else { // if it's a video
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
	}
}

