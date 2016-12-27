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
using Android.Graphics;
using Android.Util;

namespace Tastings
{
    [Activity(Label = "Tastings", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);
            List<Wine1> myArr1;
            myArr1 = SampleData1();

            ListView wineList = FindViewById<ListView>(Resource.Id.listView1);
            myArr1 = SampleData1();
            ListViewAdapter adapter = new ListViewAdapter(this, myArr1);
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

