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
using System.Net;
using Hangout.Models;
using Java.Util;

namespace WineHangouts
{
    [Activity(Label = "MyFavoriteAdapter")]
    public class MyFavoriteAdapter : BaseAdapter<Item>
    {
        private List<Item> myItems;
        private Context myContext;
        public override Item this[int position]
        {
            get
            {
                return myItems[position];
            }
        }

        public MyFavoriteAdapter(Context con, List<Item> strArr)
        {
            myContext = con;
            myItems = strArr;
        }
        public override int Count
        {
            get
            {
                return myItems.Count;
            }
        }


       
        public override long GetItemId(int position)
        {
            return position;
        }


        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            View row = convertView;
            if (row == null)
                row = LayoutInflater.From(myContext).Inflate(Resource.Layout.MyFavorite, null, false);
            else
                return row;

          
            TextView Name = row.FindViewById<TextView>(Resource.Id.txtNamefav);
            TextView Vintage = row.FindViewById<TextView>(Resource.Id.txtVintagefav);
            ImageView Wine = row.FindViewById<ImageView>(Resource.Id.imgWinefav);
            TextView Price = row.FindViewById<TextView>(Resource.Id.txtPricefav);
            RatingBar Avgrating = row.FindViewById<RatingBar>(Resource.Id.rtbProductRatingfav);
            ImageView place = row.FindViewById<ImageView>(Resource.Id.placeholdefavr);
            ImageView Heart = row.FindViewById<ImageView>(Resource.Id.imgHeartfav);


            //////TextView txtVintage1 = row.FindViewById<TextView>(Resource.Id.txtVintagefav);

            //////TextView txtPrice1 = row.FindViewById<TextView>(Resource.Id.txtPricefav);
            //////ImageView imgWine1 = row.FindViewById<ImageView>(Resource.Id.imgWinefav);
            //////RatingBar avgrating1 = row.FindViewById<RatingBar>(Resource.Id.rtbProductRatingfav);
            //////ImageView imgPlaceHolder1 = row.FindViewById<ImageView>(Resource.Id.placeholdefavr);
            //////ImageView heartImg1 = row.FindViewById<ImageView>(Resource.Id.imgHeartfav);
            //RelativeLayout rel = row.FindViewById<RelativeLayout>(Resource.Id.relative);
            //var place11 = new RelativeLayout.LayoutParams(520, 620)
            //rel.LayoutParameters = place11;
            //rel.LayoutParameters = new RelativeLayout.LayoutParams(520, 520);
            //txtName1.Text = myItems[position].RegPrice.ToString();\
            String str = "lokesh";
            Name.Text = myItems[position].Name;
            //txtRatings.Text = myItems[position].Ratings;
            //txtUserRatings.Text = myItems[position].UserRatings;
            Price.Text = myItems[position].RegPrice.ToString();
            Price.Text = "$ " + Price.Text;
            Avgrating.Rating = (float)myItems[position].AverageRating;
            Vintage.Text = myItems[position].Vintage.ToString();
            //heartImg.t = myItems[position].s;

            Heart.SetImageResource(Resource.Drawable.heart_empty);
            var heartLP = new RelativeLayout.LayoutParams(80, 80);

            var metrics = myContext.Resources.DisplayMetrics;
            var widthInDp = ConvertPixelsToDp(metrics.WidthPixels);
            var heightInDp = ConvertPixelsToDp(metrics.HeightPixels);
            heartLP.LeftMargin = parent.Resources.DisplayMetrics.WidthPixels / 2;
            Heart.LayoutParameters = heartLP;





            //heartImg.Layout(50, 50, 50, 50);
            bool count = Convert.ToBoolean(myItems[position].IsLike);
            if (count == true)
            {
                Heart.SetImageResource(Resource.Drawable.heart_full);
            }
            else
            {
                Heart.SetImageResource(Resource.Drawable.heart_empty);
            }

            Heart.Click += async delegate
            {
                bool x;
                if (count == false)
                {
                    Heart.SetImageResource(Resource.Drawable.heart_full);
                    x = true;
                    count = true;
                }
                else
                {
                    Heart.SetImageResource(Resource.Drawable.heart_empty);
                    x = false;
                    count = false;
                }
                SKULike like = new SKULike();
                like.UserID = Convert.ToInt32(CurrentUser.getUserId());
                like.SKU = Convert.ToInt32(myItems[position].SKU);
                like.Liked = x;
                ServiceWrapper sw = new ServiceWrapper();
                await sw.InsertUpdateLike(like);
            };


            place.SetImageResource(Resource.Drawable.placeholder);
            Wine.SetImageResource(Resource.Drawable.wine1);
            var place1 = new RelativeLayout.LayoutParams(heightInDp, heightInDp);
            // var place = new RelativeLayout.LayoutParams(520, 620);
            place1.LeftMargin = parent.Resources.DisplayMetrics.WidthPixels / 2 - 530;
            Wine.LayoutParameters = place1;

            var place2 = new RelativeLayout.LayoutParams(heightInDp, heightInDp);
            place2.LeftMargin = parent.Resources.DisplayMetrics.WidthPixels / 2 - 530;
            place.LayoutParameters = place2;
            //imgPlaceHolder.LayoutParameters = new RelativeLayout.LayoutParams(520, 520);
            //imgWine.LayoutParameters = new RelativeLayout.LayoutParams(520, 520);


            //  txtName1.Focusable = false;
            Name.Focusable = false;
            //txtRatings.Focusable = false;
            //txtUserRatings.Focusable = false;
            Vintage.Focusable = false;
            Price.Focusable = false;
            Wine.Focusable = false;
            
            NotifyDataSetChanged();
            return row;
        }

        private Bitmap GetImageBitmapFromUrl(string url)
        {
            Bitmap imageBitmap = null;

            using (var webClient = new WebClient())
            {
                var imageBytes = webClient.DownloadData(url);
                if (imageBytes != null && imageBytes.Length > 0)
                {
                    imageBitmap = BitmapFactory.DecodeByteArray(imageBytes, 0, imageBytes.Length);
                }
            }

            return imageBitmap;
        }

        private int ConvertPixelsToDp(float pixelValue)
        {
            var dp = (int)((pixelValue) / myContext.Resources.DisplayMetrics.Density);
            return dp;
        }
    }
}