using System;
using UIKit;
using CoreGraphics;
using Foundation;
using PatridgeDev;
using Hangout.Models;
using BigTed;

namespace WineHangoutz
{
	public class DeletePopup : UIViewController
	{
		public UINavigationController NavController;
		public UIViewController parent;

		//Possible Inputss
		public decimal StartsSelected;
		public string Comments = "";
		public int WineId;

		public DeletePopup(int Wine) : base()
		{
			this.Title = "Delete Popup";
			WineId = Wine;
			//this.TabBarItem.Image = UIImage.FromBundle("Images/first");
		}
		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
			this.View.BackgroundColor = new UIColor(0, 0, 0, 0.8f);

			var lblProducer = new UILabel();
			lblProducer.Frame = new CGRect(4, 180, View.Frame.Width - 8, 30);
			lblProducer.Text = "My Tasting";
			lblProducer.BackgroundColor = UIColor.Purple;
			lblProducer.TextAlignment = UITextAlignment.Center;
			this.View.AddSubview(lblProducer);

			//this.View.Alpha = 0.5f;
			UIButton btnClose = new UIButton(new CGRect(9, 185, 20, 20));
			btnClose.SetBackgroundImage(new UIImage("Close.png"), UIControlState.Normal);
			this.View.AddSubview(btnClose);

			btnClose.TouchUpInside += (sender, e) =>
			{
				//var viewCtrl = NavController.PopViewController(false);
				//viewCtrl.Dispose();
				//NavController.PopToViewController(parent,false);
				NavController.DismissViewController(true, null);

			};

			UIImageView imgBtl = new UIImageView(new CGRect(View.Frame.Width - 64, 149, 60, 60));
			imgBtl.Image = UIImage.FromFile("wine_review.png");
			//imgBtl.BackgroundColor = UIColor.White;
			this.View.AddSubview(imgBtl);

			var lblWhite = new UILabel();
			lblWhite.Frame = new CGRect(4, 210, View.Frame.Width - 8, 200);
			lblWhite.BackgroundColor = UIColor.White;
			lblWhite.TextAlignment = UITextAlignment.Center;
			this.View.AddSubview(lblWhite);

			UIButton btnDelete = new UIButton(new CGRect(14, 370, View.Frame.Width - 28, 20));
			UIButton btnCancel = new UIButton(new CGRect(14, 370, View.Frame.Width-30,20));

			//btnSave.SetBackgroundImage(new UIImage("Close.png"), UIControlState.Normal);
			btnCancel.SetTitle("Cancel", UIControlState.Normal);
			btnCancel.HorizontalAlignment = UIControlContentHorizontalAlignment.Right;
			btnCancel.SetTitleColor(UIColor.Purple, UIControlState.Normal);

			btnDelete.SetTitle("Delete", UIControlState.Normal);
			btnDelete.HorizontalAlignment = UIControlContentHorizontalAlignment.Left;
			btnDelete.SetTitleColor(UIColor.Purple, UIControlState.Normal);

			this.View.AddSubview(btnDelete);
			this.View.AddSubview(btnCancel);
			btnCancel.TouchUpInside += (sender, e) =>
			{
				NavController.DismissViewController(true, null);
			};
			try
			{
				btnDelete.TouchDown += async delegate
				{
					ServiceWrapper sw = new ServiceWrapper();
					Review review = new Review();
					review.WineId = WineId;
					review.ReviewUserId = Convert.ToInt32(CurrentUser.RetreiveUserId());
					await sw.DeleteReview(review);

					((IPopupParent)parent).RefreshParent();

					NavController.DismissViewController(true, null);
					//Save Service Call.
					//txtComments
					BTProgressHUD.ShowSuccessWithStatus("Thank you!!!");
				};
			}
			catch (Exception exe)
			{
				string msg = exe.Message;
			}

		}
	}


}
