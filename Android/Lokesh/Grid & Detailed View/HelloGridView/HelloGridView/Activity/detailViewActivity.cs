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

namespace HelloGridView
{
    [Activity(Label = "detailViewActivity", MainLauncher = false, Icon = "@drawable/icon")]
    public class detailViewActivity : Activity
    {
       
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.detailedView);
            //var listview = FindViewById<ListView>(Resource.Id.listView1);
                        
            string[] arr1 = new string[] { "Silver Oak Napa Valley", "Cabernet ", "2011", " This is the description about wine,This is the description about wine,This is the description about wine" };
            TextView TopName = FindViewById<TextView>(Resource.Id.textView6); //Assigning to respected Textfield
            TextView TopBrand = FindViewById<TextView>(Resource.Id.textView7);
            TextView TopVintage = FindViewById<TextView>(Resource.Id.textView8);
            TextView WineDescription = FindViewById<TextView>(Resource.Id.textView36);
            ImageView placeholder = FindViewById<ImageView>(Resource.Id.imageView2);
            TableRow tr5 = FindViewById<TableRow>(Resource.Id.tableRow5);

            RatingBar rb = FindViewById<RatingBar>(Resource.Id.rating);

            //placeholder.SetScaleType(ImageView.ScaleType.FitCenter);
            //placeholder.SetPadding(8, 8, 8, 8);
            var metrics = Resources.DisplayMetrics;
            var widthInDp = ConvertPixelsToDp(metrics.WidthPixels);
            //var heightInDp = ConvertPixelsToDp(metrics.HeightPixels);
            //placeholder.LayoutParameters = new TableRow.LayoutParams(heightInDp, widthInDp);
            //tr5.Layout(0, 0, 100,100 );
            //placeholder.Layout(0, 0, widthInDp, widthInDp);
            //placeholder.LayoutParameters.Width = widthInDp;
            //Java.Lang.ClassCastException: android.widget.TableLayout$LayoutParams cannot be cast to android.widget.TableRow$LayoutParams
            rb.NumStars = 5;

            placeholder.Visibility = ViewStates.Visible;


            TopName.Focusable = false;
            TopBrand.Focusable = false;
            TopVintage.Focusable = false;
            WineDescription.Focusable = false;
            placeholder.Focusable = false;

            TopName.Text = arr1[0]; //Assigning value
            TopBrand.Text = arr1[1];
            TopVintage.Text = arr1[2];
            WineDescription.Text = arr1[3];
 


            BitmapFactory.Options options = new BitmapFactory.Options
            {
                InJustDecodeBounds = false,
                OutHeight = 75,
                OutWidth = 75

            };

            // The result will be null because InJustDecodeBounds == true.
            Bitmap result =  BitmapFactory.DecodeResource(Resources, Resource.Drawable.placeholder, options);

            placeholder.SetImageBitmap(result);
        }
        private int ConvertPixelsToDp(float pixelValue)
        {
            var dp = (int)((pixelValue) / Resources.DisplayMetrics.Density);
            return dp;
        }

        
    }
    
}

