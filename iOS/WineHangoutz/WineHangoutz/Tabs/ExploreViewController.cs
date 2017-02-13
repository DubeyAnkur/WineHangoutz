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
			nfloat margin = 1;
			nfloat start = 50;
			UIButton btnBlog = new UIButton();
			UIButton btnWineries = new UIButton();
			UIButton btnRegions = new UIButton();

			btnBlog.Frame = new CGRect(0, start, UIScreen.MainScreen.Bounds.Width, ScreenHeight);
			btnWineries.Frame = new CGRect(0, start + ScreenHeight + margin, UIScreen.MainScreen.Bounds.Width, ScreenHeight);
			btnRegions.Frame = new CGRect(0, start + (ScreenHeight + margin) * 2, UIScreen.MainScreen.Bounds.Width, ScreenHeight);
			btnBlog.SetTitle("Profile", UIControlState.Normal);
			btnWineries.SetTitle("Eno View", UIControlState.Normal);
			btnRegions.SetTitle("Region", UIControlState.Normal);
			btnBlog.SetBackgroundImage(new UIImage("Images/myprofile.jpg"), UIControlState.Normal);
			btnWineries.SetBackgroundImage(new UIImage("Images/Wineries.jpg"), UIControlState.Normal);
			btnRegions.SetBackgroundImage(new UIImage("Images/Region.jpg"), UIControlState.Normal);

			View.AddSubview(btnBlog);
			View.AddSubview(btnWineries);
			View.AddSubview(btnRegions);

			btnBlog.TouchUpInside += (sender, e) =>
			{
				NavigationController.PushViewController(new ProfileViewController(), false);
			};
			btnWineries.TouchUpInside += (sender, e) =>
			{
				var lineLayout = new LineLayout()
				{
					ItemSize = new CGSize(120, 300),
					SectionInset = new UIEdgeInsets(10.0f, 10.0f, 10.0f, 10.0f),
					ScrollDirection = UICollectionViewScrollDirection.Horizontal
				};

				NavigationController.PushViewController(new SimpleCollectionViewController(lineLayout,2), false);
			};
		}
		public override void ViewDidAppear(bool animated)
		{
			base.ViewDidAppear(animated);
			//NavigationController.Title = "Locations";
			NavigationController.NavigationBar.TopItem.Title = "Explore";
		}
	}
}
