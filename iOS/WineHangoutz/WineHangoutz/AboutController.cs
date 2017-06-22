using System;
using CoreGraphics;
using UIKit;
using Foundation;
using BigTed;
using ZXing.Mobile;

namespace WineHangoutz
{
	public class AboutController1:UIViewController
	{
		private int screenid = 9;
		UINavigationController NavCtrl;
		UIScrollView scrollView;

		public AboutController1(UINavigationController navCtrl) : base("AboutController", null)
		{
			NavCtrl = navCtrl;
		}
		public override void ViewDidLoad()
		{
			try
			{
				base.ViewDidLoad();
				LoggingClass.LogInfo("Entered into About View ", screenid);
				UITextView Title1 = new UITextView();
				Title1.Frame = new CGRect(0, 0, UIScreen.MainScreen.Bounds.Width-20, UIScreen.MainScreen.Bounds.Height);
				Title1.Text = "Wine Hangouts";
				Title1.TextAlignment = UITextAlignment.Center;
				Title1.TextColor = UIColor.Purple;
				Title1.Font=UIFont.FromName("AmericanTypewriter", 35f);
				Title1.Editable = false;

				UITextView Heading1 = new UITextView();
				Heading1.Frame = new CGRect(5, 55, UIScreen.MainScreen.Bounds.Width-20, UIScreen.MainScreen.Bounds.Height);
				Heading1.Text = "Uncork the Merriment";
				Heading1.TextAlignment = UITextAlignment.Left;
				//Heading1.TextAlignment = UITextAlignment.Justified;
				Heading1.TextColor = UIColor.Black;
				Heading1.Font=UIFont.FromName("AmericanTypewriter", 25f);
				Heading1.Editable = false;

				UITextView p1 = new UITextView();
				p1.Frame = new CGRect(5, 95, UIScreen.MainScreen.Bounds.Width-20, UIScreen.MainScreen.Bounds.Height);
				p1.Text = "A delicious bottled wine is the way to the perfect blend of joy. Wine Hangouts is thus,developed to facilitate the wine lovers to deliver a complete wine experience through the internationally acclaimed wines and beers. Sticking strict to the quality, Wine Hangouts brings in the best self-serve partners in the industry: Enomatic- The World’s #1Wine Dispenser and Wine Preservation System and iPoruIt – A revolutionary self-serve solution for beers, to elevate the merriment of our guests to the best.";
				p1.TextColor = UIColor.Black;
				p1.TextAlignment = UITextAlignment.Justified;
				p1.Font=UIFont.FromName("AmericanTypewriter", 13f);
				p1.Editable = false;

				UITextView Heading2 = new UITextView();
				Heading2.Frame = new CGRect(5, 240, UIScreen.MainScreen.Bounds.Width-20, UIScreen.MainScreen.Bounds.Height);
				Heading2.Text = "WHO WE ARE";
				Heading2.TextColor = UIColor.Black;
				Heading2.TextAlignment = UITextAlignment.Justified;
				Heading2.Font=UIFont.FromName("AmericanTypewriter", 20f);
				Heading2.Editable = false;

				UITextView p2 = new UITextView();
				p2.Frame = new CGRect(5, 250 , UIScreen.MainScreen.Bounds.Width-20, UIScreen.MainScreen.Bounds.Height);
				p2.Text = "Wine Hangouts ensures the perfect backdrop to relish and to hang out with the wine and beer lovers, through over 24 assorted wines from the world’s best wineries. Each of our wine outlets is assured with one of the largest inventories of wine you can experience in New Jersey. The exclusive opportunity for our privileged customers makes it more enticing by offering FREE tasting of the 24 wines out of our huge collection.So, isn’t it time for a Wine Dine?";
				p2.TextColor = UIColor.Black;
				p2.TextAlignment = UITextAlignment.Justified;
				p2.Font=UIFont.FromName("AmericanTypewriter", 13f);
				p2.Editable = false;
				p2.TextAlignment = UITextAlignment.Left;

				UITextView Heading3 = new UITextView();
				Heading3.Frame = new CGRect(5, 405, UIScreen.MainScreen.Bounds.Width-20, UIScreen.MainScreen.Bounds.Height);
				Heading3.Text = "WHAT WE OFFER";
				Heading3.TextAlignment = UITextAlignment.Justified;
				Heading3.TextColor = UIColor.Black;
				Heading3.Font=UIFont.FromName("AmericanTypewriter", 20f);
				Heading3.Editable = false;
				Heading3.TextAlignment = UITextAlignment.Left;

				UITextView p3 = new UITextView();
				p3.Frame = new CGRect(5, 440, UIScreen.MainScreen.Bounds.Width-20, UIScreen.MainScreen.Bounds.Height);
				p3.Text = "We offer- not just the wines and beers, but an experience of enjoyment.Upon the credential authentication, the mobile application- Wine Hangout takes our privileged customers to the virtual vineyard of savors and flavors, through the following options:  Pick the Choice: Guest is displayed with all the available wines for tasting. My Tasting: See the list of wines you have tasted and choose more from the remaining surprises. My Reviews: View your ratings and reviews and let other wine lovers explore it along with you. My Favorites: Pick your favorites and we save the list to send you customized notifications when there are any discounts or available for wine tasting."+"/n"+" My Profile: Protect your credentials and update it as and when you wish.";
				p3.TextColor = UIColor.Black;
				p3.TextAlignment = UITextAlignment.Justified;
				p3.Font=UIFont.FromName("AmericanTypewriter", 13f);
				p3.Editable = false;
				p3.TextAlignment = UITextAlignment.Left;

				UITextView p4 = new UITextView();
				p4.Frame = new CGRect(5, 640, UIScreen.MainScreen.Bounds.Width-20, UIScreen.MainScreen.Bounds.Height);
				p4.Text = "Rate and Review";
				p4.TextColor = UIColor.Black;
				p4.TextAlignment = UITextAlignment.Justified;
				p4.Font=UIFont.FromName("AmericanTypewriter", 18f);
				p4.Editable = false;
				p4.TextAlignment = UITextAlignment.Left;

				UITextView p5 = new UITextView();
				p5.Frame = new CGRect(5, 665, UIScreen.MainScreen.Bounds.Width-20, UIScreen.MainScreen.Bounds.Height);
				p5.Text = "Let them also know the best choice of yours.";
				p5.TextColor = UIColor.Black;
				p5.TextAlignment = UITextAlignment.Justified;
				p5.Font=UIFont.FromName("AmericanTypewriter", 18f);
				p5.Editable = false;
				//p5.TextAlignment = UITextAlignment.Left;

				UITextView p6 = new UITextView();
				p6.Frame = new CGRect(5, 695, UIScreen.MainScreen.Bounds.Width-20, UIScreen.MainScreen.Bounds.Height);
				p6.Text = "Rate and Review is the spot for rating your favorite wine and Wine Hangouts displays the average rating and the individual rating of the users.";
				p5.TextColor = UIColor.Black;
				p6.Font=UIFont.FromName("AmericanTypewriter", 18f);
				p6.Editable = false;
				p6.TextAlignment = UITextAlignment.Justified;
				//p6.TextAlignment = UITextAlignment.Left;

				nfloat h = 0;

				scrollView = new UIScrollView
				{
					Frame = new CGRect(0,20, View.Frame.Width, View.Frame.Height),
					ContentSize = new CGSize(View.Frame.Width, View.Frame.Height),
					BackgroundColor = UIColor.White,
					AutoresizingMask = UIViewAutoresizing.FlexibleHeight
				};
				scrollView.AddSubview(Title1);
				scrollView.AddSubview(Heading1);
				scrollView.AddSubview(p1);
				scrollView.AddSubview(Heading2);
				scrollView.AddSubview(p2);
				scrollView.AddSubview(Heading3);
				scrollView.AddSubview(p3);
				scrollView.AddSubview(p4);
				scrollView.AddSubview(p5);
				scrollView.AddSubview(p6);
				for (int i = 0; i<scrollView.Subviews.Length ; i++)
				{
					h = scrollView.Subviews[i].Frame.Size.Height+120;
				}
				scrollView.ContentSize = new CGSize(View.Frame.Width, h);
				View = (scrollView);
			}
			catch (Exception ex)
			{
				LoggingClass.LogError(ex.ToString(), screenid, ex.StackTrace);
			}

		}
	}
}
