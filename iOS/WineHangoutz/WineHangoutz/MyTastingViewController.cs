using Foundation;
using System;
using UIKit;
using PatridgeDev;
using CoreGraphics;
using System.Collections.Generic;
using Hangout.Models;
using System.Linq;

namespace WineHangoutz
{
    public partial class MyTastingViewController : UITableViewController, IPopupParent
    {
		private int screenid = 14;
        public MyTastingViewController (IntPtr handle) : base (handle)
        {
        }
		public MyTastingViewController() : base()
		{
		}
		public override void ViewDidLoad()
		{
			ServiceWrapper svc = new ServiceWrapper();
			int userId = Convert.ToInt32(CurrentUser.RetreiveUserId());
			if (userId == 0)
			{
				UIAlertView alert = new UIAlertView()
				{
					Title = "This feature is allowed only for VIP Card holders",
					//Message = "Coming Soon..."
				};

				alert.AddButton("OK");
				alert.Show();
			}
			else
			{
				var myData = svc.GetMyTastingsList(userId).Result;
				if (myData.TastingList.Count == 0)
				{
					UIAlertView alert = new UIAlertView()
					{
						Title = "Please start tasting our amazing selection of wines.",
						//Message = "Coming Soon..."
					};
					LoggingClass.LogInfo("There are no tastings for this user " + CurrentUser.RetreiveUserName(), screenid);
					alert.AddButton("OK");
					alert.Show();
				}
				TableView.AllowsSelection = false;
				TableView.Source = new MyTastingTableSource(myData.TastingList.ToList(), NavigationController, this);
			}
		}
		public void RefreshParent()
		{
			ServiceWrapper svc = new ServiceWrapper();
			int userId = Convert.ToInt32(CurrentUser.RetreiveUserId());
			var myData = svc.GetMyTastingsList(userId).Result;
			TableView.Source = new MyTastingTableSource(myData.TastingList.ToList(), NavigationController, this);
			TableView.ReloadData();
			
		}
    }

	public class MyTastingTableSource : UITableViewSource
	{

		List<Tastings> TableItems;
		string CellIdentifier = "TableCell";
		UINavigationController NavController;
		UIViewController Parent;

		public MyTastingTableSource(List<Tastings>  items, UINavigationController NavigationController, UIViewController parent)
		{
			TableItems = items;
			NavController = NavigationController;
			Parent = parent;
		}

		public override nint RowsInSection(UITableView tableview, nint section)
		{
			return TableItems.Count;
		}

		public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
		{
			MyTastingCellView cell = tableView.DequeueReusableCell(CellIdentifier) as MyTastingCellView;
			Tastings item = TableItems[indexPath.Row];


			//---- if there are no cells to reuse, create a new one
			NSString name = new NSString(CellIdentifier);
			if (cell == null)
				cell = new MyTastingCellView(name);

			cell.NavController = NavController;
			cell.Parent = Parent;

			cell.UpdateCell(item);
			cell.SetNeedsDisplay();

			return cell;
		}
		public override nfloat GetHeightForRow(UITableView tableView, NSIndexPath indexPath)
		{
			return 180f;
		}
	}

	public class MyTastingCellView : UITableViewCell
	{
		UILabel WineName;
		UILabel ReviewDate;
		UILabel Vintage;
		UIImageView separator;
		UIButton imageView;
		UILabel WineIdLabel;
		Tastings r = new Tastings();
		public UINavigationController NavController;
		public UIViewController Parent;
		private int screenid = 7;
		public int storeid;
		public MyTastingCellView(NSString cellId) : base(UITableViewCellStyle.Default, cellId)
		{
			try
			{
				SelectionStyle = UITableViewCellSelectionStyle.Gray;


				LoggingClass.LogInfo("Entered Into Tasting View", screenid);


				//ContentView.BackgroundColor = UIColor.FromRGB(218, 255, 127);
				imageView = new UIButton();
				imageView.AutoresizingMask = UIViewAutoresizing.FlexibleHeight | UIViewAutoresizing.FlexibleWidth;
				imageView.ContentMode = UIViewContentMode.Center;
				imageView.ClipsToBounds = true;

				imageView.TouchUpInside += (object sender, EventArgs e) =>
				{
					
					r.PlantFinal =storeid;
					NavController.PushViewController(new SKUDetailView(WineIdLabel.Text,storeid.ToString()), false);
				};
				separator = new UIImageView();
				WineName = new UILabel()
				{
					Font = UIFont.FromName("Verdana", 14f),
					TextColor = UIColor.FromRGB(127, 51, 0),
					BackgroundColor = UIColor.Clear,

				};
				ReviewDate = new UILabel()
				{
					Font = UIFont.FromName("AmericanTypewriter", 10f),
					TextColor = UIColor.FromRGB(38, 127, 200),
					//TextAlignment = UITextAlignment.Center,
					BackgroundColor = UIColor.Clear
				};
				Vintage = new UILabel()
				{
					Font = UIFont.FromName("Verdana", 10f),
					TextColor = UIColor.FromRGB(127, 51, 100),
					BackgroundColor = UIColor.Clear
				};

				WineIdLabel = new UILabel();
				ContentView.AddSubviews(new UIView[] { WineName, ReviewDate, imageView, Vintage, separator });
			}
			catch (Exception ex)
			{
				LoggingClass.LogError(ex.ToString(), screenid, ex.StackTrace);
			}
		}
        public void UpdateCell(Tastings review)
		{
			
			try
			{
				imageView.SetImage(BlobWrapper.GetResizedImage(r.Barcode.ToString(), new CGRect(0, 0, 100, 155),r.PlantFinal.ToString()), UIControlState.Normal);
				separator.Image = UIImage.FromFile("separator.png");
				WineName.Text = review.Name;
				ReviewDate.Text = review.TastingDate.ToString("MM-dd-yyyy");
				Vintage.Text = review.Vintage.ToString();
				WineIdLabel.Text = review.WineId.ToString();
				//stars = new PDRatingView(new CGRect(150, 2, 60, 20), ratingConfig, review.Stars);
				//ContentView.Bounds.Height = 90;
			}
			catch (Exception ex)
			{
				LoggingClass.LogError(ex.ToString(), screenid, ex.StackTrace);
			}
		}
		public override void LayoutSubviews()
		{
			try
			{

				base.LayoutSubviews();
				int imageWidth = 110; // + 10;
				imageView.Frame = new CGRect(5, 5, imageWidth - 10, 155);
				WineName.Frame = new CGRect(imageWidth, 2, ContentView.Bounds.Width - imageWidth, 60);
				Vintage.Frame = new CGRect(imageWidth, 62, ContentView.Bounds.Width - imageWidth, 15);
				separator.Frame = new CGRect(imageWidth, 79, ContentView.Bounds.Width - imageWidth, 3);
				ReviewDate.Frame = new CGRect(imageWidth, 100, ContentView.Bounds.Width - imageWidth, 20);
				//stars.Frame = new CGRect(35, 50, 100, 20);
			}
			catch (Exception ex)
			{
				LoggingClass.LogError(ex.ToString(), screenid, ex.StackTrace);
			}
		}
	}
}