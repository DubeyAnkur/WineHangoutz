using System;
using UIKit;
using Foundation;
using System.Threading.Tasks;
using BigTed;
using Hangout.Models;
using AVFoundation;
using System.Net;
using System.IO;
using System.Drawing;
using System.Collections.Generic;

namespace WineHangoutz
{
	public partial class ProfileViewController : UIViewController
	{

		public UIViewController root;
		//public UINavigationController nav;
		private int screenid = 8;
		public UINavigationController NavCtrl;
		UIImagePickerController imagePicker;
		PickerDataModel pickerDataModel;
		public IntPtr handle;
		//static NSCache ProfileImages;
		public ProfileViewController(UINavigationController navCtrl) : base("ProfileViewController", null)
		{
			NavCtrl = navCtrl;
		}
		public ProfileViewController() : base()
		{
			this.Title = "Profile";
		}

		public override void ViewDidLoad()
		{
			try
			{
				DismissKeyboardOnBackgroundTap();
				//AboutController1.ViewDidLoad(base);
				LoggingClass.LogInfo("Entered into Profile View", screenid);
				pickerDataModel = new PickerDataModel();
				pickerDataModel.Items.Add("AL");
				pickerDataModel.Items.Add("AK");
				pickerDataModel.Items.Add("AZ");
				pickerDataModel.Items.Add("AR");
				pickerDataModel.Items.Add("CA");
				pickerDataModel.Items.Add("CO");
				pickerDataModel.Items.Add("CT");
				pickerDataModel.Items.Add("DE");
				pickerDataModel.Items.Add("FL");
				pickerDataModel.Items.Add("GA");
				pickerDataModel.Items.Add("HI");
				pickerDataModel.Items.Add("ID");
				pickerDataModel.Items.Add("IL");
				pickerDataModel.Items.Add("IN");
				pickerDataModel.Items.Add("IA");
				pickerDataModel.Items.Add("KS");
				pickerDataModel.Items.Add("KY");
				pickerDataModel.Items.Add("LA");
				pickerDataModel.Items.Add("ME");
				pickerDataModel.Items.Add("MD");
				pickerDataModel.Items.Add("MA");
				pickerDataModel.Items.Add("MI");
				pickerDataModel.Items.Add("MN");
				pickerDataModel.Items.Add("MS");
				pickerDataModel.Items.Add("MO");
				pickerDataModel.Items.Add("MT");
				pickerDataModel.Items.Add("NE");
				pickerDataModel.Items.Add("NV");
				pickerDataModel.Items.Add("NH");
				pickerDataModel.Items.Add("NJ");
				pickerDataModel.Items.Add("NM");
				pickerDataModel.Items.Add("NY");
				pickerDataModel.Items.Add("NC");
				pickerDataModel.Items.Add("ND");
				pickerDataModel.Items.Add("OH");
				pickerDataModel.Items.Add("OK");
				pickerDataModel.Items.Add("OR");
				pickerDataModel.Items.Add("PA");
				pickerDataModel.Items.Add("RI");
				pickerDataModel.Items.Add("SC");
				pickerDataModel.Items.Add("SD");
				pickerDataModel.Items.Add("TN");
				pickerDataModel.Items.Add("TX");
				pickerDataModel.Items.Add("UT");
				pickerDataModel.Items.Add("VT");
				pickerDataModel.Items.Add("VA");
				pickerDataModel.Items.Add("WA");
				pickerDataModel.Items.Add("WV");
				pickerDataModel.Items.Add("WI");
				pickerDataModel.Items.Add("WY");
				statePicker.Model = pickerDataModel;
                //statePicker.Select(5, 0, true);
				//LoggingClass.UploadErrorLogs();
				if (CurrentUser.RetreiveUserId() == 0)
				{
					DownloadAsync();
					UIAlertView alert = new UIAlertView()
					{
						Title = "This feature is allowed only for VIP Card holders",
						//Message = "Coming Soon..."
					};

					alert.AddButton("OK");
					alert.AddButton("Log in");
					alert.Clicked += (senderalert, buttonArgs) =>
						{

							if (buttonArgs.ButtonIndex == 1)
							{
								LoginViewController yourController = new LoginViewController();
								yourController.nav = NavCtrl;
								yourController.RootTabs = CurrentUser.RootTabs;
								NavCtrl.PushViewController(yourController, false);
								//NavCtrl.PopViewController(false);
								//NavCtrl.PopViewController(false);
							}
						};
					alert.Show();
				}
				else
				{
					DownloadAsync();
					ServiceWrapper sw = new ServiceWrapper();
					var cRes = sw.GetCustomerDetails(CurrentUser.RetreiveUserId()).Result;
					txtFirstName.Text = cRes.customer.FirstName;
					txtLastName.Text = cRes.customer.LastName;
					//txtCity.Text = cRes.customer.City;
					txtEmail.Text = cRes.customer.Email;
					txtPhone.Text = cRes.customer.PhoneNumber;
					string state = cRes.customer.State;
					if (pickerDataModel.Items.Contains(state))
					{
						int i = pickerDataModel.Items.FindIndex(x => x == state);
						statePicker.Select(i, 0, false);
					}

					txtAddress.Text = cRes.customer.Address1 + cRes.customer.Address2 + cRes.customer.City;
					//txtState.ShouldReturn += (TextField) =>
					// {
					//  ((UITextField)TextField).ResignFirstResponder();
					//  return true;
					// };
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

					txtCardID.ShouldReturn += (TextField) =>
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
						if (txtPhone.Text.Length > 10 || txtPhone.Text.Length < 10)
						{
							BTProgressHUD.Dismiss();
							UIAlertView alert = new UIAlertView()
							{
								Title = "Please enter correct phone number",
								//Message = "Coming Soon..."
							};
							alert.AddButton("OK");
							alert.Show();
						}
						else if ((txtEmail.Text.Contains("@")) == false || (txtEmail.Text.Contains(".")) == false)
						{
							BTProgressHUD.Dismiss();
							UIAlertView alert = new UIAlertView()
							{
								Title = "Please enter a valid email address",
								//Message = "Coming Soon..."
							};
							alert.AddButton("OK");
							alert.Show();
						}
						else
						{
							LoggingClass.LogInfo("Update button into Profile View", screenid);
							Customer cust = new Customer();
							cust.CustomerID = CurrentUser.RetreiveUserId();
							cust.Address1 = txtAddress.Text;
							cust.FirstName = txtFirstName.Text;
							cust.LastName = txtLastName.Text;
							cust.Email = txtEmail.Text;
							cust.PhoneNumber = txtPhone.Text;
							//cust.State = txtState.Text;
							await sw.UpdateCustomer(cust);
							BTProgressHUD.ShowSuccessWithStatus("Profile Updated.");
							try
							{
								NavCtrl.PopViewController(true);
								//NavCtrl.PushViewController(new FirstViewController(handle), false);
							}
							catch (Exception exe)
							{
								LoggingClass.LogError(exe.Message, screenid, exe.StackTrace.ToString());
							}
						}


					};
					btnUpdate.HorizontalAlignment = UIControlContentHorizontalAlignment.Center;


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

				imgEmail.Image = new UIImage("mail.png");

				imgAddr.Image = new UIImage("add.png");

				//imgCity.Image = new UIImage("City1.png");

				//imgCard.Image = new UIImage("card.png");

				imgPhone.Image = new UIImage("phone1.png");
			}

			catch (Exception ex)
			{
				LoggingClass.LogError(ex.ToString(), screenid, ex.StackTrace);
			}
			//View.AddSubview(imgAddr);
			//scrollView.ContentSize = new CGSize(View.Frame.Width, 300);
			//View = (scrollView);
		}
		//public override UIView GetView(UIPickerView pickerView, int row, int component, UIView view)
		//{
		//	UILabel lbl = new UILabel(new RectangleF(0, 0, 130f, 40f));
		//	lbl.TextColor = UIColor.Black;
		//	lbl.Font = UIFont.SystemFontOfSize(12f);
		//	lbl.TextAlignment = UITextAlignment.Center;
		//	lbl.Text = "Test";//values[row];
		//	return lbl;
		//	}
		protected void DismissKeyboardOnBackgroundTap()
		{
			var tap = new UITapGestureRecognizer { CancelsTouchesInView = false };
			tap.AddTarget(() => View.EndEditing(true));
			View.AddGestureRecognizer(tap);
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
						//Console.WriteLine("Image selected");
						isImage = true;
						break;
					case "public.video":
						//Console.WriteLine("Video selected");
						break;
				}

