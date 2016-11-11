using Foundation;
using System;
using UIKit;
using System.Collections.Generic;
using CoreGraphics;
using ObjCRuntime;

namespace WineHangoutz
{
    public partial class PhyCollectionView : UICollectionViewController
    {
        public PhyCollectionView (UICollectionViewLayout layout) : base (layout)
        {
			
        }

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

			CollectionView.RegisterClassForCell(typeof(APLCollectionViewCell), APLCollectionViewCell.Key);

		}

		public override UICollectionViewCell GetCell(UICollectionView collectionView, NSIndexPath indexPath)
		{
			var cell = collectionView.DequeueReusableCell(APLCollectionViewCell.Key, indexPath) as APLCollectionViewCell;
			cell.NavigationController = NavigationController;
			//cell.ImageView.Image = UIImage.FromFile ("Images/sa" + indexPath.Item + ".jpg");
			//cell.ImageView.Image = UIImage.FromFile("placeholder.jpeg");
			cell.btlImage.SetBackgroundImage(UIImage.FromFile("Wines/wine" + indexPath.Item % 8 + ".png"), UIControlState.Normal);

			return cell;
		}


		public override nint NumberOfSections(UICollectionView collectionView)
		{
			return 1;
		}

		public override nint GetItemsCount(UICollectionView collectionView, nint section)
		{
			return 20;
		}
		public override void PerformAction(UICollectionView collectionView, Selector action, NSIndexPath indexPath, NSObject sender)
		{
			System.Diagnostics.Debug.WriteLine("code to perform action");
			NavigationController.PushViewController(new PopupView(), false);
		}

	}
}