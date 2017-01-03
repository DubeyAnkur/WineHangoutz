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

namespace HelloGridView
{
    [Activity(Label = "TastingActivity")]
    public class TastingActivity : Activity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Tasting);
            List<Wine1> myArr1;
            myArr1 = SampleData1();

            ListView wineList = FindViewById<ListView>(Resource.Id.listView1);
            myArr1 = SampleData1();
            ListViewAdapter1 adapter = new ListViewAdapter1(this, myArr1);
            adapter.EditClick += (object sender, EventArgs e) =>
            {
                //Pull up Dialog
                FragmentTransaction trans = FragmentManager.BeginTransaction();
                DialogReview dr = new DialogReview();
                
                dr.Show(trans, "Wine Review");

             };
            adapter.Delete_Click += (object sender, EventArgs e) =>
            {
                //Pull up Dialog
                FragmentTransaction trans = FragmentManager.BeginTransaction();
                ReviewDeletePopup dr = new ReviewDeletePopup();
                dr.Show(trans, "Wine Review");

            };
            wineList.Adapter = adapter;
            

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
            w1.Date = "01/03/2017";

            Wine1 w2 = new Wine1();
            w2.Name = "Silver Oak Napa ";
            w2.UserRatings = "A soft lively fruitiness on the palate along with mellow";
            w2.Vintage = "2001";
            w2.Date = "01/03/2017";

            Wine1 w3 = new Wine1();
            w3.Name = "Silver Oak Napa ";
            w3.UserRatings = "A soft lively fruitiness on the palate along with mellow ";
            w3.Vintage = "2001";
            w3.Date = "01/03/2017";

            Wine1 w4 = new Wine1();
            w4.Name = "Silver Oak Napa ";
            w4.UserRatings = "A soft lively fruitiness on the palate along with mellow ";
            w4.Vintage = "2001";
            w4.Date = "01/03/2017";



            myArr1.Add(w1);
            myArr1.Add(w2);
            myArr1.Add(w3);
            myArr1.Add(w4);

            return myArr1;
        }
    }
}

