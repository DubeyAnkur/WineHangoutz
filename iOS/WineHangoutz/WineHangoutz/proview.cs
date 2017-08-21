using System;
using System.Threading.Tasks;
using BigTed;
using UIKit;
using Hangout.Models;
using CoreGraphics;
using Foundation;
using System.Net;
using System.IO;

namespace WineHangoutz
{
	public partial class proview : UIViewController
	{
		public UIScrollView Scroll;
		ServiceWrapper svc = new ServiceWrapper();
		private string screenid = "ProfileView";
		public UINavigationController NavCtrl;
		public UIImageView imgprofilepic;
		UIImagePickerController imagePicker;
		CustomerResponse cRes = new CustomerResponse();

			public proview() : base("proview", null)
			{
				Boolean internetStatus = Reachability.IsHostReachable("https://www.google.com");
				if (internetStatus == false)
				{
					UIAlertView alert = new UIAlertView()
					{
						Title = "Sorry",
						Message = "Not connected to internet.Please connect and retry."
					};
					alert.AddButton("OK");
					alert.Show();
				}
				BTProgressHUD.Show("Loading...");
	            this.Title = "Profile";
			}

			public override void ViewDidLoad()
			{
				base.ViewDidLoad();
				View.BackgroundColor = UIColor.White;
				Task.Factory.StartNew(() =>
				{
	                 InvokeOnMainThread( () => {	
						Internal_ViewDidLoad(false);
					});
				});
				// Perform any additional setup after loading the view, typically from a nib.
			}

