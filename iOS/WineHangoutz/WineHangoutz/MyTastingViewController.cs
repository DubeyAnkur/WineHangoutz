using Foundation;
using System;
using UIKit;
using PatridgeDev;
using CoreGraphics;
using System.Collections.Generic;
using Hangout.Models;

namespace WineHangoutz
{
    public partial class MyTastingViewController : UITableViewController
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

			List<Rating> tableItems = new List<Rating>();
			var review = new Rating();
			review.RatingStars = 4.0m;
			review.Country = "France";
			review.Date = DateTime.Now;
			review.Name = "Amarcord D'un Ross";
			review.RatingText = "Seems like a good wine. I opened it two hours before serving. It was deep ruby red in color.";
			review.Vintage = "2014";
			tableItems.Add(review);
			TableView.Source = new MyTastingTableSource(tableItems, NavigationController, this);
		}
    }

	public class MyTastingTableSource : UITableViewSource
	{

		List<Rating> TableItems;
		string CellIdentifier = "TableCell";
		UINavigationController NavController;
		UIViewController Parent;

		public MyTastingTableSource(List<Rating>  items, UINavigationController NavigationController, UIViewController parent)
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
			Rating item = TableItems[indexPath.Row];


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
		UITextView Comments;
		UILabel Vintage;
		UIImageView separator;
		UIImageView imageView;
		PDRatingView stars;
		UIButton btnEdit;
		UIButton btnDelete;

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
				Font = UIFont.FromName("Verdana", 15f),
				TextColor = UIColor.FromRGB(127, 51, 0),
				BackgroundColor = UIColor.Clear
			};
			ReviewDate = new UILabel()
			{
				Font = UIFont.FromName("AmericanTypewriter", 10f),
				TextColor = UIColor.FromRGB(38, 127, 200),
				//TextAlignment = UITextAlignment.Center,
				BackgroundColor = UIColor.Clear
			};
			Comments = new UITextView()
			{
				Font = UIFont.FromName("AmericanTypewriter", 14f),
				TextColor = UIColor.FromRGB(55, 127, 0),
				//TextAlignment = UITextAlignment.Center,
				BackgroundColor = UIColor.Clear
			};
			Vintage = new UILabel()
			{
				Font = UIFont.FromName("Verdana", 10f),
				TextColor = UIColor.FromRGB(127, 51, 100),
				BackgroundColor = UIColor.Clear
			};

			decimal averageRating = 4.0m;
			var ratingConfig = new RatingConfig(emptyImage: UIImage.FromBundle("Stars/empty.png"),
												filledImage: UIImage.FromBundle("Stars/filled.png"),
												chosenImage: UIImage.FromBundle("Stars/chosen.png"));

			stars = new PDRatingView(new CGRect(60, 50, 60, 20), ratingConfig, averageRating);

			btnEdit = new UIButton();
			//UIViewController that = Parent;

			btnEdit.TouchUpInside += (sender, e) =>
			{
				
				PopupView yourController = new PopupView();
				yourController.NavController = NavController;
				yourController.parent = Parent;
				yourController.StartsSelected = averageRating;
				yourController.Comments = Comments.Text;
				yourController.ModalPresentationStyle = UIModalPresentationStyle.OverCurrentContext;
				//this.PresentViewController(yourController, true, null);
				Parent.PresentModalViewController(yourController, false);

			};
			btnDelete = new UIButton();

			ContentView.AddSubviews(new UIView[] { WineName, ReviewDate, Comments, stars, imageView, Vintage, separator, btnEdit, btnDelete });

		}
		public void UpdateCell(Rating review)
		{
			imageView.Image = new UIImage("Wines/wine0.png");
			separator.Image = UIImage.FromFile("separator.png");
			WineName.Text = review.Name;
			ReviewDate.Text = review.Date.ToString("d");
			Comments.Text = review.RatingText;
			Vintage.Text = review.Vintage.ToString();
			//stars = new PDRatingView(new CGRect(150, 2, 60, 20), ratingConfig, review.Stars);
			//ContentView.Bounds.Height = 90;
			stars.AverageRating = Convert.ToDecimal(review.RatingStars);
			btnEdit.SetBackgroundImage(UIImage.FromFile("edit.png"), UIControlState.Normal);
			btnDelete.SetBackgroundImage(UIImage.FromFile("delete.png"), UIControlState.Normal);
		}
		public override void LayoutSubviews()
		{
			base.LayoutSubviews();
			imageView.Frame = new CGRect(5, 5, 50, 155);
			WineName.Frame = new CGRect(60, 2, ContentView.Bounds.Width - 60, 30);
			Vintage.Frame = new CGRect(60, 32, ContentView.Bounds.Width - 60, 15);
			separator.Frame = new CGRect(60, 49, ContentView.Bounds.Width - 60, 3);
			ReviewDate.Frame = new CGRect(60, 70, ContentView.Bounds.Width - 60, 20);
			//stars.Frame = new CGRect(35, 50, 100, 20);
			stars.UserInteractionEnabled = false;
			Comments.Frame = new CGRect(56, 82, ContentView.Bounds.Width - 60, 40);
			btnEdit.Frame = new CGRect(ContentView.Bounds.Width - 60, 10, 25, 25);
			btnDelete.Frame = new CGRect(ContentView.Bounds.Width - 30, 10, 25, 25);
		}
	}
}