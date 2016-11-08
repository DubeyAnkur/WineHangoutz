using System;
using CoreGraphics;
using UIKit;
using Foundation;

namespace WineHangoutz
{
	public partial class ExploreViewController : UIViewController
	{
		public ExploreViewController() : base()
		{
			this.Title = "Explore";
		}

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
			// Perform any additional setup after loading the view, typically from a nib.

			nfloat ScreenHeight = UIScreen.MainScreen.Bounds.Height;
			ScreenHeight = (ScreenHeight - 100) / 3;
			nfloat margin = 2;
			nfloat start = 50;
			UIButton btnBlog = new UIButton();
			UIButton btnWineries = new UIButton();
			UIButton btnRegions = new UIButton();

			btnBlog.Frame = new CGRect(0, start, UIScreen.MainScreen.Bounds.Width, ScreenHeight);
			btnWineries.Frame = new CGRect(0, start + ScreenHeight + margin, UIScreen.MainScreen.Bounds.Width, ScreenHeight);
			btnRegions.Frame = new CGRect(0, start + (ScreenHeight + margin) * 2, UIScreen.MainScreen.Bounds.Width, ScreenHeight);
			btnBlog.SetTitle("Blog", UIControlState.Normal);
			btnWineries.SetTitle("Wineries", UIControlState.Normal);
			btnRegions.SetTitle("Region", UIControlState.Normal);
			btnBlog.SetBackgroundImage(new UIImage("Images/Blog.jpg"), UIControlState.Normal);
			btnWineries.SetBackgroundImage(new UIImage("Images/Wineries.jpg"), UIControlState.Normal);
			btnRegions.SetBackgroundImage(new UIImage("Images/Region.jpg"), UIControlState.Normal);

			View.AddSubview(btnBlog);
			View.AddSubview(btnWineries);
			View.AddSubview(btnRegions);
		}
		public override void ViewWillAppear(bool animated)
		{
			base.ViewWillAppear(animated);
			NavigationController.NavigationBar.TopItem.Title = "Explore";

		}
	}
}