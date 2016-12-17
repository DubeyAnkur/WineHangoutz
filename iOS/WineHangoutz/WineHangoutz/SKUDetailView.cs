using System;
using UIKit;
using CoreGraphics;
using Foundation;
using PatridgeDev;
using System.Collections.Generic;

namespace WineHangoutz
{
	public class SKUDetailView : UITableViewController
	{
		public SKUDetailView(): base()
		{
			this.Title = "Details";
		}

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
			nfloat width = View.Frame.Width;

			TableView.SeparatorStyle = UITableViewCellSeparatorStyle.None;
			TableView.AllowsSelection = false;
			//TableView.AutosizesSubviews = true;
			//TableView.Unev
			TableView.RowHeight = UITableView.AutomaticDimension;
			TableView.Source = new SKUDetailTableSource(width, this, NavigationController);

		}
	}

	public class WineDetails
	{
		public string Name;
		public string Vintage;
		public string imageURL;
		public string WineDesc;
		public string Producer;
		public decimal averageRating;
		public string[,] tableItems;
		public List<ReviewModel> reviewData;
	}

	public class SKUDetailTableSource : UITableViewSource
	{

		PDRatingView ratingView;
		nfloat Width;
		UITableView table;
		UIViewController Parent;
		UINavigationController NavigationController;
		WineDetails data = new WineDetails();

		public SKUDetailTableSource(nfloat wid, UIViewController parent, UINavigationController navCtrl)
		{
			Width = wid;
			Parent = parent;
			NavigationController = navCtrl;

			data.Name = "Arzenton Pinot Nero";
			data.Vintage = "2013";
			data.WineDesc = "Deep ruby. Perfumes alive and intense of red berry fruit enveloped by fresh spiciness of black pepper, cloves with a finish of cinnamon stick and sensations resinous toasted. In the background, flavors of wild berries. Tannnin vibrant, but already silky and enveloping connotes tasting soft, round but at the same time fresh with a tasty thin vein of great elegance.";
			data.imageURL = "Wines/wine2.png";
			data.Producer = "Arzenton company was found in 1968, with the accomodation of the hilly area of spessa of Cividale del Friuli: thus in one of the places most suited to vityculture of the capital Doc Coli Orientali bel Friuli. The company consist of 14 hectare of which 10 are devoted to vineyards in soil consist of alternating layers of marl and sandstones that represnt the best soil of viticulture hilly.";
			data.averageRating = 4.25m;
			data.tableItems = new string[,] { { "Name", "Arzenton Pinot Nero" }, { "Classification", "Friuli Colli Orientali DOC" }, { "Grape Type:", "Pinot Nero" }, { "Alchol", "13.5%" }, { "Vintage year", "2012" }, { "Aromas", "Red fruits" }, { "Food pairings", "White Meat" }, { "Bottle size", "750ml" }, { "Serving at:", "15 °C" } };

			data.reviewData = new List<ReviewModel>();
			var review1 = new ReviewModel();
			review1.Comments = "Comments";
			review1.reviewDate = DateTime.Now;
			review1.Stars = 4.2m;
			review1.userName = "Ankur";
			data.reviewData.Add(review1);

			var review2 = new ReviewModel();
			review2.Comments = "More Comments";
			review2.reviewDate = DateTime.Now;
			review2.Stars = 3.5m;
			review2.userName = "Advait";
			data.reviewData.Add(review2);

		}

		public override nint RowsInSection(UITableView tableview, nint section)
		{
			return 17;
		}

		public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
		{
			UITableViewCell cell = new UITableViewCell();
			cell.Add(GetViewForSKUCell(indexPath.Item));

			return cell;
		}

		public UIView GetViewForSKUCell(nint index)
		{
			var ratingConfig = new RatingConfig(emptyImage: UIImage.FromBundle("Stars/empty.png"),
												filledImage: UIImage.FromBundle("Stars/filled.png"),
												chosenImage: UIImage.FromBundle("Stars/chosen.png"));

			UIView vw = new UIView();
			switch (index)
			{
				case 1:
					var lblName = new UILabel();
					lblName.Frame = new CGRect(0, 0, this.Width, 20);
					lblName.Text = data.Name;
					lblName.TextAlignment = UITextAlignment.Center;
					vw = lblName;
					break;
				case 2:
					//vw = Separator;
					break;
				case 3:
					var lblVintage = new UILabel();
					lblVintage.Frame = new CGRect(0, 0, this.Width, 20);
					lblVintage.Text = data.Vintage;
					lblVintage.TextAlignment = UITextAlignment.Center;
					lblVintage.BackgroundColor = UIColor.FromPatternImage(UIImage.FromFile("Line.png"));
					vw = lblVintage;
					//vw.AddSubview(Separator);
					break;
				case 4:
					var btlBack = new UIImageView();
					btlBack.Frame = new CGRect(0, 0, this.Width, this.Width);
					btlBack.Image = UIImage.FromFile("placeholder.jpeg");

					nfloat height = this.Width;
					nfloat width = (height / 233) * 92;
					nfloat X = (this.Width - width) / 2;
					var btlImage = new UIImageView(); //92 * 233
					btlImage.Frame = new CGRect(X, 0, width, height);
					btlImage.Image = UIImage.FromFile(data.imageURL);
					vw = btlBack;
					vw.AddSubview(btlImage);
					break;
				case 5:
					ratingView = new PDRatingView(new CGRect(this.Width * 3 / 8, 0, this.Width / 4, 20f), ratingConfig, data.averageRating);
					ratingView.UserInteractionEnabled = false;
					vw = ratingView;
					break;
				case 6:
					var lblRateTitle = new UILabel();
					lblRateTitle.Frame = new CGRect(4, 0, this.Width, 20);
					lblRateTitle.Text = "Rate this Wine";
					lblRateTitle.TextAlignment = UITextAlignment.Center;
					vw = lblRateTitle;
					break;
				case 7:
					var lblRateRequest = new UILabel();
					lblRateRequest.Frame = new CGRect(4, 0, this.Width, 10);
					lblRateRequest.Text = "Select number of Stars";
					lblRateRequest.Font = UIFont.FromName("AmericanTypewriter", 10f);
					lblRateRequest.TextAlignment = UITextAlignment.Center;
					vw = lblRateRequest;
					break;
				case 8:
					var starUpLine = new UIImageView(new CGRect(4, 0, this.Width - 8, 1));
					starUpLine.AutoresizingMask = UIViewAutoresizing.FlexibleWidth;
					starUpLine.Image = UIImage.FromFile("separator.png");
					starUpLine.ContentMode = UIViewContentMode.ScaleAspectFill;
					starUpLine.ClipsToBounds = true;
					starUpLine.Layer.BorderColor = UIColor.White.CGColor;
					starUpLine.BackgroundColor = UIColor.LightGray;
					vw = starUpLine;
					break;
				case 9:
					PDRatingView ratingView2 = new PDRatingView(new CGRect(this.Width * 2 / 8, 0, this.Width / 2, 36f), ratingConfig, 0m);
					// [Optional] Do something when the user selects a rating.
					UIViewController that = Parent;
					ratingView2.RatingChosen += (sender, e) =>
					{
						PopupView yourController = new PopupView();
						yourController.NavController = NavigationController;
						yourController.parent = that;
						yourController.StartsSelected = e.Rating;
						yourController.ModalPresentationStyle = UIModalPresentationStyle.OverCurrentContext;
						//this.PresentViewController(yourController, true, null);
						that.PresentModalViewController(yourController, false);

						//ShowModal(false);
					};
					vw = ratingView2;
					break;
				case 10:
					var starDownLine = new UIImageView(new CGRect(4, 0, this.Width - 8, 1));
					starDownLine.AutoresizingMask = UIViewAutoresizing.FlexibleWidth;
					starDownLine.Image = UIImage.FromFile("separator.png");
					starDownLine.ContentMode = UIViewContentMode.ScaleAspectFill;
					starDownLine.ClipsToBounds = true;
					starDownLine.Layer.BorderColor = UIColor.White.CGColor;
					starDownLine.BackgroundColor = UIColor.LightGray;
					vw = starDownLine;
					break;
				case 11:
					var lblDesc = new UILabel();
					lblDesc.Frame = new CGRect(4, 0, this.Width, 20);
					lblDesc.Text = "Description: ";
					lblDesc.TextAlignment = UITextAlignment.Left;
					vw = lblDesc;
					break;
				case 12:
					var lblDescText = new UITextView();
					lblDescText.Text = data.WineDesc;
					lblDescText.TextAlignment = UITextAlignment.Justified;
					lblDescText.BackgroundColor = UIColor.LightGray;
					CGSize sTemp = new CGSize(this.Width, 100);
					sTemp = lblDescText.SizeThatFits(sTemp);
					lblDescText.Frame = new CGRect(0, 0, this.Width, sTemp.Height);
					vw = lblDescText;
					break;
				case 13:
					table = new UITableView();
					//string[,] tableItems 
					table.Frame = new CGRect(0, 0, this.Width, data.tableItems.Length * 22);
					table.Source = new WineInfoTableSource(data.tableItems);
					table.AllowsSelection = false;
					table.ScrollEnabled = false;
					vw = table;
					break;
				case 14:
					var lblProducer = new UILabel();
					lblProducer.Frame = new CGRect(4, 0, this.Width, 20);
					lblProducer.Text = "Producer: ";
					lblProducer.TextAlignment = UITextAlignment.Left;
					vw = lblProducer;
					break;
				case 15:
					var lblProducerText = new UITextView();
					//lblProducerText.Frame = new CGRect(0, 0, this.Width, 100);
					lblProducerText.Text = data.Producer;
					lblProducerText.TextAlignment = UITextAlignment.Justified;
					lblProducerText.BackgroundColor = UIColor.LightGray;
					sTemp = new CGSize(this.Width, 100);
					sTemp = lblProducerText.SizeThatFits(sTemp);
					lblProducerText.Frame = new CGRect(0, 0, this.Width, sTemp.Height);
					vw = lblProducerText;
					break;
				case 16:
					vw = LoadReviews();
					break;
				default:
					break;
			}
			return vw;
		}

		public override nfloat GetHeightForRow(UITableView tableView, NSIndexPath indexPath)
		{
			var uiCell = this.GetCell(tableView, indexPath);

			uiCell.SetNeedsLayout();
			uiCell.LayoutIfNeeded();

			CGSize sTemp = new CGSize(this.Width, 100);
			sTemp = uiCell.Subviews[1].SizeThatFits(sTemp);

			//uiCell.ContentView.SizeToFit();

			nfloat height = sTemp.Height;
			//if (indexPath.Item == 9)
			//	return (nfloat)(80);
			
			if (indexPath.Item == 13) // table
				return (nfloat)(data.tableItems.Length * 22)+10;// 22 is hard coded height of rows

			if (indexPath.Item == 16) // Reviews
				return (nfloat)(data.reviewData.Count * 90)+35;// 90 is hard coded height of rows

			return (nfloat)Math.Min(height, this.Width);
			//var viewCell = uiCell.GetPropertyValue<ViewCell>(uiCell.GetType(), "ViewCell");
			//return (float)viewCell.RenderHeight;
		}
		public UITableView LoadReviews()
		{
			var reviewTable = new UITableView();


			reviewTable.Frame = new CGRect(0, 0, this.Width, (data.reviewData.Count * 90) + 35);
			reviewTable.Source = new ReviewTableSource(data.reviewData);
			reviewTable.AllowsSelection = false;
			reviewTable.ScrollEnabled = false;

			return reviewTable;
		}
	}

}
