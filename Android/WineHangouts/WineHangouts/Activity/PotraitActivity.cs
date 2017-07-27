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
    [Activity(Label = "MyTastingActivity", MainLauncher = false, ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    public class PotraitActivity : Activity
    {
        public int uid;
        public string StoreName = "";
        protected override void OnCreate(Bundle bundle)
        {
            if (StoreName == "")
                StoreName = Intent.GetStringExtra("MyData");
            this.Title = StoreName;
			//LoggingClass.LogInfo("Entered into Potrait Activity", screenid);
            int StoreId = 1;
            //if (StoreName == "Wall Store")
            //    StoreId = 1;
            //else if (StoreName == "Point Pleasent Store")
            //    StoreId = 2;
            //else
            //    StoreId = 3;
            int userId = Convert.ToInt32(CurrentUser.getUserId());
            ServiceWrapper sw = new ServiceWrapper();
            var output = sw.GetItemList(StoreId, userId).Result;
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.Potrait);
            ActionBar.SetHomeButtonEnabled(true);
            ActionBar.SetDisplayHomeAsUpEnabled(true);
            List<Item> myArr;      
            myArr = output.ItemList.ToList();
            ListView wineList = FindViewById<ListView>(Resource.Id.listView1);
            PotraitAdapter adapter = new PotraitAdapter(this, myArr);
            wineList.Adapter = adapter;
            wineList.ItemClick += delegate (object sender, AdapterView.ItemClickEventArgs args)
            {
                var intent = new Intent(this, typeof(DetailViewActivity));
                StartActivity(intent);

            };
           
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            if (item.ItemId == Android.Resource.Id.Home)
            {
                Finish();
                StartActivity(typeof(TabActivity));
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

       }
}