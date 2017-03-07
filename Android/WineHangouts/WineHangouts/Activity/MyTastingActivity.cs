using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Util;
using Hangout.Models;

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
                ActionBar.SetHomeButtonEnabled(true);
                ActionBar.SetDisplayHomeAsUpEnabled(true);

                ServiceWrapper svc = new ServiceWrapper();

                var MYtastings = svc.GetMyTastingsList(customerid).Result;

                List<Tastings> myArr;

                //  myArr1 = SampleData1();

                ListView wineList = FindViewById<ListView>(Resource.Id.MyTasting);
                // myArr1 = SampleData1();
                MyTastingAdapter adapter = new MyTastingAdapter(this, MYtastings.TastingList.ToList());
                wineList.Adapter = adapter;
                ProgressIndicator.Hide();
            }

            catch (Exception exe)
            {
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
            var MYtastings = svc.GetMyTastingsList(customerid).Result;
            ListView wineList = FindViewById<ListView>(Resource.Id.MyTasting);
            // myArr1 = SampleData1();
            MyTastingAdapter adapter = new MyTastingAdapter(this, MYtastings.TastingList.ToList());


            wineList.Adapter = adapter;
            adapter.NotifyDataSetChanged();
        }
    }
}