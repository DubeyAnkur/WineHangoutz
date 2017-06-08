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
		private int screenid = 1;

		public override void ViewDidLoad()
		{
				
		//AboutController1.ViewDidLoad(base);

			// Perform any additional setup after loading the view, typically from a nib.
			//LoggingClass.UploadErrorLogs();
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

		public void BindClicks(UIButton btnMan, UIButton btnSec, UIButton btnPP, UIView parentView)
		{
			try
			{



				btnMan.TouchDown += (sender, e) =>
			   {
				   BTProgressHUD.Show("Loading..."); //show spinner + text
			   };
				btnPP.TouchDown += (sender, e) =>
				{
					BTProgressHUD.Show("Loading..."); //show spinner + text
				};
				btnMan.TouchUpInside += (sender, e) =>
				{
					//https://components.xamarin.com/gettingstarted/btprogresshud/true
					//BTProgressHUD.Show(); //shows the spinner

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
					LoggingClass.LogInfo("Entered into Wall", screenid);
					BTProgressHUD.Dismiss();
				};
				btnSec.TouchUpInside += (sender, e) =>
				{
					UIAlertView alert = new UIAlertView()
					{
						Title = "Secaucus Store",
						Message = "Coming Soon..."
					};
					LoggingClass.LogInfo("Clicked on seacuces", screenid);


					alert.AddButton("OK");
					alert.Show();

					//LoginViewController yourController = new LoginViewController();
					//yourController.ModalPresentationStyle = UIModalPresentationStyle.OverCurrentContext;
					//this.PresentModalViewController(yourController, false);

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
					LoggingClass.LogInfo("Entered into Point Plesant", screenid);
					NavigationController.NavigationBar.TopItem.Title = "Locations";
					NavigationController.PushViewController(new PhyCollectionView(flowLayout, 2), false);

					BTProgressHUD.Dismiss();
				};
			}
				catch (Exception ex)
				{
					LoggingClass.LogError (ex.ToString(), screenid,ex.StackTrace);
				}
		}
		public override void ViewDidAppear(bool animated)
		{
			base.ViewDidAppear(animated);
			//NavigationController.Title = "Locations";
			NavigationController.NavigationBar.TopItem.Title = "Locations";
			string validUser = CurrentUser.RetreiveUserName();

			LoggingClass.LogInfo("opened app " + validUser, screenid);

			if (validUser == "" || validUser == null)
			{
						LoginViewController yourController = new LoginViewController();
						yourController.nav = NavigationController;
						yourController.root = this;
						yourController.ModalPresentationStyle = UIModalPresentationStyle.FullScreen;
						this.PresentModalViewController(yourController, false);
				//ProfileViewController yourController = new ProfileViewController();
				//yourController.NavCtrl = NavigationController;
				//yourController.root = this;
				//yourController.ModalPresentationStyle = UIModalPresentationStyle.FullScreen;
				//this.PresentModalViewController(yourController, false);
			}
			//login check in
		}

		public override void DidReceiveMemoryWarning()
		{
			base.DidReceiveMemoryWarning();
			// Release any cached data, images, etc that aren't in use.
		}

	}
}
