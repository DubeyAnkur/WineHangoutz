using System;
using System.Collections.Generic;
using System.Linq;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;
using Android.Util;
using Hangout.Models;
using System.Diagnostics;
using AndroidHUD;

namespace WineHangouts
{
    [Activity(Label = "My Reviews", ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    public class MyReviewActivity : Activity, IPopupParent
    {
        public int uid;
        private int screenid = 5;
        Context parent;
        public int x;
        protected override void OnCreate(Bundle bundle)
        {
			Stopwatch st = new Stopwatch();
			st.Start();
			base.OnCreate(bundle);
            uid = Convert.ToInt32(CurrentUser.getUserId());
            
            
            try
            {
                ActionBar.SetHomeButtonEnabled(true);
                ActionBar.SetDisplayHomeAsUpEnabled(true);
                ServiceWrapper svc = new ServiceWrapper();
                ItemReviewResponse uidreviews = new ItemReviewResponse();
                uidreviews = svc.GetItemReviewUID(uid).Result;
                List<Review> myArr1;
                myArr1 = uidreviews.Reviews.ToList();
				int c = uidreviews.Reviews.Count;
				if (c == 0)
				{
                    SetContentView(Resource.Layout.ReviewEmpty);
                    TextView txtName = FindViewById<TextView>(Resource.Id.textView1);
                    ImageView Imag = FindViewById<ImageView>(Resource.Id.imageView1);
				}
				else
				{
                    SetContentView(Resource.Layout.Tasting);
                    var wineList = FindViewById<ListView>(Resource.Id.listView1);
					Review edit = new Review();
					ReviewPopup editPopup = new ReviewPopup(this, edit);
					MyReviewAdapter adapter = new MyReviewAdapter(this, myArr1);
					wineList.Adapter = adapter;

					wineList.ItemClick += delegate (object sender, AdapterView.ItemClickEventArgs args)
					{
						string WineBarcode = myArr1[args.Position].Barcode;
						int storeID = Convert.ToInt32(myArr1[args.Position].PlantFinal);
						LoggingClass.LogInfoEx("Clicked on " + myArr1[args.Position].Barcode + " to enter into wine details From ReviewAct", screenid);
                        //ProgressIndicator.Show(this);
                        AndHUD.Shared.Show(this, "Loading...", Convert.ToInt32(MaskType.Clear));
                        var intent = new Intent(this, typeof(DetailViewActivity));
						intent.PutExtra("WineBarcode", WineBarcode);
						intent.PutExtra("storeid", storeID);
						StartActivity(intent);
					};
					
					LoggingClass.LogInfo("Entered into My Review", screenid);
				}
                ProgressIndicator.Hide();
                AndHUD.Shared.Dismiss();
            }
            catch (Exception exe)
            {
                LoggingClass.LogError(exe.Message, screenid, exe.StackTrace.ToString());
                AndHUD.Shared.Dismiss();
                ProgressIndicator.Hide();
                AlertDialog.Builder aler = new AlertDialog.Builder(this);
                aler.SetTitle("Sorry");
                aler.SetMessage("We're under maintainence");
                aler.SetNegativeButton("Ok", delegate { });
                Dialog dialog = aler.Create();
                dialog.Show();
            }
			st.Stop();
			LoggingClass.LogTime("Reviewactivity", st.Elapsed.TotalSeconds.ToString());
		}
		protected override void OnPause()
		{
			base.OnPause();
			LoggingClass.LogInfo("OnPause state in MyREview ativity" , screenid);

		}

		protected override void OnResume()
		{
			base.OnResume();
			LoggingClass.LogInfo("OnResume state in MyREview activity" , screenid);
		}
		public override bool OnOptionsItemSelected(IMenuItem item)
        {
            if (item.ItemId == Android.Resource.Id.Home)
            {
                Finish();

                //base.OnBackPressed();
                LoggingClass.LogInfo("Exited from My Review", screenid);
                return false;
            }
            return base.OnOptionsItemSelected(item);
        }
        private void Close_Click(object sender, EventArgs e)
        {
			LoggingClass.LogInfo("Exited from My Review", screenid);
			throw new NotImplementedException();
        }

        private int ConvertPixelsToDp(float pixelValue)
        {
            var dp = (int)((pixelValue) / Resources.DisplayMetrics.Density);
            return dp;
        }
        private int PixelsToDp(int pixels)
        {
            return (int)TypedValue.ApplyDimension(ComplexUnitType.Dip, pixels, Resources.DisplayMetrics);
        } 
        public void RefreshParent()
        {
            ServiceWrapper svc = new ServiceWrapper();
            var uidreviews = svc.GetItemReviewUID(uid).Result;
            ListView wineList = FindViewById<ListView>(Resource.Id.listView1);
            Review edit = new Review();
            ReviewPopup editPopup = new ReviewPopup(this, edit);
            MyReviewAdapter adapter = new MyReviewAdapter(this, uidreviews.Reviews.ToList());
            //adapter.Edit_Click += editPopup.EditPopup;

            wineList.Adapter = adapter;
            adapter.NotifyDataSetChanged();
        }
    }
}

