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
using Android.Graphics;

namespace HelloGridView
{
    [Activity(Label = "detailViewActivity", MainLauncher = false, Icon = "@drawable/icon")]
    public class detailViewActivity : Activity
    {
        List<WineDetails> myArr1;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.detailedView);


            //ListView wineList = FindViewById<ListView>(Resource.Id.listView1);
            //myArr1 = SampleData1();

            //ListViewAdapter adapter = new ListViewAdapter(this, myArr1);
            //wineList.Adapter = adapter;





            string[] arr1 = new string[] { "Silver napa valley", "Cabernet ", "2011", " This is the description about wine,This is the description about wine,This is the description about wine" };
            TextView TopName = FindViewById<TextView>(Resource.Id.textView6); //Assigning to respected Textfield
            TextView TopBrand = FindViewById<TextView>(Resource.Id.textView7);
            TextView TopVintage = FindViewById<TextView>(Resource.Id.textView8);
            TextView WineDescription = FindViewById<TextView>(Resource.Id.textView36);
            //ImageView placeholder = FindViewById<ImageView>(Resource.Id.imageView2);
            TableRow tr5 = FindViewById<TableRow>(Resource.Id.tableRow5);
            RatingBar Rating = FindViewById<RatingBar>(Resource.Id.rating);
            Rating.RatingBarChange += Rating_RatingBarChange;

            //placeholder.SetScaleType(ImageView.ScaleType.FitCenter);
            //placeholder.SetPadding(8, 8, 8, 8);
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
            //placeholder.Focusable = false;

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
            Bitmap result = BitmapFactory.DecodeResource(Resources, Resource.Drawable.placeholder_11, options);

            //placeholder.SetImageBitmap(result);
        }

        private void Rating_RatingBarChange(object sender, RatingBar.RatingBarChangeEventArgs e)
        {
            FragmentTransaction trans = FragmentManager.BeginTransaction();
            DialogReview dr = new DialogReview();
            dr.Show(trans, "Wine Review");
        }

        public class WineDetails
        {

            public string Name { get; set; }
            public string Vintage { get; set; }
            public string Price { get; set; }
            public string UserRatings { get; set; }

            public string Ratings { get; set; }
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
        public List<WineDetails> SampleData1()
        {
            List<WineDetails> myArr1 = new List<WineDetails>();
            WineDetails w1 = new WineDetails();
            w1.Name = "Silver ";
            w1.Ratings = "Ratings";
            w1.UserRatings = "User Ratings";
            w1.Price = "Prise$";
            w1.Vintage = "2001";
            // w1.imageURL = "http://cdn.fluidretail.net/customers/c1477/13/97/48/_s/pi/n/139748_spin_spin2/main_variation_na_view_01_204x400.jpg";

            WineDetails w2 = new WineDetails();
            w2.Name = "Bodega Norton Reserve Malbec 2013";
            w2.Price = "$19.99";
            w2.Ratings = "15";
            w2.UserRatings = "12";
            w2.Vintage = "2001";
            // w2.imageURL = "http://cdn.fluidretail.net/customers/c1477/13/97/48/_s/pi/n/139748_spin_spin2/main_variation_na_view_01_204x400.jpg";


            WineDetails w3 = new WineDetails();
            w3.Name = "Bodega Norton Reserve Malbec 2013";
            w3.Ratings = "15";
            w3.UserRatings = "12";
            w3.Price = "$19.99";
            w3.Vintage = "2001";
            // w3.imageURL = "http://www.savvyitsol.com/placeholder.jpeg";

            WineDetails w4 = new WineDetails();
            w4.Name = "Bodega Norton Reserve Malbec 2013";
            w4.Price = "$19.99";
            w4.Ratings = "15";
            w4.UserRatings = "12";
            w4.Vintage = "2001";
            // w4.imageURL = "http://www.savvyitsol.com/placeholder.jpeg";

            //Wine w5 = new Wine();
            //w5.Name = "Silver Oak Napa Valley Cabernet Sauvignon 2011";
            //w5.Price = "$15.99";
            //w5.Ratings = "15";
            //w5.UserRatings = "12";
            //w5.Vintage = "2001";
            //w5.imageURL = "http://www.savvyitsol.com/placeholder.jpeg";


            //Wine w6 = new Wine();
            //w6.Name = "Bodega Norton Reserve Malbec 2013";
            //w6.Price = "$19.99";
            //w6.Ratings = "15";
            //w6.UserRatings = "12";
            //w6.Vintage = "2001";
            //w6.imageURL = "http://www.savvyitsol.com/placeholder.jpeg";


            //Wine w7 = new Wine();
            //w7.Name = "Bodega Norton Reserve Malbec 2013";
            //w7.Price = "$19.99";
            //w7.Ratings = "15";
            //w7.UserRatings = "12";
            //w7.Vintage = "2001";
            //w7.imageURL = "http://www.savvyitsol.com/placeholder.jpeg";

            //Wine w8 = new Wine();
            //w8.Name = "Bodega Norton Reserve Malbec 2013";
            //w8.Price = "$19.99";
            //w8.Ratings = "15";
            //w8.UserRatings = "12";
            //w8.Vintage = "2001";
            //w8.imageURL = "http://www.savvyitsol.com/placeholder.jpeg";

            myArr1.Add(w1);
            myArr1.Add(w2);
            myArr1.Add(w3);
            myArr1.Add(w4);
            //myArr.Add(w5);
            //myArr.Add(w6);
            //myArr.Add(w7);
            //myArr.Add(w8);
            return myArr1;
        }
    }
}