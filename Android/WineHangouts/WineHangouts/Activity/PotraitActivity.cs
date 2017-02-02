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
    [Activity(Label = "MyTastingActivity", MainLauncher = false)]
    public class PotraitActivity : Activity
    {
        public int uid;
        public string StoreName = "";
        protected override void OnCreate(Bundle bundle)
        {
            if (StoreName == "")
                StoreName = Intent.GetStringExtra("MyData");
            this.Title = StoreName;

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
                var intent = new Intent(this, typeof(detailViewActivity));
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

        public List<Wine1> SampleData1()
        {
            List<Wine1> myArr1 = new List<Wine1>();
            Wine1 w1 = new Wine1();
            w1.Name = "Silver Oak Napa ";
            w1.UserRatings = "A soft lively fruitiness on the palate along with mellow";
            w1.Vintage = "2001";
            w1.Date = "27/12/2016";

            Wine1 w2 = new Wine1();
            w2.Name = "Silver Oak Napa ";
            w2.UserRatings = "A soft lively fruitiness on the palate along with mellow";
            w2.Vintage = "2001";
            w2.Date = "27/12/2016";

            Wine1 w3 = new Wine1();
            w3.Name = "Silver Oak Napa ";
            w3.UserRatings = "A soft lively fruitiness on the palate along with mellow ";
            w3.Vintage = "2001";
            w3.Date = "27/12/2016";



            myArr1.Add(w1);
            myArr1.Add(w2);
            myArr1.Add(w3);

            return myArr1;
        }
    }
}