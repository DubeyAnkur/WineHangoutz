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

namespace WineHangouts
{
    [Activity(Label = "My Reviews")]
    public class MyReviewActivity : Activity, IPopupParent
    {
        public int uid;
        private int screenid = 5;
        Context parent;
        public int x;
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            uid = Convert.ToInt32(CurrentUser.getUserId());
            
            SetContentView(Resource.Layout.Tasting);
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
					AlertDialog.Builder aler = new AlertDialog.Builder(this);
					//aler.SetTitle("No Reviews Avalilable");
					aler.SetMessage("Sorry you haven't Reviewed our wines");
					aler.SetNegativeButton("Ok", delegate {
						Finish();
						
					});
					LoggingClass.LogInfo("Clicked on Secaucus", screenid);
					Dialog dialog = aler.Create();
					dialog.Show();
				}
				else
				{
					var wineList = FindViewById<ListView>(Resource.Id.listView1);
					// myArr1 = SampleData1();
					Review edit = new Review();
					ReviewPopup editPopup = new ReviewPopup(this, edit);
					MyReviewAdapter adapter = new MyReviewAdapter(this, myArr1);
					wineList.Adapter = adapter;

					wineList.ItemClick += delegate (object sender, AdapterView.ItemClickEventArgs args)
					{
						int WineID = myArr1[args.Position].WineId;
						int storeID = Convert.ToInt32(myArr1[args.Position].PlantFinal);
						ProgressIndicator.Show(this);
						var intent = new Intent(this, typeof(DetailViewActivity));
						intent.PutExtra("WineID", WineID);
						intent.PutExtra("storeid", storeID);
						StartActivity(intent);
					};
					
					LoggingClass.LogInfo("Entered into My Review", screenid);
				}
				ProgressIndicator.Hide();
			}
            catch (Exception exe)
            {
                LoggingClass.LogError(exe.Message, screenid, exe.StackTrace.ToString());
                ProgressIndicator.Hide();
				AlertDialog.Builder aler = new AlertDialog.Builder(this);
                aler.SetTitle("Sorry");
                aler.SetMessage("We're under maintainence");
                aler.SetNegativeButton("Ok", delegate { });
                Dialog dialog = aler.Create();
                dialog.Show();
            }

        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            if (item.ItemId == Android.Resource.Id.Home)
            {
                Finish();
                LoggingClass.LogInfo("Exited from My Review", screenid);
                return false;
            }
            return base.OnOptionsItemSelected(item);
        }
        private void Close_Click(object sender, EventArgs e)
        {
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

        //void listView_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        //{
        //    //Get our item from the list adapter
        //    int WineID = e.Position;
        //        //var intent = new Intent(this, typeof(detailViewActivity));
        //        //intent.PutExtra("WineID", WineID);
        //        //StartActivity(intent);

        //}

        
    }

}

