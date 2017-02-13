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
			base.ViewDidLoad();

			UITextField usernameField;

			nfloat h = 31.0f;
			nfloat w = View.Bounds.Width;
			nfloat imageSize = 80;

			var imgLogo = new UIImageView();
			imgLogo.Frame = new CGRect((w - imageSize)/2, 70, imageSize, imageSize);
			imgLogo.Image = UIImage.FromFile("whLogo.jpg");


			lblError = new UILabel();
			lblError.Frame = new CGRect(10, imageSize + 70, View.Frame.Width, h);
			lblError.Text = "";
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


			UIButton btnSave = new UIButton(new CGRect(14, imageSize + 270, View.Frame.Width - 28, 20));
			btnSave.SetTitle("Login", UIControlState.Normal);
			btnSave.HorizontalAlignment = UIControlContentHorizontalAlignment.Right;
			btnSave.SetTitleColor(UIColor.Purple, UIControlState.Normal);

			btnSave.TouchUpInside += (sender, e) =>
			{
				SaveUserDetails(usernameField.Text);
				nav.DismissViewController(true, null);
			};

			View.BackgroundColor = UIColor.White;
			View.AddSubview(imgLogo);
			View.AddSubview(lblError);
			View.AddSubview(lblFN);
			View.AddSubview(btnSave);
			View.AddSubview(usernameField);
			View.AddSubview(txtPassword);
			View.AddSubview(lblCard);
		}

		public void SaveUserDetails(string userName)
		{
			ServiceWrapper svc = new ServiceWrapper();
			var myData = svc.AuthencateUser(userName).Result;
			if (myData.customer.CustomerID != 0)
			{
				lblError.Text = "";
				CurrentUser.Store(myData.customer.CustomerID.ToString(), userName);
			}
			else
			{
				lblError.Text = "Wrong User Name.";
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
			return Convert.ToInt32(savedUserId);
		}
	}
}
