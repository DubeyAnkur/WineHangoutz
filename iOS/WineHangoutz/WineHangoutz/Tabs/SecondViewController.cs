﻿using System;
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
			btnFavourites.SetTitle("My Favourites", UIControlState.Normal);
			btnReviews.SetBackgroundImage(new UIImage("Images/My.png"), UIControlState.Normal);
			btnTastings.SetBackgroundImage(new UIImage("Images/New.jpg"), UIControlState.Normal);
			btnFavourites.SetBackgroundImage(new UIImage("Images/Top.jpg"), UIControlState.Normal);

			btnReviews.TouchUpInside += (sender, e) =>
			{
				var MyReview = new MyTastingViewController();
				NavigationController.PushViewController(MyReview, false);
			};

			btnTastings.TouchUpInside += (sender, e) =>
			{
				var MyTaste = new MyTastingViewController();
				NavigationController.PushViewController(MyTaste, false);
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
				NavigationController.PushViewController(new PhyCollectionView(flowLayout, 1, true), false);
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
			NavigationController.NavigationBar.TopItem.Title = "Taste";
		}
	}
}
