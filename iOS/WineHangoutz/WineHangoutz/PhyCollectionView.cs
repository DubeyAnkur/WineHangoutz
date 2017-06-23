using Foundation;
using System;
using UIKit;
using System.Collections.Generic;
using CoreGraphics;
using ObjCRuntime;
using Hangout.Models;
using System.Globalization;
using BigTed;

namespace WineHangoutz
{
	public partial class PhyCollectionView : UICollectionViewController
	{
		private int screenid = 12;

		public ItemListResponse myData;
		public int storeId = 2;
		Boolean fav = false;
		UIImage img = new UIImage("Wines/bottle.jpg");
		//public int userId = 2;

		public bool FaviouriteView = false;
		public PhyCollectionView(UICollectionViewLayout layout, int StoreId, bool favView = false) : base(layout)
		{

			FaviouriteView = favView;
			if (StoreId == 1)
			{
				this.Title = "Wall";
				storeId = StoreId;
			}
			else if (StoreId == 2)
			{
				this.Title = "Pt. Pleasant Beach";
				storeId = StoreId;
			}
			else
			{
				this.Title = "My favourites";

			}

		}

		public override void ViewDidLoad()
		{
			//AboutController1.ViewDidLoad(base);
			try
			{


				ServiceWrapper svc = new ServiceWrapper();
				if (FaviouriteView)
				{

					LoggingClass.LogInfo("Entered into favorite", screenid);
					myData = svc.GetItemFavsUID(CurrentUser.RetreiveUserId()).Result;
					fav = true;
					if (myData.ItemList.Count == 0)
					{
						UIAlertView alert = new UIAlertView()
						{
							Title = "Pick your favorites and we will notify you when it is available for tasting.",
							//Message = "Coming Soon..."
						};
						//LoggingClass.LogInfo("Entered into seacuces", screenid);


						alert.AddButton("OK");
						alert.Show();
					}
				}
				else

					myData = svc.GetItemLists(storeId, CurrentUser.RetreiveUserId()).Result;

				BTProgressHUD.Dismiss();
				this.View.BackgroundColor = new UIColor(256, 256, 256, 0.8f);
				this.CollectionView.BackgroundColor = UIColor.White;
				CollectionView.RegisterClassForCell(typeof(APLCollectionViewCell), APLCollectionViewCell.Key);
			}
			catch (Exception ex)
			{
				LoggingClass.LogError(ex.ToString(), screenid, ex.StackTrace);
			}
		}

		public override void ViewDidAppear(bool animated)
		{
			base.ViewDidAppear(animated);
		}

		public static class Cultures
		{
			public static readonly CultureInfo UnitedState =
					CultureInfo.GetCultureInfo("en-US");
		}

		public override UICollectionViewCell GetCell(UICollectionView collectionView, NSIndexPath indexPath)
		{
			var cell = collectionView.DequeueReusableCell(APLCollectionViewCell.Key, indexPath) as APLCollectionViewCell;
			BindData(cell, indexPath, fav);

			cell.Layer.BorderWidth = 1;
			cell.Layer.BorderColor = new CGColor(0.768f, 0.768f, 0.768f);

			return cell;
		}


		public override nint NumberOfSections(UICollectionView collectionView)
		{
			return 1;
		}

		public override nint GetItemsCount(UICollectionView collectionView, nint section)
		{
			nint cou = 0;
			try
			{
				cou = myData.ItemList.Count;
				//myData.ErrorDescription
			}
			catch (Exception ex)
			{
				LoggingClass.LogError(ex.Message, screenid, ex.StackTrace.ToString());
				UIAlertView alert = new UIAlertView()
				{
					Title = "Sorry",
					Message = "We are under maintenance"
				};

				alert.AddButton("OK");
				alert.Show();
			}
			return cou;
		}
		public override void PerformAction(UICollectionView collectionView, Selector action, NSIndexPath indexPath, NSObject sender)
		{
			//This do not work.
			System.Diagnostics.Debug.WriteLine("code to perform action");
			//NavigationController.PushViewController(new PopupView(), false);
		}
		public void BindData(APLCollectionViewCell cell, NSIndexPath indexPath, Boolean fav)
		{
			try
			{
				cell.NavigationController = NavigationController;
				//cell.btlImage.SetBackgroundImage(UIImage.FromFile("Wines/wine" + indexPath.Item % 8 + ".png"), UIControlState.Normal);
				cell.parent = this.View;


				int index = (int)indexPath.Item;

				//Data from Model
				//cell.WineName = myData.ItemList[index].Name;
				cell.Vintage = myData.ItemList[index].Vintage.ToString();
				cell.RegPrice = myData.ItemList[index].SalePrice.ToString();
				cell.averageRating = (decimal)myData.ItemList[index].AverageRating;
				cell.WineId = myData.ItemList[index].Barcode;
				if (fav == true)
				{
					cell.storeId = myData.ItemList[index].PlantFinal.ToString();
				}
				else
				{
					cell.storeId = storeId.ToString();
				}
				cell.lblName.Text = myData.ItemList[index].Name;
				cell.lblYear.Text = myData.ItemList[index].Vintage.ToString();
				cell.lblRegPrice.Text = myData.ItemList[index].RegPrice.ToString("C", Cultures.UnitedState);
				cell.ratingView.AverageRating = (decimal)myData.ItemList[index].AverageRating;
				cell.myItem = myData.ItemList[index];

				cell.btnItemname.SetTitle(myData.ItemList[index].Name, UIControlState.Normal);
				if (myData.ItemList[index].IsLike == true)
				{
					cell.heartImage.SetImage(UIImage.FromFile("heart_full.png"), UIControlState.Normal);
				}
				else
				{
					cell.heartImage.SetImage(UIImage.FromFile("heart_empty.png"), UIControlState.Normal);
				}
				//UIImage image = BlobWrapper.GetImageBitmapFromWineId(myData.ItemList[index].WineId.ToString());
				UIImage image = BlobWrapper.GetResizedImage(myData.ItemList[index].Barcode.ToString(), cell.btlImage.Bounds, cell.storeId.ToString());
				if (image != null)
				{
					cell.btlImage.SetImage(image, UIControlState.Normal);
				}
				else
					cell.btlImage.SetImage(img, UIControlState.Normal);
			}
			catch (Exception ex)
			{
				LoggingClass.LogError(ex.Message, screenid, ex.StackTrace.ToString());
				UIAlertView alert = new UIAlertView()
				{
					Title = "We are under maintenance",
					//Message = "Coming Soon..."
				};

				alert.AddButton("OK");
				alert.Show();
			}
		}
		public UIImage ResizeImage(UIImage sourceImage, float width, float height)
		{
			UIGraphics.BeginImageContext(new CGSize(width, height));
			sourceImage.Draw(new CGRect(0, 0, width, height));
			var resultImage = UIGraphics.GetImageFromCurrentImageContext();
			UIGraphics.EndImageContext();
			return resultImage;
		}
	}
}