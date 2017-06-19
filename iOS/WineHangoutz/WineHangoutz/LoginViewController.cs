using System;
using UIKit;
using CoreGraphics;
using Foundation;
using Hangout.Models;
using BigTed;
using ZXing.Mobile;

namespace WineHangoutz
{

	public class LoginViewController : UIViewController
	{
		public UIViewController root;
		UIImageView imgSampleCard;
		public UINavigationController nav;
		public UIButton btnGuestLogin;
		public UILabel lblIns;
		public UIButton btnVerify;
		protected string deviceToken = string.Empty;
		CustomerResponse cr = new CustomerResponse();
		ServiceWrapper svc = new ServiceWrapper();
		public string DeviceToken { get { return deviceToken; } }
		private int screenid = 1;
		public LoginViewController() : base()
		{
			this.Title = "Login";
		}
		UILabel lblFN;
		UILabel lblError;
		public override void ViewDidLoad()
		{
			
			//AboutController1.ViewDidLoad(base);
			try
			{
               // this.View.InsertSubview (new UIImageView(UIImage.FromBundle("Info.png")),9000);
				DismissKeyboardOnBackgroundTap();
				UITextField txtCardID;
				MobileBarcodeScanner scanner = new MobileBarcodeScanner();
				nfloat h = 31.0f;
				nfloat w = View.Bounds.Width;
				nfloat imageSize = 50;

				var imgLogo = new UIImageView();
				//start,top,width,height
				imgLogo.Frame = new CGRect(130, 30, 70, 70);
				imgLogo.Image = UIImage.FromFile("logo5.png");

				//EmailVerification();

				lblError = new UILabel();
				lblError.Frame = new CGRect(20, imageSize + 60, View.Frame.Width, h * 2);
				lblError.Text = "";
				lblError.LineBreakMode = UILineBreakMode.WordWrap;
				lblError.Lines = 0;
				lblError.TextColor = UIColor.Red;
				lblError.TextAlignment = UITextAlignment.Left;


				lblFN = new UILabel();
				lblFN.Frame = new CGRect(20, imageSize + 100, View.Frame.Width, h);
				lblFN.Text = "";
				lblFN.TextAlignment = UITextAlignment.Left;


				var lblIns = new UILabel();
				lblIns.Frame = new CGRect(20, imageSize + 80, View.Frame.Width - 150, h);
				//lblIns.LineBreakMode = UILineBreakMode.WordWrap;
				lblIns.Lines = 0;
				lblIns.Text = "Enter your details to Log In";
				lblIns.TextAlignment = UITextAlignment.Left;
				//lblIns.Editable = false;

				var lblName = new UILabel();
				lblName.Frame = new CGRect(20, imageSize + 120, View.Frame.Width, 20);
				lblName.TextAlignment = UITextAlignment.Left;
				lblName.Text = "Card ID";

				txtCardID = new UITextField
				{
					Placeholder = "e.g. 123456789123",
					BorderStyle = UITextBorderStyle.RoundedRect,
					Frame = new CGRect(20, imageSize + 145, 300, h)
						
				};

				UIButton btnInfo = new UIButton();
				btnInfo.Frame = new CGRect(80, imageSize + 120, 25, 25);
				btnInfo.SetBackgroundImage(new UIImage("Info.png"), UIControlState.Normal);
				btnInfo.TouchUpInside += async (sender, e) =>
			   {
				   try
				   {
					   scanner.UseCustomOverlay = false;
					   var result = await scanner.Scan();

					   if (result != null)
						   txtCardID.Text = result.Text;//Console.WriteLine("Scanned Barcode: " + result.Text);
				   }
				   catch (Exception exe)
				   { 
						
					}
					//imgSampleCard = new UIImageView();
				 //  imgSampleCard.Image=UIImage.FromFile("card.jpg");
					//imgSampleCard.Frame = new CGRect(10, imageSize + 120, 200, 100);
				 //  View.AddSubview(imgSampleCard);

			   };



				UIToolbar toolbar = new UIToolbar(new CGRect(0.0f, 0.0f, this.View.Frame.Size.Width, 44.0f));
				toolbar.Items = new UIBarButtonItem[] { 
					new UIBarButtonItem(UIBarButtonSystemItem.Done, delegate {
						txtCardID.ResignFirstResponder();
	    			})
				};
				txtCardID.InputAccessoryView = toolbar;
				txtCardID.KeyboardType = UIKeyboardType.PhonePad;

				var lblEmail = new UILabel();
				lblEmail.Frame = new CGRect(20, imageSize + 195, View.Frame.Width, h);
				lblEmail.Text = "Email";
				lblEmail.TextAlignment = UITextAlignment.Left;


				var txtEmail = new UITextField
				{
					Placeholder = "e.g. john@wineoutlet.com",
					BorderStyle = UITextBorderStyle.RoundedRect,
					Frame = new CGRect(20, imageSize + 225, 300, h)
				};
				txtEmail.ShouldReturn += (TextField) =>
				  {
					  ((UITextField)TextField).ResignFirstResponder();
					  return true;
				  };
				txtEmail.InputAccessoryView = toolbar;
				txtEmail.KeyboardType = UIKeyboardType.EmailAddress;
				if (CurrentUser.RetreiveUserName() != "" && CurrentUser.GetEmail() != "")
				{
					txtCardID.Text = CurrentUser.RetreiveUserName();
					txtEmail.Text = CurrentUser.GetEmail();
				}

				UIButton btnLogin = new UIButton(new CGRect(130, imageSize + 280, 120, 30));
				btnLogin.SetTitle("Log In", UIControlState.Normal);
				btnLogin.HorizontalAlignment = UIControlContentHorizontalAlignment.Center;
				btnLogin.SetTitleColor(UIColor.White, UIControlState.Normal);
				btnLogin.BackgroundColor = UIColor.Purple;
				//btnLogin.SetImage(UIImage.FromFile ("Images/gl.png"), UIControlState.Normal);

				var lblGuest = new UILabel();
				lblGuest.Frame = new CGRect(20, imageSize + 340, View.Frame.Width, h);
				lblGuest.Text = "Not a VIP Member?";
				lblGuest.TextAlignment = UITextAlignment.Left;
				lblGuest.Font = UIFont.ItalicSystemFontOfSize(17);

				btnGuestLogin = new UIButton(new CGRect(180, imageSize + 340, 120, 30));
				btnGuestLogin.SetTitle("Guest Log In", UIControlState.Normal);
				btnGuestLogin.HorizontalAlignment = UIControlContentHorizontalAlignment.Center;
				btnGuestLogin.SetTitleColor(UIColor.White, UIControlState.Normal);
				btnGuestLogin.BackgroundColor = UIColor.Purple;
				//btnGuestLogin.SetImage(UIImage.FromFile ("Images/gl.png"), UIControlState.Normal);


				btnVerify = new UIButton(new CGRect(24, imageSize + 270, 240, 20));
				btnVerify.SetTitle("Verify", UIControlState.Normal);
				btnVerify.HorizontalAlignment = UIControlContentHorizontalAlignment.Right;
				btnVerify.SetTitleColor(UIColor.Purple, UIControlState.Normal);
				//for 
				string uid_device = UIKit.UIDevice.CurrentDevice.IdentifierForVendor.AsString();
				btnGuestLogin.TouchDown += (sender, e) =>
			   {
				   CurrentUser.Store("0", "Guest");
				   //CurrentUser.PutGuestLoginStatus(true);
				   nav.DismissViewController(true, null);
			   };
				//btnVerify.TouchUpInside += (sender, e) =>
				//{
				//	//EmailVerification();
				//};
				btnLogin.TouchUpInside += async (sender, e) =>
				{
					//var smsTo = NSUrl.FromString("sms:"+txtPassword.Text);
					//UIApplication.SharedApplication.OpenUrl(smsTo);
					//if (UIApplication.SharedApplication.CanOpenUrl(smsTo))
					//{
					//	UIApplication.SharedApplication.OpenUrl(smsTo);
					//}
					//else
					//{
					//	// warn the user, or hide the button...
					//}
					lblIns.Hidden = true;

					if (txtCardID.Text == null || txtCardID.Text == "")
					{
						lblError.Text = "Please enter a valid Card ID";
					}
					else if (txtEmail.Text == null || txtEmail.Text == "")
					{
						lblError.Text = "Please enter a valid Email Address.";
						
					}
					else if (txtEmail.Text.Contains("@") == false || txtEmail.Text.Contains(".") == false)
					{ 
							lblError.Text = "Please enter a valid Email Address.";
					}
					else
					{
						BTProgressHUD.Show("Validating Credentials...");
						//CurrentUser.StoreEmail(txtEmail.Text);
						try
						{
							cr = await svc.AuthencateUser(txtEmail.Text, txtCardID.Text, uid_device);
						
						if (cr.customer.CustomerID != 0)
						{
							CurrentUser.Store(cr.customer.CustomerID.ToString(), cr.customer.FirstName + cr.customer.LastName);
							//nav.DismissViewController(true, null);
							CurrentUser.PutLoginStatus(true);
							BTProgressHUD.ShowSuccessWithStatus("Success");
							nav.PushViewController(new ProfileViewController(nav), false);
							BTProgressHUD.Dismiss();
							nav.DismissViewController(true, null);
							lblError.Hidden = true;
						}
						BTProgressHUD.Dismiss();
						LoggingClass.LogInfo(txtEmail.Text + " tried to login", screenid);
						lblError.Text = "Card ID is invalid";//cr.ErrorDescription;

						}
						catch (Exception ex)
						{
							lblError.Text = cr.ErrorDescription;
							LoggingClass.LogError(ex.Message, screenid, ex.StackTrace.ToString());
						}
						//if (cr.customer.IsMailSent == 1)
						//{
						//	CurrentUser.PutEmailStatus(t
						//}

						//EmailVerification();
						//.Hidden = true;

					}
					//var loadPop = new LoadingOverlay(UIScreen.MainScreen.Bounds);
					//View.AddSubview(loadPop);
					//loadPop.Hide();
				};




				View.BackgroundColor = UIColor.White;
				View.AddSubview(imgLogo);
				View.AddSubview(lblError);
				View.AddSubview(lblEmail);
				View.AddSubview(btnLogin);
				View.AddSubview(btnGuestLogin);
				View.AddSubview(txtCardID);
				View.AddSubview(txtEmail);
				View.AddSubview(lblName);
				View.AddSubview(lblIns);
				View.AddSubview(btnInfo);
				View.AddSubview(lblGuest);
			}
			catch (Exception exe)
			{
				Console.WriteLine(exe.Message);
			}
		}
		//private void OnDoubleTap(UIGestureRecognizer gesture)
		//{
		//	if (scrollView.ZoomScale >= 1)
		//		scrollView.SetZoomScale(0.25f, true);
		//	else
		//		scrollView.SetZoomScale(2f, true);
		//}
		//public override void ViewDidAppear(bool animated)
		//{
		//	base.ViewDidAppear(animated);
		//	//NavigationController.Title = "Locations";
		//	//NavigationController.NavigationBar.TopItem.Title = "Locations";
		//	//string validUser = CurrentUser.RetreiveUserName();

