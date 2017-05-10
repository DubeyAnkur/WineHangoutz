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
				//var lblRateTitle = new UILabel();
				//lblRateTitle.Frame = new CGRect(4, 10, UIScreen.MainScreen.Bounds.Width, 30);
				//lblRateTitle.Text = "Wine Hangouts";
				//lblRateTitle.TextAlignment = UITextAlignment.Center;
				//lblRateTitle.Font = UIFont.FromName("Verdana-Bold", 16f);
				//lblRateTitle.TextColor = UIColor.Purple;


				LoggingClass.LogInfo("Entered into About View", screenid);


				UITextView About = new UITextView();
				About.Frame = new CGRect(0, 0, UIScreen.MainScreen.Bounds.Width, UIScreen.MainScreen.Bounds.Height);
				About.Text = "Wine Hangouts\n\nUncork the Merriment\n\nA delicious bottled wine is the way to the perfect blend of joy. Wine Hangouts is thus,\n\ndeveloped to facilitate the wine lovers to deliver a complete wine experience through the\n\ninternationally acclaimed wines and beers. Sticking strict to the quality, Wine Hangouts brings\n\nin the best self-serve partners in the industry: Enomatic- The World’s #1Wine Dispenser and\n\nWine Preservation System and iPoruIt – A revolutionary self-serve solution for beers, to elevate\n\nthe merriment of our guests to the best.\n\nWHO WE ARE\n\nWine Hangouts ensures the perfect backdrop to relish and to hang out with the wine and beer\n\nlovers, through over 24 assorted wines from the world’s best wineries. Each of our wine outlets\n\nis assured with one of the largest inventories of wine you can experience in New Jersey. The\n\nexclusive opportunity for our privileged customers makes it more enticing by offering FREE\n\ntasting of the 24 wines out of our huge collection.\n\nSo, isn’t it time for a Wine Dine?\n\nWHAT WE OFFER\n\nWe offer- not just the wines and beers, but an experience of enjoyment.\n\nUpon the credential authentication, the mobile application- Wine Hangout takes our privileged\n\ncustomers to the virtual vineyard of savors and flavors, through the following options:\n\n Pick the Choice: Guest is displayed with all the available wines for tasting.\n\n My Tasting: See the list of wines you have tasted and choose more from the remaining\n\nsurprises.\n\n My Reviews: View your ratings and reviews and let other wine lovers explore it along\n\nwith you.\n\n My Favorites: Pick your favorites and we save the list to send you customized\n\nnotifications when there are any discounts or available for wine tasting.\n\n My Profile: Protect your credentials and update it as and when you wish.\n\nRate and Review\n\nLet them also know the best choice of yours.\n\nRate and Review is the spot for rating your favorite wine and Wine Hangouts displays the\n\naverage rating and the individual rating of the users.";
				//View.Add(About);
				//View.ScrollEnabled = true;
				nfloat h = View.Frame.Height * 2.3f;
				//nfloat w = View.Frame.Width * 2.3f;
				scrollView = new UIScrollView
				{
					Frame = new CGRect(0, 0, UIScreen.MainScreen.Bounds.Width, View.Frame.Height),
					ContentSize = new CGSize(UIScreen.MainScreen.Bounds.Width, h),
					BackgroundColor = UIColor.LightGray,
					AutoresizingMask = UIViewAutoresizing.FlexibleHeight
				};
				scrollView.AddSubview(About);
				//scrollView.AddSubview(lblRateTitle);
				View.AddSubview(scrollView);
			}
			catch (Exception ex)
			{
				LoggingClass.LogError(ex.ToString(), screenid, ex.StackTrace);
			}

		}
	}
}
