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
		public ServiceWrapper svc = new ServiceWrapper();
		public string DeviceToken { get { return deviceToken; } }
		private int screenid = 1;
		public nfloat start;
		public nfloat strtbtn;
		public UIViewController RootTabs;
		public UIWindow _window;
		public UIButton btnLogin;
		public LoginViewController() : base()
		{
			this.Title = "Login";
		}
		public override void ViewDidLoad()
		{
			try
			{
				CGSize sTemp = new CGSize(View.Frame.Width, 100);
				//checking is cust is scanned card or not
				if (CurrentUser.GetCardNumber()!= null)
				{
                    ShowInfo(CurrentUser.GetCardNumber());
				}

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
				imgLogo.Frame = new CGRect((View.Frame.Width / 2) - 50, 70, 100, 100);

				imgLogo.Image = UIImage.FromFile("logo5.png");

				lblIns = new UILabel();
				lblIns.Text = "Please scan your VIP card barcode by touching below card:";
				lblIns.LineBreakMode = UILineBreakMode.WordWrap;
				lblIns.Lines = 0;
				sTemp = lblIns.SizeThatFits(sTemp);
				lblIns.Frame = new CGRect(0, 180, View.Frame.Width, sTemp.Height);


				lblIns.TextAlignment = UITextAlignment.Center;
				lblIns.TextColor = UIColor.White;

				lblInfo = new UILabel();
				lblInfo.Frame = new CGRect(0, 300, View.Frame.Width, h);
				lblInfo.LineBreakMode = UILineBreakMode.WordWrap;
				lblInfo.Lines = 0;
				lblInfo.TextAlignment = UITextAlignment.Center;
				lblInfo.TextColor = UIColor.White;


				nfloat hei = 180 + lblIns.Frame.Height + 10;
				UIButton btnCardScanner = new UIButton();
				btnCardScanner.Frame = new CGRect((View.Frame.Width / 2) - 50, hei, 100, 100);
				btnCardScanner.SetBackgroundImage(new UIImage("card.png"), UIControlState.Normal);
				start = hei + btnCardScanner.Frame.Height + 10;
				btnCardScanner.TouchUpInside += async (sender, e) =>
				{
					try
					{
						scanner.UseCustomOverlay = false;
						var result = await scanner.Scan();

						if (result != null)
						{
							LoggingClass.LogInfo("User tried to login with" + result.Text, screenid);
							BTProgressHUD.Show("Please wait...");
							ShowInfo(result.Text);
						}
					}
					catch (Exception exe)
					{
						LoggingClass.LogError(exe.Message, screenid, exe.StackTrace);
					}
				};


				//nfloat strtguest = strtbtn + btnLogin.Frame.Height + 10;

				UILabel lblGuest = new UILabel();
				lblGuest.Frame = new CGRect(20, View.Frame.Height-100, View.Frame.Width, h);
				lblGuest.Text = "Not a VIP Member?";
				lblGuest.TextAlignment = UITextAlignment.Left;
				lblGuest.Font = UIFont.ItalicSystemFontOfSize(17);
				lblGuest.TextColor = UIColor.White;

				btnGuestLogin = new UIButton(new CGRect(180, View.Frame.Height-100, 120, 30));
				btnGuestLogin.SetTitle("Guest Log In", UIControlState.Normal);
				btnGuestLogin.HorizontalAlignment = UIControlContentHorizontalAlignment.Center;
				btnGuestLogin.SetTitleColor(UIColor.White, UIControlState.Normal);
				btnGuestLogin.BackgroundColor = UIColor.Purple;
				//btnGuestLogin.SetImage(UIImage.FromFile ("Images/gl.png"), UIControlState.Normal);


				btnVerify = new UIButton(new CGRect(24, imageSize + 270, 240, 20));
				btnVerify.SetTitle("Verify", UIControlState.Normal);
				btnVerify.HorizontalAlignment = UIControlContentHorizontalAlignment.Right;
				btnVerify.SetTitleColor(UIColor.Purple, UIControlState.Normal);

				string uid_device = UIKit.UIDevice.CurrentDevice.IdentifierForVendor.AsString();
				btnGuestLogin.TouchDown += async (sender, e) =>
			   	{
						CurrentUser.Store("0", "Guest");
					   if (RootTabs == null || _window == null)
					   {
						   _window = CurrentUser.window;
						   RootTabs = CurrentUser.RootTabs;
						   nav = new UINavigationController(RootTabs);
						   //AddNavigationButtons(nav);
						   _window.RootViewController = nav;
						   //nav.DismissViewController(true);
					   }
					   	nav = new UINavigationController(RootTabs);
						AddNavigationButtons(nav);
						CurrentUser.RootTabs = RootTabs;
						_window.RootViewController = nav;
						CurrentUser.window = _window;
						await svc.InsertUpdateGuest(CurrentUser.GetToken());
						
                        //this.NavigationController.PopToRootViewController (true);
					
			   	};
				View.BackgroundColor = UIColor.White;
				View.AddSubview(imgLogo);
				View.AddSubview(btnGuestLogin);
				View.AddSubview(lblIns);
				View.AddSubview(btnCardScanner);
				View.AddSubview(lblInfo);
				View.AddSubview(lblGuest);

				View.BackgroundColor = UIColor.FromRGB(97, 100, 142);
			}
			catch (Exception exe)
			{
				LoggingClass.LogError(exe.Message, screenid, exe.StackTrace);
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
		public async void  ShowInfo(string CardNumber)
		{	
			CGSize sTemp = new CGSize(View.Frame.Width, 100);
			cr = await svc.AuthencateUser("test", CardNumber, "test");
			CurrentUser.PutCardNumber(CardNumber);
			CurrentUser.StoreId(cr.customer.CustomerID.ToString());
			EmailVerification();
			if (cr != null)
			{
				lblInfo.Text = " Hi " + cr.customer.FirstName + cr.customer.LastName + ",\n We have sent an email at  " + cr.customer.Email + ".\n Please verify email to continue login. \n If you have not received email Click Resend Email.\n To get Email Id changed, contact store.";
				lblInfo.LineBreakMode = UILineBreakMode.WordWrap;
				lblInfo.Lines = 0;
				sTemp = lblInfo.SizeThatFits(sTemp);
				lblInfo.Frame = new CGRect(0, start, View.Frame.Width, sTemp.Height);
				lblInfo.TextAlignment = UITextAlignment.Left;
				CurrentUser.StoreId(cr.customer.CustomerID.ToString());
				strtbtn = start + lblInfo.Frame.Height + 10;
				btnLogin = new UIButton(new CGRect(180, strtbtn, 120, 30));
				btnLogin.SetTitle("Log In", UIControlState.Normal);
				btnLogin.HorizontalAlignment = UIControlContentHorizontalAlignment.Center;
				btnLogin.SetTitleColor(UIColor.White, UIControlState.Normal);
				btnLogin.BackgroundColor = UIColor.Purple;

				btnResend = new UIButton(new CGRect(30, strtbtn, 120, 30));
				btnResend.SetTitle("Resend Email", UIControlState.Normal);
				btnResend.HorizontalAlignment = UIControlContentHorizontalAlignment.Center;
				btnResend.SetTitleColor(UIColor.White, UIControlState.Normal);
				btnResend.BackgroundColor = UIColor.Purple;
				View.AddSubview(btnResend);
				View.AddSubview(btnLogin);
				btnResend.TouchUpInside += async (send, eve) =>
				{
					BTProgressHUD.Show("Sending verification email to"+cr.customer.Email);
					await svc.AuthencateUser("", CardNumber, "");
					BTProgressHUD.ShowSuccessWithStatus("Sent");
				};
				btnLogin.TouchUpInside += (sen, ev) =>
				{
					try
					{
						BTProgressHUD.Show("Checking email verifification");
						EmailVerification();
					}
					catch (Exception ex)
					{
						LoggingClass.LogError(ex.Message, screenid, ex.StackTrace.ToString());
					}

				};
			}
			else
			{
				lblInfo.Text = "Sorry. Your Card number is not matching our records.\n Please re-scan Or Try app as Guest Log In.";
				lblInfo.TextColor=UIColor.Red;
				sTemp = lblInfo.SizeThatFits(sTemp);
				lblInfo.Frame = new CGRect(0, start, View.Frame.Width, sTemp.Height);
			}
		}
		public async void EmailVerification()
		{
			DeviceToken Dt = new DeviceToken();
			Dt = await svc.VerifyMail(CurrentUser.GetId());

			try
			{

				if (Dt.VerificationStatus == 1)
				{
					
					CurrentUser.Store(cr.customer.CustomerID.ToString(), cr.customer.FirstName + cr.customer.LastName);
					if (RootTabs == null || _window == null)
					{
						RootTabs = CurrentUser.RootTabs;
						_window = CurrentUser.window;
						nav = new UINavigationController(RootTabs);
						AddNavigationButtons(nav);
						_window.RootViewController = nav;
						int DeviceType = 2;
						await svc.InsertUpdateToken(CurrentUser.GetToken(), CurrentUser.RetreiveUserId().ToString(), DeviceType);
						LoggingClass.LogInfo("The User logged in with" + CurrentUser.RetreiveUserId(), screenid);
					}
					else
					{
						nav = new UINavigationController(RootTabs);
						AddNavigationButtons(nav);
						_window.RootViewController = nav;
						int DeviceType = 2;
						await svc.InsertUpdateToken(CurrentUser.GetToken(), CurrentUser.RetreiveUserId().ToString(), DeviceType);
						LoggingClass.LogInfo("The User logged in with" + CurrentUser.RetreiveUserId(), screenid);
					}
					BTProgressHUD.ShowSuccessWithStatus("Verified");
				}
				else
				{
					try
					{
						BTProgressHUD.ShowErrorWithStatus("Your email is not verified plesase check email and verify.");
						View.AddSubview(btnResend);
					}
					catch (Exception ex)
					{
						LoggingClass.LogError(ex.Message, screenid, ex.StackTrace.ToString());
					}
				}
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
		public static UIViewController RootTabs { get; set; }
		public static UIWindow window { get; set; }
		public static void StoreId(string id)
		{
			plist.SetString(id, "id");
		}
		public static string GuestId { get; set; }
		public static string GetId()
		{
			string id = plist.StringForKey("id");
			return id;
		}
		public static void PutLoginStatus(Boolean status)
		{
			plist.SetBool(status, "sfalerttatus");
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
		public static UINavigationController navig { get; set; }
		public static void Clear()
		{
			plist.RemoveObject("userName");
			plist.RemoveObject("userId");
			plist.RemoveObject("email");
			plist.RemoveObject("token");
			plist.RemoveObject("CardNumber");
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
		public static void SaveAuthToken(string Authtoken)
		{
			plist.SetString(Authtoken, "Authtoken");
		}
		public static string GetAuthToken()
		{
			string token = plist.StringForKey("Authtoken");
			return token;
		}
	}
}
