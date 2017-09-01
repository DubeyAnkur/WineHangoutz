using System;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;
using Android.Graphics;
using Android.Util;
using System.Net;
using Hangout.Models;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;
using System.Collections.Generic;

using AndroidHUD;
using Android.Views.InputMethods;
//using System.Drawing.Drawing2D;

namespace WineHangouts
{
	[Activity(Label = "My Profile", ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
	public class ProfileActivity : Activity, IPopupParent
    {
		Stopwatch st;
		ImageView propicimage;
		WebClient webClient;
		private int screenid = 8;
        ImageView gifImageView;
        Boolean indirect = true;

        protected override void OnCreate(Bundle bundle)
		{
			CheckInternetConnection();
			base.OnCreate(bundle);

			SetContentView(Resource.Layout.Profile);
			//st.Start();
			try
			{
				///LoggingClass.UploadErrorLogs(LoggingClass.CreateDirectoryForLogs());
				LoggingClass.LogInfo("Entered into Profile Activity", screenid);
				ActionBar.SetHomeButtonEnabled(true);
				ActionBar.SetDisplayHomeAsUpEnabled(true);
				int userId = Convert.ToInt32(CurrentUser.getUserId());
				ServiceWrapper sw = new ServiceWrapper();
				var output = sw.GetCustomerDetails(userId).Result;
				propicimage = FindViewById<ImageView>(Resource.Id.propic);
                propicimage.SetImageResource(Resource.Drawable.Loading);
                RefreshParent();
                if (indirect == true)
                {
                    System.Threading.Timer timer = null;
                    timer = new System.Threading.Timer((obj) =>
                    {
                        DownloadAsync(this, System.EventArgs.Empty);
                        timer.Dispose();
                    },
                                null, 4000, System.Threading.Timeout.Infinite);
                }
                else
                {
                    DownloadAsync(this, System.EventArgs.Empty);
                }
               
               
                ImageButton changepropic = FindViewById<ImageButton>(Resource.Id.btnChangePropic);
				changepropic.Click += delegate
				{
					LoggingClass.LogInfo("Clicked on change propic", screenid);
					Intent intent = new Intent(this, typeof(ProfilePicturePickDialog));
					StartActivity(intent);
				};
				changepropic.Dispose();
				EditText Firstname = FindViewById<EditText>(Resource.Id.txtFirstName);
				Firstname.Text = output.customer.FirstName;
                
                Firstname.FocusableInTouchMode = false;
                Firstname.Click+= delegate {
                    Firstname.FocusableInTouchMode = true;
                };
                
                TextView card = FindViewById<TextView>(Resource.Id.txtcard1);
                card.Text = output.customer.CardNumber;
                TextView exp = FindViewById<TextView>(Resource.Id.txtexp);
                exp.Text = output.customer.ExpireDate.ToString();
                if (exp.Text == null || exp.Text == "")
                { exp.Text = "--"; }
                else { exp.Text = output.customer.ExpireDate.ToString("yyyy/MM/dd"); }

                EditText Lastname = FindViewById<EditText>(Resource.Id.txtLastName);
				Lastname.Text = output.customer.LastName;
                Lastname.FocusableInTouchMode = false;
                Lastname.Click += delegate {
                    Lastname.FocusableInTouchMode = true;
                };
                EditText Mobilenumber = FindViewById<EditText>(Resource.Id.txtMobileNumber);
                Mobilenumber.FocusableInTouchMode = false;
                string phno1 = output.customer.PhoneNumber;
				string phno2 = output.customer.Phone2;
                if (phno1 != null)
                {
                    Mobilenumber.Text = phno1;
                }
                else
                {
                    Mobilenumber.Text = phno2;
                }
                Mobilenumber.Click += delegate {
                    Mobilenumber.FocusableInTouchMode = true;
                };
                EditText Email = FindViewById<EditText>(Resource.Id.txtEmail);
               
                    Email.Text = output.customer.Email;
                Email.FocusableInTouchMode = false;
                Email.Click += delegate {
                    Email.FocusableInTouchMode = true;
                };

                EditText Address = FindViewById<EditText>(Resource.Id.txtAddress);
				string Addres2 = output.customer.Address2;
				string Addres1 = output.customer.Address1;
				Address.Text = string.Concat(Addres1, Addres2);
                Address.FocusableInTouchMode = false;
                Address.Click += delegate {
                    Address.FocusableInTouchMode = true;
                };
                //EditText City = FindViewById<EditText>(Resource.Id.txtCity);
                //City.Text = output.customer.CardNumber;
                //if (CurrentUser.getUserId() != null)
                //{
                //	City.Enabled = false;
                //}
                //else { City.Enabled = true; }
                EditText PinCode = FindViewById<EditText>(Resource.Id.txtZip);
                PinCode.Text = output.customer.Zip;
                PinCode.FocusableInTouchMode = false;
                PinCode.Click += delegate {
                    PinCode.FocusableInTouchMode = true;
                };
                Button updatebtn = FindViewById<Button>(Resource.Id.UpdateButton);
				Spinner spn = FindViewById<Spinner>(Resource.Id.spinner);
				Spinner Prefered = FindViewById<Spinner>(Resource.Id.spinner1);
				//spn.SetSelection(4);

				string state = output.customer.State;
				int Preferedstore = output.customer.PreferredStore;
				List<string> storelist = new List<string>();
				storelist.Add("--Select your preferred store--");
				storelist.Add("Wall");
				storelist.Add("Point Pleasant");
				storelist.Add("All");
                 gifImageView = FindViewById<ImageView>(Resource.Id.gifImageView1);
                //gifImageView.StartAnimation();
            
                List<string> StateList = new List<string>();
				StateList.Add("AL");
				StateList.Add("AK");
				StateList.Add("AZ");
				StateList.Add("AR");
				StateList.Add("CA");
				StateList.Add("CO");
				StateList.Add("CT");
				StateList.Add("DE");
				StateList.Add("FL");
				StateList.Add("GA");
				StateList.Add("HI");
				StateList.Add("ID");
				StateList.Add("IL");
				StateList.Add("IN");
				StateList.Add("IA");
				StateList.Add("KS");
				StateList.Add("KY");
				StateList.Add("LA");
				StateList.Add("ME");
				StateList.Add("MD");
				StateList.Add("MA");
				StateList.Add("MI");
				StateList.Add("MN");
				StateList.Add("MS");
				StateList.Add("MO");
				StateList.Add("MT");
				StateList.Add("NE");
				StateList.Add("NV");
				StateList.Add("NH");
				StateList.Add("NJ");
				StateList.Add("NM");
				StateList.Add("NY");
				StateList.Add("NC");
				StateList.Add("ND");
				StateList.Add("OH");
				StateList.Add("OK");
				StateList.Add("OR");
				StateList.Add("PA");
				StateList.Add("RI");
				StateList.Add("SC");
				StateList.Add("SD");
				StateList.Add("TN");
				StateList.Add("TX");
				StateList.Add("UT");
				StateList.Add("VT");
				StateList.Add("VA");
				StateList.Add("WA");
				StateList.Add("WV");
				StateList.Add("WI");
				StateList.Add("WY");
				int i =		StateList.IndexOf(state.ToString());
				spn.SetSelection(i);
				//int p = storelist.IndexOf(Prefered.SelectedItem.ToString());
				Prefered.SetSelection(Preferedstore);
               
                if (CurrentUser.getUserId() == null)
				{
					AlertDialog.Builder aler = new AlertDialog.Builder(this, Resource.Style.MyDialogTheme);
					aler.SetTitle("Sorry");
					aler.SetMessage("This feature is available only  for VIP Users");
					aler.SetPositiveButton("Login", delegate
					{
						var intent = new Intent(this, typeof(LoginActivity));
						StartActivity(intent);
					});
                    aler.SetNegativeButton("KnowMore", delegate
                    {
                        var uri = Android.Net.Uri.Parse("https://hangoutz.azurewebsites.net/index.html");
                        var intent = new Intent(Intent.ActionView, uri);
                        StartActivity(intent);
                        

                    
                    });
                    aler.SetNeutralButton("Cancel", delegate
                    {
                        var intent = new Intent(this, typeof(TabActivity));
                        StartActivity(intent);
                    });
                    Dialog dialog1 = aler.Create();
					dialog1.Show();
				}
				else
				{
					updatebtn.Click += async delegate
					{
                        if ((Email.Text.Contains("@")) == false || (Email.Text.Contains(".")) == false)
                        {
                            AndHUD.Shared.ShowErrorWithStatus(this, "Email is invalid", MaskType.Clear, TimeSpan.FromSeconds(2));
                        }
                        else if ((PinCode.Text.Length != 5))
                        {
                            AndHUD.Shared.ShowErrorWithStatus(this, "Zipcode is invalid", MaskType.Clear, TimeSpan.FromSeconds(2));
                        }
                        else
                        {
                            AndHUD.Shared.Show(this, "Please Wait...", Convert.ToInt32(MaskType.Clear));
                         
                            Customer customer = new Customer()
                            {
                                FirstName = Firstname.Text,
                                LastName = Lastname.Text,
                                PhoneNumber = Mobilenumber.Text,
                                Address1 = Address.Text,
                                Email = Email.Text,
                                CustomerID = userId,
                                //State = State.Text,
                                State = spn.SelectedItem.ToString(),

                                //City = City.Text
                                //CardNumber = City.Text,
                                Zip = PinCode.Text,
                                PreferredStore = Convert.ToInt32(Prefered.SelectedItemId)



                            };
                            CurrentUser.SavePrefered(Convert.ToInt32(Prefered.SelectedItemId));
                            LoggingClass.LogInfo("Clicked on update info", screenid);
                            var x = await sw.UpdateCustomer(customer);
                            if (x == 1)
                            {
                               // Toast.MakeText(this, "Thank you your profile is Updated", ToastLength.Short).Show();
                            }
                            AndHUD.Shared.Dismiss();
                            AndHUD.Shared.ShowSuccess(this, "Profile Updated", MaskType.Clear, TimeSpan.FromSeconds(2));
                          //StartActivity(intent);
                        }
                    }; 

				}
			}
			catch (Exception exe)
			{
				LoggingClass.LogError(exe.Message, screenid, exe.StackTrace.ToString());
				AlertDialog.Builder aler = new AlertDialog.Builder(this);
				aler.SetTitle("Sorry");
				aler.SetMessage("We're under maintainence");
				aler.SetNegativeButton("Ok", delegate { });
				Dialog dialog = aler.Create();
				dialog.Show();
			}
		
			ProgressIndicator.Hide();
		}

        public bool CheckInternetConnection()
		{

			string CheckUrl = "http://google.com";

			try
			{
				HttpWebRequest iNetRequest = (HttpWebRequest)WebRequest.Create(CheckUrl);

				iNetRequest.Timeout = 5000;

				WebResponse iNetResponse = iNetRequest.GetResponse();

				// Console.WriteLine ("...connection established..." + iNetRequest.ToString ());
				iNetResponse.Close();

				return true;

			}
			catch (WebException ex)
			{
				AlertDialog.Builder aler = new AlertDialog.Builder(this, Resource.Style.MyDialogTheme);
				aler.SetTitle("Sorry");
				LoggingClass.LogInfo("Please check your internet connection", screenid);
				aler.SetMessage("Please check your internet connection");
				aler.SetNegativeButton("Ok", delegate { });
				Dialog dialog = aler.Create();
				dialog.Show();
				return false;
			}
		}

		public async void DownloadAsync(object sender, System.EventArgs ea)
		{
			try
			{
			
					webClient = new WebClient();
					var url = new Uri("https://icsintegration.blob.core.windows.net/profileimages/" + Convert.ToInt32(CurrentUser.getUserId()) + ".jpg");
					byte[] imageBytes = null;
					//progressLayout.Visibility = ViewStates.Visible;
					try
					{
						imageBytes = await webClient.DownloadDataTaskAsync(url);

					}
					catch (TaskCanceledException)
					{
						//this.progressLayout.Visibility = ViewStates.Gone;
						return;
					}
					catch (Exception exe)
					{
						LoggingClass.LogError(exe.Message, screenid, exe.StackTrace.ToString());
						
						Bitmap imgWine = BlobWrapper.ProfileImages(Convert.ToInt32(CurrentUser.getUserId()));
						propicimage.SetImageBitmap(imgWine);
						return;
					}
                if (imageBytes != null)
                {

                    try
                    {
                        string documentsPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
                        string localFilename = "user.png";
                        string localPath = System.IO.Path.Combine(documentsPath, localFilename);

                        FileStream fs = new FileStream(localPath, FileMode.OpenOrCreate);
                        await fs.WriteAsync(imageBytes, 0, imageBytes.Length);
                        //Console.WriteLine("Saving image in local path: " + localPath);
                        fs.Close();
                        BitmapFactory.Options options = new BitmapFactory.Options()
                        {
                            InJustDecodeBounds = true
                        };
                        await BitmapFactory.DecodeFileAsync(localPath, options);

                        Bitmap bitmap = await BitmapFactory.DecodeFileAsync(localPath);
                        if (bitmap == null)
                        {
                            propicimage.SetImageResource(Resource.Drawable.ProfileEmpty);
                        }
                        propicimage.SetImageBitmap(bitmap);

                    }
                    catch (Exception exe)
                    {
                        LoggingClass.LogError(exe.Message, screenid, exe.StackTrace.ToString());
                    }
                    st.Stop();
                    LoggingClass.LogTime("Download aSync image profile", st.Elapsed.TotalSeconds.ToString());
                    propicimage.Dispose();
                }else
                {
                    propicimage.SetImageResource(Resource.Drawable.ProfileEmpty);
                }
			}
			catch (Exception exe)
			{
				LoggingClass.LogError(exe.Message, screenid, exe.StackTrace.ToString());
			}
		}
        Boolean isValidEmail(String email)
        {
            return Android.Util.Patterns.EmailAddress.Matcher(email).Matches();
        }
        public override bool OnOptionsItemSelected(IMenuItem item)
		{
			if (item.ItemId == Android.Resource.Id.Home)
			{
				//MoveTaskToBack(true);
				//Finish();
				//LoggingClass.LogInfo("Exited from profile ", screenid);
				//return false;
				var intent = new Intent(this, typeof(TabActivity));
				LoggingClass.LogInfo("Clicked on options menu About", screenid);
				StartActivity(intent);

			}
			return base.OnOptionsItemSelected(item);
		}

		public void RefreshParent()
		{
			ServiceWrapper svc = new ServiceWrapper();
			int userId = Convert.ToInt32(CurrentUser.getUserId());
			var output = svc.GetCustomerDetails(userId).Result;
			Bitmap imageBitmap = BlobWrapper.ProfileImages(userId);
		}

		public Bitmap ResizeAndRotate(Bitmap image, int width, int height)
		{
			var matrix = new Matrix();
			var scaleWidth = ((float)width) / image.Width;
			var scaleHeight = ((float)height) / image.Height;
			matrix.PostRotate(90);
			matrix.PreScale(scaleWidth, scaleHeight);
			return Bitmap.CreateBitmap(image, 0, 0, image.Width, image.Height, matrix, true);
		}
		public Bitmap Resize(Bitmap image, int width, int height)
		{
			var matrix = new Matrix();
			var scaleWidth = ((float)width) / image.Width;
			var scaleHeight = ((float)height) / image.Height;
			matrix.PreScale(scaleWidth, scaleHeight);
			return Bitmap.CreateBitmap(image, 0, 0, image.Width, image.Height, matrix, true);
		}
		protected override void OnPause()
		{
			base.OnPause();
			LoggingClass.LogInfo("OnPause state in Profile activity", screenid);

		}

		protected override void OnResume()
		{
			base.OnResume();
			LoggingClass.LogInfo("OnResume state in Profile activity", screenid);
		}

	}

}