using System;
using System.Linq;

using Android.App;
using Android.OS;
using Android.Views;
using Android.Widget;
using Android.Util;

namespace WineHangouts
{
    [Activity(Label = "My Tastings")]
    public class MyTastingActivity : Activity, IPopupParent
    {
        public int customerid;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            customerid = Convert.ToInt32(CurrentUser.getUserId());
            SetContentView(Resource.Layout.MyTasting);
            try
            {
                LoggingClass.LogInfo("Entered into MyTastings");
                ActionBar.SetHomeButtonEnabled(true);
                ActionBar.SetDisplayHomeAsUpEnabled(true);

                ServiceWrapper svc = new ServiceWrapper();

                var MYtastings = svc.GetMyTastingsList(customerid).Result;

                //List<Tastings> myArr;

                //  myArr1 = SampleData1();

                ListView wineList = FindViewById<ListView>(Resource.Id.MyTasting);
                // myArr1 = SampleData1();
                MyTastingAdapter adapter = new MyTastingAdapter(this, MYtastings.TastingList.ToList());
                wineList.Adapter = adapter;
                ProgressIndicator.Hide();
            }

            catch (Exception exe)
            {
                LoggingClass.LogError(exe.Message+"In my review's");
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
                LoggingClass.LogInfo("Exited from MyTastings");
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