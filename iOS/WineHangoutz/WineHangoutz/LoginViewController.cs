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
			this.NavigationItem.SetHidesBackButton(true, false);
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
				//root = nav;
				NavigationController.PushViewController(root, true);
			};

			View.BackgroundColor = UIColor.White;
			View.AddSubview(lblError);
			//View.AddSubview(lblName);
			//View.AddSubview(txtUserName);
			View.AddSubview(btnSave);
			View.AddSubview(usernameField);
		}

		public void SaveUserDetails(string userName)
		{
			ServiceWrapper svc = new ServiceWrapper();
			int myData = svc.AuthencateUser(userName).Result;
			if (myData == 1)
			{
				lblError.Text = "";
				Dictionary<string, string> data = new Dictionary<string, string>();
				data.Add("User Name", userName);

				IOSSecuredDataProvider provider = new IOSSecuredDataProvider();
				provider.Store(userName, "com.wineoutlet.wine-hangoutz", data);
			}
			else
			{
				lblError.Text = "Wrong User Name.";
			}
		}
	}

	public class IOSSecuredDataProvider //: ISecuredDataProvider
	{
		NSUserDefaults plist;// = NSUserDefaults.StandardUserDefaults;

		public IOSSecuredDataProvider()
		{
			plist = NSUserDefaults.StandardUserDefaults;
		}
		public void Store(string userId, string providerName, IDictionary<string, string> data)
		{
			Clear();

			plist.SetString(userId, "username");

			//plist.SetString(passwordField.Text.ToString(), "password");
			//var accountStore = AccountStore.Create();
			//var account = new Account(userId, data);
			//accountStore.Save(account, providerName);
		}

		public void Clear()
		{
			plist.RemoveObject("username");
			//var accountStore = AccountStore.Create();
			//var accounts = accountStore.FindAccountsForService(providerName);
			//foreach (var account in accounts)
			//{
		//		accountStore.Delete(account, providerName);
		//	}
		}

		public string Retreive()
		{
			string SavedUserId = plist.StringForKey("username");
			//var accountStore = AccountStore.Create();
			//var accounts = accountStore.FindAccountsForService(providerName).FirstOrDefault();

			//return (accounts != null) ? accounts.Properties : new Dictionary<string, string>();
			return SavedUserId;
		}
	}
}
