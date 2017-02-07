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
		UILabel lblError;
		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

			UITextField usernameField;

			nfloat h = 31.0f;
			nfloat w = View.Bounds.Width;
			lblError = new UILabel();
			lblError.Frame = new CGRect(10, 70, View.Frame.Width, h);
			lblError.Text = "Username:";
			lblError.TextAlignment = UITextAlignment.Left;

			//var lblName = new UILabel();
			//lblName.Frame = new CGRect(0, 150, View.Frame.Width, 20);
			//lblName.Text = "Please Login here.";
			//lblName.TextAlignment = UITextAlignment.Center;

			usernameField = new UITextField
			{
				Placeholder = "Enter your username",
				BorderStyle = UITextBorderStyle.RoundedRect,
				Frame = new CGRect(10, 100, w - 20, h)
			};

			//var txtUserName = new UITextView();
			//txtUserName.Frame = new CGRect(0, 180, View.Frame.Width, 20);
			//txtUserName.BackgroundColor = UIColor.Purple;

			UIButton btnSave = new UIButton(new CGRect(14, 270, View.Frame.Width - 28, 20));
			btnSave.SetTitle("Login", UIControlState.Normal);
			btnSave.HorizontalAlignment = UIControlContentHorizontalAlignment.Right;
			btnSave.SetTitleColor(UIColor.Purple, UIControlState.Normal);

			btnSave.TouchUpInside += (sender, e) =>
			{
				SaveUserDetails(usernameField.Text);
				nav.DismissViewController(true, null);
			};

			View.BackgroundColor = UIColor.White;
			View.AddSubview(lblError);
			View.AddSubview(btnSave);
			View.AddSubview(usernameField);
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
