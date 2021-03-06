using System;
using System.Linq;

using Android.App;
using Android.OS;
using Android.Views;
using Android.Widget;
using Android.Util;
using Hangout.Models;
using Android.Content;
using System.Collections.Generic;
using System.Diagnostics;

namespace WineHangouts
{
    [Activity(Label = "My Tastings", ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    public class MyTastingActivity : Activity, IPopupParent
    {
        public int customerid;
        private int screenid = 6;
		public Context parent;
        List<Tastings> myArr1;
        protected override void OnCreate(Bundle bundle)
        {
			Stopwatch st = new Stopwatch();
			st.Start();
			base.OnCreate(bundle);
            customerid = Convert.ToInt32(CurrentUser.getUserId());
          
            try
            {
                LoggingClass.LogInfo("Entered into My Tasting", screenid);
                ActionBar.SetHomeButtonEnabled(true);
                ActionBar.SetDisplayHomeAsUpEnabled(true);

                ServiceWrapper svc = new ServiceWrapper();

                var MYtastings = svc.GetMyTastingsList(customerid).Result;
                myArr1 = MYtastings.TastingList.ToList();
				if (MYtastings.TastingList.Count == 0)
				{
                    SetContentView(Resource.Layout.EmptyTaste);
                    TextView te = FindViewById<TextView>(Resource.Id.textView123a);
                    //AlertDialog.Builder aler = new AlertDialog.Builder(this, Resource.Style.MyDialogTheme);
                    ////aler.SetTitle("No Reviews Avalilable");
                    //aler.SetMessage("Sorry you haven't Tasted our wines");
                    //LoggingClass.LogInfo("Sorry you haven't Tasted our wines alert", screenid);
                    //aler.SetNegativeButton("Ok", delegate { Finish(); });
                    //LoggingClass.LogInfo("Clicked on Secaucus", screenid);
                    //Dialog dialog = aler.Create();
                    //dialog.Show();
                }
				else
				{
                    SetContentView(Resource.Layout.MyTasting);
                    ListView wineList = FindViewById<ListView>(Resource.Id.MyTasting);

					MyTastingAdapter adapter = new MyTastingAdapter(this, MYtastings.TastingList.ToList());
					wineList.Adapter = adapter;
					wineList.ItemClick += delegate (object sender, AdapterView.ItemClickEventArgs args)
					{
						string WineBarcode = myArr1[args.Position].Barcode;
                        int storeID = myArr1[args.Position].PlantFinal;
						LoggingClass.LogInfo("Clicked on " + myArr1[args.Position].Barcode + " to enter into wine from tasting  details", screenid);
						ProgressIndicator.Show(this);
						var intent = new Intent(this, typeof(DetailViewActivity));
						intent.PutExtra("WineBarcode", WineBarcode);
						intent.PutExtra("storeid", storeID);
						StartActivity(intent);
					};
					
				}
				ProgressIndicator.Hide();
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
			st.Stop();
			LoggingClass.LogTime("Tastingactivity", st.Elapsed.TotalSeconds.ToString());
		}

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            if (item.ItemId == Android.Resource.Id.Home)
            {
                Finish();
                LoggingClass.LogInfo("Exited from My Tasting", screenid);
                return false;
            }
            return base.OnOptionsItemSelected(item);
        }
		protected override void OnPause()
		{
			base.OnPause();
			LoggingClass.LogInfo("OnPause state in MyTasting ativity", screenid);

		}

		protected override void OnResume()
		{
			base.OnResume();
			LoggingClass.LogInfo("OnResume state in MyTasting activity", screenid);
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
            var MYtastings = svc.GetMyTastingsList(customerid).Result;
            ListView wineList = FindViewById<ListView>(Resource.Id.MyTasting);
            // myArr1 = SampleData1();
            MyTastingAdapter adapter = new MyTastingAdapter(this, MYtastings.TastingList.ToList());


            wineList.Adapter = adapter;
            adapter.NotifyDataSetChanged();
        }
    }
}