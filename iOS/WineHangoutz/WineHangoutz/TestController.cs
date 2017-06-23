using System;
using System.Collections.Generic;
using UIKit;
using Hangout.Models;
using Foundation;
using System.Drawing;
using CoreGraphics;

namespace WineHangoutz
{
	public partial class TestController : UIViewController
	{
		//UIPickerView samplePicker;
		PickerDataModel pickerDataModel;
		public TestController() : base()
		{
		}
		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

			// Perform any additional setup after loading the view, typically from a nib.
			// create our simple picker model
			pickerDataModel = new PickerDataModel();
			pickerDataModel.Items.Add("blue");
			pickerDataModel.Items.Add("red");
			//pickerDataModel.Items.Add(“Purple”);
			//pickerDataModel.Items.Add(“White”);

			// set it on our picker class
			statePicker.Model = pickerDataModel;

			// wire up the value change method
			pickerDataModel.ValueChanged += (s, e) =>
			{
				// colorValueLabel.Text = pickerDataModel.SelectedItem;
			};

			// set our initial selection on the label
			//colorValueLabel.Text = pickerDataModel.SelectedItem;

		}
		protected UIView ViewToCenterOnKeyboardShown;
		public virtual bool HandlesKeyboardNotifications()
		{
			return false;
		}

		protected virtual void RegisterForKeyboardNotifications()
		{
			NSNotificationCenter.DefaultCenter.AddObserver(UIKeyboard.WillHideNotification, OnKeyboardNotification);
			NSNotificationCenter.DefaultCenter.AddObserver(UIKeyboard.WillShowNotification, OnKeyboardNotification);
		}
		protected virtual UIView KeyboardGetActiveView()
		{
			return View.FindFirstResponder();
		}

		private void OnKeyboardNotification(NSNotification notification)
		{
			if (!IsViewLoaded) return;
			var visible = notification.Name == UIKeyboard.WillShowNotification;
			UIView.BeginAnimations("AnimateForKeyboard");
			UIView.SetAnimationBeginsFromCurrentState(true);
			UIView.SetAnimationDuration(UIKeyboard.AnimationDurationFromNotification(notification));
			UIView.SetAnimationCurve((UIViewAnimationCurve)UIKeyboard.AnimationCurveFromNotification(notification));
			bool landscape = InterfaceOrientation == UIInterfaceOrientation.LandscapeLeft || InterfaceOrientation == UIInterfaceOrientation.LandscapeRight;
			var keyboardFrame = visible
									? UIKeyboard.FrameEndFromNotification(notification)
									: UIKeyboard.FrameBeginFromNotification(notification);

			//OnKeyboardChanged(visible, landscape.keyboardFrame.Width : keyboardFrame.Height);
			UIView.CommitAnimations();
		}
		protected virtual void OnKeyboardChanged(bool visible, float keyboardHeight)
		{
			var activeView = ViewToCenterOnKeyboardShown ?? KeyboardGetActiveView();
			if (activeView == null)
				return;

			var scrollView = activeView.FindSuperviewOfType(View, typeof(UIScrollView)) as UIScrollView;
			if (scrollView == null)
				return;
			
			if (!visible)
				RestoreScrollPosition(scrollView);
			else
				CenterViewInScroll(activeView, scrollView, keyboardHeight);
		}
		protected virtual void CenterViewInScroll(UIView viewToCenter, UIScrollView scrollView, float keyboardHeight)
		{
			var contentInsets = new UIEdgeInsets(0.0f, 0.0f, keyboardHeight, 0.0f);
			scrollView.ContentInset = contentInsets;
			scrollView.ScrollIndicatorInsets = contentInsets;

			CGRect box = new CGRect(View.Bounds.Location, View.Bounds.Size);
			// Position of the active field relative isnside the scroll view
			//RectangleF relativeFrame = viewToCenter.Superview.ConvertRectToView(bo) ;//viewToCenter.Superview.ConvertRectToView(viewToCenter.Frame, scrollView);

			bool landscape = InterfaceOrientation == UIInterfaceOrientation.LandscapeLeft || InterfaceOrientation == UIInterfaceOrientation.LandscapeRight;
			var spaceAboveKeyboard = (landscape ? scrollView.Frame.Width : scrollView.Frame.Height) - keyboardHeight;

			// Move the active field to the center of the available space
			//var offset = relativeFrame.Y - (spaceAboveKeyboard - viewToCenter.Frame.Height) / 2;
			scrollView.ContentOffset = new PointF(10, 10);
		}
		protected virtual void RestoreScrollPosition(UIScrollView scrollView)
		{
			scrollView.ContentInset = UIEdgeInsets.Zero;
			scrollView.ScrollIndicatorInsets = UIEdgeInsets.Zero;
		}
		protected void DismissKeyboardOnBackgroundTap()
		{
			// Add gesture recognizer to hide keyboard
			var tap = new UITapGestureRecognizer { CancelsTouchesInView = false };
			tap.AddTarget(() => View.EndEditing(true));
			View.AddGestureRecognizer(tap);
		}
		//public override bool HandlesKeyboardNotifications()
		//{
		//	return true;
	}


	public class PickerDataModel : UIPickerViewModel

	{
		public event EventHandler<EventArgs> ValueChanged;

		/// <summary>
		/// The items to show up in the picker
		/// </summary>
		public List<string> Items { get; private set; }

		/// <summary>
		/// The current selected item
		/// </summary>
		public string SelectedItem
		{
			get { return Items[selectedIndex]; }
		}

		int selectedIndex = 0;

		public PickerDataModel()
		{
			Items = new List<string>();
		}

		/// <summary>
		/// Called by the picker to determine how many rows are in a given spinner item
		/// </summary>
		public override nint GetRowsInComponent(UIPickerView picker, nint component)
		{
			return Items.Count;
		}

		/// <summary>
		/// called by the picker to get the text for a particular row in a particular
		/// spinner item
		/// </summary>
		public override string GetTitle(UIPickerView picker, nint row, nint component)
		{
			return Items[(int)row];
		}

		/// <summary>
		/// called by the picker to get the number of spinner items
		/// </summary>
		public override nint GetComponentCount(UIPickerView picker)
		{
			return 1;
		}

		/// <summary>
		/// called when a row is selected in the spinner
		/// </summary>
		public override void Selected(UIPickerView picker, nint row, nint component)
		{
			selectedIndex = (int)row;
				if (ValueChanged != null)
				{
					ValueChanged(this, new EventArgs());
				}
		}
	}
	public static class ViewExtensions
	{
		public static UIView FindFirstResponder(this UIView view)
		{
			if (view.IsFirstResponder)
			{
				return view;
			}
			foreach (UIView subView in view.Subviews)
			{
				var firstResponder = subView.FindFirstResponder();
				if (firstResponder != null)
					return firstResponder;
			}
			return null;
		}
		public static UIView FindSuperviewOfType(this UIView view, UIView stopAt, Type type)
		{
			if (view.Superview != null)
			{
				if (type.IsAssignableFrom(view.Superview.GetType()))
				{
					return view.Superview;
				}

				if (view.Superview != stopAt)
					return view.Superview.FindSuperviewOfType(stopAt, type);
			}

			return null;
		}
	}

}

