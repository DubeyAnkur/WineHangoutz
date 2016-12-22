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

namespace HelloGridView
{
    [Activity(Label = "detailViewActivity", MainLauncher = false, Icon = "@drawable/icon")]
    public class detailViewActivity : Activity
    {
        //List<Wine> myArr;


        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.detailedView);

            //Top detailed view
            string[] arr1 = new string[] { "Silver napa valley",
                                           "Cabernet ",
                                           "2011",
                                           " This is the description about wine,This is the description about wine,This is the description about wine" };
            TextView TopName = FindViewById<TextView>(Resource.Id.textView6); //Assigning to respected Textfield
            TextView TopBrand = FindViewById<TextView>(Resource.Id.textView7);
            TextView TopVintage = FindViewById<TextView>(Resource.Id.textView8);
            TextView WineDescription = FindViewById<TextView>(Resource.Id.textView36);
            TextView ListName = FindViewById<TextView>(Resource.Id.textView47);
            TextView Classification = FindViewById<TextView>(Resource.Id.textView49);
            TextView GrapeType = FindViewById<TextView>(Resource.Id.textView51);
            TextView Alcohol = FindViewById<TextView>(Resource.Id.textView53);
            TextView VintageYear = FindViewById<TextView>(Resource.Id.textView55);
            TextView Aramos = FindViewById<TextView>(Resource.Id.textView57);
            TextView FoodPairings = FindViewById<TextView>(Resource.Id.textView59);
            TextView BottleSize = FindViewById<TextView>(Resource.Id.textView61);
            TextView Serving = FindViewById<TextView>(Resource.Id.textView63);
            TableRow tr5 = FindViewById<TableRow>(Resource.Id.tableRow5);
            RatingBar rb = FindViewById<RatingBar>(Resource.Id.rating);
            String x;
            rb.RatingBarChange += (o, e) =>
            {
                Toast.MakeText(this, "You have selected " + rb.Rating.ToString() + " Stars", ToastLength.Short).Show();
                x = rb.Rating.ToString();
            };
          
            var metrics = Resources.DisplayMetrics;
            var widthInDp = ConvertPixelsToDp(metrics.WidthPixels);
            var heightInDp = ConvertPixelsToDp(metrics.HeightPixels);
            //ImageView iv = new ImageView(this);
            //iv.LayoutParameters = new LinearLayout.LayoutParams(widthInDp, widthInDp);
            ImageButton ib = FindViewById<ImageButton>(Resource.Id.imageButton1);
            var pa = ib.LayoutParameters;
            pa.Height = PixelsToDp(widthInDp);
            pa.Width = PixelsToDp(widthInDp);
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
            ListName.Focusable = false;
            Classification.Focusable = false;
            GrapeType.Focusable = false;
            Alcohol.Focusable = false;
            VintageYear.Focusable = false;
            Aramos.Focusable = false;
            FoodPairings.Focusable = false;
            BottleSize.Focusable = false;
            Serving.Focusable = false;

            //placeholder.Focusable = false;
            TopName.Text = arr1[0]; //Assigning value
            TopBrand.Text = arr1[1];
            TopVintage.Text = arr1[2];
            WineDescription.Text = arr1[3];
            ListName.Text = arr1[0];
            Classification.Text = arr1[1];
            GrapeType.Text = arr1[1];
            Alcohol.Text = arr1[1];
            VintageYear.Text = arr1[1];
            Aramos.Text = arr1[1];
            FoodPairings.Text = arr1[1];
            Serving.Text = arr1[1];
            BottleSize.Text = arr1[1];

            BitmapFactory.Options options = new BitmapFactory.Options
            {
                InJustDecodeBounds = false,
                OutHeight = 75,
                OutWidth = 75

            };

            // The result will be null because InJustDecodeBounds == true.
            Bitmap result = BitmapFactory.DecodeResource(Resources, Resource.Drawable.placeholder_11, options);

            //placeholder.SetImageBitmap(result);
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

