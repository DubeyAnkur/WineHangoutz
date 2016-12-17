using System;
using CoreGraphics;
using UIKit;

namespace WineHangoutz
{
	public partial class SecondViewController : UIViewController
	{
		protected SecondViewController(IntPtr handle) : base(handle)
		{
			this.Title = "Taste";
		}

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
			// Perform any additional setup after loading the view, typically from a nib.

			nfloat ScreenHeight = UIScreen.MainScreen.Bounds.Height;
			ScreenHeight = (ScreenHeight - 100) / 3;
			nfloat margin = 2;
			nfloat start = 50;
			UIButton btnMy = new UIButton();
			UIButton btnNew = new UIButton();
			UIButton btnTop = new UIButton();

			btnMy.Frame = new CGRect(0, start, UIScreen.MainScreen.Bounds.Width, ScreenHeight);
			btnNew.Frame = new CGRect(0, start + ScreenHeight + margin, UIScreen.MainScreen.Bounds.Width, ScreenHeight);
			btnTop.Frame = new CGRect(0, start + (ScreenHeight + margin) * 2, UIScreen.MainScreen.Bounds.Width, ScreenHeight);
			btnMy.SetTitle("My Tasting", UIControlState.Normal);
			btnNew.SetTitle("New Tasting", UIControlState.Normal);
			btnTop.SetTitle("Top Wines", UIControlState.Normal);
			btnMy.SetBackgroundImage(new UIImage("Images/My.png"), UIControlState.Normal);
			btnNew.SetBackgroundImage(new UIImage("Images/New.jpg"), UIControlState.Normal);
			btnTop.SetBackgroundImage(new UIImage("Images/Top.jpg"), UIControlState.Normal);

			btnMy.TouchUpInside += (sender, e) =>
			{
				var MyTaste = new MyTastingViewController();
				NavigationController.PushViewController(MyTaste, false);
			};

			View.AddSubview(btnMy);
			View.AddSubview(btnNew);
			View.AddSubview(btnTop);
		}

		public override void DidReceiveMemoryWarning()
		{
			base.DidReceiveMemoryWarning();
			// Release any cached data, images, etc that aren't in use.
		}
		public override void ViewDidAppear(bool animated)
		{
			base.ViewDidAppear(animated);
			//NavigationController.Title = "Locations";
			NavigationController.NavigationBar.TopItem.Title = "Taste";
		}
	}
}
