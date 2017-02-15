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
        public MyTastingViewController (IntPtr handle) : base (handle)
        {
        }
		public MyTastingViewController() : base()
		{
		}
		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
			//table = new UITableView(View.Bounds); // defaults to Plain style
			//string[] tableItems = new string[] { "Vegetables", "Fruits", "Flower Buds", "Legumes", "Bulbs", "Tubers" };
			//List<Reviews> tableItems = new List<Reviews>();>

			ServiceWrapper svc = new ServiceWrapper();
			int userId = Convert.ToInt32(CurrentUser.RetreiveUserId());
			var myData = svc.GetItemReviewUID(userId).Result;
			TableView.AllowsSelection = false;
			TableView.Source = new MyTastingTableSource(myData.Reviews.ToList(), NavigationController, this);
		}
		public void RefreshParent()
		{
			ServiceWrapper svc = new ServiceWrapper();
			int userId = Convert.ToInt32(CurrentUser.RetreiveUserId());
			var myData = svc.GetItemReviewUID(userId).Result;
			TableView.Source = new MyTastingTableSource(myData.Reviews.ToList(), NavigationController, this);
			TableView.ReloadData();
			
		}
    }

	public class MyTastingTableSource : UITableViewSource
	{

		List<Review> TableItems;
		string CellIdentifier = "TableCell";
		UINavigationController NavController;
		UIViewController Parent;

		public MyTastingTableSource(List<Review>  items, UINavigationController NavigationController, UIViewController parent)
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
			Review item = TableItems[indexPath.Row];


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
		UIImageView imageView;
		UILabel WineIdLabel;

		public UINavigationController NavController;
		public UIViewController Parent;

		public MyTastingCellView(NSString cellId) : base(UITableViewCellStyle.Default, cellId)
		{
			SelectionStyle = UITableViewCellSelectionStyle.Gray;
			//ContentView.BackgroundColor = UIColor.FromRGB(218, 255, 127);
			imageView = new UIImageView();
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
		public void UpdateCell(Review review)
		{
			imageView.Image = BlobWrapper.GetResizedImage(review.WineId.ToString(), new CGRect(0,0,100,155));
			separator.Image = UIImage.FromFile("separator.png");
			WineName.Text = review.Name;
			ReviewDate.Text = review.Date.ToString("d");
			Vintage.Text = review.Vintage.ToString();
			WineIdLabel.Text = review.WineId.ToString();
			//stars = new PDRatingView(new CGRect(150, 2, 60, 20), ratingConfig, review.Stars);
			//ContentView.Bounds.Height = 90;
		}
		public override void LayoutSubviews()
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
	}
}