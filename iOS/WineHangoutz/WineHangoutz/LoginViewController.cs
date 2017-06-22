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
		public UINavigationController nav;
		public UIButton btnGuestLogin;
		public UILabel lblIns;
		public UIButton btnResend;
		public UILabel lblInfo;
		public UIButton btnVerify;
		public string CardNumber = null;
		protected string deviceToken = string.Empty;
		CustomerResponse cr = new CustomerResponse();
		ServiceWrapper svc = new ServiceWrapper();
		public string DeviceToken { get { return deviceToken; } }
		private int screenid = 1;
		public UIViewController RootTabs;
		public UIWindow _window;
		public LoginViewController() : base()
		{
			this.Title = "Login";
		}
		public override void ViewDidLoad()
		{
			try
			{
				CGSize sTemp = new CGSize(View.Frame.Width, 100);
				//Checking user is logged in or not
				if (CurrentUser.RetreiveUserId() != 0)
				{ 
					nav = new UINavigationController(RootTabs);
				   AddNavigationButtons(nav);
					_window.RootViewController = nav;
				}
				//for backgroud
				//var bGround = new UIImageView(UIImage.FromBundle("Info.png"));
				//bGround.Frame = new CGRect(0,0,View.Frame.Width, View.Frame.Height);
				//this.View.InsertSubview (bGround,9000);

					MobileBarcodeScanner scanner = new MobileBarcodeScanner();
					nfloat h = 31.0f;
					nfloat w = View.Bounds.Width;
					nfloat imageSize = 50;

					var imgLogo = new UIImageView();
					imgLogo.Frame = new CGRect(130, 70, 100, 100);
					imgLogo.Image = UIImage.FromFile("logo5.png");

					lblIns = new UILabel();
					lblIns.Frame = new CGRect(20, 170, View.Frame.Width - 100, h * 2);
					//lblIns.LineBreakMode = UILineBreakMode.WordWrap;
					lblIns.Lines = 0;
					lblIns.Text = "Please scan your VIP card barcode by touching on below card";
					lblIns.TextAlignment = UITextAlignment.Center;
					lblIns.TextColor = UIColor.White;

					btnResend = new UIButton(new CGRect(30, 400, 120, 30));
					btnResend.SetTitle("Resend Mail", UIControlState.Normal);
					btnResend.HorizontalAlignment = UIControlContentHorizontalAlignment.Center;
					btnResend.SetTitleColor(UIColor.White, UIControlState.Normal);
					btnResend.BackgroundColor = UIColor.Purple;

					lblInfo = new UILabel();
					sTemp = lblInfo.SizeThatFits(sTemp);
					lblInfo.Frame = new CGRect(20, 300, View.Frame.Width - 100, h * 2);
					lblInfo.LineBreakMode = UILineBreakMode.WordWrap;
					lblInfo.Lines = 0;
					lblInfo.TextAlignment = UITextAlignment.Center;
					lblInfo.TextColor = UIColor.White;

					UIButton btnCardScanner = new UIButton();
					btnCardScanner.Frame = new CGRect(130, 220, 100, 100);
					btnCardScanner.SetBackgroundImage(new UIImage("card.png"), UIControlState.Normal);
					btnCardScanner.TouchUpInside += async (sender, e) =>
					{
						try
						{
							scanner.UseCustomOverlay = false;
							var result = await scanner.Scan();

							if (result != null)
							{
								//CardNumber = result.Text;
								LoggingClass.LogInfo("User tried to login with" + result.Text, screenid);
								cr = await svc.AuthencateUser("test", result.Text, "test");
								CurrentUser.PutCardNumber(result.Text);
							if (cr.customer.CustomerID != 0)
								{
									// CurrentUser.StoreEmail(cr.customer.Email);
									lblInfo.Text = "Hi " + cr.customer.FirstName + cr.customer.LastName + ", We have sent an email at " + cr.customer.Email + ". Please verify email to continue login. Please contact store to get email Id corrected.";
									CurrentUser.StoreId(cr.customer.CustomerID.ToString());
									btnResend.Hidden = false;
								}
								else
								{
									lblInfo.Text = "Sorry. Your Card is invalid, Please use Guest Log In.";
								}
								
							}

						}
						catch (Exception exe)
						{
							LoggingClass.LogError(exe.Message, screenid, exe.StackTrace);
						}

					};
					UIButton btnLogin = new UIButton(new CGRect(180, 400, 120, 30));
					btnLogin.SetTitle("Log In", UIControlState.Normal);
					btnLogin.HorizontalAlignment = UIControlContentHorizontalAlignment.Center;
					btnLogin.SetTitleColor(UIColor.White, UIControlState.Normal);
					btnLogin.BackgroundColor = UIColor.Purple;
					//btnLogin.SetImage(UIImage.FromFile ("Images/gl.png"), UIControlState.Normal);

					UILabel lblGuest = new UILabel();
					lblGuest.Frame = new CGRect(20, 440, View.Frame.Width, h);
					lblGuest.Text = "Not a VIP Member?";
					lblGuest.TextAlignment = UITextAlignment.Left;
					lblGuest.Font = UIFont.ItalicSystemFontOfSize(17);
					lblGuest.TextColor = UIColor.White;

					btnGuestLogin = new UIButton(new CGRect(180, 440, 120, 30));
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
						//NavigationController.PushViewController(new TestController(), false);
					    //CurrentUser.PutGuestLoginStatus(true);
					    //nav.DismissViewController(true, null);
					   nav = new UINavigationController(RootTabs);
					   AddNavigationButtons(nav);
					   _window.RootViewController = nav;
				   };
					//btnVerify.TouchUpInside += (sender, e) =>
					//{
					//	//EmailVerification();
					//};
					btnLogin.TouchUpInside += (sender, e) =>
					{
						lblIns.Hidden = true;
						//BTProgressHUD.Show("Verifying Email Verified...");
						//CurrentUser.StoreEmail(txtEmail.Text);
						try
						{
							EmailVerification();
							//cr = await svc.AuthencateUser("Test", CardNumber, uid_device);

							//if (cr.customer.CustomerID != 0)
							//{
							//	CurrentUser.Store(cr.customer.CustomerID.ToString(), cr.customer.FirstName + cr.customer.LastName);
							//	//nav.DismissViewController(true, null);
							//	CurrentUser.PutLoginStatus(true);
							//	BTProgressHUD.ShowSuccessWithStatus("Success");
							//	nav.PushViewController(new ProfileViewController(nav), false);
							//	BTProgressHUD.Dismiss();
							//	nav.DismissViewController(true, null);
							//}
							//BTProgressHUD.Dismiss();
						}
						catch (Exception ex)
						{
							//lblError.Text = cr.ErrorDescription;
							LoggingClass.LogError(ex.Message, screenid, ex.StackTrace.ToString());
						}
						//if (cr.customer.IsMailSent == 1)
						//{
						//	CurrentUser.PutEmailStatus(t
						//}

						//EmailVerification();
						//.Hidden = true;

					};
					btnResend.Hidden = true;



					View.BackgroundColor = UIColor.White;
					View.AddSubview(imgLogo);
					View.AddSubview(btnLogin);
					View.AddSubview(btnGuestLogin);
					View.AddSubview(lblIns);
					View.AddSubview(btnCardScanner);
					View.AddSubview(lblInfo);
					View.AddSubview(lblGuest);
					View.AddSubview(btnResend);
				View.BackgroundColor = UIColor.FromRGB(97, 100, 142);
				}
				catch (Exception exe)
				{
					Console.WriteLine(exe.Message);
				}

		}
		public void AddNavigationButtons(UINavigationController nav)
		{
			UIImage profile = UIImage.FromFile("profile.png");
			profile = ResizeImage(profile, 25, 25);

			UIImage info = UIImage.FromFile("Info.png");
			info = ResizeImage(info, 25, 25);

			var topBtn = new UIBarButtonItem(profile, UIBarButtonItemStyle.Plain, (sender, args) =>
				{
					BTProgressHUD.Show("Loading,,,");
					nav.PushViewController(new ProfileViewController(nav), false);
					nav.NavigationBar.TopItem.Title = "Profile";
					BTProgressHUD.Dismiss();
				});
			var optbtn = new UIBarButtonItem(info, UIBarButtonItemStyle.Plain, (sender, args) =>
			{
				BTProgressHUD.Show("Loading,,,");
				nav.PushViewController(new AboutController1(nav), false);
				nav.NavigationBar.TopItem.Title = "About Us";
				BTProgressHUD.Dismiss();
			});

			nav.NavigationBar.TopItem.SetRightBarButtonItem(optbtn, true);
			nav.NavigationBar.TopItem.SetLeftBarButtonItem(topBtn, true);

		}

		public UIImage ResizeImage(UIImage sourceImage, float width, float height)
		{
			UIGraphics.BeginImageContext(new CGSize(width, height));
			sourceImage.Draw(new CGRect(0, 0, width, height));
			var resultImage = UIGraphics.GetImageFromCurrentImageContext();
			UIGraphics.EndImageContext();
			return resultImage;
		}

		public async void EmailVerification()
		{
			DeviceToken Dt = new DeviceToken();
			Dt = await svc.VerifyMail(CurrentUser.GetId());

			try
			{
				//int sentstatus = cr.customer.IsMailSent;
				//if (CurrentUser.GetEmailStatus() == true)
				//{
				//	lblFN.Text = "We have sent a verification link to your email address. Please verify and revisit the app to login. Thank you.";
				//}
				//View.AddSubview(lblFN);
				if (Dt.VerificationStatus == 1)
				{
					CurrentUser.Store(cr.customer.CustomerID.ToString(), cr.customer.FirstName + cr.customer.LastName);
					nav = new UINavigationController(RootTabs);
				   	AddNavigationButtons(nav);
					_window.RootViewController = nav;
					int DeviceType = 2;
					await svc.InsertUpdateToken(CurrentUser.GetToken(), CurrentUser.RetreiveUserId().ToString(), DeviceType);
					LoggingClass.LogInfo("The User logged in with" + CurrentUser.RetreiveUserId(), screenid);
				}
				else
				{
					try
					{
						View.AddSubview(btnResend);
						//View.AddSubview(lblIns);
					}
					catch (Exception ex)
					{
						Console.WriteLine(ex.Message.ToString());
					}
				}
				//if (sentstatus == 0)
				//{
				//	lblError.Text = "Email address is not valid. Please provide a valid Email to verify.";
				//	lblError.LineBreakMode = UILineBreakMode.WordWrap;
				//	lblFN.Hidden = true;
				//}
				//else
				//{
				//	lblError.Text = "If you don't receive verification email click on resend";
				//}
			}
			catch (Exception Exe)
			{
				LoggingClass.LogError(Exe.Message, screenid, Exe.StackTrace.ToString());
			}

		}
	}


	public static class CurrentUser //: ISecuredDataProvider
	{
		static NSUserDefaults plist;// = NSUserDefaults.StandardUserDefaults;

		static CurrentUser()
		{
			plist = NSUserDefaults.StandardUserDefaults;
		}
		public static void PutCardNumber(string CardNumber)
		{
			plist.SetString(CardNumber, "CardNumber");
		}
		public static string GetCardNumber()
		{
			string CardNumber = plist.StringForKey("CardNumber");
			return CardNumber;
		}
		public static void Store(string userId, string userName)
		{
			//Clear();
			plist.SetString(userName, "userName");
			plist.SetString(userId, "userId");

		}
		public static void StoreId(string id)
		{
			plist.SetString(id, "id");
		}
		public static string GetId()
		{
			string id = plist.StringForKey("id");
			return id;
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
