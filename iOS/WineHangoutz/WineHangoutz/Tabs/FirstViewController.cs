using System;
using CoreGraphics;
using UIKit;
using Foundation;
using System.Threading.Tasks;
using BigTed;


namespace WineHangoutz
{
	public partial class FirstViewController : UIViewController
	{
		public FirstViewController(IntPtr handle) : base(handle)
		{
			this.Title = "Locations";
		}
		private string screen = "FirstViewController";

		public override bool ShouldAutorotate()
		{
		return false;
		}

		public override UIInterfaceOrientationMask GetSupportedInterfaceOrientations()
		{
		return UIInterfaceOrientationMask.Portrait;
		}

		public override UIInterfaceOrientation PreferredInterfaceOrientationForPresentation()
		{
		return UIInterfaceOrientation.Portrait;
		}
		void RestrictRotation(bool restriction)
		{
			AppDelegate app = (AppDelegate)UIApplication.SharedApplication.Delegate;
			app.RestrictRotation = restriction;
		}
		public override void ViewDidLoad()
		{
			try
			{
				nfloat width = UIScreen.MainScreen.Bounds.Width;
				width = width / 2 - 15;
				UICollectionViewFlowLayout flowLayout;
				flowLayout = new UICollectionViewFlowLayout()
				{
					ItemSize = new CGSize(width, 325.0f),
									SectionInset = new UIEdgeInsets(10.0f, 10.0f, 10.0f, 10.0f),
									ScrollDirection = UICollectionViewScrollDirection.Vertical
				} ;
                TokenUpdate();
				if (CurrentUser.GetStore() == 0)
				{
					BTProgressHUD.Show("Please wait...");
					this.Title = "Locations";
					NavigationController.PushViewController(new PhyCollectionView(flowLayout, 1), false);
				}
				else if (CurrentUser.GetStore() == 1)
				{
					BTProgressHUD.Show("Please wait...");
                    this.Title = "Locations";
					NavigationController.PushViewController(new PhyCollectionView(flowLayout, 2), false);
				}
			}
			catch(Exception ex)
			{
				LoggingClass.LogError(ex.Message+" User not allowed to send notifications.", screen, ex.StackTrace);
			}
			//AboutController1.ViewDidLoad(base);
			// Perform any additional setup after loading the view, typically from a nib.
			//LoggingClass.UploadErrorLogs();
			this.RestrictRotation(false);
			nfloat ScreenHeight = UIScreen.MainScreen.Bounds.Height;
			ScreenHeight = (ScreenHeight - 100) / 3;
			nfloat margin = 1;
			nfloat start = 50;
			UIButton btnMan = new UIButton();
			UIButton btnSec = new UIButton();
			UIButton btnPP = new UIButton();
			btnMan.Frame = new CGRect(0, start, UIScreen.MainScreen.Bounds.Width, ScreenHeight);
			btnPP.Frame = new CGRect(0, start + ScreenHeight + margin, UIScreen.MainScreen.Bounds.Width, ScreenHeight);
			btnSec.Frame = new CGRect(0, start + (ScreenHeight + margin) * 2, UIScreen.MainScreen.Bounds.Width, ScreenHeight);
			btnMan.SetTitle("Wall",UIControlState.Normal);
			btnSec.SetTitle("Secaucus",UIControlState.Normal);
			btnPP.SetTitle("Pt. Pleasant Beach", UIControlState.Normal);
			btnMan.SetBackgroundImage(new UIImage("Images/city.jpg"), UIControlState.Normal);
			btnSec.SetBackgroundImage(new UIImage("Images/city1.jpg"), UIControlState.Normal);
			btnPP.SetBackgroundImage(new UIImage("Images/beach.jpg"), UIControlState.Normal);

			View.AddSubview(btnMan);
			View.AddSubview(btnSec);
			View.AddSubview(btnPP);

			BindClicks(btnMan, btnSec, btnPP, View);
		}
		public void TokenUpdate()
		{
			try
			{
				AppDelegate ap = new AppDelegate();
				ap.RegisteredForRemoteNotifications(CurrentUser.app,CurrentUser.dt);
			}
			catch (Exception ex)
			{
				LoggingClass.LogError(ex.Message+" User not allowed to send notifications.", screen, ex.StackTrace);
			}
		}
		public void BindClicks(UIButton btnMan, UIButton btnSec, UIButton btnPP, UIView parentView)
		{
			try
			{
				btnMan.TouchDown += (sender, e) =>
			   {
					
				   BTProgressHUD.Show("Loading...",500); //show spinner + text

			   };
				btnPP.TouchDown += (sender, e) =>
				{
					BTProgressHUD.Show("Loading..."); //show spinner + text
				};
				btnMan.TouchUpInside += (sender, e) =>
				{
					nfloat width = UIScreen.MainScreen.Bounds.Width;
					width = width / 2 - 15;

					UICollectionViewFlowLayout flowLayout;
					flowLayout = new UICollectionViewFlowLayout()
					{
						ItemSize = new CGSize(width, 325.0f),
						SectionInset = new UIEdgeInsets(10.0f, 10.0f, 10.0f, 10.0f),
						ScrollDirection = UICollectionViewScrollDirection.Vertical
					};
					NavigationController.NavigationBar.TopItem.Title = "Locations";
					NavigationController.PushViewController(new PhyCollectionView(flowLayout, 1), false);
					LoggingClass.LogInfo("Entered into Wall", screen);
					BTProgressHUD.Dismiss();
				};
				btnSec.TouchUpInside += (sender, e) =>
				{
					UIAlertView alert = new UIAlertView()
					{
						Title = "Secaucus Store",
						Message = "Coming Soon..."
					};
					LoggingClass.LogInfo("Clicked on seacuces", screen);


					alert.AddButton("OK");
					alert.Show();
				};

				btnPP.TouchUpInside += (sender, e) =>
				{
					//async (sender, e)
					//ServiceWrapper svc = new ServiceWrapper();
					//string ret = await svc.GetDataAsync();
					//((UIButton)sender).SetTitle(ret, UIControlState.Normal);
					nfloat width = UIScreen.MainScreen.Bounds.Width;
					width = width / 2 - 15;

					UICollectionViewFlowLayout flowLayout;
					flowLayout = new UICollectionViewFlowLayout()
					{
						//HeaderReferenceSize = new CGSize(width, 275.0f),
						ItemSize = new CGSize(width, 325.0f),
						SectionInset = new UIEdgeInsets(10.0f, 10.0f, 10.0f, 10.0f),
						//SectionInset = new UIEdgeInsets(20, 20, 20, 20),
						ScrollDirection = UICollectionViewScrollDirection.Vertical
						//MinimumInteritemSpacing = 50, // minimum spacing between cells
						//MinimumLineSpacing = 50 // minimum spacing between rows if ScrollDirection is Vertical or between columns if Horizontal
						
					};
					LoggingClass.LogInfo("Entered into Point Plesant", screen);
					NavigationController.NavigationBar.TopItem.Title = "Locations";
					NavigationController.PushViewController(new PhyCollectionView(flowLayout, 2), false);

					BTProgressHUD.Dismiss();
				};
			}
				catch (Exception ex)
				{
				LoggingClass.LogError (ex.ToString(), screen,ex.StackTrace);
				}
		}
		public override void ViewDidAppear(bool animated)
		{
			base.ViewDidAppear(animated);
			NavigationController.Title = "Locations";
			NavigationController.NavigationBar.TopItem.Title = "Locations";
			CurrentUser.navig = NavigationController;
			//string validUser = CurrentUser.GetId();
			//LoggingClass.LogInfo("opened app " + validUser, screenid);
			//if (validUser == "" || validUser == null)
			//{
			//			LoginViewController yourController = new LoginViewController();
			//			yourController.nav = NavigationController;
			//			yourController.root = this;
			//			yourController.ModalPresentationStyle = UIModalPresentationStyle.FullScreen;
			//			this.PresentModalViewController(yourController, false);
			//	//ProfileViewController yourController = new ProfileViewController();
			//	//yourController.NavCtrl = NavigationController;
			//	//yourController.root = this;
			//	//yourController.ModalPresentationStyle = UIModalPresentationStyle.FullScreen;
			//	//this.PresentModalViewController(yourController, false);
			//	CurrentUser.PutLoginStatus(false);
			//}
			//CurrentUser.PutLoginStatus(false);
			//login check in
		}
		public override void DidReceiveMemoryWarning()
		{
			base.DidReceiveMemoryWarning();
			// Release any cached data, images, etc that aren't in use.
		}

	}
}
