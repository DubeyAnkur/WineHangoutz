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
	public class ReSendEmailController : UIViewController
	{
		public UIViewController root;
		public UINavigationController nav;

		//protected string deviceToken = string.Empty;

		//public string DeviceToken { get { return deviceToken; } }

		public ReSendEmailController() : base()
		{
			this.Title = "Verification";
		}


		ServiceWrapper svc = new ServiceWrapper();
		public override void ViewDidLoad()
		{
			//AboutController1.ViewDidLoad(base);

		

			nfloat h = 31.0f;
			nfloat w = View.Bounds.Width;
			nfloat imageSize = 150;

			EmailVerification();






			//var lblName = new UILabel();
			//lblName.Frame = new CGRect(0, 150, View.Frame.Width, 20);
			//lblName.Text = "Please Login here.";
			//lblName.TextAlignment = UITextAlignment.Center;


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
			if ( CurrentUser.GetEmail() != "")
			{
				
				txtPassword.Text = CurrentUser.GetEmail();
			}

			UIButton btnVerify = new UIButton(new CGRect(14, imageSize + 270, View.Frame.Width - 28, 20));
			btnVerify.SetTitle("Verify", UIControlState.Normal);
			btnVerify.HorizontalAlignment = UIControlContentHorizontalAlignment.Left;
			btnVerify.SetTitleColor(UIColor.Purple, UIControlState.Normal);
			UIButton btnResendEmail = new UIButton(new CGRect(14, imageSize + 270, View.Frame.Width - 28, 20));
			btnResendEmail.SetTitle("ReSend", UIControlState.Normal);
			btnResendEmail.HorizontalAlignment = UIControlContentHorizontalAlignment.Center;
			btnResendEmail.SetTitleColor(UIColor.Purple, UIControlState.Normal);

			btnVerify.TouchUpInside +=  (sender, e) =>
			{
				EmailVerification();
			};
			btnResendEmail.TouchUpInside += async (sender, e) =>
			{ 
				await svc.AuthencateUser1(txtPassword.Text);
			
			};

			View.BackgroundColor = UIColor.White;

			View.AddSubview(btnVerify);
			View.AddSubview(btnResendEmail);
			View.AddSubview(txtPassword);
			View.AddSubview(lblCard);
		}
		public async void EmailVerification()
		{
			DeviceToken Dt = new DeviceToken();
			//ServiceWrapper svc = new ServiceWrapper();
			Dt=await svc.VerifyMail(CurrentUser.RetreiveUserId().ToString());

			//dd = await svc.VerifyMail(CurrentUser.GetEmail());
			try
			{
				if (Dt.VerificationStatus == 1)
				{
					nav.PushViewController(new ProfileViewController(nav), false);
				}
				else
				{
					UIAlertView alert = new UIAlertView()
					{
						Title = "Sorry",
						Message = "Please Verify Email id"
					};
					alert.AddButton("OK");
					alert.Show();
				}
			}
			catch (Exception Exe)
			{
				
			}
			//return true;

		}
	
	}
}
