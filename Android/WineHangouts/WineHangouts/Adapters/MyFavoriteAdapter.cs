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
            View gridView;
            if (convertView == null)
            {
                gridView = new View(myContext);
            gridView = LayoutInflater.From(myContext).Inflate(Resource.Layout.MyFavorite, null, false);
                
            }
            else {
                gridView = (View)convertView;
            }

            TextView txtName = convertView.FindViewById<TextView>(Resource.Id.txtNamefav);
            //TextView txtRatings = row.FindViewById<TextView>(Resource.Id.txtRatings);
            TextView txtVintage = convertView.FindViewById<TextView>(Resource.Id.txtVintagefav);
            //TextView txtUserRatings = row.FindViewById<TextView>(Resource.Id.txtUserRatings);
            TextView txtPrice = convertView.FindViewById<TextView>(Resource.Id.txtPricefav);
            ImageView imgWine = convertView.FindViewById<ImageView>(Resource.Id.imgWinefav);
            RatingBar avgrating = convertView.FindViewById<RatingBar>(Resource.Id.rtbProductRatingfav);
            ImageView imgPlaceHolder = convertView.FindViewById<ImageView>(Resource.Id.placeholdefavr);
            ImageView heartImg = convertView.FindViewById<ImageView>(Resource.Id.imgHeartfav);
            //RelativeLayout rel = row.FindViewById<RelativeLayout>(Resource.Id.relative);
            //var place11 = new RelativeLayout.LayoutParams(520, 620)
            //rel.LayoutParameters = place11;
            //rel.LayoutParameters = new RelativeLayout.LayoutParams(520, 520);
            txtName.Text = myItems[position].Name;
            //txtRatings.Text = myItems[position].Ratings;
            //txtUserRatings.Text = myItems[position].UserRatings;
            txtPrice.Text = myItems[position].RegPrice.ToString();
            txtPrice.Text = "$ " + txtPrice.Text;
            avgrating.Rating = (float)myItems[position].AverageRating;
            txtVintage.Text = myItems[position].Vintage.ToString();
            //heartImg.t = myItems[position].s;

            heartImg.SetImageResource(Resource.Drawable.heart_empty);
            var heartLP = new RelativeLayout.LayoutParams(80, 80);

            var metrics = myContext.Resources.DisplayMetrics;
            var widthInDp = ConvertPixelsToDp(metrics.WidthPixels);
            var heightInDp = ConvertPixelsToDp(metrics.HeightPixels);
            heartLP.LeftMargin = parent.Resources.DisplayMetrics.WidthPixels / 2;
            heartImg.LayoutParameters = heartLP;





            //heartImg.Layout(50, 50, 50, 50);
            bool count = Convert.ToBoolean(myItems[position].IsLike);
            if (count == true)
            {
                heartImg.SetImageResource(Resource.Drawable.heart_full);
            }
            else
            {
                heartImg.SetImageResource(Resource.Drawable.heart_empty);
            }

            heartImg.Click += async delegate
            {
                bool x;
                if (count == false)
                {
                    heartImg.SetImageResource(Resource.Drawable.heart_full);
                    x = true;
                    count = true;
                }
                else
                {
                    heartImg.SetImageResource(Resource.Drawable.heart_empty);
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


            imgPlaceHolder.SetImageResource(Resource.Drawable.placeholder);
            imgWine.SetImageResource(Resource.Drawable.wine1);
            var place = new RelativeLayout.LayoutParams(heightInDp, heightInDp);
            // var place = new RelativeLayout.LayoutParams(520, 620);
            place.LeftMargin = parent.Resources.DisplayMetrics.WidthPixels / 2 - 530;
            imgWine.LayoutParameters = place;

            var place1 = new RelativeLayout.LayoutParams(heightInDp, heightInDp);
            place1.LeftMargin = parent.Resources.DisplayMetrics.WidthPixels / 2 - 530;
            imgPlaceHolder.LayoutParameters = place1;
            //imgPlaceHolder.LayoutParameters = new RelativeLayout.LayoutParams(520, 520);
            //imgWine.LayoutParameters = new RelativeLayout.LayoutParams(520, 520);


            txtName.Focusable = false;
            //txtRatings.Focusable = false;
            //txtUserRatings.Focusable = false;
            txtVintage.Focusable = false;
            txtPrice.Focusable = false;
            imgWine.Focusable = false;
            
            NotifyDataSetChanged();
            return gridView;
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