using System;
using UIKit;
using CoreGraphics;
using Foundation;
using PatridgeDev;
using System.Collections.Generic;

namespace WineHangoutz
{
	public class LoginViewController : UIViewController
	{

		public LoginViewController() : base()
		{
			this.Title = "Login";
		}

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

			var lblName = new UILabel();
			lblName.Frame = new CGRect(0, 150, View.Frame.Width, 20);
			lblName.Text = "Please Login here.";
			lblName.TextAlignment = UITextAlignment.Center;

			View.BackgroundColor = UIColor.White;
			View.AddSubview(lblName);
		}

	}
}
