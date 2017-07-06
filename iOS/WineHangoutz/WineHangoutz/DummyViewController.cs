using System;

using UIKit;

namespace WineHangoutz
{
	public partial class DummyViewController : UIViewController
	{
		public DummyViewController() : base("DummyViewController", null)
		{
		}

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
			// Perform any additional setup after loading the view, typically from a nib.
			//txt.BecomeFirstResponder();
			txt.TouchUpInside+= delegate
			{
				txt.AccessibilityScroll(UIAccessibilityScrollDirection.Up);
			};

		}

		public override void DidReceiveMemoryWarning()
		{
			base.DidReceiveMemoryWarning();
			// Release any cached data, images, etc that aren't in use.
		}
		void KeyboardWillShowHandler(object sender, UIKeyboardEventArgs e)
		{
		UpdateButtomLayoutConstraint(e);
		}

		void KeyboardWillHideHandler(object sender, UIKeyboardEventArgs e)
		{
		UpdateButtomLayoutConstraint(e);
		}

		void UpdateButtomLayoutConstraint(UIKeyboardEventArgs e)
		{
			//UIViewAnimationCurve curve = e.AnimationCurve;
			//UIView.Animate(e.AnimationDuration, 0, ConvertToAnimationOptions(e.AnimationCurve), () =>
			//{
			//	nfloat offsetFromBottom = 15;
			//	offsetFromBottom = NMath.Max(0, offsetFromBottom);
			//	SetToolbarContstraint(offsetFromBottom);
			//}, null);
		}
	}
}

