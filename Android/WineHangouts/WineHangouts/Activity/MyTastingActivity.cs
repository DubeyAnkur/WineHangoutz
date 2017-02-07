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
using System.Linq;
namespace WineHangouts
{
    [Activity(Label = "MyTastingActivity")]
    public class MyTastingActivity : Activity
    {
        public int customerid;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
           int customerid = Convert.ToInt32(CurrentUser.getUserId());
            //   uid = 4;//checking purpose
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.MyTasting);
            ActionBar.SetHomeButtonEnabled(true);
            ActionBar.SetDisplayHomeAsUpEnabled(true);
         //   customerid = 38691;
                        ServiceWrapper svc = new ServiceWrapper();
            // ItemRatingResponse irr = svc.GetItemReviewUID(uid).Result;
            var MYtastings = svc.GetMyTastingsList(customerid).Result;

            List<Item> myArr;
          
            //  myArr1 = SampleData1();

            ListView wineList = FindViewById<ListView>(Resource.Id.MyTasting);
            // myArr1 = SampleData1();
            ReviewPopup editPopup = new ReviewPopup(this);
            MyTastingAdapter adapter = new MyTastingAdapter(this, MYtastings.ItemList.ToList());
            adapter.Edit_Click += editPopup.EditPopup;

            adapter.Delete_Click += (object sender, EventArgs e) =>
            {
                //Pull up Dialog
                FragmentTransaction trans = FragmentManager.BeginTransaction();
                DeleteReview dr = new DeleteReview();
                dr.Show(trans, "Wine Review");
                //Dialog DeleteDialog = new Dialog(this);
                //DeleteDialog.SetContentView(Resource.Layout.DeleteReviewPop);
                //DeleteDialog.Window.RequestFeature(WindowFeatures.NoTitle);
                //DeleteDialog.Show();
            };
            wineList.Adapter = adapter;


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