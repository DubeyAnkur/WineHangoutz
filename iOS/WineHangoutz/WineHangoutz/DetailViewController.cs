using System;
using UIKit;
using CoreGraphics;
using Foundation;
using PatridgeDev;
using System.Collections.Generic;
using BigTed;
using Hangout.Models;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using System.Linq;

namespace WineHangoutz
{
	public class DetailViewController : UIViewController, IPopupParent
	{
		int _wineId;
		public int _storeId;
		private int screenid = 112;
		NSData HighImgData = null;

		public DetailViewController(string WineId,string storeid) : base ()
		{
			_wineId = Convert.ToInt32(WineId);
			_storeId = Convert.ToInt32(storeid);
            this.Title = "Wine Details";

		}
		UIScrollView scrollView;
		PDRatingView ratingView;

		public void RefreshParent()
		{
			Internal_ViewDidLoad();	
		}

		public override void ViewDidLoad()
		{
			View.BackgroundColor = UIColor.White;
			Task.Factory.StartNew(() =>
			{
                 InvokeOnMainThread( () => {	
				 	Internal_ViewDidLoad();
				});
			});
		}

		public void Internal_ViewDidLoad()
		{
			try
			{
				LoggingClass.LogInfo("Entered into detail view of " + _wineId, screenid);
				BTProgressHUD.Show();
				nfloat width = View.Frame.Width;
				ServiceWrapper svc = new ServiceWrapper();
				ItemDetailsResponse mydata = svc.GetItemDetails(_wineId, _storeId).Result;
				//ItemReviewResponse rv = svc.GetItemReviewUID(CurrentUser.RetreiveUserId()).Result;
				var data = mydata.ItemDetails;


				var lblName = new UILabel();
				lblName.Frame = new CGRect(0, 0, width, 40);
				lblName.Text = data.Name;
				lblName.Font = UIFont.FromName("Verdana-Bold", 16f);
				lblName.TextAlignment = UITextAlignment.Center;
				lblName.TextColor = UIColor.Purple;

				//var Separator = new UIImageView();
				//Separator.Frame = new CGRect(0, 50, View.Frame.Width, 2);
				//Separator.Image = UIImage.FromFile("separator.png");

				var lblVintage = new UILabel();
				lblVintage.Frame = new CGRect(View.Frame.Width/2 - 10, 40, 40, 20);
				lblVintage.Text = data.Vintage.ToString();
				lblVintage.Font = UIFont.FromName("Verdana", 12f);
				lblVintage.TextAlignment = UITextAlignment.Center;
				lblVintage.BackgroundColor = UIColor.FromPatternImage(UIImage.FromFile("line123.png"));


				var btlImage = new UIImageView(); //92 * 233
				btlImage.Frame = new CGRect(0, 10, width, width);
				UIImage image = new UIImage("Images/loadin.png");

				CGRect rect = btlImage.Bounds;
				nfloat boxHeight = rect.Height; // which is = width;
				nfloat imgHeight = image.Size.Height;
				nfloat ratio = boxHeight / imgHeight;
				CGSize newSize = new CGSize(image.Size.Width * ratio, image.Size.Height * ratio);
				image = image.Scale(newSize);
				nfloat X = (boxHeight - image.Size.Width) / 2;
				btlImage.Frame = new CGRect(X, 70, image.Size.Width, image.Size.Height);
				btlImage.Image = image;
				DownloadAsync(data.WineId, _storeId, btlImage, boxHeight, 70);



				var ratingConfig = new RatingConfig(emptyImage: UIImage.FromBundle("Stars/empty.png"),
												filledImage: UIImage.FromBundle("Stars/star.png"),
												chosenImage: UIImage.FromBundle("Stars/star.png"));
				nfloat Y = 70 + View.Frame.Width;
				ratingView = new PDRatingView(new CGRect(width* 3 / 8 + 2, Y, width / 4, 20f), ratingConfig, data.AverageRating);
				ratingView.UserInteractionEnabled = false;


				var lblRateTitle = new UILabel();
				lblRateTitle.Frame = new CGRect(4, Y+40, width, 50);
				lblRateTitle.Text = "Rate this Wine";
				lblRateTitle.TextAlignment = UITextAlignment.Center;
				lblRateTitle.Font = UIFont.FromName("Verdana-Bold", 16f);
				lblRateTitle.TextColor = UIColor.Purple;
			
				var lblRateRequest = new UILabel();
				lblRateRequest.Frame = new CGRect(4, Y+75, width, 10);
				lblRateRequest.Text = "Select number of Stars";
				lblRateRequest.Font = UIFont.FromName("AmericanTypewriter", 10f);
				lblRateRequest.TextAlignment = UITextAlignment.Center;

				var starUpLine = new UIImageView(new CGRect(4, Y+90, width - 8, 1));
				starUpLine.AutoresizingMask = UIViewAutoresizing.FlexibleWidth;
				starUpLine.Image = UIImage.FromFile("separator.png");
				starUpLine.ContentMode = UIViewContentMode.ScaleAspectFill;
				starUpLine.ClipsToBounds = true;
				starUpLine.Layer.BorderColor = UIColor.White.CGColor;
				starUpLine.BackgroundColor = UIColor.LightGray;
				Y = Y + 10;
				PDRatingView ratingViewSelect = new PDRatingView(new CGRect(width * 2 / 8, Y + 82, width / 2, 36f), ratingConfig, 0m);
				UIViewController that = this;

				ratingViewSelect.RatingChosen += (sender, e) =>
				{
					LoggingClass.LogInfo("Clicked on stars to give rating on " + data.WineId, screenid);
					PopupView yourController = new PopupView(Convert.ToInt32(data.WineId), _storeId);
					yourController.NavController = NavigationController;
					yourController.parent = that;
					yourController.StartsSelected = e.Rating;

					yourController.ModalPresentationStyle = UIModalPresentationStyle.OverCurrentContext;
					that.PresentModalViewController(yourController, false);
				};

				var starDownLine = new UIImageView(new CGRect(4, Y + 120, View.Frame.Width - 8, 1));
				starDownLine.AutoresizingMask = UIViewAutoresizing.FlexibleWidth;
				starDownLine.Image = UIImage.FromFile("separator.png");
				starDownLine.ContentMode = UIViewContentMode.ScaleAspectFill;
				starDownLine.ClipsToBounds = true;
				starDownLine.Layer.BorderColor = UIColor.White.CGColor;
				starDownLine.BackgroundColor = UIColor.LightGray;

				Y = Y + 140;
				var lblDesc = new UILabel();
				lblDesc.Frame = new CGRect(4, Y, View.Frame.Width, 20);
				lblDesc.Text = "Description: ";
				lblDesc.TextAlignment = UITextAlignment.Left;

				Y = Y + 20;
				var lblDescText = new UITextView();
				lblDescText.Editable = false;
				if (data.Description == null || data.Description == "")
				{
					lblDescText.Text = "Not available";
					lblDescText.Font = UIFont.FromName("EuphemiaUCAS-Italic", 10f);
				}
				else
				{
					lblDescText.Text = data.Description;
				}
				lblDescText.TextAlignment = UITextAlignment.Justified;
				CGSize sTemp = new CGSize(width, 100);
				sTemp = lblDescText.SizeThatFits(sTemp);
				lblDescText.Frame = new CGRect(0, Y, width, sTemp.Height);

				Y = Y + lblDescText.Frame.Size.Height;
				var table = new UITableView(); 
				table.Frame = new CGRect(0, Y, width, data.WineProperties.Count* 22);
				table.Source = new WineInfoTableSource(data.WineProperties);
				table.AllowsSelection = false;
				table.ScrollEnabled = false;

				Y = Y + table.Frame.Size.Height + 10;
				var lblProducer = new UILabel();
				lblProducer.Frame = new CGRect(4, Y, width, 20);
				lblProducer.Text = "Producer: ";
				lblProducer.TextAlignment = UITextAlignment.Left;

				Y = Y + 20;
				var lblProducerText = new UITextView();
				lblProducerText.Editable = false;
				if (data.Producer == null || data.Producer == "")
				{
					lblProducerText.Text = "Not available";
					lblProducerText.Font = UIFont.FromName("EuphemiaUCAS-Italic", 10f);
				}
				else
				{
					lblProducerText.Text = data.Producer;
				}
				lblProducerText.TextAlignment = UITextAlignment.Justified;
				sTemp = new CGSize(width, 100);
				sTemp = lblProducerText.SizeThatFits(sTemp);
				lblProducerText.Frame = new CGRect(0, Y, width, sTemp.Height);

				ItemReviewResponse ratings = svc.GetItemReviewsByWineID(Convert.ToInt32(data.WineId)).Result;
				data.Reviews = ratings.Reviews.ToList();
				Y = Y + lblProducerText.Frame.Size.Height;
				var review = LoadReviews(data, Y, width);
				Y = Y + review.Frame.Size.Height;

				scrollView = new UIScrollView
				{
					Frame = new CGRect(0, 70, View.Frame.Width, View.Frame.Height),
					ContentSize = new CGSize(View.Frame.Width, Y + 70),
					BackgroundColor = UIColor.White,
					AutoresizingMask = UIViewAutoresizing.FlexibleHeight
				};
				//When making it async the Frame.Y is messing up by image Y. So changing it to 70. Ideally it should be 0.
				//Same will apply to ContentSize.Y
				View.AddSubview(scrollView);

				scrollView.AddSubview(lblName);
				//scrollView.AddSubview(Separator);
				scrollView.AddSubview(lblVintage);
				//scrollView.AddSubview(btlBack);
				scrollView.AddSubview(btlImage);
				scrollView.AddSubview(ratingView);
				scrollView.AddSubview(lblRateTitle);
				scrollView.AddSubview(lblRateRequest);
				scrollView.AddSubview(ratingViewSelect);
				scrollView.AddSubview(starUpLine);
				scrollView.AddSubview(starDownLine);
				scrollView.AddSubview(lblDesc);
				scrollView.AddSubview(lblDescText);
				scrollView.AddSubview(table);
				scrollView.AddSubview(lblProducer);
				scrollView.AddSubview(lblProducerText);


				scrollView.AddSubview(review);



				BTProgressHUD.Dismiss();
			}
			catch (Exception ex)
			{
				LoggingClass.LogError(ex.Message, screenid, ex.StackTrace.ToString());
			}
		}

