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
    [Activity(Label = "detailViewActivity", MainLauncher = false, Icon = "@drawable/icon")]
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
            sku = 5757;
            ItemDetailsResponse myData = svc.GetItemDetails(sku).Result;
            var SkuRating = svc.GetItemReviewSKU(sku).Result;
            
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
            TextView TopName = FindViewById<TextView>(Resource.Id.textView6); //Assigning to respected Textfield
            TextView TopBrand = FindViewById<TextView>(Resource.Id.textView7);
            TextView TopVintage = FindViewById<TextView>(Resource.Id.textView8);
            TextView WineDescription = FindViewById<TextView>(Resource.Id.textView36);
            TableRow tr5 = FindViewById<TableRow>(Resource.Id.tableRow5);
            RatingBar rb = FindViewById<RatingBar>(Resource.Id.ratingBar1);
            ReviewPopup editPopup = new ReviewPopup(this);
            editPopup.SKU = sku;
            rb.RatingBarChange += editPopup.CreatePopup;
            //String x;
            //rb.RatingBarChange += (o, e) =>
            //{
            //    Toast.MakeText(this, "You have selected " + rb.Rating.ToString() + " Stars", ToastLength.Short).Show();
            //    x = rb.Rating.ToString();
            //};
          
            var metrics = Resources.DisplayMetrics;
            var widthInDp = ConvertPixelsToDp(metrics.WidthPixels);
            var heightInDp = ConvertPixelsToDp(metrics.HeightPixels);
            //ImageView iv = new ImageView(this);
            //iv.LayoutParameters = new LinearLayout.LayoutParams(widthInDp, widthInDp);



            ////ImageButton ib = FindViewById<ImageButton>(Resource.Id.imageButton1);
            ////var pa = ib.LayoutParameters;
            ////pa.Height = PixelsToDp(widthInDp);
            ////pa.Width = PixelsToDp(widthInDp);
            ////ib.SetImageResource(Resource.Drawable.wine1);

            ImageView imgWine = FindViewById<ImageView>(Resource.Id.imgWine12);
            ImageView imgPlaceHolder =FindViewById<ImageView>(Resource.Id.placeholder1);
            imgPlaceHolder.SetImageResource(Resource.Drawable.placeholder_11);
            imgWine.SetImageResource(Resource.Drawable.wine1);
            imgPlaceHolder.LayoutParameters = new RelativeLayout.LayoutParams(1100, 1100);
            imgWine.LayoutParameters = new RelativeLayout.LayoutParams(1100, 1100);


            //placeholder.LayoutParameters = new TableRow.LayoutParams(heightInDp, widthInDp);
            //tr5.Layout(0, 0, 100,100 );
            //placeholder.Layout(0, 0, widthInDp, widthInDp);
            //placeholder.LayoutParameters.Width = widthInDp;
            //Java.Lang.ClassCastException: android.widget.TableLayout$LayoutParams cannot be cast to android.widget.TableRow$LayoutParams
            //rb.NumStars = 5;
            //placeholder.Visibility = ViewStates.Visible;
            // iv.SetImageResource(Resource.Drawable.placeholder_bottiglia_lista);
            //tr5.AddView(iv);
            TopName.Focusable = false;
            TopBrand.Focusable = false;
            TopVintage.Focusable = false;
            WineDescription.Focusable = false;
            

            //placeholder.Focusable = false;
            TopName.Text = myData.ItemDetails.Name; //Assigning value
            TopBrand.Text = arr1[1];
            TopVintage.Text = myData.ItemDetails.Vintage.ToString();
            WineDescription.Text = myData.ItemDetails.Description;
           

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

        //private void Rb_RatingBarChange(object sender, RatingBar.RatingBarChangeEventArgs e)
        //{
        //    Dialog editDialog = new Dialog(this);

        //    //editDialog.Window.RequestFeature(WindowFeatures.NoTitle);
        //    //editDialog.Window.SetBackgroundDrawable(new Android.Graphics.Drawables.ColorDrawable(Android.Graphics.Color.White));// (Android.Graphics.Color.Transparent));
        //    editDialog.SetContentView(Resource.Layout.EditReviewPopup);
        //    //editDialog.SetTitle();

        //    ImageButton ibs = editDialog.FindViewById<ImageButton>(Resource.Id.imageButton1);
        //    ImageButton close = editDialog.FindViewById<ImageButton>(Resource.Id.imageButton2);
        //    TextView blanktxt = editDialog.FindViewById<TextView>(Resource.Id.textView2);
        //    ibs.SetImageResource(Resource.Drawable.wine_review);
        //    ibs.SetScaleType(ImageView.ScaleType.CenterCrop);

        //    close.SetImageResource(Resource.Drawable.Close);
        //    close.SetScaleType(ImageView.ScaleType.CenterCrop);

        //    editDialog.Window.SetBackgroundDrawable(new Android.Graphics.Drawables.ColorDrawable(Android.Graphics.Color.Transparent));
        //    editDialog.Show();


        //    close.Click += delegate
        //    {
        //        editDialog.Dismiss();
        //    };

        //}
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

