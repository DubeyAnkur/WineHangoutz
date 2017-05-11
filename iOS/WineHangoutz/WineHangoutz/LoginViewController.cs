using System;
using UIKit;
using CoreGraphics;
using Foundation;
using PatridgeDev;
using System.Collections.Generic;
using Xamarin.Auth;
using System.Linq;
using Hangout.Models;


namespace WineHangoutz
{
	
	public class LoginViewController : UIViewController
	{
		public UIViewController root;
		public UINavigationController nav;
		public UIButton btnResendEmail;
		public UIButton btnVerify;
		protected string deviceToken = string.Empty;
		CustomerResponse cr = new CustomerResponse();
		ServiceWrapper svc = new ServiceWrapper();
		public string DeviceToken { get { return deviceToken; } }

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
			//EmailVerification();

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
			lblCard.Text = "Email";
			lblCard.TextAlignment = UITextAlignment.Left;

			var txtPassword = new UITextField
			{
				Placeholder = "Enter your mail id",
				BorderStyle = UITextBorderStyle.RoundedRect,
				Frame = new CGRect(10, imageSize + 200, w - 20, h)
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

			btnResendEmail = new UIButton(new CGRect(14, imageSize + 270, View.Frame.Width - 28, 20));
			btnResendEmail.SetTitle("Resend", UIControlState.Normal);
			btnResendEmail.HorizontalAlignment = UIControlContentHorizontalAlignment.Center;
			btnResendEmail.SetTitleColor(UIColor.Purple, UIControlState.Normal);

			btnVerify = new UIButton(new CGRect(14, imageSize + 270, View.Frame.Width - 28, 20));
			btnVerify.SetTitle("Verify", UIControlState.Normal);
			btnVerify.HorizontalAlignment = UIControlContentHorizontalAlignment.Right;
			btnVerify.SetTitleColor(UIColor.Purple, UIControlState.Normal);

			btnResendEmail.TouchUpInside += async (sender, e) =>
			{
				await svc.AuthencateUser1(txtPassword.Text);
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
					//CurrentUser.Store(cr.customer.CustomerID.ToString(),"Tester");
					EmailVerification();
					//2. Check if Email sent status.
					// 2.1 Not sent, Call the service which sends email.
					// 2.2 If sent, Ask them to verify
					// 2.3 Call authenticate user and then Go to Tabs View.



					//try
					//{
					//	View.AddSubview(btnResendEmail);

					//}
					//catch (Exception ex)
					//{
					//	Console.WriteLine(ex.Message.ToString());
					//}
					//nav.DismissViewController(true, null);
				}
				var loadPop = new LoadingOverlay(UIScreen.MainScreen.Bounds);
				View.AddSubview(loadPop);

				//var output = SaveUserDetails(usernameField.Text,txtPassword.Text);

				//if (output == true)
				//{
				//	nav.DismissViewController(true, null);

				//	int DeviceType = 2;
				//	await svc.InsertUpdateToken(CurrentUser.GetToken(), CurrentUser.RetreiveUserId().ToString(),DeviceType);
				//}

				loadPop.Hide();
			};

			View.BackgroundColor = UIColor.White;
			View.AddSubview(imgLogo);
			View.AddSubview(lblError);
			//View.AddSubview(lblFN);
			View.AddSubview(btnLogin);
			//View.AddSubview(usernameField);
			View.AddSubview(txtPassword);
			View.AddSubview(lblCard);
		}

		public async void EmailVerification()
		{
			DeviceToken Dt = new DeviceToken();

			//ServiceWrapper svc = new ServiceWrapper();
			Dt = await svc.VerifyMail(cr.customer.CustomerID.ToString());

			//dd = await svc.VerifyMail(CurrentUser.GetEmail());
			try
			{
				if (Dt.VerificationStatus == 1)
				{
					
					CurrentUser.Store(cr.customer.CustomerID.ToString(), "Tester");
					nav.DismissViewController(true, null);

					int DeviceType = 2;
					await svc.InsertUpdateToken(CurrentUser.GetToken(), CurrentUser.RetreiveUserId().ToString(), DeviceType);
				}
				else
				{
					try
					{
						View.AddSubview(btnResendEmail);

					}
					catch (Exception ex)
					{
						Console.WriteLine(ex.Message.ToString());
					}

					//lblError.Text = "If you don't get email click on resend";
				}
			}
			catch (Exception Exe)
			{

			}
			//return true;

		}

		//public bool SaveUserDetails(string userName,string email)
		//{
		//	ServiceWrapper svc = new ServiceWrapper();

		//	if (userName.Trim() == "")
		//	{
		//		lblError.Text = "Wrong First Name.";
		//		return false;
		//	}
		//	else if (email.Trim() == "")
		//	{
		//		lblError.Text = "Wrong Email id";
		//	}

		//	var myData = svc.AuthencateUser(userName).Result;
		//	var Authen = svc.AuthencateUser1(email).Result;
		//	//Boolean Authen = svc.AuthencateUser1(email).Result;
		//	if (myData.customer.CustomerID != 0 && Authen!=null)
		//	{
		//		lblError.Text = "";
		//		CurrentUser.Store(myData.customer.CustomerID.ToString(), userName);
		//		CurrentUser.StoreEmail(email);
		//		return true;
		//		//return false;
		//	}
		//	else
		//	{
		//		lblError.Text = "Wrong Details.";
		//		return false;
		//	}
		//}
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
