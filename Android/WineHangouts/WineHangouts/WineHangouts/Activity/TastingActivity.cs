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

namespace WineHangouts
{
    [Activity(Label = "TastingActivity",  Icon = "@drawable/logo")]
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
            adapter.Edit_Click += (object sender, EventArgs e) =>
            {
                //Pull up Dialog
                //FragmentTransaction trans = FragmentManager.BeginTransaction();
                //EditReview dr = new EditReview();
                //dr.Show(trans, "Wine Review");
                Dialog editDialog = new Dialog(this);
                //editDialog.Window.RequestFeature(WindowFeatures.NoTitle);
                //editDialog.Window.SetBackgroundDrawable(new Android.Graphics.Drawables.ColorDrawable(Android.Graphics.Color.White));// (Android.Graphics.Color.Transparent));
                editDialog.SetContentView(Resource.Layout.EditReviewPopup);
                //editDialog.SetTitle();
                //ImageButton ibs = editDialog.FindViewById<ImageButton>(Resource.Id.imageButton1);
                ImageButton close = editDialog.FindViewById<ImageButton>(Resource.Id.imageButton2);
                //ibs.SetImageResource(Resource.Drawable.wine_review);
                //ibs.SetScaleType(ImageView.ScaleType.CenterCrop);
                close.SetImageResource(Resource.Drawable.Close);
                close.SetScaleType(ImageView.ScaleType.CenterCrop);
                editDialog.Show();
                //
            };
            adapter.Delete_Click += (object sender, EventArgs e) =>
            {
                //Pull up Dialog
                FragmentTransaction trans = FragmentManager.BeginTransaction();
                DeleteReview dr = new DeleteReview();
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

