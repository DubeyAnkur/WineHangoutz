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
using System.Net;

namespace WineHangouts
{
    [Activity(Label = "Wine Details", MainLauncher = false, Icon = "@drawable/icon")]
    public class detailViewActivity : Activity, IPopupParent
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
            int wineid = Intent.GetIntExtra("WineID", 123);
            ItemDetailsResponse myData = svc.GetItemDetails(wineid).Result;
            var SkuRating = svc.GetItemReviewsByWineID(wineid).Result;
            this.Title = "Wine Details";
            var commentsView = FindViewById<ListView>(Resource.Id.listView2);
            reviewAdapter comments = new reviewAdapter(this, SkuRating.Reviews.ToList());
            commentsView.Adapter = comments;

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
            AvgRating.Focusable = false;
            AvgRating.Rating = (float)myData.ItemDetails.AverageRating;
            TableRow tr5 = FindViewById<TableRow>(Resource.Id.tableRow5);

            Review edit = new Review();
            edit.WineId = wineid;
            ReviewPopup editPopup = new ReviewPopup(this, edit);
            RatingBar RatingInput = FindViewById<RatingBar>(Resource.Id.ratingInput);//Taking rating stars input
            RatingInput.RatingBarChange += editPopup.CreatePopup;

            var metrics = Resources.DisplayMetrics;
            var widthInDp = ConvertPixelsToDp(metrics.WidthPixels);
            var heightInDp = ConvertPixelsToDp(metrics.HeightPixels);

            ImageView imgWine = FindViewById<ImageView>(Resource.Id.imgWine12);
            //ImageView imgPlaceHolder = FindViewById<ImageView>(Resource.Id.placeholder1);
            //imgPlaceHolder.SetImageResource(Resource.Drawable.placeholder_11);
            BlobWrapper bvb = new BlobWrapper();
            Bitmap imageBitmap = bvb.Bottleimages(wineid);
            //ImageHelper im = new ImageHelper();
            //Bitmap imageBitmap = im.GetImageBitmapFromUrl("https://icsintegration.blob.core.windows.net/bottleimages/" + wineid + ".jpg");
            imgWine.SetImageBitmap(imageBitmap);

            //imgPlaceHolder.LayoutParameters = new RelativeLayout.LayoutParams(1100, 1100);
            imgWine.LayoutParameters = new RelativeLayout.LayoutParams(1100, 1100);




            BitmapFactory.Options options = new BitmapFactory.Options
            {
                InJustDecodeBounds = false,
                OutHeight = 75,
                OutWidth = 75

            };


            Bitmap result = BitmapFactory.DecodeResource(Resources, Resource.Drawable.placeholder_re, options);


        }

       
        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            if (item.ItemId == Android.Resource.Id.Home)
            {
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

        public void RefreshParent()
        {
            ServiceWrapper svc = new ServiceWrapper();
            int wineid = Intent.GetIntExtra("WineID", 138);


            ItemDetailsResponse myData = svc.GetItemDetails(wineid).Result;
            var SkuRating = svc.GetItemReviewsByWineID(wineid).Result;

            this.Title = "Details";



            var commentsView = FindViewById<ListView>(Resource.Id.listView2);

            reviewAdapter comments = new reviewAdapter(this, SkuRating.Reviews.ToList());
            commentsView.Adapter = comments;
            comments.NotifyDataSetChanged();
        }
    }

}

