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
using Hangout.Models;
using System.Linq;

namespace WineHangouts
{
    [Activity(Label = "detailViewActivity", MainLauncher = false, Icon = "@drawable/icon" )]
    public class detailViewActivity : Activity
    {
        public int sku;
        List<ItemDetails> DetailsArray;
        List<Review> ReviewArray;
        
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.detailedView);
            ActionBar.SetHomeButtonEnabled(true);
            ActionBar.SetDisplayHomeAsUpEnabled(true);
            ServiceWrapper svc = new ServiceWrapper();
            int wineid =Intent.GetIntExtra("WineID",138);

           // wineid = 138;
            ItemDetailsResponse myData = svc.GetItemDetails(wineid).Result;
            var SkuRating = svc.GetItemReviewsByWineID(wineid).Result;
            
            this.Title = "Details";
           
            //Top detailed view
            string[] arr1 = new string[] { "Silver napa valley",
                                           "Cabernet ",
                                           "2011",
                                          " This is the description about wine,This is the description about wine,This is the description about wine" };
            //var detailView = FindViewById<ListView>(Resource.Id.listView1);
            //DetailsArray = DetailsData();
            //DetailsViewAdapter Details = new DetailsViewAdapter(this, DetailsArray);
            //detailView.Adapter = Details;

            var commentsView = FindViewById<ListView>(Resource.Id.listView2);
            //ReviewArray = ReviewData();
            reviewAdapter comments = new reviewAdapter(this, SkuRating.Reviews.ToList());
            commentsView.Adapter = comments;


            //DetailsArray = myData.ItemDetails.ToList();
            //ReviewArray = SkuRating.Ratings.ToList();
            //setListViewHeightBasedOnChildren(detailView);
            setListViewHeightBasedOnChildren1(commentsView);
            TextView WineName = FindViewById<TextView>(Resource.Id.txtWineName); //Assigning values to respected Textfields
            WineName.Focusable = false;
            WineName.Text = myData.ItemDetails.Name;
            
            TextView Vintage = FindViewById<TextView>(Resource.Id.txtVintage);
            Vintage.Focusable = false;
            Vintage.Text = myData.ItemDetails.Vintage.ToString();

            TextView WineProducer = FindViewById<TextView>(Resource.Id.txtProducer);
            WineProducer.Focusable = false;
            WineProducer.Text = myData.ItemDetails.Producer;

            TextView WineDescription = FindViewById<TextView>(Resource.Id.txtWineDescription);
            WineDescription.Focusable = false;
            WineDescription.Text = myData.ItemDetails.Description;

            
            
            RatingBar AvgRating = FindViewById<RatingBar>(Resource.Id.avgrating);
            AvgRating.Focusable =false;
            AvgRating.Rating = (float)myData.ItemDetails.AverageRating;
            TableRow tr5 = FindViewById<TableRow>(Resource.Id.tableRow5);
            ReviewPopup editPopup = new ReviewPopup(this);
            editPopup.SKU = sku;
            editPopup.WineId = wineid;
            RatingBar RatingInput = FindViewById<RatingBar>(Resource.Id.ratingInput);//Taking rating stars input
            RatingInput.RatingBarChange += editPopup.CreatePopup;
                      
            var metrics = Resources.DisplayMetrics;
            var widthInDp = ConvertPixelsToDp(metrics.WidthPixels);
            var heightInDp = ConvertPixelsToDp(metrics.HeightPixels);
           
            ImageView imgWine = FindViewById<ImageView>(Resource.Id.imgWine12);
            ImageView imgPlaceHolder =FindViewById<ImageView>(Resource.Id.placeholder1);
            imgPlaceHolder.SetImageResource(Resource.Drawable.placeholder_11);
            imgWine.SetImageResource(Resource.Drawable.finca1);
            imgPlaceHolder.LayoutParameters = new RelativeLayout.LayoutParams(1100, 1100);
            imgWine.LayoutParameters = new RelativeLayout.LayoutParams(1100, 1100);


                      

            BitmapFactory.Options options = new BitmapFactory.Options
            {
                InJustDecodeBounds = false,
                OutHeight = 75,
                OutWidth = 75

            };

            // The result will be null because InJustDecodeBounds == true.
            Bitmap result = BitmapFactory.DecodeResource(Resources, Resource.Drawable.placeholder_re, options);

