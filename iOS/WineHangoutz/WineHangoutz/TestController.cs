using System;
using System.Collections.Generic;
using UIKit;
using Hangout.Models;

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
}

