using System;
using UIKit;
using CoreGraphics;
using Foundation;
using PatridgeDev;
using System.Collections.Generic;

namespace WineHangoutz
{
	public class DetailViewController : UIViewController
	{
		
		public DetailViewController() : base ()
		{
			this.Title = "Details";
		}
		UIScrollView scrollView;
		PDRatingView ratingView;
		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

			nfloat h = View.Frame.Height * 2.3f;
			nfloat w = UIScreen.MainScreen.Bounds.Width;

			scrollView = new UIScrollView
			{
				Frame = new CGRect(0, 20, View.Frame.Width, View.Frame.Height),
				ContentSize = new CGSize(View.Frame.Width, h),
				BackgroundColor = UIColor.LightGray,
				AutoresizingMask = UIViewAutoresizing.FlexibleHeight
			};

			var lblName = new UILabel();
			lblName.Frame = new CGRect(0, 0, View.Frame.Width, 20);
			lblName.Text = "Arzenton Pinot Nero";
			lblName.TextAlignment = UITextAlignment.Center;
			scrollView.AddSubview(lblName);

			var Separator = new UIImageView();
			Separator.Frame = new CGRect(0, 50, View.Frame.Width, 2);
			Separator.Image = UIImage.FromFile("separator.png");
			scrollView.AddSubview(Separator);

			var lblVintage = new UILabel();
			lblVintage.Frame = new CGRect(View.Frame.Width/2 - 10, 40, 40, 20);
			lblVintage.Text = "2013";
			lblVintage.TextAlignment = UITextAlignment.Center;
			lblVintage.BackgroundColor = UIColor.LightGray;
			scrollView.AddSubview(lblVintage);

			var btlBack = new UIImageView();
			btlBack.Frame = new CGRect(0, 70, View.Frame.Width, View.Frame.Width);
			btlBack.Image = UIImage.FromFile("placeholder.jpeg");
			scrollView.AddSubview(btlBack);

			nfloat height = View.Frame.Width - 20;
			nfloat width = (height / 233) * 92;
			nfloat X = (View.Frame.Width - width) / 2;
			var btlImage = new UIImageView(); //92 * 233
			btlImage.Frame = new CGRect(X, 90, width, height);
			btlImage.Image = UIImage.FromFile("Wines/wine2.png");
			scrollView.AddSubview(btlImage);

			var ratingConfig = new RatingConfig(emptyImage: UIImage.FromBundle("Stars/empty.png"),
												filledImage: UIImage.FromBundle("Stars/filled.png"),
												chosenImage: UIImage.FromBundle("Stars/chosen.png"));

			nfloat Y = 70 + View.Frame.Width;
			// Create the view.
			decimal averageRating = 3.25m;
			ratingView = new PDRatingView(new CGRect(View.Bounds.Width * 3 / 8, Y, View.Bounds.Width / 4, 25f), ratingConfig, averageRating);
			ratingView.UserInteractionEnabled = false;

			scrollView.AddSubview(ratingView);

			var lblRateTitle = new UILabel();
			lblRateTitle.Frame = new CGRect(4, Y+40, View.Frame.Width, 20);
			lblRateTitle.Text = "Rate this Wine";
			lblRateTitle.TextAlignment = UITextAlignment.Center;
			scrollView.AddSubview(lblRateTitle);

			var lblRateRequest = new UILabel();
			lblRateRequest.Frame = new CGRect(4, Y+60, View.Frame.Width, 20);
			lblRateRequest.Text = "Select number of Stars";
			lblRateRequest.Font = UIFont.FromName("AmericanTypewriter", 10f);
			lblRateRequest.TextAlignment = UITextAlignment.Center;
			scrollView.AddSubview(lblRateRequest);

			ratingView = new PDRatingView(new CGRect(View.Bounds.Width * 2 / 8, Y+82, View.Bounds.Width / 2, 36f), ratingConfig, 0m);
			// [Optional] Do something when the user selects a rating.
			UIViewController that = this;
			ratingView.RatingChosen += (sender, e) =>
			{
				//(new UIAlertView("Rated!", e.Rating.ToString() + " stars", null, "Ok")).Show();
				PopupView yourController = new PopupView();
				yourController.NavController = NavigationController;
				yourController.parent = that;
				yourController.ModalPresentationStyle = UIModalPresentationStyle.OverCurrentContext;
				//this.PresentViewController(yourController, true, null);
				this.PresentModalViewController(yourController, false);

				//ShowModal(false);
			};
			scrollView.AddSubview(ratingView);

			var starUpLine = new UIImageView(new CGRect(4, Y + 80, View.Frame.Width-8, 1));
			starUpLine.AutoresizingMask = UIViewAutoresizing.FlexibleWidth;
			starUpLine.Image = UIImage.FromFile("separator.png");
			starUpLine.ContentMode = UIViewContentMode.ScaleAspectFill;
			starUpLine.ClipsToBounds = true;
			starUpLine.Layer.BorderColor = UIColor.White.CGColor;
			starUpLine.BackgroundColor = UIColor.LightGray;
			scrollView.AddSubview(starUpLine);


