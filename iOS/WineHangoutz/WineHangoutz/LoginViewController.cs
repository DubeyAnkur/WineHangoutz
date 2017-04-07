using System;
using UIKit;
using CoreGraphics;
using Foundation;
using PatridgeDev;
using System.Collections.Generic;
using Xamarin.Auth;
using System.Linq;

namespace WineHangoutz
{
	
	public class LoginViewController : UIViewController
	{
		public UIViewController root;
		public UINavigationController nav;
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
			nfloat imageSize = 150;

			var imgLogo = new UIImageView();
			imgLogo.Frame = new CGRect((w - imageSize)/3, 70, imageSize, imageSize);
			imgLogo.Image = UIImage.FromFile("logo5.png");


			lblError = new UILabel();
			lblError.Frame = new CGRect(10, imageSize + 70, View.Frame.Width, h);
			lblError.Text = "";
			lblError.TextColor = UIColor.Red;
			lblError.TextAlignment = UITextAlignment.Left;


			lblFN = new UILabel();
			lblFN.Frame = new CGRect(10, imageSize+ 100, View.Frame.Width, h);
			lblFN.Text = "First Name:";
			lblFN.TextAlignment = UITextAlignment.Left;

			//var lblName = new UILabel();
			//lblName.Frame = new CGRect(0, 150, View.Frame.Width, 20);
			//lblName.Text = "Please Login here.";
			//lblName.TextAlignment = UITextAlignment.Center;

			usernameField = new UITextField
			{
				Placeholder = "Enter your username",
				BorderStyle = UITextBorderStyle.RoundedRect,
				Frame = new CGRect(10, imageSize + 130, w - 20, h)
			};

			var lblCard = new UILabel();
			lblCard.Frame = new CGRect(10, imageSize + 170, View.Frame.Width, h);
			lblCard.Text = "Card # or Email or Phone:";
			lblCard.TextAlignment = UITextAlignment.Left;

			var txtPassword = new UITextField
			{
				Placeholder = "Enter any of above",
				BorderStyle = UITextBorderStyle.RoundedRect,
				Frame = new CGRect(10, imageSize + 200, w - 20, h)
			};


			UIButton btnLogin = new UIButton(new CGRect(14, imageSize + 270, View.Frame.Width - 28, 20));
			btnLogin.SetTitle("Login", UIControlState.Normal);
			btnLogin.HorizontalAlignment = UIControlContentHorizontalAlignment.Left;
			btnLogin.SetTitleColor(UIColor.Purple, UIControlState.Normal);

			btnLogin.TouchUpInside += (sender, e) =>
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



				var loadPop = new LoadingOverlay(UIScreen.MainScreen.Bounds);
				View.AddSubview(loadPop);

				var output = SaveUserDetails(usernameField.Text);

				if (output == true)
					nav.DismissViewController(true, null);

				loadPop.Hide();
			};

			View.BackgroundColor = UIColor.White;
			View.AddSubview(imgLogo);
			View.AddSubview(lblError);
			View.AddSubview(lblFN);
			View.AddSubview(btnLogin);
			View.AddSubview(usernameField);
			View.AddSubview(txtPassword);
			View.AddSubview(lblCard);
		}

		public bool SaveUserDetails(string userName)
		{
			ServiceWrapper svc = new ServiceWrapper();

			if (userName.Trim() == "")
			{
				lblError.Text = "Wrong First Name.";
				return false;
			}

			var myData = svc.AuthencateUser(userName).Result;
			if (myData.customer.CustomerID != 0)
			{
				lblError.Text = "";
				CurrentUser.Store(myData.customer.CustomerID.ToString(), userName);
				return true;
			}
			else
			{
				lblError.Text = "Wrong First Name.";
				return false;
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

		public static void Clear()
		{
			plist.RemoveObject("userName");
			plist.RemoveObject("userId");
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