		////	LoggingClass.LogInfo("opened app " + validUser, screenid);

		//	//if (validUser == "" || validUser == null)
		//	//{
		//		ProfileViewController yourController = new ProfileViewController();
		//		yourController.NavCtrl = NavigationController;
		//		yourController.root = this;
		//		yourController.ModalPresentationStyle = UIModalPresentationStyle.FullScreen;
		//		this.PresentModalViewController(yourController, false);
		//		//ProfileViewController yourController = new ProfileViewController();
		//		//yourController.NavCtrl = NavigationController;
		//		//yourController.root = this;
		//		//yourController.ModalPresentationStyle = UIModalPresentationStyle.FullScreen;
		//		//this.PresentModalViewController(yourController, false);
		//	//}
		//	//login check 
		//}

		protected void DismissKeyboardOnBackgroundTap()
		{
			var tap = new UITapGestureRecognizer { CancelsTouchesInView = false };
			tap.AddTarget(() => View.EndEditing(true));
			View.AddGestureRecognizer(tap);	
		}
	}
		//	public async void EmailVerification()
		//	{
		//		DeviceToken Dt = new DeviceToken();
		//		Dt = await svc.VerifyMail(cr.customer.CustomerID.ToString());

		//		try
		//		{
		//			int sentstatus=cr.customer.IsMailSent;
		//			if (CurrentUser.GetEmailStatus() == true)
		//			{
		//				lblFN.Text = "We have sent a verification link to your email address. Please verify and revisit the app to login. Thank you.";
		//			}
		//				View.AddSubview(lblFN);
		//				if (Dt.VerificationStatus == 1)
		//				{
		//					CurrentUser.Store(cr.customer.CustomerID.ToString(), cr.customer.FirstName+cr.customer.LastName);
		//					nav.DismissViewController(true, null);
		//					int DeviceType = 2;
		//					await svc.InsertUpdateToken(CurrentUser.GetToken(), CurrentUser.RetreiveUserId().ToString(), DeviceType);
		//					LoggingClass.LogInfo("The User logged in with" + CurrentUser.RetreiveUserId(), screenid);
		//				}
		//				else
		//				{
		//					try
		//					{
		//						View.AddSubview(btnResendEmail);
		//						View.AddSubview(lblIns);
		//					}
		//					catch (Exception ex)
		//					{
		//						Console.WriteLine(ex.Message.ToString());
		//					}
		//				}
		//			if (sentstatus == 0)
		//			{
		//				lblError.Text = "Email address is not valid. Please provide a valid Email to verify.";
		//				lblError.LineBreakMode = UILineBreakMode.WordWrap;
		//				lblFN.Hidden = true;
		//			}
		//			else
		//			{
		//				lblError.Text = "If you don't receive verification email click on resend";
		//			}
		//		}
		//		catch (Exception Exe)
		//		{
		//			LoggingClass.LogError(Exe.Message, screenid, Exe.StackTrace.ToString());
		//		}

