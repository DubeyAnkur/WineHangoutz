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
	public partial class MyReviewViewController : UITableViewController, IPopupParent
	{
		public MyReviewViewController(IntPtr handle) : base(handle)
		{

		}
		public MyReviewViewController() : base()
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
			TableView.Source = new MyReviewTableSource(myData.Reviews.ToList(), NavigationController, this);
		}
		public void RefreshParent()
		{
			ServiceWrapper svc = new ServiceWrapper();
			int userId = Convert.ToInt32(CurrentUser.RetreiveUserId());
			var myData = svc.GetItemReviewUID(userId).Result;
			TableView.Source = new MyReviewTableSource(myData.Reviews.ToList(), NavigationController, this);
			TableView.ReloadData();

		}
	}

	public class MyReviewTableSource : UITableViewSource
	{

		List<Review> TableItems;
		string CellIdentifier = "TableCell";
		UINavigationController NavController;
		UIViewController Parent;

		public MyReviewTableSource(List<Review> items, UINavigationController NavigationController, UIViewController parent)
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
			MyReviewCellView cell = tableView.DequeueReusableCell(CellIdentifier) as MyReviewCellView;
			Review item = TableItems[indexPath.Row];


			//---- if there are no cells to reuse, create a new one
			NSString name = new NSString(CellIdentifier);
			if (cell == null)
				cell = new MyReviewCellView(name);

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

	public class MyReviewCellView : UITableViewCell
	{
		UITextView WineName;
		UILabel ReviewDate;
		UITextView Comments;
		UILabel Vintage;
		UIImageView separator;
		UIImageView imageView;
		PDRatingView stars;
		UIButton btnEdit;
		UIButton btnDelete;
		UILabel WineIdLabel;

		public UINavigationController NavController;
		public UIViewController Parent;
		public int wineId;

		public MyReviewCellView(NSString cellId) : base(UITableViewCellStyle.Default, cellId)
		{
			SelectionStyle = UITableViewCellSelectionStyle.Gray;
			//ContentView.BackgroundColor = UIColor.FromRGB(218, 255, 127);
			imageView = new UIImageView();
			imageView.AutoresizingMask = UIViewAutoresizing.FlexibleHeight | UIViewAutoresizing.FlexibleWidth;
			imageView.ContentMode = UIViewContentMode.Center;
			imageView.ClipsToBounds = true;
			Review review = new Review();

			separator = new UIImageView();
			WineName = new UITextView()
			{
				Font = UIFont.FromName("Verdana", 14f),
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
				TextAlignment = UITextAlignment.Justified,
				//TextAlignment = UITextAlignment.Natural,
				BackgroundColor = UIColor.Clear,
				//LineBreakMode = UILineBreakMode.WordWrap
				Editable = false,
				Selectable = false
			};
			Vintage = new UILabel()
			{
				Font = UIFont.FromName("Verdana", 10f),
				TextColor = UIColor.FromRGB(127, 51, 100),
				BackgroundColor = UIColor.Clear
			};

			decimal averageRating = 0.0m;
			var ratingConfig = new RatingConfig(emptyImage: UIImage.FromBundle("Stars/empty.png"),
												filledImage: UIImage.FromBundle("Stars/filled.png"),
												chosenImage: UIImage.FromBundle("Stars/chosen.png"));

			stars = new PDRatingView(new CGRect(110, 60, 60, 20), ratingConfig, averageRating);

			btnEdit = new UIButton();
			//UIViewController that = Parent;

			btnEdit.TouchUpInside += (sender, e) =>
			{
				PopupView yourController = new PopupView(Convert.ToInt32(WineIdLabel.Text));
				yourController.NavController = NavController;
				yourController.parent = Parent;
				yourController.StartsSelected = stars.AverageRating;
				yourController.Comments = Comments.Text;
				//yourController.WineId = Convert.ToInt32(WineIdLabel.Text);
				yourController.ModalPresentationStyle = UIModalPresentationStyle.OverCurrentContext;
				//this.PresentViewController(yourController, true, null);
				Parent.PresentModalViewController(yourController, false);
			};
			btnDelete = new UIButton();


			btnDelete.TouchUpInside += (sender, e) =>
			{
				//////DeletePopup deleteController = new DeletePopup(Convert.ToInt32(WineIdLabel.Text));
				//////deleteController.NavController = NavController;
				//////deleteController.parent = Parent;
				//yourController.StartsSelected = stars.AverageRating;
				//yourController.Comments = Comments.Text;
				//yourController.WineId = Convert.ToInt32(WineIdLabel.Text);
				//yourController.ModalPresentationStyle = UIModalPresentationStyle.OverCurrentContext;
				//this.PresentViewController(yourController, true, null);
				//////Parent.PresentModalViewController(deleteController, false);

				//Show the pop, Are you sure. With Yes & No Button.
				//If Yes, Then delete, lse close the popup.

				UIAlertView alert = new UIAlertView()
				{
					Title = "Delete Review",
					Message = "Do you want to delete this review."
				};
				alert.AddButton("Yes");
				alert.AddButton("No");

				alert.Clicked += async (senderalert, buttonArgs) =>
				{
					if (buttonArgs.ButtonIndex == 0)
					{
						//Review review = new Review();
						ServiceWrapper sw = new ServiceWrapper();
						review.WineId = Convert.ToInt32(WineIdLabel.Text);

						review.ReviewUserId = Convert.ToInt32(CurrentUser.RetreiveUserId());

						await sw.DeleteReview(review);

						((IPopupParent)Parent).RefreshParent();
					}
				};

				alert.Show();

			};






			WineIdLabel = new UILabel();
			ContentView.AddSubviews(new UIView[] { WineName, ReviewDate, Comments, stars, imageView, Vintage, separator, btnEdit, btnDelete });

		}

		public void UpdateCell(Review review)
		{
			imageView.Image = BlobWrapper.GetResizedImage(review.WineId.ToString(), new CGRect(0, 0, 100, 155));
			separator.Image = UIImage.FromFile("separator.png");
			WineName.Text = review.Name;
			ReviewDate.Text = review.Date.ToString("d");
			Comments.Text = review.RatingText;
			Vintage.Text = review.Vintage.ToString();
			WineIdLabel.Text = review.WineId.ToString();
			//stars = new PDRatingView(new CGRect(150, 2, 60, 20), ratingConfig, review.Stars);
			//ContentView.Bounds.Height = 90;
			stars.AverageRating = Convert.ToDecimal(review.RatingStars);
			btnEdit.SetBackgroundImage(UIImage.FromFile("edit.png"), UIControlState.Normal);
			btnDelete.SetBackgroundImage(UIImage.FromFile("delete.png"), UIControlState.Normal);
		}
		public override void LayoutSubviews()
		{
			base.LayoutSubviews();
			int imageWidth = 110; // + 10;
			imageView.Frame = new CGRect(5, 5, imageWidth - 10, 155);
			WineName.Frame = new CGRect(imageWidth - 4, 2, ContentView.Bounds.Width - imageWidth - 60, 60);
			Vintage.Frame = new CGRect(imageWidth, 43, ContentView.Bounds.Width - imageWidth, 15);
			separator.Frame = new CGRect(imageWidth, 79, ContentView.Bounds.Width - imageWidth, 3);
			ReviewDate.Frame = new CGRect(imageWidth, 85, ContentView.Bounds.Width - imageWidth, 20);
			//stars.Frame = new CGRect(35, 50, 100, 20);
			stars.UserInteractionEnabled = false;
			Comments.Frame = new CGRect(imageWidth - 4, 99, ContentView.Bounds.Width - imageWidth - 2, 70);
			btnEdit.Frame = new CGRect(ContentView.Bounds.Width - 60, 10, 25, 25);
			btnDelete.Frame = new CGRect(ContentView.Bounds.Width - 30, 10, 25, 25);
		}


	}

}