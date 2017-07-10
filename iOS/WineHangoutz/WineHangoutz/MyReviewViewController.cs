using Foundation;
using System;
using UIKit;
using PatridgeDev;
using CoreGraphics;
using System.Collections.Generic;
using Hangout.Models;
using System.Linq;
using BigTed;

namespace WineHangoutz
{
	public partial class MyReviewViewController : UITableViewController, IPopupParent
	{
		private string screenid = "MyReviewView Controller";
		public int storeid;
		public UILabel Noreviews;
		public UIImageView ImgIns;
		ServiceWrapper sw = new ServiceWrapper();

		public MyReviewViewController(IntPtr handle) : base(handle)
		{

		}
		public MyReviewViewController() : base()
		{
		}
		public override void ViewDidLoad()
		{
			try
			{
				LoggingClass.LogInfo("Entered into MyReviews View", screenid);
				int userId = Convert.ToInt32(CurrentUser.RetreiveUserId());
				var myData = sw.GetItemReviewUID(userId).Result;
				var data = sw.GetMyTastingsList(userId).Result;
				Noreviews = new UILabel();
				if (data.TastingList.Count != 0)
				{
					Noreviews.Text = "You have tasted " + data.TastingList.Count + " wines.\n We would love to hear your feedback.";
				}
				else
				{
					Noreviews.Text = "Please taste and then review.";
				}
				Noreviews.TextAlignment = UITextAlignment.Center;
				Noreviews.LineBreakMode = UILineBreakMode.WordWrap;
				Noreviews.Lines = 0;
				Noreviews.TextColor = UIColor.Black;
				CGSize sTemp = new CGSize(View.Frame.Width, 100);
				sTemp = Noreviews.SizeThatFits(sTemp);
				Noreviews.Frame = new CGRect(0, 50, View.Frame.Width, sTemp.Height);
				ImgIns = new UIImageView();
				ImgIns.Image=UIImage.FromFile("ReviewIns.png");
				ImgIns.Frame=new CGRect((View.Frame.Width / 2)-150, 120, 300, 300);
				if (myData.Reviews.Count == 0)
				{
					TableView.SeparatorColor = UIColor.Clear;
					View.AddSubview(Noreviews);
					View.AddSubview(ImgIns);
				}
				else
				{
					TableView.AllowsSelection = false;
					TableView.Source = new MyReviewTableSource(myData.Reviews.ToList(), NavigationController, this);
				}
			}
			catch (Exception ex)
			{
				LoggingClass.LogError(ex.Message, screenid, ex.StackTrace);
			}
		}
		public void RefreshParent()
		{
			ServiceWrapper svc = new ServiceWrapper();
			int userId = Convert.ToInt32(CurrentUser.RetreiveUserId());
			var myData = svc.GetItemReviewUID(userId).Result;
			if (myData.Reviews.Count == 0)
				{
					TableView.SeparatorColor = UIColor.Clear;
					View.AddSubview(Noreviews);
					View.AddSubview(ImgIns);
				}
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
		UIButton imageView;
		PDRatingView stars;
		UIButton btnEdit;
		UIButton btnDelete;
		UILabel WineIdLabel;
		UIButton ReadMore;

		public UINavigationController NavController;
		public UIViewController Parent;
		public string wineId;
		private string screenid = "MyReviewCellView";
		public int storeid;
		ServiceWrapper sw = new ServiceWrapper();
		public MyReviewCellView(NSString cellId) : base(UITableViewCellStyle.Default, cellId)
		{
			try
			{
				SelectionStyle = UITableViewCellSelectionStyle.Gray;

				//LoggingClass.UploadErrorLogs();
				//ContentView.BackgroundColor = UIColor.FromRGB(218, 255, 127);
				imageView = new UIButton();
				imageView.AutoresizingMask = UIViewAutoresizing.FlexibleHeight | UIViewAutoresizing.FlexibleWidth;
				imageView.ContentMode = UIViewContentMode.Center;
				imageView.ClipsToBounds = true;

				imageView.TouchDown += (object sender, EventArgs e) =>
				{
					BTProgressHUD.Show("Loading...");
				};
				imageView.TouchUpInside += (object sender, EventArgs e) =>
				{
					NavController.PushViewController(new DetailViewController(WineIdLabel.Text, storeid.ToString(), false), false);
				};
				Review review = new Review();

				separator = new UIImageView();
				WineName = new UITextView()
				{
					Font = UIFont.FromName("Verdana", 14f),
					TextColor = UIColor.FromRGB(127, 51, 0),
					BackgroundColor = UIColor.Clear,
					Editable = false

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
				ReadMore = new UIButton()
				{
					Font = UIFont.FromName("Verdana", 10f),
					BackgroundColor = UIColor.White
				};
				Vintage = new UILabel()
				{
					Font = UIFont.FromName("Verdana", 10f),
					TextColor = UIColor.FromRGB(127, 51, 100),
					BackgroundColor = UIColor.Clear
				};

				decimal averageRating = 0.0m;
				var ratingConfig = new RatingConfig(emptyImage: UIImage.FromBundle("Stars/star-silver2.png"),
							filledImage: UIImage.FromBundle("Stars/star.png"),
							chosenImage: UIImage.FromBundle("Stars/star.png"));

				stars = new PDRatingView(new CGRect(110, 60, 60, 20), ratingConfig, averageRating);

				btnEdit = new UIButton();
				//UIViewController that = Parent;
				btnEdit.SetImage(UIImage.FromFile("edit.png"), UIControlState.Normal);
				btnEdit.TouchUpInside += (sender, e) =>
				{
					PopupView yourController = new PopupView(WineIdLabel.Text, storeid);
					yourController.NavController = NavController;
					yourController.parent = Parent;
					yourController.StartsSelected = stars.AverageRating;
					yourController.Comments = Comments.Text;
					LoggingClass.LogInfo("Edited the review of " + wineId, screenid);


					//yourController.WineId = Convert.ToInt32(WineIdLabel.Text);
					yourController.ModalPresentationStyle = UIModalPresentationStyle.OverCurrentContext;
					//this.PresentViewController(yourController, true, null);
					Parent.PresentModalViewController(yourController, false);
				};
				btnDelete = new UIButton();
				btnDelete.SetImage(UIImage.FromFile("delete.png"), UIControlState.Normal);

				btnDelete.TouchUpInside += (sender, e) =>
				{
					UIAlertView alert = new UIAlertView()
					{
						Title = "Delete Review ",
						Message = "Do you want to delete this review?"
					};
					alert.AddButton("Yes");
					alert.AddButton("No");

					alert.Clicked += async (senderalert, buttonArgs) =>
					{

						if (buttonArgs.ButtonIndex == 0)
						{
							review.Barcode = WineIdLabel.Text;
							review.ReviewUserId = Convert.ToInt32(CurrentUser.RetreiveUserId());
							BTProgressHUD.Show("Deleting review");
							await sw.DeleteReview(review);
							LoggingClass.LogInfo("Deleting the review of " + wineId, screenid);
							BTProgressHUD.ShowSuccessWithStatus("Done");
							((IPopupParent)Parent).RefreshParent();
						}
					};

					alert.Show();

				};

				WineIdLabel = new UILabel();
				ContentView.AddSubviews(new UIView[] { WineName, ReviewDate, Comments, stars, imageView, Vintage,separator, btnEdit, btnDelete });

			}
			catch (Exception ex)
			{
				LoggingClass.LogError(ex.ToString(), screenid, ex.StackTrace);
			}
		}

		public void UpdateCell(Review review)
		{
			try
			{

				imageView.SetImage(BlobWrapper.GetResizedImage(review.Barcode, new CGRect(0, 0, 100, 155), review.PlantFinal), UIControlState.Normal);
				separator.Image = UIImage.FromFile("separator.png");
				WineName.Text = review.Name + " " + review.Vintage.ToString();
				ReviewDate.Text = review.Date.ToString("MM-dd-yyyy");
				Comments.Text = review.RatingText;
				//CGSize sTemp = new CGSize(ContentView.Frame.Width, 100);
				//sTemp = Comments.SizeThatFits(sTemp);
				if (review.RatingText.Length > 100)
				{
					//ContentView.AddSubview(ReadMore);
					ReadMore.TouchUpInside += delegate {
						{
							UIAlertView alert = new UIAlertView()
							{
								Title = review.RatingText,
								//Message = "Coming Soon..."
							};

							alert.AddButton("OK");
							alert.Show();
						};
					};
					//ReadMore.Hidden = false;
				}
				//Vintage.Text = " ";//"Vintage:"+review.Vintage.ToString();
				storeid = Convert.ToInt32(review.PlantFinal);
				WineIdLabel.Text = review.Barcode.ToString();
				ReadMore.SetTitle("... Read More", UIControlState.Normal);
				ReadMore.SetTitleColor(UIColor.Blue, UIControlState.Normal);

				//ReadMore.Hidden = true;
					
				//stars = new PDRatingView(new CGRect(150, 2, 60, 20), ratingConfig, review.Stars);
				//ContentView.Bounds.Height = 90;
				stars.AverageRating = Convert.ToDecimal(review.RatingStars);
				//btnEdit.SetBackgroundImage(UIImage.FromFile("edit.png"), UIControlState.Normal);
				//btnDelete.SetBackgroundImage(UIImage.FromFile("delete.png"), UIControlState.Normal);
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
				WineName.Frame = new CGRect(imageWidth - 4, 2, ContentView.Bounds.Width - imageWidth - 60, 60);
				//Vintage.Frame = new CGRect(imageWidth, 43, ContentView.Bounds.Width - imageWidth, 15);
				separator.Frame = new CGRect(imageWidth, 79, WineName.Frame.Width-100, 3);
				ReviewDate.Frame = new CGRect(imageWidth, 85, ContentView.Bounds.Width - imageWidth, 20);
				//stars.Frame = new CGRect(35, 50, 100, 20);
				stars.UserInteractionEnabled = false;
				Comments.Frame = new CGRect(imageWidth - 4, 99, ContentView.Bounds.Width - imageWidth - 2, 70);
				btnEdit.Frame = new CGRect(ContentView.Bounds.Width - 60, 10, 25, 25);
				btnDelete.Frame = new CGRect(ContentView.Bounds.Width - 30, 10, 25, 25);
				ReadMore.Frame = new CGRect(ContentView.Bounds.Width - 80, 150, 75, 20);
			}
			catch (Exception ex)
			{
				LoggingClass.LogError(ex.ToString(), screenid, ex.StackTrace);
			}
		}


	}

}