using System;
using CoreGraphics;
using UIKit;

namespace WineHangoutz
{
	public partial class ProfileViewController : UIViewController
	{
		public ProfileViewController() : base("ProfileViewController", null)
		{
		}

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
			txtFirstName.Text = "Ankur";
			txtLastName.Text = "Dubey";
			txtEmail.Text = "ankur@wineoutlet.com";
			txtPhone.Text = "123456789";

			//ScrollView.Layer.Bounds = new CGRect(0,0, UIScreen.MainScreen.Bounds.Width, 1600);

			// Perform any additional setup after loading the view, typically from a nib.
		}

		public override void DidReceiveMemoryWarning()
		{
			base.DidReceiveMemoryWarning();
			// Release any cached data, images, etc that aren't in use.
		}
	}
}

