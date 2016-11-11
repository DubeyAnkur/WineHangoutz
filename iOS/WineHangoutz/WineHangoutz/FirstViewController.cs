using System;
using CoreGraphics;
using UIKit;
using Foundation;

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
			base.ViewDidLoad();
			// Perform any additional setup after loading the view, typically from a nib.
			nfloat ScreenHeight = UIScreen.MainScreen.Bounds.Height;
			ScreenHeight = (ScreenHeight - 100) / 3;
			nfloat margin = 2;
			nfloat start = 50;
			UIButton btnMan = new UIButton();
			UIButton btnSec = new UIButton();
			UIButton btnPP = new UIButton();

			btnMan.Frame = new CGRect(0, start, UIScreen.MainScreen.Bounds.Width, ScreenHeight);
			btnSec.Frame = new CGRect(0, start + ScreenHeight + margin, UIScreen.MainScreen.Bounds.Width, ScreenHeight);
			btnPP.Frame = new CGRect(0, start + (ScreenHeight + margin) * 2, UIScreen.MainScreen.Bounds.Width, ScreenHeight);
			btnMan.SetTitle("Manasquan",UIControlState.Normal);
			btnSec.SetTitle("Secacus", UIControlState.Normal);
			btnPP.SetTitle("Point Pleasant", UIControlState.Normal);
			btnMan.SetBackgroundImage(new UIImage("Images/Man.png"), UIControlState.Normal);
			btnSec.SetBackgroundImage(new UIImage("Images/Sec.jpg"), UIControlState.Normal);
			btnPP.SetBackgroundImage(new UIImage("Images/PP.jpg"), UIControlState.Normal);

			View.AddSubview(btnMan);
			View.AddSubview(btnSec);
			View.AddSubview(btnPP);

			BindClicks(btnMan, btnSec, btnPP);
			//TabBarItem.Image = new UIImage("Star4.png");
			//btnNavigate.TouchUpInside += (sender, e) => {
			//	NavigationController.PushViewController(new IsolatedView(), false);
			//};
		}

		public void BindClicks(UIButton btnMan, UIButton btnSec, UIButton btnPP)
		{
			btnMan.TouchUpInside += (sender, e) =>
			{
				//NavigationController.PushViewController(myList, false);
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
				NavigationController.PushViewController(new PhyCollectionView(flowLayout), false);
			};
			btnSec.TouchUpInside += (sender, e) => { 
				NavigationController.PushViewController(new DetailViewController(), false);
			};

			btnPP.TouchUpInside += async (sender, e) =>
			{
				ServiceWrapper svc = new ServiceWrapper();
				string ret = await svc.GetDataAsync();
				((UIButton)sender).SetTitle(ret, UIControlState.Normal);
			};
		}

		public override void DidReceiveMemoryWarning()
		{
			base.DidReceiveMemoryWarning();
			// Release any cached data, images, etc that aren't in use.
		}
	}
}
