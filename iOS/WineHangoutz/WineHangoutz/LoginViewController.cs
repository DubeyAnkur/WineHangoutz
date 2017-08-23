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
		public Boolean internetStatus = Reachability.IsHostReachable("https://www.google.com");
		public UIViewController root;
		public UINavigationController nav;
		public UIButton btnGuestLogin;
		public UILabel lblIns;
		public UILabel lblContactus;
		public UIButton BtnTest1;
		public UIButton BtnTest2;
		public UIButton btnResend;
		public UILabel lblInfo;
		public UIButton btnVerify;
		public string CardNumber = null;
		protected string deviceToken = string.Empty;
		CustomerResponse cr = new CustomerResponse();
		public ServiceWrapper svc = new ServiceWrapper();
		public string DeviceToken { get { return deviceToken; } }
		private string screenid = "LoginView Controller";
		public nfloat start;
		UIButton btnCardScanner;
		public nfloat strtbtn;
		public UIViewController RootTabs;
		public UIWindow _window;
		public UIButton btnLogin;
		string uid_device = UIKit.UIDevice.CurrentDevice.IdentifierForVendor.AsString();
		public LoginViewController() : base()
		{
			this.Title = "Login";
		}
		public override void ViewDidLoad()
		{
			try
			{
				nfloat width = UIScreen.MainScreen.Bounds.Width;
				width = width / 2 - 15; 				if (internetStatus == false)
				{
					UIAlertView alert = new UIAlertView()
					{
						Title = "Sorry",
						Message = "Not connected to internet,Connect and re try it."
					};

					alert.AddButton("OK");
					alert.Show();
				}
				if (CurrentUser.GetCardNumber() != null)
				{
					PreInfo(CurrentUser.GetCardNumber());
				}
				CGSize sTemp = new CGSize(View.Frame.Width, 100);
				//checking is cust is scanned card or not
				//if (CurrentUser.RetreiveUserId() != 0)
				//{
				//	nav = new UINavigationController(RootTabs);
				//	AddNavigationButtons(nav);
				//	_window.RootViewController = nav;
				//	LoggingClass.LogInfo(CurrentUser.RetreiveUserName() + " Logged in", screenid);
				//	//}
				//}
				//if (CurrentUser.GetCardNumber() != null)
				//{
				//	PreInfo(CurrentUser.GetCardNumber());
				//}
				//Checking user is logged in or not
				//catch (Exception ex)
				//{
				//	LoggingClass.LogError(ex.Message, screenid, ex.StackTrace);
				//}
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
				lblIns.TextColor = UIColor.Black;

				lblInfo = new UILabel();
				lblInfo.Frame = new CGRect(10, 300, View.Frame.Width, h);
				lblInfo.LineBreakMode = UILineBreakMode.WordWrap;
				lblInfo.Lines = 0;
				lblInfo.TextAlignment = UITextAlignment.Center;
				lblInfo.TextColor = UIColor.Black;

				lblContactus = new UILabel();
				lblContactus.TextColor = UIColor.Red;
				lblContactus.TextAlignment = UITextAlignment.Center;
				lblContactus.Hidden = true;

				nfloat hei = 180 + lblIns.Frame.Height + 10;
				btnCardScanner= new UIButton();
				btnCardScanner.Frame = new CGRect((View.Frame.Width / 2) - 100, hei, 200, 152);
				btnCardScanner.SetBackgroundImage(new UIImage("card-icon.png"), UIControlState.Normal);
				start = hei + btnCardScanner.Frame.Height + 10;
				//btnCardScanner.SetTitle("Touch here to scan", UIControlState.Normal);
				btnCardScanner.TouchUpInside += async (sender, e) =>
				{
					try
					{
						scanner.UseCustomOverlay = false;
						var result = await scanner.Scan();

						if (result != null)
						{
							LoggingClass.LogInfo("User tried to login with" + result.Text, screenid);
							PreInfo(result.Text);
						}
					}
					catch (Exception exe)
					{
						LoggingClass.LogError(exe.Message, screenid, exe.StackTrace);
					}
				};
				//nfloat strtguest = strtbtn + btnLogin.Frame.Height + 10;
				UILabel lblGuest = new UILabel();
				lblGuest.Frame = new CGRect(20, View.Frame.Height - 70, View.Frame.Width, h);
				lblGuest.Text = "Not a VIP Member?";
				lblGuest.TextAlignment = UITextAlignment.Left;
				lblGuest.Font = UIFont.ItalicSystemFontOfSize(17);
				lblGuest.TextColor = UIColor.Black;

				btnGuestLogin = new UIButton(new CGRect(180, View.Frame.Height - 70, 120, 30));
				btnGuestLogin.SetTitle("Guest Log In", UIControlState.Normal);
				btnGuestLogin.HorizontalAlignment = UIControlContentHorizontalAlignment.Center;
				btnGuestLogin.SetTitleColor(UIColor.White, UIControlState.Normal);
				btnGuestLogin.BackgroundColor = UIColor.Purple;
				//btnGuestLogin.SetImage(UIImage.FromFile ("Images/gl.png"), UIControlState.Normal);

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

				BtnTest1 = new UIButton(new CGRect(200, strtbtn, 120, 30));
				BtnTest1.SetTitle("Continue", UIControlState.Normal);
				BtnTest1.HorizontalAlignment = UIControlContentHorizontalAlignment.Center;
				BtnTest1.SetTitleColor(UIColor.White, UIControlState.Normal);
				BtnTest1.BackgroundColor = UIColor.Purple;

				BtnTest2 = new UIButton(new CGRect(30, strtbtn, 140, 30));
				BtnTest2.SetTitle("Update E-Mail Id", UIControlState.Normal);
				BtnTest2.HorizontalAlignment = UIControlContentHorizontalAlignment.Center;
				BtnTest2.SetTitleColor(UIColor.White, UIControlState.Normal);
				BtnTest2.BackgroundColor = UIColor.Purple;

				btnVerify = new UIButton(new CGRect(24, imageSize + 270, 240, 20));
				btnVerify.SetTitle("Verify", UIControlState.Normal);
				btnVerify.HorizontalAlignment = UIControlContentHorizontalAlignment.Right;
				btnVerify.SetTitleColor(UIColor.Purple, UIControlState.Normal);
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
					   await svc.InsertUpdateGuest("Didn't get the token");

					   //this.NavigationController.PopToRootViewController (true);

				   };
				View.AddSubview(BtnTest1);
				View.AddSubview(BtnTest2);
				View.AddSubview(btnResend);
				View.AddSubview(btnLogin);
				View.AddSubview(imgLogo);
				View.AddSubview(btnGuestLogin);
				View.AddSubview(lblIns);
				View.AddSubview(btnCardScanner);
				View.AddSubview(lblInfo);
				View.AddSubview(lblGuest);
				View.AddSubview(lblContactus);
				View.BackgroundColor = UIColor.White;

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
					//nav.PushViewController(new ProfileViewController(nav), false);
					nav.PushViewController(new proview(nav), false);
					nav.NavigationBar.TopItem.Title = "Profile";
					//BTProgressHUD.Dismiss();
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
		public async void ShowInfo(CustomerResponse cr,Boolean Continue)
		{
			CGSize sTemp = new CGSize(View.Frame.Width, 100);
			try
			{
				BTProgressHUD.Show("Please wait...");

				//cr = await svc.AuthencateUser("email", CardNumber, uid_device);
				if (CardNumber != null)
				{
					CurrentUser.PutCardNumber(CardNumber);
				}
				//if (cr != null)
				//{
				//	CurrentUser.StoreId(cr.customer.CustomerID.ToString());
				// EmailVerification();
				//}
				if (cr != null)
				{
					//CurrentUser.StoreId(cr.customer.CustomerID.ToString());
					//EmailVerification();
					if (cr.customer.Email != "" && cr.customer.Email != null)
					{
						if (Continue == true)
						{
							lblInfo.Text=" Hi " + cr.customer.FirstName + " " + cr.customer.LastName + ",\n We have sent an email at\n " + cr.customer.Email + ".\n Please verify email to continue login. ";//\n If you have not received email Click Resend Email.\n To get Email Id changed, contact store.";
						}
						else
						{
							lblInfo.Text = cr.ErrorDescription;// " Hi " + cr.customer.FirstName + " " + cr.customer.LastName + ",\n We have sent an email at\n " + cr.customer.Email + ".\n Please verify email to continue login. ";//\n If you have not received email Click Resend Email.\n To get Email Id changed, contact store.";
						}
						lblInfo.LineBreakMode = UILineBreakMode.WordWrap;
						lblInfo.Lines = 0;
						sTemp = lblInfo.SizeThatFits(sTemp);
						lblInfo.Frame = new CGRect(0, start, View.Frame.Width, sTemp.Height);
						lblInfo.TextAlignment = UITextAlignment.Left;
						lblInfo.TextColor = UIColor.Black;
						CurrentUser.StoreId(cr.customer.CustomerID.ToString());
						try
						{
							BtnTest1.Hidden = true;
							BtnTest2.Hidden = true;
							btnLogin.Hidden = false;
							btnResend.Hidden = false;
						}
						catch(Exception exe) 
						{
						}
						strtbtn = start + lblInfo.Frame.Height + 10;
						btnLogin.Frame=new CGRect(180, strtbtn, 120, 30);
						btnResend.Frame = new CGRect(30, strtbtn, 120, 30);

						btnResend.TouchUpInside += async (send, eve) =>
						{
							BTProgressHUD.Show("Sending verification email to " + cr.customer.Email);
							if (CardNumber != null)
							{
								await svc.ResendEMail(CardNumber);
							}
							else
							{
								await svc.ResendEMail(CurrentUser.GetCardNumber());
							}
							BTProgressHUD.ShowSuccessWithStatus("Sent");
						};
						btnLogin.TouchUpInside += (sen, ev) =>
						{
							try
							{
								BTProgressHUD.Show("Checking email verification");
								EmailVerification();
							}
							catch (Exception ex)
							{
								LoggingClass.LogError(ex.Message, screenid, ex.StackTrace.ToString());
							}

						};
						BTProgressHUD.Dismiss();
					}
					else
					{
						lblInfo.Text = cr.ErrorDescription;
						//lblInfo.TextAlignment = UITextAlignment.Center;
						lblInfo.TextColor = UIColor.Red;
						try
						{
							btnLogin.Hidden = true;
							btnResend.Hidden = true;
						}
						catch (Exception exe)
						{
							LoggingClass.LogError(exe.Message, screenid, exe.StackTrace);
						}
						sTemp = lblInfo.SizeThatFits(sTemp);
						lblInfo.Frame = new CGRect(0, start, View.Frame.Width, sTemp.Height);
						BTProgressHUD.Dismiss();
					}
				}
				else
				{

					lblInfo.Text = "Sorry. Your Card number is not matching our records.\n Please re-scan Or Try app as Guest Log In.";
					lblInfo.TextColor = UIColor.Red;
					lblInfo.TextAlignment = UITextAlignment.Center;
					sTemp = lblInfo.SizeThatFits(sTemp);
					lblInfo.Frame = new CGRect(0, start, View.Frame.Width, sTemp.Height);
					try
					{
						if (btnLogin != null || btnResend != null)
						{
							btnLogin.SetTitleColor(UIColor.White, UIControlState.Normal);
							btnResend.SetTitleColor(UIColor.White, UIControlState.Normal);
							btnLogin.BackgroundColor = UIColor.White;
							btnResend.BackgroundColor = UIColor.White;
						}
					}
					catch (Exception ex)
					{
						LoggingClass.LogError(ex.Message, screenid, ex.StackTrace);
					}
					BTProgressHUD.Dismiss();
				}
				BTProgressHUD.Dismiss();
			}
			catch (Exception exe)
			{
				lblInfo.Text = "Something went wrong.We're on it.";
				lblInfo.TextColor = UIColor.Red;
				sTemp = lblInfo.SizeThatFits(sTemp);
				lblInfo.Frame = new CGRect(0, start, View.Frame.Width, sTemp.Height);
				LoggingClass.LogError(exe.Message, screenid, exe.StackTrace);
			}
			BTProgressHUD.Dismiss();
		}
		public async void EmailVerification()
		{
			try
			{
				DeviceToken Dt = new DeviceToken();
				if (CurrentUser.GetId() != null)
				{
					Dt = await svc.VerifyMail(CurrentUser.GetId());
				}
				if (Dt.VerificationStatus == 1)
				{
					CurrentUser.Store(cr.customer.CustomerID.ToString(), cr.customer.FirstName + cr.customer.LastName);
					CurrentUser.PutStore(cr.customer.PreferredStore);
					if (RootTabs == null || _window == null)
					{
						RootTabs = CurrentUser.RootTabs;
						_window = CurrentUser.window;
						nav = new UINavigationController(RootTabs);
						AddNavigationButtons(nav);
						_window.RootViewController = nav;
						LoggingClass.LogInfo("The User logged in with user id: " + CurrentUser.RetreiveUserId(), screenid);
					}
					else
					{
						nav = new UINavigationController(RootTabs);
						AddNavigationButtons(nav);
						_window.RootViewController = nav;
						LoggingClass.LogInfo("The User logged in with user id: " + CurrentUser.RetreiveUserId(), screenid);
					}
					BTProgressHUD.Dismiss();
				}
				else
				{
					try
					{
						BTProgressHUD.ShowErrorWithStatus("Your email is not verified plesase check email and verify.", 5000);
						View.AddSubview(btnResend);
					}
					catch (Exception ex)
					{
						LoggingClass.LogError(ex.Message, screenid, ex.StackTrace);
					}
				}
			}
			catch (Exception Exe)
			{
				LoggingClass.LogError(Exe.Message, screenid, Exe.StackTrace);
			}
		}
		public void UpdateEmail(string Message)
		{
			string Email;
			BTProgressHUD.Dismiss();
			UIAlertView alert = new UIAlertView()
			{
				Title = Message,
				//Message = Message
			};
			alert.AlertViewStyle = UIAlertViewStyle.PlainTextInput; 			alert.GetTextField(0).Placeholder = "johndie@winehangouts.com";
			alert.AddButton("Cancel"); 			alert.AddButton("Update"); 			alert.Clicked += async (senderalert, buttonArgs) => 			{
				 				if (buttonArgs.ButtonIndex == 1) 				{
					
					Email = alert.GetTextField(0).Text;
					if (Email == null && Email == "")
					{
						//BTProgressHUD.ShowErrorWithStatus("Email is invalid",3000);
						UpdateEmail("Entered email id is invalid,Please enter again");
					}
					else if (Email.Contains("@") != true && Email.Contains(".") != true)
					{
                        UpdateEmail("Entered email id is invalid,Please enter again");
					}
					else
					{
						//BTProgressHUD.ShowSuccessWithStatus("We're sending mail to the updated mail");
						BtnTest1.Hidden = true;
						BtnTest2.Hidden = true;
						CurrentUser.PutEmail(Email);
						cr=await svc.UpdateMail(alert.GetTextField(0).Text, CurrentUser.GetId());
						ShowInfo(cr, false);
					} 
					//Console.WriteLine(updatedEmail);
					//Update service;
					//alert.CancelButtonIndex = 0; 				} 			} ;
			//alert.DismissWithClickedButtonIndex(0, true); 			//alert.AlertViewStyle = UIAlertViewStyle.PlainTextInput; 			alert.Show();
		}
		public async void PreInfo(string CardNumber)
		{
			CGSize sTemp = new CGSize(View.Frame.Width, 100);
			try
			{
				if (btnLogin != null && btnResend != null)
				{
					btnLogin.Hidden = true;
					btnResend.Hidden = true;
				}
				BTProgressHUD.Show("Please wait...");
				cr = await svc.AuthencateUser("email", CardNumber, uid_device);
				if (CardNumber != null)
				{
					CurrentUser.PutCardNumber(CardNumber);
				}
				if (cr != null)
				{
					if (cr.customer.Email != null && cr.customer.Email != "")
					{
						lblInfo.Text = cr.ErrorDescription;// " Hi " + cr.customer.FirstName + " " + cr.customer.LastName + ",\n We are going to send an verification email at\n " + cr.customer.Email;//+ ".\n Please verify email to continue login. \n If you have not received email Click Resend Email.\n To get Email Id changed, contact store.";
						lblInfo.LineBreakMode = UILineBreakMode.WordWrap;
						lblInfo.Lines = 0;
						sTemp = lblInfo.SizeThatFits(sTemp);
						lblInfo.Frame = new CGRect(0, start, View.Frame.Width, sTemp.Height);
						lblInfo.TextAlignment = UITextAlignment.Left;
						lblInfo.TextColor = UIColor.Black;
						CurrentUser.StoreId(cr.customer.CustomerID.ToString());
						strtbtn = start + lblInfo.Frame.Height + 10;

						BtnTest1.Frame = new CGRect(200, strtbtn, 120, 30);
						BtnTest2.Frame = new CGRect(30, strtbtn, 140, 30);
						try
						{
							BtnTest1.Hidden = false;
							BtnTest2.Hidden = false;
						}
						catch (Exception ex)
						{ 
						}
						BtnTest1.TouchUpInside += async delegate
						 {
							BTProgressHUD.Show(LoggingClass.txtloading);
							 cr = await svc.ContinueService(cr);
							 //btnCardScanner.Hidden = true;
							//await svc.ResendEMail(CurrentUser.GetCardNumber());
							ShowInfo(cr, false);
						 };
						BtnTest2.TouchDown += delegate
						{
							BTProgressHUD.Show(LoggingClass.txtloading);
							UpdateEmail("Please enter your new E-mail Id");
						};
					}
					else
					{
						UpdateEmail(cr.ErrorDescription);
					}
					BTProgressHUD.Dismiss();
				}
				else
				{
					try
					{
						BtnTest2.Hidden = true;
						BtnTest1.Hidden = true;
						btnLogin.Hidden = true;
						btnResend.Hidden = true;
					}
					catch (Exception ex)
					{ 
					}
					lblInfo.Text = "Sorry. Your Card number is not matching our records.\n Please re-scan Or Try app as Guest Log In.";
					lblInfo.TextColor = UIColor.Red;
					lblInfo.TextAlignment = UITextAlignment.Center;
					sTemp = lblInfo.SizeThatFits(sTemp);
					lblInfo.Frame = new CGRect(0, start, View.Frame.Width, sTemp.Height);
					BTProgressHUD.Dismiss();
				}
			}
			catch (Exception ex)
			{ 
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
		public static void PutStore(int storeid)
		{
			plist.SetInt(storeid, "storeid");
		}
		public static nint GetStore()
		{
			nint storeid = plist.IntForKey("storeid");
			return storeid;
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
		public static void PutGuestId(string GuestId)
		{
			plist.SetString(GuestId, "GuestId");
		}
		public static string GetGuestId()
		{
			string GuestId = plist.StringForKey("GuestId");
			return GuestId;
		}
		public static string GetId()
		{
			string id = plist.StringForKey("id");
			return id;
		}
		//public static void PutLoginStatus(Boolean status)
		//{
		//	plist.SetBool(status, "sfalerttatus");
		//}
		//public static Boolean GetLoginStatus()
		//{
		//	Boolean status = plist.BoolForKey("status");
		//	return status;
		//}
		public static UIApplication app { get; set; }
		public static NSData dt { get; set; }
		public static UINavigationController navig { get; set; }
		public static void Clear()
		{
			plist.RemoveObject("userName");
			plist.RemoveObject("userId");
			plist.RemoveObject("email");
			plist.RemoveObject("CardNumber");
			plist.RemoveObject("GuestId");
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
		//public static void PutDeviceToken(string DeviceToken)
		//{
		//	plist.SetString(DeviceToken, "DeviceToken");
		//}
		//public static string GetDeviceToken()
		//{
		//	string token = plist.StringForKey("DeviceToken");
		//	return token;
		//}
		public static void PutEmail(string Email)
		{
		plist.SetString(Email, "Email");
		}
		public static string GetEmail()
		{
			string Email = plist.StringForKey("Email");
			return Email; 		}
	}
}
