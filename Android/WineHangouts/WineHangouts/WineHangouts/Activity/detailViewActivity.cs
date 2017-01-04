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

namespace WineHangouts
{
    [Activity(Label = "detailViewActivity", MainLauncher = false, Icon = "@drawable/logo")]
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
            setListViewHeightBasedOnChildren(detailView);
            setListViewHeightBasedOnChildren1(commentsView);
            TextView TopName = FindViewById<TextView>(Resource.Id.textView6); //Assigning to respected Textfield
            TextView TopBrand = FindViewById<TextView>(Resource.Id.textView7);
            TextView TopVintage = FindViewById<TextView>(Resource.Id.textView8);
            TextView WineDescription = FindViewById<TextView>(Resource.Id.textView36);
            TableRow tr5 = FindViewById<TableRow>(Resource.Id.tableRow5);
            RatingBar rb = FindViewById<RatingBar>(Resource.Id.rating);
            rb.RatingBarChange += Rb_RatingBarChange;
            //String x;
            //rb.RatingBarChange += (o, e) =>
            //{
            //    Toast.MakeText(this, "You have selected " + rb.Rating.ToString() + " Stars", ToastLength.Short).Show();
            //    x = rb.Rating.ToString();
            //};
          
            var metrics = Resources.DisplayMetrics;
            var widthInDp = ConvertPixelsToDp(metrics.WidthPixels);
            //var heightInDp = ConvertPixelsToDp(metrics.HeightPixels);
            //ImageView iv = new ImageView(this);
            //iv.LayoutParameters = new LinearLayout.LayoutParams(widthInDp, widthInDp);
            ImageButton ib = FindViewById<ImageButton>(Resource.Id.imageButton1);
            var pa = ib.LayoutParameters;
            pa.Height = PixelsToDp(widthInDp);
            pa.Width = PixelsToDp(widthInDp);
            ib.SetImageResource(Resource.Drawable.wine1);
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
            Bitmap result = BitmapFactory.DecodeResource(Resources, Resource.Drawable.placeholder_re, options);

            //placeholder.SetImageBitmap(result);
        }

        private void Rb_RatingBarChange(object sender, RatingBar.RatingBarChangeEventArgs e)
        {
            FragmentTransaction trans = FragmentManager.BeginTransaction();
            DialogReview dr = new DialogReview();
            dr.Show(trans, "Wine Review");
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