		public UITableView LoadReviews(ItemDetails data, nfloat Y, nfloat width)
		{
			var reviewTable = new UITableView();
			reviewTable.Frame = new CGRect(0, Y, width, (data.Reviews.Count * 90) + 35);
			reviewTable.Source = new ReviewTableSource(data.Reviews);
			reviewTable.AllowsSelection = false;
			reviewTable.ScrollEnabled = false;
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
public async void DownloadAsync(int wineid, int storeid, UIImageView btlImage, nfloat boxHeight, int Y)
{


	WebClient webClient = new WebClient();
	string url = null;
	if (storeid == 1)
	{
		url = "https://icsintegration.blob.core.windows.net/bottleimagedetailswall/" + wineid + ".jpg";
	}
	else if (storeid == 2)
	{
		url = "https://icsintegration.blob.core.windows.net/bottleimagedetailspp/" + wineid + ".jpg";
	}
	byte[] imageBytes = null;
	try
	{
		imageBytes = await webClient.DownloadDataTaskAsync(url);
		HighImgData = NSData.FromStream(new MemoryStream(imageBytes));
	}
	catch (TaskCanceledException)
	{
		//this.progressLayout.Visibility = ViewStates.Gone;
		return;
	}
	catch (Exception exe)
	{
		LoggingClass.LogError("while downloading image of wine id" + wineid + "  " + exe.Message, screenid, exe.StackTrace.ToString());
	}

	//HighresImg  =UIImage.LoadFromData(HighImgData);
	try
	{
		if (HighImgData != null)
		{
			UIImage image = UIImage.LoadFromData(HighImgData);
			nfloat imgHeight = image.Size.Height;
			nfloat ratio = boxHeight / imgHeight;
			CGSize newSize = new CGSize(image.Size.Width * ratio, image.Size.Height * ratio);
			image = image.Scale(newSize);
			nfloat X = (boxHeight - image.Size.Width) / 2;
			btlImage.Frame = new CGRect(X, Y, image.Size.Width, image.Size.Height);
			btlImage.Image = image;
		}
		else
		{
			btlImage.Image = new UIImage("Wines/bottle.jpg");
		}
	}
	catch (Exception Ex)
	{
		LoggingClass.LogError(Ex.Message, screenid, Ex.StackTrace.ToString());
	}
	}
	}
}
