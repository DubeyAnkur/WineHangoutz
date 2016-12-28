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
        List<WineDetails> DetailsArray;
        List<Review> ReviewArray;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.detailedView);

            //Top detailed view
            string[] arr1 = new string[] { "Silver napa valley",
                                           "Cabernet ",
                                           "2011",
                                           " This is the description about wine,This is the description about wine,This is the description about wine" };
            var detailView = FindViewById<ListView>(Resource.Id.listView1);
            DetailsArray = DetailsData();
            DetailsViewAdapter Details = new DetailsViewAdapter(this, DetailsArray);
            detailView.Adapter = Details;

            var commentsView = FindViewById<ListView>(Resource.Id.listView2);
            ReviewArray = ReviewData();
            reviewAdapter comments = new reviewAdapter(this, ReviewArray);
            commentsView.Adapter = comments;


            DetailsArray = DetailsData();
            ReviewArray = ReviewData();
            TextView TopName = FindViewById<TextView>(Resource.Id.textView6); //Assigning to respected Textfield
            TextView TopBrand = FindViewById<TextView>(Resource.Id.textView7);
            TextView TopVintage = FindViewById<TextView>(Resource.Id.textView8);
            TextView WineDescription = FindViewById<TextView>(Resource.Id.textView36);
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
        private int ConvertPixelsToDp(float pixelValue)
        {
            var dp = (int)((pixelValue) / Resources.DisplayMetrics.Density);
            return dp;
        }

        private int PixelsToDp(int pixels)
        {
            return (int)TypedValue.ApplyDimension(ComplexUnitType.Dip, pixels, Resources.DisplayMetrics);
        }
        public List<WineDetails> DetailsData()
        {
            List<WineDetails> DetailsArray = new List<WineDetails>();
            WineDetails w1 = new WineDetails();
            w1.Name = "Name";
            w1.NameValue = "Napa Valley";
            w1.Classification = "Classification";
            w1.ClassificationValue = "Extra Extra ";
            w1.Grapetype = "Grapetype";
            w1.GrapeTypeValue = "Erbaluce";
            w1.Alcohol = "Alcohol";
            w1.AlcoholValue = "70%";
            w1.Vintage = "Vintage";
            w1.VintageValue = "2011";
            w1.Aromas = "Aromas";
            w1.AromasValue = "Floral";
            w1.FoodPairings = "FoodPairings";
            w1.FoodPairingsValue = "fish";
            w1.Bottlesize = "Bottlesize";
            w1.BottleSizeValue = "750ml";
            w1.ServingAt = "ServingAt";
            w1.ServingAtValue = "10C";

            DetailsArray.Add(w1);
            return DetailsArray;
        }
        public List<Review> ReviewData()
        {
            List<Review> ReviewArray = new List<Review>();
            Review w1 = new Review();
            w1.Name = "Person X";
            w1.Comments = "This wine is taste good as like grape juice.";
            w1.imageURL = "http://cdn.fluidretail.net/customers/c1477/13/97/48/_s/pi/n/139748_spin_spin2/main_variation_na_view_01_204x400.jpg";

            Review w2 = new Review();
            w2.Name = "Person X";
            w2.Comments = "This wine is taste good as like grape juice.";
            w2.imageURL = "http://cdn.fluidretail.net/customers/c1477/13/97/48/_s/pi/n/139748_spin_spin2/main_variation_na_view_01_204x400.jpg";


            Review w3 = new Review();
            w3.Name = "Person X";
            w3.Comments = "This wine is taste good as like grape juice.";
            w3.imageURL = "http://www.savvyitsol.com/placeholder.jpeg";

            Review w4 = new Review();
            w4.Name = "Person X";
            w4.Comments = "This wine is taste good as like grape juice.";
            w4.imageURL = "http://www.savvyitsol.com/placeholder.jpeg";



            ReviewArray.Add(w1);
            ReviewArray.Add(w2);
            ReviewArray.Add(w3);
            ReviewArray.Add(w4);
            return ReviewArray;
        }
    }

}

