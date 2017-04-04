using System;
using CoreGraphics;
using UIKit;
using BigTed;

namespace WineHangoutz
{
	public partial class SecondViewController : UIViewController
	{
		protected SecondViewController(IntPtr handle) : base(handle)
		{
			this.Title = "My Hangouts";
		}

		public override void ViewDidLoad()
		{
			//AboutController1.ViewDidLoad(base);
			// Perform any additional setup after loading the view, typically from a nib.

			nfloat ScreenHeight = UIScreen.MainScreen.Bounds.Height;
			ScreenHeight = (ScreenHeight - 100) / 3;
			nfloat margin = 1;
			nfloat start = 50;
			UIButton btnReviews = new UIButton();
			UIButton btnTastings = new UIButton();
			UIButton btnFavourites = new UIButton();

			btnReviews.Frame = new CGRect(0, start, UIScreen.MainScreen.Bounds.Width, ScreenHeight);
			btnTastings.Frame = new CGRect(0, start + ScreenHeight + margin, UIScreen.MainScreen.Bounds.Width, ScreenHeight);
			btnFavourites.Frame = new CGRect(0, start + (ScreenHeight + margin) * 2, UIScreen.MainScreen.Bounds.Width, ScreenHeight);
			btnReviews.SetTitle("My Reviews", UIControlState.Normal);
			btnTastings.SetTitle("My Tastings", UIControlState.Normal);
			btnFavourites.SetTitle("My Favorites", UIControlState.Normal);
			btnReviews.SetBackgroundImage(new UIImage("Images/winereviews.jpg"), UIControlState.Normal);
			btnTastings.SetBackgroundImage(new UIImage("Images/winetasting.jpg"), UIControlState.Normal);
			btnFavourites.SetBackgroundImage(new UIImage("Images/myfavorate.jpg"), UIControlState.Normal);

			btnReviews.TouchDown += (sender, e) =>
		    {
			   BTProgressHUD.Show("Loading..."); //show spinner + text
			};
			btnTastings.TouchDown += (sender, e) =>
			{
				BTProgressHUD.Show("Loading..."); //show spinner + text
			};
			btnFavourites.TouchDown += (sender, e) =>
			{
				BTProgressHUD.Show("Loading..."); //show spinner + text
			};

			btnReviews.TouchUpInside += (sender, e) =>
			{
				
				var MyReview = new MyReviewViewController();
				NavigationController.PushViewController(MyReview, false);
				BTProgressHUD.Dismiss();
				NavigationController.NavigationBar.TopItem.Title = "My Reviews";
			};

			btnTastings.TouchUpInside += (sender, e) =>
			{
				var MyTaste = new MyTastingViewController();
				NavigationController.PushViewController(MyTaste, false);
				BTProgressHUD.Dismiss();
				NavigationController.NavigationBar.TopItem.Title = "My Tastings" ;
			};

			btnFavourites.TouchUpInside += (sender, e) =>
			{
				nfloat width = UIScreen.MainScreen.Bounds.Width;
				width = width / 2 - 15;

				UICollectionViewFlowLayout flowLayout;
				flowLayout = new UICollectionViewFlowLayout()
				{
					ItemSize = new CGSize(width, 325.0f),
					SectionInset = new UIEdgeInsets(10.0f, 10.0f, 10.0f, 10.0f),
					ScrollDirection = UICollectionViewScrollDirection.Vertical
				};
				NavigationController.NavigationBar.TopItem.Title = "My Favorites";
				NavigationController.PushViewController(new PhyCollectionView(flowLayout, 1, true), false);
				BTProgressHUD.Dismiss();
			};

			View.AddSubview(btnReviews);
			View.AddSubview(btnTastings);
			View.AddSubview(btnFavourites);
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
			NavigationController.NavigationBar.TopItem.Title = "My Hangouts";
		}
	}
}
