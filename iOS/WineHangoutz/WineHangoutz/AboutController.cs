using System;
using CoreGraphics;
using UIKit;
using Foundation;
using BigTed;
using ZXing.Mobile;
using MessageUI;

namespace WineHangoutz
{
	public class AboutController1 : UIViewController
	{
		private string screen = "About Controller";
		UINavigationController NavCtrl;
		UIScrollView scrollView;
		MFMailComposeViewController mailController;
		public AboutController1(UINavigationController navCtrl) : base("AboutController", null)
		{
			NavCtrl = navCtrl;
		}
		public override void ViewDidLoad()
		{
			try
			{
				
				//mailController = new MFMailComposeViewController ();

				base.ViewDidLoad();
				//LoggingClass.UploadLogs();
				LoggingClass.LogInfo("Entered into About View ", screen);
				nfloat ScreenHeight = UIScreen.MainScreen.Bounds.Height;
				ScreenHeight = (ScreenHeight - 100) / 3;
				ScreenHeight = ScreenHeight + 30;
				UIImageView backgroud = new UIImageView();
				backgroud.Frame = new CGRect(0, 0, UIScreen.MainScreen.Bounds.Width, ScreenHeight - 10);
				backgroud.Image = new UIImage("Images/aboutus.jpg");

				UITextView Title1 = new UITextView();
				Title1.Frame = new CGRect(0, ScreenHeight, UIScreen.MainScreen.Bounds.Width - 20, UIScreen.MainScreen.Bounds.Height);
				Title1.Text = "Wine Hangouts";
				Title1.TextAlignment = UITextAlignment.Center;
				Title1.TextColor = UIColor.Purple;
				Title1.Font = UIFont.FromName("HelveticaNeue-Bold", 35f);
				Title1.Editable = false;

				UITextView Heading1 = new UITextView();
				Heading1.Frame = new CGRect(5, ScreenHeight + 55, UIScreen.MainScreen.Bounds.Width - 20, UIScreen.MainScreen.Bounds.Height);
				Heading1.Text = "Uncork the Merriment";
				Heading1.TextAlignment = UITextAlignment.Left;
				//Heading1.TextAlignment = UITextAlignment.Justified;
				Heading1.TextColor = UIColor.Black;
				Heading1.Font = UIFont.FromName("Verdana-Bold", 25f);
				Heading1.Editable = false;

				UITextView p1 = new UITextView();
				p1.Frame = new CGRect(5, ScreenHeight + 95, UIScreen.MainScreen.Bounds.Width - 20, UIScreen.MainScreen.Bounds.Height);
				p1.Text = "A delicious bottled wine is the way to the perfect blend of joy. Wine Hangouts is thus,developed to facilitate the wine lovers to deliver a complete wine experience through the internationally acclaimed wines and beers. Sticking strict to the quality, Wine Hangouts brings in the best self-serve partners in the industry: Enomatic- The World’s #1Wine Dispenser and Wine Preservation System and iPoruIt – A revolutionary self-serve solution for beers, to elevate the merriment of our guests to the best.";
				p1.TextColor = UIColor.Black;
				p1.TextAlignment = UITextAlignment.Justified;
				p1.Font = UIFont.FromName("Verdana", 13f);
				p1.Editable = false;

				UITextView Heading2 = new UITextView();
				Heading2.Frame = new CGRect(5, ScreenHeight + 280, UIScreen.MainScreen.Bounds.Width - 20, UIScreen.MainScreen.Bounds.Height);
				Heading2.Text = "WHO WE ARE";
				Heading2.TextColor = UIColor.Black;
				Heading2.TextAlignment = UITextAlignment.Justified;
				Heading2.Font = UIFont.FromName("Verdana-Bold", 20f);
				Heading2.Editable = false;

				UITextView p2 = new UITextView();
				p2.Frame = new CGRect(5, ScreenHeight + 320, UIScreen.MainScreen.Bounds.Width - 20, UIScreen.MainScreen.Bounds.Height);
				p2.Text = "Wine Hangouts ensures the perfect backdrop to relish and to hang out with the wine and beer lovers, through over 24 assorted wines from the world’s best wineries. Each of our wine outlets is assured with one of the largest inventories of wine you can experience in New Jersey. The exclusive opportunity for our privileged customers makes it more enticing by offering FREE tasting of the 24 wines out of our huge collection.So, isn’t it time for a Wine Dine?";
				p2.TextColor = UIColor.Black;
				p2.TextAlignment = UITextAlignment.Justified;
				p2.Font = UIFont.FromName("Verdana", 13f);
				p2.Editable = false;

				UITextView Heading3 = new UITextView();
				Heading3.Frame = new CGRect(5, ScreenHeight + 495, UIScreen.MainScreen.Bounds.Width - 20, UIScreen.MainScreen.Bounds.Height);
				Heading3.Text = "WHAT WE OFFER";
				Heading3.TextAlignment = UITextAlignment.Justified;
				Heading3.TextColor = UIColor.Black;
				Heading3.Font = UIFont.FromName("Verdana-Bold", 20f);
				Heading3.Editable = false;
				Heading3.TextAlignment = UITextAlignment.Left;

				UITextView p3 = new UITextView();
				p3.Frame = new CGRect(5, ScreenHeight + 535, UIScreen.MainScreen.Bounds.Width - 20, UIScreen.MainScreen.Bounds.Height);
				p3.Text = "We offer- not just the wines and beers, but an experience of enjoyment.Upon the credential authentication, the mobile application- Wine Hangout takes our privileged customers to the virtual vineyard of savors and flavors, through the following options:  Pick the Choice: Guest is displayed with all the available wines for tasting.\n My Tasting: See the list of wines you have tasted and choose more from the remaining surprises.\n My Reviews: View your ratings and reviews and let other wine lovers explore it along with you.\n My Favorites: Pick your favorites and we save the list to send you customized notifications when there are any discounts or available for wine tasting.\n My Profile: Protect your credentials and update it as and when you wish.";
				p3.TextColor = UIColor.Black;
				p3.TextAlignment = UITextAlignment.Justified;
				p3.Font = UIFont.FromName("Verdana", 13f);
				p3.Editable = false;

				UITextView p4 = new UITextView();
				p4.Frame = new CGRect(5, ScreenHeight + 815, UIScreen.MainScreen.Bounds.Width - 20, UIScreen.MainScreen.Bounds.Height);
				p4.Text = "Rate and Review";
				p4.TextColor = UIColor.Black;
				p4.TextAlignment = UITextAlignment.Justified;
				p4.Font = UIFont.FromName("Verdana", 18f);
				p4.Editable = false;
				p4.TextAlignment = UITextAlignment.Left;

				UITextView p5 = new UITextView();
				p5.Frame = new CGRect(5, ScreenHeight + 850, UIScreen.MainScreen.Bounds.Width - 20, UIScreen.MainScreen.Bounds.Height);
				p5.Text = "Let them also know the best choice of yours.";
				p5.TextColor = UIColor.Black;
				p5.TextAlignment = UITextAlignment.Justified;
				p5.Font = UIFont.FromName("Verdana", 18f);
				p5.Editable = false;
				//p5.TextAlignment = UITextAlignment.Left;

				UITextView p6 = new UITextView();
				p6.Frame = new CGRect(5, ScreenHeight + 900, UIScreen.MainScreen.Bounds.Width - 20, UIScreen.MainScreen.Bounds.Height);
				p6.Text = "Rate and Review is the spot for rating your favorite wine and Wine Hangouts displays the average rating and the individual rating of the users.";
				p5.TextColor = UIColor.Black;
				p6.Font = UIFont.FromName("Verdana", 18f);
				p6.Editable = false;
				p6.TextAlignment = UITextAlignment.Justified;
				//p6.TextAlignment = UITextAlignment.Left;

				UITextView VersionText = new UITextView();
				VersionText.Frame = new CGRect(5, ScreenHeight + 930, UIScreen.MainScreen.Bounds.Width - 20, UIScreen.MainScreen.Bounds.Height);
				var ver = NSBundle.MainBundle.InfoDictionary["CFBundleVersion"];
				//MonoTouch.Constants.Version;
				//Console.WriteLine(ver);
				VersionText.Text = "Version 1.15("+ver+").\nAll rights reserved.\n Reach us at";
				VersionText.TextColor = UIColor.Black;
				VersionText.Font = UIFont.FromName("Verdana", 18f);
				VersionText.Editable = false;
				VersionText.TextAlignment = UITextAlignment.Center;

				UITextView ContactUsText = new UITextView();
				ContactUsText.Frame = new CGRect(5, ScreenHeight + 1000, UIScreen.MainScreen.Bounds.Width - 20, UIScreen.MainScreen.Bounds.Height);
				ContactUsText.Text = "savvyitsol@gmail.com";
				ContactUsText.TextColor = UIColor.Purple;
				ContactUsText.Font = UIFont.FromName("Verdana", 18f);
				ContactUsText.Editable = false;
				ContactUsText.TextAlignment = UITextAlignment.Center;

				if (MFMailComposeViewController.CanSendMail)
				{
					ContactUsText.UserInteractionEnabled = true;
				}
				else
				{
					ContactUsText.UserInteractionEnabled = false;
				}

				//ContactUsText.ShouldInteractWithUrl += ContactUsText_ShouldInteractWithUrl;
				var tap = new UITapGestureRecognizer { CancelsTouchesInView = false };
				tap.AddTarget(() =>
				{
					mailController = new MFMailComposeViewController();
					mailController.SetToRecipients (new string[]{"savvyitsol@gmail.com"});
					mailController.SetSubject ("Feedback "+ver);
					mailController.SetMessageBody("User info "+CurrentUser.RetreiveUserId().ToString(),true);
                    this.PresentViewController (mailController, true, null);
					mailController.Finished += ( object s, MFComposeResultEventArgs args) =>
					{
						args.Controller.DismissViewController (true, null);
					};
				});
				//ContactUsText.UserInteractionEnabled = true;
				ContactUsText.AddGestureRecognizer(tap);

				nfloat h = 0;

				scrollView = new UIScrollView
				{
					Frame = new CGRect(0, 20, View.Frame.Width, View.Frame.Height),
					ContentSize = new CGSize(View.Frame.Width, View.Frame.Height),
					BackgroundColor = UIColor.White,
					AutoresizingMask = UIViewAutoresizing.FlexibleHeight
				};


				scrollView.AddSubview(backgroud);
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
				scrollView.AddSubview(VersionText);
				scrollView.AddSubview(ContactUsText);

				for (int i = 0; i < scrollView.Subviews.Length; i++)
				{
					h = scrollView.Subviews[i].Frame.Size.Height + 370 + ScreenHeight;
				}
				scrollView.ContentSize = new CGSize(UIScreen.MainScreen.Bounds.Width, h);
				View = (scrollView);
			}
			catch (Exception ex)
			{
				LoggingClass.LogError(ex.ToString(), screen, ex.StackTrace);
				Console.WriteLine(ex.Message);
			}
		}
}
}
