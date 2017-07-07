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
    public partial class MyTastingViewController : UITableViewController, IPopupParent
    {
		private int screenid = 14;
		ServiceWrapper svc = new ServiceWrapper();
        public MyTastingViewController (IntPtr handle) : base (handle)
        {
        }
		public MyTastingViewController() : base()
		{
		}
		public override void ViewDidLoad()
		{
			try
			{
				

				int userId = Convert.ToInt32(CurrentUser.RetreiveUserId());
				if (userId == 0)
				{
					UIAlertView alert = new UIAlertView()
					{
						Title = "This feature is allowed only for VIP Card holders",
						//Message = "Coming Soon..."
					};
					alert.AddButton("OK");
					alert.AddButton("Know more");
						alert.Clicked += (senderalert, buttonArgs) =>
						{
							if (buttonArgs.ButtonIndex == 1)
							{
								UIApplication.SharedApplication.OpenUrl(new NSUrl("http://savvyitdev.com/winehangouts/"));
							}
						};
					alert.Show();
				}
				else
				{
					var myData = svc.GetMyTastingsList(userId).Result;
					if (myData.TastingList.Count == 0)
					{
						UILabel lblNoTastings = new UILabel();
						lblNoTastings.Text = myData.ErrorDescription;
						lblNoTastings.TextAlignment = UITextAlignment.Center;
						lblNoTastings.LineBreakMode = UILineBreakMode.WordWrap;
						lblNoTastings.Lines = 0;
						CGSize sTemp = new CGSize(View.Frame.Width, 100);
						sTemp = lblNoTastings.SizeThatFits(sTemp);
						lblNoTastings.Frame = new CGRect(0, 50, View.Bounds.Width, sTemp.Height);
						TableView.SeparatorColor = UIColor.Clear;
						View.AddSubview(lblNoTastings);
					}
					TableView.AllowsSelection = false;
					TableView.Source = new MyTastingTableSource(myData.TastingList.ToList(), NavigationController, this);
				}
			}
			catch (Exception ex)
			{
				LoggingClass.LogError(ex.Message, screenid, ex.StackTrace);
			}
		}
		public void RefreshParent()
		{
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
		//UILabel Notastings;
		UILabel ReviewDate;
		UILabel Vintage;
		UIImageView separator;
		UIButton imageView;
		UILabel WineIdLabel;
		Tastings taste = new Tastings();
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
					
					BTProgressHUD.Show("Loading...");
					NavController.PushViewController(new DetailViewController(WineIdLabel.Text, storeid.ToString(), false), false);
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
				LoggingClass.LogError(ex.Message, screenid, ex.StackTrace);
			}
		}
        public void UpdateCell(Tastings tasting)
		{
				try
				{
					imageView.SetImage(BlobWrapper.GetResizedImage(tasting.Barcode.ToString(), new CGRect(0, 0, 100, 155), tasting.PlantFinal.ToString()), UIControlState.Normal);
					separator.Image = UIImage.FromFile("separator.png");
					WineName.Text = tasting.Name;
					ReviewDate.Text = "Tasted on :" + tasting.TastingDate.ToString("MM-dd-yyyy");
					Vintage.Text = tasting.Vintage.ToString();
					WineIdLabel.Text = tasting.Barcode;
					storeid = tasting.PlantFinal;
					//stars = new PDRatingView(new CGRect(150, 2, 60, 20), ratingConfig, review.Stars);
					//ContentView.Bounds.Height = 90;
				}
				catch (Exception ex)
				{
					UIAlertView alert = new UIAlertView()
					{
						Title = "Sorry",
						Message = "Something went wrong. We are on it"
					};
					alert.AddButton("OK");
					alert.Show();
					LoggingClass.LogError(ex.Message, screenid, ex.StackTrace);
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
				//Notastings.Frame=new CGRect(imageWidth, 2, ContentView.Bounds.Width - imageWidth, 60);
				//stars.Frame = new CGRect(35, 50, 100, 20);
			}
			catch (Exception ex)
			{
				LoggingClass.LogError(ex.ToString(), screenid, ex.StackTrace);
			}
		}
	}
}