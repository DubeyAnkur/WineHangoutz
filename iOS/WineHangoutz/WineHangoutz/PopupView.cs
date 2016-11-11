using System;
using UIKit;
using CoreGraphics;
using Foundation;
using PatridgeDev;

namespace WineHangoutz
{
	public class PopupView : UIViewController
	{
		public UINavigationController NavController;
		public UIViewController parent;

		public PopupView() : base ()
		{
			this.Title = "Isolated";
			//this.TabBarItem.Image = UIImage.FromBundle("Images/first");
		}
		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
			this.View.BackgroundColor = new UIColor(0, 0, 0, 0.8f);

			var lblProducer = new UILabel();
			lblProducer.Frame = new CGRect(4, 180, View.Frame.Width-8, 30);
			lblProducer.Text = "My Tasting";
			lblProducer.BackgroundColor = UIColor.Purple;
			lblProducer.TextAlignment = UITextAlignment.Center;
			this.View.AddSubview(lblProducer);

			//this.View.Alpha = 0.5f;
			UIButton btnClose = new UIButton(new CGRect(9, 185, 20, 20));
			btnClose.SetBackgroundImage(new UIImage("Close.png"), UIControlState.Normal);
			this.View.AddSubview(btnClose);

			btnClose.TouchUpInside += (sender, e) =>
			{
				//var viewCtrl = NavController.PopViewController(false);
				//viewCtrl.Dispose();
				//NavController.PopToViewController(parent,false);
				NavController.DismissViewController(true, null);

			};

			UIImageView imgBtl = new UIImageView(new CGRect(View.Frame.Width - 64, 149, 60, 60));
			imgBtl.Image = UIImage.FromFile("wine_review.png");
			//imgBtl.BackgroundColor = UIColor.White;
			this.View.AddSubview(imgBtl);

			var lblWhite = new UILabel();
			lblWhite.Frame = new CGRect(4, 210, View.Frame.Width - 8, 200);
			lblWhite.BackgroundColor = UIColor.White;
			lblWhite.TextAlignment = UITextAlignment.Center;
			this.View.AddSubview(lblWhite);

			var Separator = new UIImageView(new CGRect(14, 225, View.Frame.Width - 28, 2));
			Separator.AutoresizingMask = UIViewAutoresizing.FlexibleWidth;
			Separator.Image = UIImage.FromFile("separator.png");
			this.View.AddSubview(Separator);


			var ratingConfig = new RatingConfig(emptyImage: UIImage.FromBundle("Stars/empty.png"),
												filledImage: UIImage.FromBundle("Stars/filled.png"),
												chosenImage: UIImage.FromBundle("Stars/chosen.png"));

			var lblStarBack = new UILabel();
			lblStarBack.Frame = new CGRect(View.Bounds.Width * 3 / 9, 210, View.Bounds.Width / 3, 35f);
			lblStarBack.BackgroundColor = UIColor.White;
			lblStarBack.TextAlignment = UITextAlignment.Center;
			this.View.AddSubview(lblStarBack);

			// Create the view.
			decimal averageRating = 3.25m;
			PDRatingView ratingView = new PDRatingView(new CGRect(View.Bounds.Width * 3 / 8, 210, View.Bounds.Width / 4, 35f), ratingConfig, averageRating);
			ratingView.UserInteractionEnabled = false;
			ratingView.BackgroundColor = UIColor.White;
			this.View.AddSubview(ratingView);

			var txtComments = new UITextView();
			txtComments.Frame = new CGRect(14, 240, View.Frame.Width-28, 130);
			txtComments.Text = "Describe your testing";
			//txtComments.TextAlignment = UITextAlignment.Justified;
			//txtComments.BackgroundColor = UIColor.LightGray;
			txtComments.Text = "Describe your testing";
			txtComments.Started += (sender, e) => {
				((UITextView)sender).Text = "";
			}; 
			this.View.AddSubview(txtComments);


			UIButton btnSave = new UIButton(new CGRect(14, 370, View.Frame.Width - 28, 20));
			//btnSave.SetBackgroundImage(new UIImage("Close.png"), UIControlState.Normal);
			btnSave.SetTitle("SAVE", UIControlState.Normal);
			btnSave.HorizontalAlignment = UIControlContentHorizontalAlignment.Right;
			btnSave.SetTitleColor(UIColor.Purple, UIControlState.Normal);
			this.View.AddSubview(btnSave);
		}
	}
}