			var starDownLine = new UIImageView(new CGRect(4, Y + 120, View.Frame.Width - 8, 1));
			starDownLine.AutoresizingMask = UIViewAutoresizing.FlexibleWidth;
			starDownLine.Image = UIImage.FromFile("separator.png");
			starDownLine.ContentMode = UIViewContentMode.ScaleAspectFill;
			starDownLine.ClipsToBounds = true;
			starDownLine.Layer.BorderColor = UIColor.White.CGColor;
			starDownLine.BackgroundColor = UIColor.LightGray;
			scrollView.AddSubview(starDownLine);

			Y = Y + 140;
			var lblDesc = new UILabel();
			lblDesc.Frame = new CGRect(4, Y, View.Frame.Width, 20);
			lblDesc.Text = "Description: ";
			lblDesc.TextAlignment = UITextAlignment.Left;
			scrollView.AddSubview(lblDesc);

			var lblDescText = new UITextView();
			lblDescText.Frame = new CGRect(0, Y + 40, View.Frame.Width, 100);
			lblDescText.Text = "Deep ruby. Perfumes alive and intense of red berry fruit enveloped by fresh spiciness of black pepper, cloves with a finish of cinnamon stick and sensations resinous toasted. In the background, flavors of wild berries. Tannnin vibrant, but already silky and enveloping connotes tasting soft, round but at the same time fresh with a tasty thin vein of great elegance.";
			lblDescText.TextAlignment = UITextAlignment.Justified;
			lblDescText.BackgroundColor = UIColor.LightGray;
			scrollView.AddSubview(lblDescText);


			var table = new UITableView();
			string[,] tableItems = new string[,] { { "Name", "Arzenton Pinot Nero" }, { "Classification", "Friuli Colli Orientali DOC" }, { "Grape Type:", "Pinot Nero" }, { "Alchol", "13.5%" }, { "Vintage year", "2012"}, { "Aromas", "Red fruits" }, { "Food pairings", "White Meat"}, { "Bottle size", "750ml"}, {"Serving at:","15 °C"} };
			table.Frame = new CGRect(0, Y + 140, View.Frame.Width, tableItems.Length * 22);
			table.Source = new WineInfoTableSource(tableItems);
			table.AllowsSelection = false;
			scrollView.AddSubview(table);

			Y = Y + 160 + tableItems.Length * 22;
			var lblProducer = new UILabel();
			lblProducer.Frame = new CGRect(4, Y, View.Frame.Width, 20);
			lblProducer.Text = "Producer: ";
			lblProducer.TextAlignment = UITextAlignment.Left;
			scrollView.AddSubview(lblProducer);

			var lblProducerText = new UITextView();
			lblProducerText.Frame = new CGRect(0, Y + 40, View.Frame.Width, 100);
			lblProducerText.Text = "Arzenton company was found in 1968, with the accomodation of the hilly area of spessa of Cividale del Friuli: thus in one of the places most suited to vityculture of the capital Doc Coli Orientali bel Friuli. The company consist of 14 hectare of which 10 are devoted to vineyards in soil consist of alternating layers of marl and sandstones that represnt the best soil of viticulture hilly.";
			lblProducerText.TextAlignment = UITextAlignment.Justified;
			lblProducerText.BackgroundColor = UIColor.LightGray;
			scrollView.AddSubview(lblProducerText);

			scrollView.AddSubview(LoadReviews());

			View.AddSubview(scrollView);
		}

		public UITableView LoadReviews()
		{
			var reviewTable = new UITableView();
			List<ReviewModel> reviewData = new List<ReviewModel>();
			var review1 = new ReviewModel();
			review1.Comments = "Comments";
			review1.reviewDate = DateTime.Now;
			review1.Stars = 4.2m;
			review1.userName = "Ankur";
			reviewData.Add(review1);

			var review2 = new ReviewModel();
			review2.Comments = "More Comments";
			review2.reviewDate = DateTime.Now;
			review2.Stars = 3.2m;
			review2.userName = "Advait";
			reviewData.Add(review2);

			reviewTable.Frame = new CGRect(0, 1300, View.Frame.Width, (reviewData.Count * 90) + 35);
			reviewTable.Source = new ReviewTableSource(reviewData);
			reviewTable.AllowsSelection = false;

			return reviewTable;
		}

		public void ShowModal(bool animated = true)
		{
			UIView parentView = UIApplication.SharedApplication.KeyWindow;
			CGRect frame = parentView.Bounds;

			var rootController = NavigationController;


			UIView transView = new UIView(rootController.View.Frame);
			transView.BackgroundColor = UIColor.Black;
			transView.Alpha = 0.5f;
			parentView.AddSubview(transView);
			parentView.BringSubviewToFront(transView);


			UIView modalView = new UIView(new CGRect(4, 200, View.Frame.Width-8, 150));
			modalView.BackgroundColor = UIColor.White;
			//modalView.Opaque = true;
			//modalView.Alpha = 2.0f;

			var lblProducer = new UILabel();
			lblProducer.Frame = new CGRect(4, 0, View.Frame.Width, 20);
			lblProducer.Text = "Producer: ";
			lblProducer.TextAlignment = UITextAlignment.Left;
			modalView.AddSubview(lblProducer);
			//modalView.Transform = rootController.View.Transform;
			transView.AddSubview(modalView);
			transView.BringSubviewToFront(modalView);

			if (!animated) return;

			modalView.Alpha = 0;
			//FluentAnimate.EaseIn(AnimationDuration, () => modalView.Alpha = 1).Start();
		}
	}
}
