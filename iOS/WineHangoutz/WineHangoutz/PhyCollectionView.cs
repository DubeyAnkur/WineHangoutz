using Foundation;
using System;
using UIKit;
using System.Collections.Generic;
using CoreGraphics;
using ObjCRuntime;
using Hangout.Models;

namespace WineHangoutz
{
    public partial class PhyCollectionView : UICollectionViewController
    {

		public ItemListResponse myData;
		public int storeId = 2;
        public PhyCollectionView (UICollectionViewLayout layout) : base (layout)
        {

        }

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

			//Uncomment below lines once services are fixed.
			ServiceWrapper svc = new ServiceWrapper();
			myData = svc.GetItemList(storeId).Result;

			CollectionView.RegisterClassForCell(typeof(APLCollectionViewCell), APLCollectionViewCell.Key);

		}

		public override UICollectionViewCell GetCell(UICollectionView collectionView, NSIndexPath indexPath)
		{
			var cell = collectionView.DequeueReusableCell(APLCollectionViewCell.Key, indexPath) as APLCollectionViewCell;
			BindData(cell, indexPath);
			return cell;
		}


		public override nint NumberOfSections(UICollectionView collectionView)
		{
			return 1;
		}

		public override nint GetItemsCount(UICollectionView collectionView, nint section)
		{
			return myData.ItemList.Count;
		}
		public override void PerformAction(UICollectionView collectionView, Selector action, NSIndexPath indexPath, NSObject sender)
		{
			//This do not work.
			System.Diagnostics.Debug.WriteLine("code to perform action");
			NavigationController.PushViewController(new PopupView(), false);
		}
		public void BindData(APLCollectionViewCell cell, NSIndexPath indexPath)
		{
			cell.NavigationController = NavigationController;
			cell.btlImage.SetBackgroundImage(UIImage.FromFile("Wines/wine" + indexPath.Item % 8 + ".png"), UIControlState.Normal);

			int index = (int)indexPath.Item;

			//Data from Model
			//cell.WineName = myData.ItemList[index].Name;
			cell.Vintage = myData.ItemList[index].Vintage.ToString();
			cell.RegPrice = myData.ItemList[index].SalePrice.ToString();
			cell.averageRating = (decimal)myData.ItemList[index].AverageRating;

			cell.lblName.Text = myData.ItemList[index].Name;
			cell.lblYear.Text= myData.ItemList[index].Vintage.ToString();
			cell.lblRegPrice.Text= myData.ItemList[index].SalePrice.ToString();
			cell.ratingView.AverageRating = (decimal)myData.ItemList[index].AverageRating;
		}
	}
}