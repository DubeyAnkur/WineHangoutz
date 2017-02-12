using System;

using UIKit;
using Hangout.Models;

namespace WineHangoutz
{
	public partial class ProfileViewController : UIViewController
	{
		public ProfileViewController() : base("ProfileViewController", null)
		{
		}

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

			ServiceWrapper sw = new ServiceWrapper();
			var cRes = sw.GetCustomerDetails(CurrentUser.RetreiveUserId()).Result;
			txtFirstName.Text = cRes.customer.FirstName;
			txtLastName.Text = cRes.customer.LastName;
			txtCity.Text = cRes.customer.City;
			txtEmail.Text = cRes.customer.Email;
			txtPhone.Text = cRes.customer.PhoneNumber;
			txtAddress.Text = cRes.customer.Address1 + cRes.customer.Address2;
			txtState.Text = cRes.customer.State;

			btnUpdate.TouchUpInside += async (sender, e) =>
			{
				Customer cust = new Customer();
				cust.CustomerID = CurrentUser.RetreiveUserId();
				cust.Address1 = txtAddress.Text;
				cust.FirstName = txtFirstName.Text;
				cust.LastName = txtLastName.Text;
				cust.City = txtCity.Text;
				cust.Email = txtCity.Text;
				cust.Email = txtEmail.Text;
				cust.PhoneNumber = txtPhone.Text;
				cust.State = txtState.Text;

				await sw.UpdateCustomer(cust);

			};
			// Perform any additional setup after loading the view, typically from a nib.
		}

		public override void DidReceiveMemoryWarning()
		{
			base.DidReceiveMemoryWarning();
			// Release any cached data, images, etc that aren't in use.
		}
	}
}

