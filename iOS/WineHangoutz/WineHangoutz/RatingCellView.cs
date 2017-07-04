using System;
using UIKit;
using Foundation;
using CoreGraphics;
using System.Collections.Generic;
using PatridgeDev;
using Hangout.Models;

namespace WineHangoutz
{
	public class ReviewCellView: UITableViewCell
	{
		UILabel userName;
		UILabel ReviewDate;
		UITextView Comments;
		UIImageView imageView;
		//UIBotton Readmore;
		PDRatingView stars;
		private int screenid = 10;
		public ReviewCellView(NSString cellId) : base (UITableViewCellStyle.Default, cellId)
    	{
			try
			{
				SelectionStyle = UITableViewCellSelectionStyle.Gray;
				//ContentView.BackgroundColor = UIColor.FromRGB(218, 255, 127);
				imageView = new UIImageView();

				userName = new UILabel()
				{
					Font = UIFont.FromName("Verdana", 15f),
					TextColor = UIColor.FromRGB(127, 51, 0),
					BackgroundColor = UIColor.Clear
				};
				ReviewDate = new UILabel()
				{
					Font = UIFont.FromName("AmericanTypewriter", 10f),
					TextColor = UIColor.FromRGB(38, 127, 0),
					//TextAlignment = UITextAlignment.Center,
					BackgroundColor = UIColor.Clear
				};
				Comments = new UITextView()
				{
					Font = UIFont.FromName("AmericanTypewriter", 14f),
					TextColor = UIColor.FromRGB(255, 127, 0),
					//TextAlignment = UITextAlignment.Center,
					BackgroundColor = UIColor.Clear,
					Editable = false
					                         
				};
				var ratingConfig = new RatingConfig(emptyImage: UIImage.FromBundle("Stars/star-silver2.png"),
							filledImage: UIImage.FromBundle("Stars/star.png"),
							chosenImage: UIImage.FromBundle("Stars/star.png"));

				stars = new PDRatingView(new CGRect(ContentView.Bounds.Width-200, 20, 60, 20), ratingConfig, 5.0m);

				ContentView.AddSubviews(new UIView[] { userName, ReviewDate, Comments, stars, imageView });
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

				UIImage img = BlobWrapper.GetProfileImageforUser(review.ReviewUserId);
				if (img != null)
				{
					imageView.Image = img;
				}
				else
				{
					imageView.Image = new UIImage("user1.png");
				}
				userName.Text = review.Username;
				ReviewDate.Text = review.Date.ToString("MM-dd-yyyy");
				Comments.Text = review.RatingText;
				CGSize sTemp = new CGSize(ContentView.Bounds.Width - 50, 100);
				sTemp = Comments.SizeThatFits(sTemp);
				Comments.Frame = new CGRect(0, 30, ContentView.Bounds.Width - 50, sTemp.Height);

				//stars = new PDRatingView(new CGRect(150, 2, 60, 20), ratingConfig, review.Stars);
				//ContentView.Bounds.Height = 90;
				stars.AverageRating = review.RatingStars;
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
				imageView.Frame = new CGRect(5, 5, 33, 33);
				userName.Frame = new CGRect(50, 2, ContentView.Bounds.Width - 35, 20);
				ReviewDate.Frame = new CGRect(50, 20, ContentView.Bounds.Width - 35, 20);
				//stars.Frame = new CGRect(35, 50, 100, 20);
				stars.UserInteractionEnabled = false;
				Comments.Frame = new CGRect(45, 30, ContentView.Bounds.Width - 50, 150);
			}
			catch (Exception ex)
			{
				LoggingClass.LogError(ex.ToString(), screenid, ex.StackTrace);
			}
		}
	}

	public class ReviewTableSource : UITableViewSource
	{
		
		//string CellIdentifier = "TableCell";
		List<Review> Reviews;
		private int screenid = 10;
		public ReviewTableSource(List<Review> reviews)
		{
			Reviews = reviews;
		}

		public override nint RowsInSection(UITableView tableview, nint section)
		{
			return Reviews.Count;
		}

		public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
		{
			ReviewCellView cell = null;
			try
			{

				NSString name = new NSString("TableCell");
				 cell = tableView.DequeueReusableCell(name) as ReviewCellView;
				if (cell == null)
					cell = new ReviewCellView(name);
				cell.UpdateCell(Reviews[indexPath.Row]);
				cell.SetNeedsDisplay();


			}
			catch(Exception ex) { 
				LoggingClass.LogError(ex.ToString(), screenid, ex.StackTrace);
			}
			return cell;

		}
		public override nfloat GetHeightForRow(UITableView tableView, NSIndexPath indexPath)
		{
			//string item = this.Lst.Items[indexPath.Section];
			//SizeF size = new SizeF(tableView.Bounds.Width - 40, float.MaxValue);
			//float height = tableView.StringSize(item, Font, size, LineBreakMode).Height + 10;
			return 90f;	
		}
		public override UIView GetViewForHeader(UITableView tableView, nint section)
		{
			UILabel headerLabel = new UILabel(); // Set the frame size you need
			headerLabel.TextColor = UIColor.Purple; // Set your color
			headerLabel.Text = "Reviews";
			headerLabel.BackgroundColor = UIColor.LightGray;
			headerLabel.TextAlignment = UITextAlignment.Center;
			return headerLabel;
		}
		public override nfloat GetHeightForHeader(UITableView tableView, nint section)
		{
			return 35f;	
		}
	}

	public class ReviewModel
	{
		public string userName;
		public decimal Stars;
		public DateTime reviewDate;
		public string Comments;
	}
}