		void Internal_ViewDidLoad(bool v)
		{
			try
			{
				//Getting Screen height and width
				nfloat ScreenHeight = UIScreen.MainScreen.Bounds.Height;
				nfloat ScreenWidth = UIScreen.MainScreen.Bounds.Width;
				//Caliculating height for profile background image
				nfloat probackimgheight = (ScreenHeight - 100) / 3;
				nfloat imgprofile = 120;

				BTProgressHUD.Dismiss();

				//Background image controller
				UIImageView backgroud = new UIImageView();
				backgroud.Frame = new CGRect(0, 0, UIScreen.MainScreen.Bounds.Width, probackimgheight-20);
				backgroud.Image = new UIImage("proback.png");
				backgroud.UserInteractionEnabled = false;

				nfloat x = (ScreenWidth/2)-(imgprofile/2);
				x = x - 10;
				nfloat y = ((backgroud.Frame.Height) / 2) ;
				//Console.WriteLine("Screen width " + ScreenWidth + "\nstart point=" + test+"\nimgprofile/2="+(imgprofile/2));

				UIBotton btnImageBack = new UIBotton
				{
					UserInteractionEnabled = false,
					Frame = new CGRect(x, y + 10, imgprofile + 20, imgprofile + 20),
					BackgroundColor=UIColor.Black
				};
				imgprofilepic = new UIImageView
				{
					Frame = new CGRect(x+10, y+ 20, imgprofile, imgprofile),
					BackgroundColor = UIColor.White
				};
				imgprofilepic.Image=new UIImage("Images/loadin.png");

				UIButton btnChange = new UIButton
				{
					Frame = new CGRect(x+(imgprofilepic.Frame.Width-10),y+(imgprofilepic.Frame.Height), 20, 20)
				};
				UIImage imgbtnCam = UIImage.FromFile("cam.png");
				imgbtnCam = ResizeImage(imgbtnCam, 25, 25);
				btnChange.SetImage(imgbtnCam, UIControlState.Normal);
				//btnChange.BackgroundColor = UIColor.Blue;

				y = y + 20;

				UILabel lblName = new UILabel
				{
					Text="Name:",
					Frame=new CGRect(10,y,ScreenHeight,ScreenWidth)
				};
				UITextField txtName = new UITextField
				{
					Frame = new CGRect(80,y, ScreenHeight,ScreenWidth),
					Placeholder="E.g. John Doe",
					UserInteractionEnabled=true
				};
				y = y + 25;
				UILabel lblEmail = new UILabel
				{
					Text = "Email:",
					Frame = new CGRect(10, y, ScreenHeight, ScreenWidth)
				};
				//y = y + lblEmail.Bounds.Height;
				UITextField txtEmail = new UITextField
				{
					Placeholder="E.g. johndoe@example.com",
					Frame=new CGRect(80,y,ScreenHeight, ScreenWidth),
					UserInteractionEnabled=true
				};
				y = y + 25;
				UILabel lblMobile=new UILabel
				{
					Text="Mobile:",
					Frame=new CGRect(10,y,ScreenHeight,ScreenWidth),
				};
				UITextField txtMobile = new UITextField
				{
					Placeholder = "E.g. 123456891",
					Frame = new CGRect(80, y, ScreenHeight, ScreenWidth),
					UserInteractionEnabled = true
				};
				Scroll = new UIScrollView
				{
					Frame = new CGRect(0,0, View.Frame.Width, View.Frame.Height),
					ContentSize = new CGSize(View.Frame.Width, View.Frame.Height),
					BackgroundColor = UIColor.White,
					AutoresizingMask = UIViewAutoresizing.FlexibleHeight,
				};
				Scroll.AddSubview(backgroud);
				Scroll.AddSubview(btnImageBack);
				Scroll.AddSubview(imgprofilepic);
				Scroll.AddSubview(btnChange);
				Scroll.AddSubview(lblName);
				Scroll.AddSubview(txtName);
				Scroll.AddSubview(lblEmail);
				Scroll.AddSubview(txtEmail);
				Scroll.AddSubview(lblMobile);
				Scroll.AddSubview(txtMobile);
				nfloat h = 0;
				for (int i = 0; i<Scroll.Subviews.Length ; i++)
				{
					nfloat n = Scroll.Subviews[i].Frame.Size.Height;
					h = h + n;
				}
				Scroll.ContentSize = new CGSize(UIScreen.MainScreen.Bounds.Width, h);
				View = (Scroll);
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message + "\n" + e.StackTrace);
			}
		}
		public UIImage ResizeImage(UIImage sourceImage, float width, float height)
		{
			UIGraphics.BeginImageContext(new CGSize(width, height));
			sourceImage.Draw(new CGRect(0, 0, width, height));
			var resultImage = UIGraphics.GetImageFromCurrentImageContext();
			UIGraphics.EndImageContext();
			return resultImage;
		}
		public override void DidReceiveMemoryWarning()
		{
			base.DidReceiveMemoryWarning();
			// Release any cached data, images, etc that aren't in use.
		}
		public async void DownloadAsync()
		{
			//Boolean internetStatus = Reachability.IsHostReachable("https://www.google.com");
			NSData HighImgData = null;
			//UIImage HighresImg = null;
			try
			{
				imgprofilepic.Image = new UIImage("Images/loadin.png");
			}
			catch (Exception ex)
			{
				//Console.WriteLine(ex.StackTrace);
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
					imgprofilepic.Image = UIImage.LoadFromData(HighImgData);
				}
				else
				{
					imgprofilepic.Image = new UIImage("user1.png");
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
		public async void UploadProfilePic(UIImage originalImage)
		{
			if (originalImage != null)
			{
				imgprofilepic.Image = originalImage; // display
				using (NSData imagedata = originalImage.AsJPEG())
				{
					byte[] myByteArray = new byte[imagedata.Length];
					System.Runtime.InteropServices.Marshal.Copy(imagedata.Bytes, myByteArray, 0, Convert.ToInt32(imagedata.Length));
					byte[] img = BlobWrapper.ResizeImageIOS(myByteArray, 250, 300);
					int i = img.Length;
					await BlobWrapper.UploadProfilePic(img, i);
				}
			}
		}
	}
}

