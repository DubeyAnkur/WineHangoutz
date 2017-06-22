using System;
using CoreGraphics;
using CoreAnimation;
using Foundation;
using UIKit;
using ObjCRuntime;
using PatridgeDev;
using Hangout.Models;
using BigTed;
using System.Globalization;
using System.Threading;

namespace WineHangoutz {

	public class APLCollectionViewCell : UICollectionViewCell {
		private int screenid = 4;
		public static readonly NSString Key = new NSString ("APLCollectionViewCell");
		public UINavigationController NavigationController;
		public string WineName = "Wine Name";
		public string Vintage = "2012";
		public string RegPrice = "9.99";
		public decimal averageRating = 3.3m;
		public string WineId = "0";
		public string storeId = "2";
		public Item myItem;
		public UIView parent;

		//public void Dowork()
		//{
			
		//	NavigationController.PushViewController(new SKUDetailView(WineId,storeId), false);
		//}

		[Export ("initWithFrame:")]
		public APLCollectionViewCell (CGRect frame) : base (frame)
		{
			//NavigationController.NavigationBar.TopItem.Title = "List";
			try
			{
				CGRect box = new CGRect(Bounds.Location, Bounds.Size);
				box.X = 0;
				box.Y = 0;
				box.Height = box.Height - 140;
				BackgroundColor = UIColor.White;
				ImageView = new UIButton(box);
				ImageView.AutoresizingMask = UIViewAutoresizing.FlexibleHeight | UIViewAutoresizing.FlexibleWidth;
				ImageView.ContentMode = UIViewContentMode.ScaleAspectFill;
				//ImageView.Layer.BorderWidth = 3.0f;
				ImageView.ClipsToBounds = true;
				ImageView.Layer.BorderColor = UIColor.White.CGColor;
				ImageView.Layer.EdgeAntialiasingMask = CAEdgeAntialiasingMask.LeftEdge | CAEdgeAntialiasingMask.RightEdge | CAEdgeAntialiasingMask.BottomEdge | CAEdgeAntialiasingMask.TopEdge;

				box.Y = 3;
				btlImage = new UIButton(box);
				btlImage.AutoresizingMask = UIViewAutoresizing.FlexibleHeight | UIViewAutoresizing.FlexibleWidth;
				//btlImage.ContentMode = UIViewContentMode.ScaleAspectFill;
				btlImage.ClipsToBounds = true;
				btlImage.Layer.BorderColor = UIColor.White.CGColor;
				btlImage.Layer.EdgeAntialiasingMask = CAEdgeAntialiasingMask.LeftEdge | CAEdgeAntialiasingMask.RightEdge | CAEdgeAntialiasingMask.BottomEdge | CAEdgeAntialiasingMask.TopEdge;

				btlImage.TouchDown += (sender, e) =>
				{
						BTProgressHUD.Show("Loading..."); //show spinner + text
				};

				btlImage.TouchUpInside += (object sender, EventArgs e) =>
				{
					BTProgressHUD.Show();
					//NavigationController.PushViewController(new SKUDetailView(WineId,storeId), false);
					NavigationController.PushViewController(new DetailViewController(WineId, storeId), false);
					LoggingClass.LogInfo("Clicked on " + WineId + " to enter into Details", screenid);

				};

				box.Height = 25;
				box.Width = 25;
				box.X = (Bounds.Width - 30);
				box.Y = 5;
				heartImage = new UIButton(box);
				heartImage.ClipsToBounds = true;
				heartImage.Layer.BorderColor = UIColor.White.CGColor;
				heartImage.Layer.EdgeAntialiasingMask = CAEdgeAntialiasingMask.LeftEdge | CAEdgeAntialiasingMask.RightEdge | CAEdgeAntialiasingMask.BottomEdge | CAEdgeAntialiasingMask.TopEdge;
				heartImage.SetImage(UIImage.FromFile("heart_empty.png"), UIControlState.Normal);
				heartImage.Tag = 0;

				heartImage.TouchUpInside += async (object sender, EventArgs e) =>
				{
				//Do some actionn
				UIButton temp = (UIButton)sender;
					if (temp.Tag == 0)
					{
						heartImage.SetImage(UIImage.FromFile("heart_full.png"), UIControlState.Normal);
						temp.Tag = 1;
						myItem.IsLike = true;
						LoggingClass.LogInfo("Liked Wine "+WineId, screenid);
					}
					else
					{
						heartImage.SetImage(UIImage.FromFile("heart_empty.png"), UIControlState.Normal);
						temp.Tag = 0;
						myItem.IsLike = false;
						LoggingClass.LogInfo("Unliked Wine "+WineId, screenid);
					}
				//NavigationController.PushViewController(new DetailViewController(), false);
				SKULike like = new SKULike();
					like.UserID = Convert.ToInt32(CurrentUser.RetreiveUserId());
					like.WineId = Convert.ToInt32(WineId);
					like.Liked = Convert.ToBoolean(temp.Tag);

					ServiceWrapper sw = new ServiceWrapper();
					await sw.InsertUpdateLike(like);
				};

				CGRect lower = new CGRect(Bounds.Location, Bounds.Size);
				lower.Y = 50; //lower.Y + (ratio)*(Bounds.Height);
				lblName = new UILabel(lower);
				lblName.Font = UIFont.FromName("Verdana-Bold", 13f);
				lblName.TextColor = UIColor.Purple;
				lblName.Text = WineName;
				lblName.TextAlignment = UITextAlignment.Center;
				lblName.LineBreakMode = UILineBreakMode.WordWrap;
				lblName.Lines = 0;



				lower.Y = 245;
				lower.Height = 1;
				lower.Width = lower.Width - 20;
				lower.X = lower.X + 10;

				Separator = new UIImageView(lower);
				Separator.AutoresizingMask = UIViewAutoresizing.FlexibleWidth;
				Separator.Image = UIImage.FromFile("separator.png");
				Separator.ContentMode = UIViewContentMode.ScaleAspectFill;
				Separator.ClipsToBounds = true;
				Separator.Layer.BorderColor = UIColor.White.CGColor;
				Separator.BackgroundColor = UIColor.LightGray;

				CGRect year = new CGRect(Bounds.Location, Bounds.Size);
				year.Y = lower.Y - 15;
				year.X = year.Width / 2 - 25;
				year.Height = 30;
				year.Width = 50;
				lblYear = new UILabel(year);
				lblYear.Font = UIFont.FromName("Verdana", 12f);
				lblYear.Text = Vintage;
				lblYear.TextAlignment = UITextAlignment.Center;
				lblYear.BackgroundColor = UIColor.White;


				lblRegPrice = new UILabel(new CGRect(0, Bounds.Height - 60, Bounds.Width, 12f));
				lblRegPrice.Text = RegPrice;

				lblRegPrice.Font = UIFont.FromName("Verdana", 13f);

				lblRegPrice.TextAlignment = UITextAlignment.Center;

				var ratingConfig = new RatingConfig(emptyImage: UIImage.FromBundle("Stars/star-silver2.png"),
							filledImage: UIImage.FromBundle("Stars/star.png"),
							chosenImage: UIImage.FromBundle("Stars/star.png"));
				//decimal averageRating = 3.25m;

				ratingView = new PDRatingView(new CGRect(Bounds.Width * 1 / 4, Bounds.Height - 40, Bounds.Width / 2, 14f), ratingConfig, averageRating);
				ratingView.UserInteractionEnabled = false;
				//ratingView.BackgroundColor = UIColor.White;

				ContentView.AddSubview(ImageView);
				ContentView.InsertSubviewAbove(btlImage, ImageView);
				//ContentView.AddSubview(btlImage);
				ContentView.AddSubview(heartImage);
				ContentView.AddSubview(lblName);
				ContentView.AddSubview(Separator);
				ContentView.AddSubview(lblYear);
				ContentView.AddSubview(lblRegPrice);
				ContentView.AddSubview(ratingView);

			}
			catch (Exception ex)
			{
				LoggingClass.LogError(ex.ToString(), screenid, ex.StackTrace);
			}
        }

		public UIButton ImageView { get; private set; }
		public UIButton heartImage { get; private set; }
		public UIButton btlImage { get; private set; }
        public UILabel lblName { get; private set; }
        public UIImageView Separator { get; private set; }
        public UILabel lblYear { get; private set; }
		public UILabel lblRegPrice { get; private set; }
        public PDRatingView ratingView { get; private set; }

		private void NavigateToDetail()
		{
			
		}
    }
}