				// get common info (shared between images and video)
				NSUrl referenceURL = e.Info[new NSString("UIImagePickerControllerReferenceUrl")] as NSUrl;
				if (referenceURL != null)
					//Console.WriteLine("Url:" + referenceURL.ToString());

				// if it was an image, get the other image info
				if (isImage)
				{
					// get the original image

					UIImage originalImage = e.Info[UIImagePickerController.OriginalImage] as UIImage;
					if (originalImage != null)
					{
						// do something with the image

						//Console.WriteLine("got the original image");
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
						//Console.WriteLine(mediaURL.ToString());
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
				Console.WriteLine(ex.StackTrace);
				LoggingClass.LogError(ex.Message, screenid, ex.StackTrace.ToString());
			}
			WebClient webClient = new WebClient();
			//string url = "http://www.my-hd-wallpapers.com/wall/1405244488_moutain-reflect-on-a-lake_800.jpg";
			string url = "https://icsintegration.blob.core.windows.net/profileimages/" + CurrentUser.RetreiveUserId() + ".jpg";
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
		//public override void ViewDidAppear(bool animated)
		//{
		//	base.ViewDidAppear(animated);
		//	//NavigationController.Title = "Locations";
		//	//NavigationController.NavigationBar.TopItem.Title = "Locations";
		//	string validUser = CurrentUser.RetreiveUserName();
		//	LoggingClass.LogInfo("opened app " + validUser, screenid);
		//	if (validUser == "" || validUser == null)
		//	{
		//		LoginViewController yourController = new LoginViewController();
		//		yourController.nav = NavigationController;
		//		yourController.root = this;
		//		yourController.ModalPresentationStyle = UIModalPresentationStyle.FullScreen;
		//		this.PresentModalViewController(yourController, false);
		//	}
		//}
	}
	public class PickerDataModel : UIPickerViewModel

	{
		public event EventHandler<EventArgs> ValueChanged;


		public List<string> Items { get; private set; }


		public string SelectedItem
		{
			get { return Items[selectedIndex]; }
		}

		int selectedIndex = 0;

		public PickerDataModel()
		{
			Items = new List<string>();
		}


		public override nint GetRowsInComponent(UIPickerView picker, nint component)
		{
			return Items.Count;
		}

		public override string GetTitle(UIPickerView picker, nint row, nint component)
		{
			return Items[(int)row];
		}

		public override nint GetComponentCount(UIPickerView picker)
		{
			return 1;
		}


		public override void Selected(UIPickerView picker, nint row, nint component)
		{
			selectedIndex = (int)row;
			if (ValueChanged != null)
			{
				ValueChanged(this, new EventArgs());
			}
		}
	}
}