            //placeholder.SetImageBitmap(result);
        }

         public override bool OnOptionsItemSelected(IMenuItem item)
        {
            if (item.ItemId == Android.Resource.Id.Home)
            {
                //    Finish();
                //    StartActivity(typeof(MainActivity));
                base.OnBackPressed();
                return false;
            }
            return base.OnOptionsItemSelected(item);
        }

        private int ConvertPixelsToDp(float pixelValue)
        {
            var dp = (int)((pixelValue) / Resources.DisplayMetrics.Density);
            return dp;
        }
        public void setListViewHeightBasedOnChildren(ListView listView)
        {
            DetailsViewAdapter listAdapter = (DetailsViewAdapter)listView.Adapter;
            if (listAdapter == null)
                return;

            int desiredWidth = View.MeasureSpec.MakeMeasureSpec(listView.Width, MeasureSpecMode.Unspecified);
            int heightMeasureSpec = View.MeasureSpec.MakeMeasureSpec(ViewGroup.LayoutParams.WrapContent, MeasureSpecMode.Exactly);
            int totalHeight = 0;
            View view = null;
            for (int i = 0; i < listAdapter.Count; i++)
            {
                view = listAdapter.GetView(i, view, listView);
                if (i == 0)
                    view.LayoutParameters = new ViewGroup.LayoutParams(desiredWidth, WindowManagerLayoutParams.WrapContent);

                view.Measure(desiredWidth, heightMeasureSpec);
                totalHeight += view.MeasuredHeight;
            }
            ViewGroup.LayoutParams params1 = listView.LayoutParameters;
            params1.Height = totalHeight + (listView.DividerHeight * (listAdapter.Count - 1));
            listView.LayoutParameters = params1;
        }
        public void setListViewHeightBasedOnChildren1(ListView listView1)
        {
            reviewAdapter listAdapter = (reviewAdapter)listView1.Adapter;
            if (listAdapter == null)
                return;

            int desiredWidth = View.MeasureSpec.MakeMeasureSpec(listView1.Width, MeasureSpecMode.Unspecified);
            int heightMeasureSpec = View.MeasureSpec.MakeMeasureSpec(ViewGroup.LayoutParams.WrapContent, MeasureSpecMode.Exactly);
            int totalHeight = 0;
            View view = null;
            for (int i = 0; i < listAdapter.Count; i++)
            {
                view = listAdapter.GetView(i, view, listView1);
                if (i == 0)
                    view.LayoutParameters = new ViewGroup.LayoutParams(desiredWidth, WindowManagerLayoutParams.WrapContent);

                view.Measure(desiredWidth, heightMeasureSpec);
                totalHeight += view.MeasuredHeight;
            }
            ViewGroup.LayoutParams params1 = listView1.LayoutParameters;
            params1.Height = totalHeight + (listView1.DividerHeight * (listAdapter.Count - 1));
            listView1.LayoutParameters = params1;
        }

        public int PixelsToDp(int pixels)
        {
            return (int)TypedValue.ApplyDimension(ComplexUnitType.Dip, pixels, Resources.DisplayMetrics);
        }
        public List<WineDetails> DetailsData()
        {
            List<WineDetails> DetailsArray = new List<WineDetails>();
            WineDetails w1 = new WineDetails();
            w1.Type = "Name";
            w1.Value = "Napa";
            WineDetails w3 = new WineDetails();
            w3.Type = "Grapetype";
            w3.Value = "Erbaluce";
            //w1.Type = "Grapetype";
            //w1.Value = "Erbaluce";
            //w1.Type = "Alcohol";
            //w1.Value = "70%";
            //w1.Type = "Vintage";
            //w1.Value = "2011";
            //w1.Type = "Aromas";
            //w1.Value = "Floral";
            //w1.Type = "FoodPairings";
            //w1.Value = "fish";
            //w1.Type = "Bottlesize";
            //w1.Value = "750ml";
            //w1.Type = "ServingAt";
            //w1.Value = "10C";

            WineDetails w2 = new WineDetails();
             w2.Type = "Alcohol";
            w2.Value = "Extra";
            WineDetails w4 = new WineDetails();
            w4.Type = "Vintage";
            w4.Value = "2011";
            WineDetails w5 = new WineDetails();
            w5.Type = "Classification";
            w5.Value = "Extra";
            WineDetails w6 = new WineDetails();
            w6.Type = "Classification";
            w6.Value = "Extra";
            WineDetails w7 = new WineDetails();
            w7.Type = "Classification";
            w7.Value = "Extra";
            WineDetails w8 = new WineDetails();
            w8.Type = "Classification";
            w8.Value = "Extra";


            DetailsArray.Add(w1);
            DetailsArray.Add(w2);
            DetailsArray.Add(w3);
            DetailsArray.Add(w4);
            DetailsArray.Add(w5);
            DetailsArray.Add(w6);
            DetailsArray.Add(w7);
            DetailsArray.Add(w8);
            return DetailsArray;
        }
     
    }
    
}

