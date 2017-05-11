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

namespace WineHangouts
{
    [Activity(Label = "My Tastings")]
    public class MyTastingActivity : Activity, IPopupParent
    {
        public int customerid;
        private int screenid = 6;
        List<Tastings> myArr1;
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            customerid = Convert.ToInt32(CurrentUser.getUserId());
            SetContentView(Resource.Layout.MyTasting);
            try
            {
                LoggingClass.LogInfo("Entered into ",screenid);
                ActionBar.SetHomeButtonEnabled(true);
                ActionBar.SetDisplayHomeAsUpEnabled(true);

                ServiceWrapper svc = new ServiceWrapper();

                var MYtastings = svc.GetMyTastingsList(customerid).Result;
                myArr1 = MYtastings.TastingList.ToList();

                ListView wineList = FindViewById<ListView>(Resource.Id.MyTasting);
                
                MyTastingAdapter adapter = new MyTastingAdapter(this, MYtastings.TastingList.ToList());
                wineList.Adapter = adapter;
                wineList.ItemClick += delegate (object sender, AdapterView.ItemClickEventArgs args)
                {
                    int WineID = myArr1[args.Position].WineId;
                    int storeID = myArr1[args.Position].PlantFinal;
                    ProgressIndicator.Show(this);
                    var intent = new Intent(this, typeof(DetailViewActivity));
                    intent.PutExtra("WineID", WineID);
                    intent.PutExtra("storeid", storeID);
                    StartActivity(intent);
                };
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

        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            if (item.ItemId == Android.Resource.Id.Home)
            {
                Finish();
                LoggingClass.LogInfo("Exited from ",screenid);
                return false;
            }
            return base.OnOptionsItemSelected(item);
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