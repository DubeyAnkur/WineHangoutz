using System;
using CoreGraphics;
using UIKit;
using Foundation;
using BigTed;
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

				nfloat h = View.Frame.Height*2;
				LoggingClass.LogInfo("Entered into About View ", screenid);
				UITextView Title1 = new UITextView();
				Title1.Frame = new CGRect(45, 0, UIScreen.MainScreen.Bounds.Width, UIScreen.MainScreen.Bounds.Height);
				Title1.Text = "Wine Hangouts";
				Title1.TextColor = UIColor.Black;
				Title1.Font=UIFont.FromName("AmericanTypewriter", 35f);
				Title1.Editable = false;

				UITextView Heading1 = new UITextView();
				Heading1.Frame = new CGRect(5, 45, UIScreen.MainScreen.Bounds.Width, UIScreen.MainScreen.Bounds.Height);
				Heading1.Text = "Uncork the Merriment";
				Heading1.TextColor = UIColor.Black;
				Heading1.Font=UIFont.FromName("AmericanTypewriter", 25f);
				Heading1.Editable = false;

				UITextView p1 = new UITextView();
				p1.Frame = new CGRect(5, 85, UIScreen.MainScreen.Bounds.Width, UIScreen.MainScreen.Bounds.Height);
				p1.Text = "A delicious bottled wine is the way to the perfect blend of joy. Wine Hangouts is thus,developed to facilitate the wine lovers to deliver a complete wine experience through the internationally acclaimed wines and beers. Sticking strict to the quality, Wine Hangouts brings in the best self-serve partners in the industry: Enomatic- The World’s #1Wine Dispenser and Wine Preservation System and iPoruIt – A revolutionary self-serve solution for beers, to elevate the merriment of our guests to the best.";
				p1.TextColor = UIColor.Black;
				p1.Font=UIFont.FromName("AmericanTypewriter", 13f);
				p1.Editable = false;

				UITextView Heading2 = new UITextView();
				Heading2.Frame = new CGRect(5, 230, UIScreen.MainScreen.Bounds.Width, UIScreen.MainScreen.Bounds.Height);
				Heading2.Text = "WHO WE ARE";
				Heading2.TextColor = UIColor.Black;
				Heading2.Font=UIFont.FromName("AmericanTypewriter", 20f);
				Heading2.Editable = false;

				UITextView p2 = new UITextView();
				p2.Frame = new CGRect(5, 260, UIScreen.MainScreen.Bounds.Width, UIScreen.MainScreen.Bounds.Height);
				p2.Text = "Wine Hangouts ensures the perfect backdrop to relish and to hang out with the wine and beer lovers, through over 24 assorted wines from the world’s best wineries. Each of our wine outlets is assured with one of the largest inventories of wine you can experience in New Jersey. The exclusive opportunity for our privileged customers makes it more enticing by offering FREE tasting of the 24 wines out of our huge collection.So, isn’t it time for a Wine Dine?";
				p2.TextColor = UIColor.Black;
				p2.Font=UIFont.FromName("AmericanTypewriter", 13f);
				p2.Editable = false;

				UITextView Heading3 = new UITextView();
				Heading3.Frame = new CGRect(5, 395, UIScreen.MainScreen.Bounds.Width, UIScreen.MainScreen.Bounds.Height);
				Heading3.Text = "WHAT WE OFFER";
				Heading3.TextColor = UIColor.Black;
				Heading3.Font=UIFont.FromName("AmericanTypewriter", 20f);
				Heading3.Editable = false;

				UITextView p3 = new UITextView();
				p3.Frame = new CGRect(5, 430, UIScreen.MainScreen.Bounds.Width, UIScreen.MainScreen.Bounds.Height);
				p3.Text = "We offer- not just the wines and beers, but an experience of enjoyment.Upon the credential authentication, the mobile application- Wine Hangout takes our privileged customers to the virtual vineyard of savors and flavors, through the following options:  Pick the Choice: Guest is displayed with all the available wines for tasting. My Tasting: See the list of wines you have tasted and choose more from the remaining surprises. My Reviews: View your ratings and reviews and let other wine lovers explore it along with you. My Favorites: Pick your favorites and we save the list to send you customized notifications when there are any discounts or available for wine tasting. My Profile: Protect your credentials and update it as and when you wish.";
				p3.TextColor = UIColor.Black;
				p3.Font=UIFont.FromName("AmericanTypewriter", 13f);
				p3.Editable = false;

				UITextView p4 = new UITextView();
				p4.Frame = new CGRect(5, 630, UIScreen.MainScreen.Bounds.Width, UIScreen.MainScreen.Bounds.Height);
				p4.Text = "Rate and Review";
				p4.TextColor = UIColor.Black;
				p4.Font=UIFont.FromName("AmericanTypewriter", 18f);
				p4.Editable = false;

				UITextView p5 = new UITextView();
				p5.Frame = new CGRect(5, 655, UIScreen.MainScreen.Bounds.Width, UIScreen.MainScreen.Bounds.Height);
				p5.Text = "Let them also know the best choice of yours.";
				p5.TextColor = UIColor.Black;
				p5.Font=UIFont.FromName("AmericanTypewriter", 18f);
				p5.Editable = false;

				UITextView p6 = new UITextView();
				p6.Frame = new CGRect(5, 655, UIScreen.MainScreen.Bounds.Width, UIScreen.MainScreen.Bounds.Height);
				p6.Text = "Rate and Review is the spot for rating your favorite wine and Wine Hangouts displays the\\ n\\naverage rating and the individual rating of the users.";
				p5.TextColor = UIColor.Black;
				p6.Font=UIFont.FromName("AmericanTypewriter", 18f);
				p6.Editable = false;

				scrollView = new UIScrollView
				{
					Frame = new CGRect(0, 0, UIScreen.MainScreen.Bounds.Width, View.Frame.Height),
					ContentSize = new CGSize(UIScreen.MainScreen.Bounds.Width, h),
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
				View.AddSubview(scrollView);
			}
			catch (Exception ex)
			{
				LoggingClass.LogError(ex.ToString(), screenid, ex.StackTrace);
			}

		}
	}
}
