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
		protected FirstViewController(IntPtr handle) : base(handle)
		{
			this.Title = "Locations";
		}

		public override void ViewDidLoad()
		{
			//AboutController1.ViewDidLoad(base);

			// Perform any additional setup after loading the view, typically from a nib.
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
			btnMan.SetTitle("Manasquan",UIControlState.Normal);
			btnSec.SetTitle("Secaucus",UIControlState.Normal);
			btnPP.SetTitle("Point Pleasant", UIControlState.Normal);
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
			btnMan.TouchDown  += (sender, e) =>
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
				NavigationController.NavigationBar.TopItem.Title = "Manasquan";
				NavigationController.PushViewController(new PhyCollectionView(flowLayout, 1), false);

				BTProgressHUD.Dismiss();
			};
			btnSec.TouchUpInside += (sender, e) => { 
				UIAlertView alert = new UIAlertView()
				{
					Title = "Secaucus Store",
					Message = "Coming Soon..."
				};
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
				NavigationController.NavigationBar.TopItem.Title = "Point Plesant";
				NavigationController.PushViewController(new PhyCollectionView(flowLayout, 2), false);
				BTProgressHUD.Dismiss();
			};
		}

		public override void DidReceiveMemoryWarning()
		{
			base.DidReceiveMemoryWarning();
			// Release any cached data, images, etc that aren't in use.
		}

		public override void ViewDidAppear(bool animated)
		{
			base.ViewDidAppear(animated);
			//NavigationController.Title = "Locations";
			NavigationController.NavigationBar.TopItem.Title = "Locations";


			//Login Check Start
			//user.Clear();
			string validUser = CurrentUser.RetreiveUserName();
			if (validUser == "" || validUser == null)
			{
				LoginViewController yourController = new LoginViewController();
				yourController.nav = NavigationController;
				yourController.root = this;
				yourController.ModalPresentationStyle = UIModalPresentationStyle.FormSheet;
				this.PresentModalViewController(yourController, false);
			}
			//Login Check End

		}
	}
}
