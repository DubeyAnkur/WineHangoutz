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
		public UIRefreshControl refreshControl = new UIRefreshControl();
		UIImage img = new UIImage("Wines/bottle.jpg");
		//public int userId = 2;

		public bool FaviouriteView = false;
		public PhyCollectionView(UICollectionViewLayout layout, int StoreId, bool favView = false) : base(layout)
		{

			FaviouriteView = favView;
			if (favView == true)
			{
				this.Title = "My Favorites";
			}
			else
			{
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
			}

		}

		public override void ViewDidLoad()
		{
			try
			{
				UIImageView ImgIns = new UIImageView();
				ImgIns.Image=UIImage.FromFile("FavIns.png");


				UILabel NoFav = new UILabel();
				NoFav.Text = "All your favorite wines will be listed here.\nTo mark a wine as favorite select the heart icon.";
				NoFav.LineBreakMode = UILineBreakMode.WordWrap;
				NoFav.Lines = 0;
				CGSize sTemp = new CGSize(View.Frame.Width, 100);
				sTemp = NoFav.SizeThatFits(sTemp);
				NoFav.Frame = new CGRect(0, 50, View.Frame.Width-20, sTemp.Height);
				NoFav.TextAlignment = UITextAlignment.Center;
				//NoFav.TextAlignment = UITextAlignment.Justified;
				ImgIns.Frame=new CGRect((View.Frame.Width / 2) - 100, 50+sTemp.Height+20, 202, 381);
				ServiceWrapper svc = new ServiceWrapper();
				if (FaviouriteView)
				{

					LoggingClass.LogInfo("Entered into favorite", screenid);
					myData = svc.GetItemFavsUID(CurrentUser.RetreiveUserId()).Result;
     				// this.CollectionView.Add(refreshControl);
					//refreshControl.ValueChanged += (rcSender, e) =>
					//{
					////Refresh this view
					//myData = svc.GetItemFavsUID(CurrentUser.RetreiveUserId()).Result;
					//CollectionView.ReloadData();
					//refreshControl.EndRefreshing();
					//};
					fav = true;
					if (myData.ItemList.Count == 0)
					{
						CollectionView.AddSubview(NoFav);
						CollectionView.AddSubview(ImgIns);
					}
				}
				else
				{
					myData = svc.GetItemLists(storeId, CurrentUser.RetreiveUserId()).Result;
					this.CollectionView.Add(refreshControl);
					refreshControl.ValueChanged += (rcSender, e) =>
					{
					//Refresh this view
					myData = svc.GetItemLists(storeId, CurrentUser.RetreiveUserId()).Result;
					CollectionView.ReloadData();
					refreshControl.EndRefreshing();
					};
				}

				BTProgressHUD.Dismiss();
				this.View.BackgroundColor = new UIColor(256, 256, 256, 0.8f);
				this.CollectionView.BackgroundColor = UIColor.White;
				CollectionView.RegisterClassForCell(typeof(APLCollectionViewCell), APLCollectionViewCell.Key);
				
			}
			catch (Exception ex)
			{
				BTProgressHUD.ShowErrorWithStatus("Something went wrong,We're on it.");
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
					Message = "Something went wrong. We are on it"
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
				//cell.AmountLeft.SetProgress(Convert.ToSingle(myData.ItemList[index].AvailableVolume),true);
				cell.Vintage = myData.ItemList[index].Vintage.ToString();
				cell.AmountLeft.Text="Wine left in bottle: "+myData.ItemList[index].AvailableVolume.ToString() + ".ml";
				cell.RegPrice = myData.ItemList[index].SalePrice.ToString();
				cell.averageRating = (decimal)myData.ItemList[index].AverageRating;
				cell.WineBarcode = myData.ItemList[index].Barcode;
				if (fav == true)
				{
					cell.storeId = myData.ItemList[index].PlantFinal.ToString();
					cell.AmountLeft.Hidden = true;
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
					Title = "Something went wrong. We are on it."
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