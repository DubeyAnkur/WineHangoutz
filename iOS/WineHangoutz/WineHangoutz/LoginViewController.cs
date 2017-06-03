using System;
using UIKit;
using CoreGraphics;
using Foundation;
using Hangout.Models;


namespace WineHangoutz
{

	public class LoginViewController : UIViewController
	{
		public UIViewController root;
		public UINavigationController nav;
		public UIButton btnResendEmail;
		public UILabel lblIns;
		public UIButton btnVerify;
		protected string deviceToken = string.Empty;
		CustomerResponse cr = new CustomerResponse();
		ServiceWrapper svc = new ServiceWrapper();
		public string DeviceToken { get { return deviceToken; } }
		private int screenid = 111;

		public LoginViewController() : base()
		{
			this.Title = "Login";
		}
		UILabel lblFN;
		UILabel lblError;
		public override void ViewDidLoad()
		{
			//AboutController1.ViewDidLoad(base);

			UITextField usernameField;

			nfloat h = 31.0f;
			nfloat w = View.Bounds.Width;
			nfloat imageSize = 50;

			var imgLogo = new UIImageView();
			//start,top,width,height
			imgLogo.Frame = new CGRect(70, 30, 70, 70);
			imgLogo.Image = UIImage.FromFile("logo5.png");
			//EmailVerification();

			lblError = new UILabel();
			lblError.Frame = new CGRect(10, imageSize + 70, View.Frame.Width, h);
			lblError.Text = "";
			lblError.TextColor = UIColor.Red;
			lblError.TextAlignment = UITextAlignment.Left;


			lblFN = new UILabel();
			lblFN.Frame = new CGRect(10, imageSize + 100, View.Frame.Width, h);
			lblFN.Text = "";
			lblFN.TextAlignment = UITextAlignment.Left;

			var lblName = new UILabel();
			lblName.Frame = new CGRect(10, imageSize + 90, View.Frame.Width, h);
			lblName.Text = "Please Login with registered email id.";
			lblName.TextAlignment = UITextAlignment.Left;

			//var lblIns = new UILabel();
			//lblName.Frame = new CGRect(0, 150, View.Frame.Width, 20);
			//lblName.Text = "Verify your mail.";
			//lblName.TextAlignment = UITextAlignment.Center;

			usernameField = new UITextField
			{
				Placeholder = "Enter your username",
				BorderStyle = UITextBorderStyle.RoundedRect,
				Frame = new CGRect(10, imageSize + 130, w - 20, h)
			};

			var lblCard = new UILabel();
			lblCard.Frame = new CGRect(10, imageSize + 170, View.Frame.Width, h);
			lblCard.Text = "Email";
			lblCard.TextAlignment = UITextAlignment.Left;

			var txtPassword = new UITextField
			{
				Placeholder = "Enter your mail id",
				BorderStyle = UITextBorderStyle.RoundedRect,
				Frame = new CGRect(10, imageSize + 200, w - 20, h)
			};
			txtPassword.ShouldReturn += (TextField) =>
			  {
				  ((UITextField)TextField).ResignFirstResponder();
				  return true;
			  };
			if (CurrentUser.RetreiveUserName() != "" && CurrentUser.GetEmail() != "")
			{
				usernameField.Text = CurrentUser.RetreiveUserName();
				txtPassword.Text = CurrentUser.GetEmail();
			}

			UIButton btnLogin = new UIButton(new CGRect(14, imageSize + 270, View.Frame.Width - 28, 20));
			btnLogin.SetTitle("Login", UIControlState.Normal);
			btnLogin.HorizontalAlignment = UIControlContentHorizontalAlignment.Left;
			btnLogin.SetTitleColor(UIColor.Purple, UIControlState.Normal);

			btnResendEmail = new UIButton(new CGRect(70, imageSize + 270, View.Frame.Width - 28, 20));
			btnResendEmail.SetTitle("Resend", UIControlState.Normal);
			btnResendEmail.HorizontalAlignment = UIControlContentHorizontalAlignment.Left;
			btnResendEmail.SetTitleColor(UIColor.Purple, UIControlState.Normal);

			btnVerify = new UIButton(new CGRect(14, imageSize + 270, 240, 20));
			btnVerify.SetTitle("Verify", UIControlState.Normal);
			btnVerify.HorizontalAlignment = UIControlContentHorizontalAlignment.Right;
			btnVerify.SetTitleColor(UIColor.Purple, UIControlState.Normal);

			btnResendEmail.TouchUpInside += async (sender, e) =>
			{
				await svc.AuthencateUser1(txtPassword.Text);
			};
			btnVerify.TouchUpInside += (sender, e) =>
			{
                EmailVerification();
			};
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
				//if (usernameField.Text == null || usernameField.Text == "")
				//{
				//	lblError.Text = "Please enter name";
				//}
				if (txtPassword.Text == null || txtPassword.Text == "")
				{
					lblError.Text = "Please enter email id";
				}
				else
				{
					CurrentUser.StoreEmail(txtPassword.Text);
					cr = await svc.AuthencateUser1(CurrentUser.GetEmail());
					if (cr.customer.IsMailSent == 1)
					{
						CurrentUser.PutEmailStatus(true);
					}
					LoggingClass.LogInfo(txtPassword.Text+" tried to login", screenid);
					EmailVerification();
					lblName.Hidden = true;

				}
				var loadPop = new LoadingOverlay(UIScreen.MainScreen.Bounds);
				View.AddSubview(loadPop);
				loadPop.Hide();
			};



			View.BackgroundColor = UIColor.White;
			View.AddSubview(imgLogo);
			View.AddSubview(lblError);
			View.AddSubview(lblName);
			View.AddSubview(btnLogin);
			//View.AddSubview(usernameField);
			View.AddSubview(txtPassword);
			View.AddSubview(lblCard);
		}

		public async void EmailVerification()
		{
			DeviceToken Dt = new DeviceToken();
			Dt = await svc.VerifyMail(cr.customer.CustomerID.ToString());

			try
			{
				int sentstatus=cr.customer.IsMailSent;
				if (CurrentUser.GetEmailStatus() == true)
				{
					lblFN.Text = "Mail sent please verify and come back here";
				}
					View.AddSubview(lblFN);
					if (Dt.VerificationStatus == 1)
					{
						CurrentUser.Store(cr.customer.CustomerID.ToString(), cr.customer.FirstName+cr.customer.LastName);
						nav.DismissViewController(true, null);
						int DeviceType = 2;
						await svc.InsertUpdateToken(CurrentUser.GetToken(), CurrentUser.RetreiveUserId().ToString(), DeviceType);
						LoggingClass.LogInfo("user logged in with" + CurrentUser.RetreiveUserId(), screenid);
					}
					else
					{
						try
						{
							View.AddSubview(btnResendEmail);
							View.AddSubview(lblIns);
						}
						catch (Exception ex)
						{
							Console.WriteLine(ex.Message.ToString());
						}
					}
				if (sentstatus == 0)
				{
					lblError.Text = "Entered email is doesn't exist with us please update at our store.";
					lblFN.Hidden = true;
				}
				else
				{
					lblError.Text = "If you don't receive verification email click on resend";
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
		public static void PutEmailStatus(Boolean status)
		{
			plist.SetBool(status, "status");
		}
		public static Boolean GetEmailStatus()
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
