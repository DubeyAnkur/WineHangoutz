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

		public LoginViewController() : base()
		{
			this.Title = "Login";
		}
		UILabel lblError;
		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

			lblError = new UILabel();
			lblError.Frame = new CGRect(0, 120, View.Frame.Width, 20);
			lblError.Text = "Error";
			lblError.TextAlignment = UITextAlignment.Center;

			var lblName = new UILabel();
			lblName.Frame = new CGRect(0, 150, View.Frame.Width, 20);
			lblName.Text = "Please Login here.";
			lblName.TextAlignment = UITextAlignment.Center;

			var txtUserName = new UITextView();
			txtUserName.Frame = new CGRect(0, 180, View.Frame.Width, 20);
			txtUserName.BackgroundColor = UIColor.Purple;

			UIButton btnSave = new UIButton(new CGRect(14, 270, View.Frame.Width - 28, 20));
			btnSave.SetTitle("Login", UIControlState.Normal);
			btnSave.HorizontalAlignment = UIControlContentHorizontalAlignment.Right;
			btnSave.SetTitleColor(UIColor.Purple, UIControlState.Normal);

			btnSave.TouchUpInside += (sender, e) =>
			{
				SaveUserDetails(txtUserName.Text);
			};

			View.BackgroundColor = UIColor.White;
			View.AddSubview(lblError);
			View.AddSubview(lblName);
			View.AddSubview(txtUserName);
			View.AddSubview(btnSave);
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
		public void Store(string userId, string providerName, IDictionary<string, string> data)
		{
			Clear(providerName);
			var accountStore = AccountStore.Create();
			var account = new Account(userId, data);
			accountStore.Save(account, providerName);
		}

		public void Clear(string providerName)
		{
			var accountStore = AccountStore.Create();
			var accounts = accountStore.FindAccountsForService(providerName);
			foreach (var account in accounts)
			{
				accountStore.Delete(account, providerName);
			}
		}

		public Dictionary<string, string> Retreive(string providerName)
		{
			var accountStore = AccountStore.Create();
			var accounts = accountStore.FindAccountsForService(providerName).FirstOrDefault();

			return (accounts != null) ? accounts.Properties : new Dictionary<string, string>();
		}
	}
}
