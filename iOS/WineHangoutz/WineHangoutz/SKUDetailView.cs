using System;
using UIKit;
using CoreGraphics;
using Foundation;
using PatridgeDev;
using System.Collections.Generic;
using Hangout.Models;
using System.Linq;

namespace WineHangoutz
{
	public class SKUDetailView : UITableViewController, IPopupParent
	{
		int _wineId;
		public SKUDetailView(string WineId): base()
		{
			_wineId = Convert.ToInt32(WineId);
			this.Title = "Details";
		}

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
			nfloat width = View.Frame.Width;

			ServiceWrapper svc = new ServiceWrapper();
			ItemDetailsResponse myData = svc.GetItemDetails(_wineId).Result;

			TableView.SeparatorStyle = UITableViewCellSeparatorStyle.None;
			TableView.AllowsSelection = false;
			//TableView.AutosizesSubviews = true;
			//TableView.Unev
			TableView.RowHeight = UITableView.AutomaticDimension;
			TableView.Source = new SKUDetailTableSource(width, this, NavigationController, myData.ItemDetails);

		}
		public void RefreshParent()
		{
			nfloat width = View.Frame.Width;
			ServiceWrapper svc = new ServiceWrapper();
			ItemDetailsResponse myData = svc.GetItemDetails(_wineId).Result;
			TableView.Source = new SKUDetailTableSource(width, this, NavigationController, myData.ItemDetails);
			TableView.ReloadData();
		}
	}

	public class SKUDetailTableSource : UITableViewSource
	{

		PDRatingView ratingView;
		nfloat Width;
		UITableView table;
		UIViewController Parent;
		UINavigationController NavigationController;
		ItemDetails data;

		public SKUDetailTableSource(nfloat wid, UIViewController parent, UINavigationController navCtrl, ItemDetails Data)
		{
			Width = wid;
			Parent = parent;
			NavigationController = navCtrl;
			data = Data;

			//data.Name = "Arzenton Pinot Nero";
			//data.Vintage = "2013";
			//data.Description = "Deep ruby. Perfumes alive and intense of red berry fruit enveloped by fresh spiciness of black pepper, cloves with a finish of cinnamon stick and sensations resinous toasted. In the background, flavors of wild berries. Tannnin vibrant, but already silky and enveloping connotes tasting soft, round but at the same time fresh with a tasty thin vein of great elegance.";
			data.LargeImageUrl = "Wines/wine2.png";
			data.Producer = Data.Producer;// "Arzenton company was found in 1968, with the accomodation of the hilly area of spessa of Cividale del Friuli: thus in one of the places most suited to vityculture of the capital Doc Coli Orientali bel Friuli. The company consist of 14 hectare of which 10 are devoted to vineyards in soil consist of alternating layers of marl and sandstones that represnt the best soil of viticulture hilly.";
			data.AverageRating = Data.AverageRating;// 4.25m;
			data.WineProperties = new Dictionary<string, string>(); //new string[,] { { "Name", "Arzenton Pinot Nero" }, { "Classification", "Friuli Colli Orientali DOC" }, { "Grape Type:", "Pinot Nero" }, { "Alchol", "13.5%" }, { "Vintage year", "2012" }, { "Aromas", "Red fruits" }, { "Food pairings", "White Meat" }, { "Bottle size", "750ml" }, { "Serving at:", "15 °C" } };

			ServiceWrapper sw = new ServiceWrapper();
			ItemReviewResponse ratings = sw.GetItemReviewsByWineID(Convert.ToInt32(data.WineID)).Result;
			data.Reviews = ratings.Reviews.ToList();
			//var review1 = new Rating();
			//review1.RatingText = "Comments";
			//review1.Date = DateTime.Now;
			//review1.RatingStars = 4.2m;
			//review1.Username = "Ankur";
			//data.Ratings.Add(review1);

			//var review2 = new Rating();
			//review2.RatingText = "More Comments";
			//review2.Date = DateTime.Now;
			//review2.RatingStars = 3.5m;
			//review2.Username = "Alpana";
			//data.Ratings.Add(review2);
		}

		public override nint RowsInSection(UITableView tableview, nint section)
		{
			return 17;//Should match this values.
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
												filledImage: UIImage.FromBundle("Stars/star-silver1.png"),
												chosenImage: UIImage.FromBundle("Stars/star-silver.png"));

			UIView vw = new UIView();
			switch (index)
			{
				case 1:
					var lblName = new UILabel();
					lblName.Frame = new CGRect(0, 0, this.Width, 40);
					lblName.Text = data.Name;
					lblName.Font = UIFont.FromName("Verdana-Bold", 16f);
					lblName.TextAlignment = UITextAlignment.Center;
					lblName.TextColor = UIColor.Purple;
					vw = lblName;
					break;
				case 2:
					//vw = Separator;
					break;
				case 3:
					var lblVintage = new UILabel();
					lblVintage.Frame = new CGRect(0, 0, this.Width, 20);
					lblVintage.Text = data.Vintage.ToString();
					lblVintage.Font = UIFont.FromName("Verdana", 12f);
					lblVintage.TextAlignment = UITextAlignment.Center;
					lblVintage.BackgroundColor = UIColor.FromPatternImage(UIImage.FromFile("Line.png"));
					vw = lblVintage;
					//vw.AddSubview(Separator);
					break;
				case 4:
					var btlBack = new UIImageView();
					btlBack.Frame = new CGRect(0, 10, this.Width, this.Width);
					btlBack.Image = UIImage.FromFile("placeholder.jpeg");

					nfloat height = this.Width;
					nfloat width = (height / 233) * 92;
					nfloat X = (this.Width - width) / 2;
					var btlImage = new UIImageView(); //92 * 233
					btlImage.Frame = new CGRect(X, 0, width, height);
					btlImage.Image = UIImage.FromFile(data.LargeImageUrl);
					vw = btlBack;
					vw.AddSubview(btlImage);
					break;
				case 5:
					ratingView = new PDRatingView(new CGRect(this.Width * 3 / 8 + 2, 10, this.Width / 4, 20f), ratingConfig, data.AverageRating);
					ratingView.UserInteractionEnabled = false;
					vw = ratingView;
					break;
				case 6:
					var lblRateTitle = new UILabel();
					lblRateTitle.Frame = new CGRect(4, 10, this.Width, 50);
					lblRateTitle.Text = "Rate this Wine";
					lblRateTitle.TextAlignment = UITextAlignment.Center;
					lblRateTitle.Font = UIFont.FromName("Verdana-Bold", 16f);
					lblRateTitle.TextColor = UIColor.Purple;
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
						PopupView yourController = new PopupView(Convert.ToInt32(data.WineID));
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
					var starDownLine = new UIImageView(new CGRect(4, 10, this.Width - 8, 1));
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
					lblDesc.Frame = new CGRect(4, 10, this.Width, 20);
					lblDesc.Text = "Description: ";
					lblDesc.TextAlignment = UITextAlignment.Left;
					vw = lblDesc;
					break;
				case 12:
					var lblDescText = new UITextView();
					lblDescText.Text = data.Description;
					lblDescText.TextAlignment = UITextAlignment.Justified;
					//lblDescText.BackgroundColor = UIColor.LightGray;
					CGSize sTemp = new CGSize(this.Width, 100);
					sTemp = lblDescText.SizeThatFits(sTemp);
					lblDescText.Frame = new CGRect(0, 0, this.Width, sTemp.Height);
					vw = lblDescText;
					break;
				case 13:
					table = new UITableView();
					//string[,] tableItems 
					table.Frame = new CGRect(0, 0, this.Width, data.WineProperties.Count * 22);
					table.Source = new WineInfoTableSource(data.WineProperties);
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
					//lblProducerText.BackgroundColor = UIColor.LightGray;
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
			if (indexPath.Item == 1) // table
				return 40;
			if (indexPath.Item == 6) // table
				return 50;

			if (indexPath.Item == 13) // table
				return (nfloat)(data.WineProperties.Count * 22)+10;// 22 is hard coded height of rows

			if (indexPath.Item == 16) // Reviews
				return (nfloat)(data.Reviews.Count * 90)+35;// 90 is hard coded height of rows

			return (nfloat)Math.Min(height, this.Width);
			//var viewCell = uiCell.GetPropertyValue<ViewCell>(uiCell.GetType(), "ViewCell");
			//return (float)viewCell.RenderHeight;
		}
		public UITableView LoadReviews()
		{
			var reviewTable = new UITableView();


			reviewTable.Frame = new CGRect(0, 0, this.Width, (data.Reviews.Count * 90) + 35);
			reviewTable.Source = new ReviewTableSource(data.Reviews);
			reviewTable.AllowsSelection = false;
			reviewTable.ScrollEnabled = false;

			return reviewTable;
		}
	}

}