		//	}
		//}

		public static class CurrentUser //: ISecuredDataProvider
		{
			static NSUserDefaults plist;// = NSUserDefaults.StandardUserDefaults;

			static CurrentUser()
			{
				plist = NSUserDefaults.StandardUserDefaults;
			}
			public static void Store(string userId, string userName)
			{
				//Clear();
				plist.SetString(userName, "userName");
				plist.SetString(userId, "userId");

			}
			public static void StoreEmail(string email)
			{
				plist.SetString(email, "email");
			}
			public static string GetEmail()
			{
				string email = plist.StringForKey("email");
				return email;
			}
			public static void PutLoginStatus(Boolean status)
			{
				plist.SetBool(status, "status");
			}
			public static Boolean GetLoginStatus()
			{
				Boolean status = plist.BoolForKey("status");
				return status;
			}
			public static void SetToken(string token)
			{
				plist.SetString(token, "token");
			}
			public static string GetToken()
			{
				string token = plist.StringForKey("token");
				return token;
			}
			public static void Clear()
			{
				plist.RemoveObject("userName");
				plist.RemoveObject("userId");
				plist.RemoveObject("email");
				plist.RemoveObject("token");
			}
			public static string RetreiveUserName()
			{
				string savedUserName = plist.StringForKey("userName");
				return savedUserName;
			}

			public static int RetreiveUserId()
			{
				string savedUserId = plist.StringForKey("userId");
				if (savedUserId == "")
					return 0;
				return Convert.ToInt32(savedUserId);
			}
		}

	}
