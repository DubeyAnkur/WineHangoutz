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
            // ImageView place = row.FindViewById<ImageView>(Resource.Id.placeholdefavr);
            ImageView Heart = row.FindViewById<ImageView>(Resource.Id.imgHeartfav);

            //String str = "lokesh";
            Name.Text = myItems[position].Name;
            Name.InputType = Android.Text.InputTypes.TextFlagNoSuggestions;

            Price.Text = myItems[position].SalePrice.ToString("C", GridViewAdapter.Cultures.UnitedState);

            Avgrating.Rating = (float)myItems[position].AverageRating;
            Vintage.Text = myItems[position].Vintage.ToString();


            Heart.SetImageResource(Resource.Drawable.heart_empty);
            var heartLP = new RelativeLayout.LayoutParams(80, 80);

            var metrics = myContext.Resources.DisplayMetrics;
            var widthInDp = ConvertPixelsToDp(metrics.WidthPixels);
            var heightInDp = ConvertPixelsToDp(metrics.HeightPixels);
            heartLP.LeftMargin = parent.Resources.DisplayMetrics.WidthPixels / 2;
            Heart.LayoutParameters = heartLP;
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
            BlobWrapper bvb = new BlobWrapper();
            Bitmap imageBitmap = bvb.Bottleimages(myItems[position].WineId);
            // place.SetImageResource(Resource.Drawable.placeholder);
            Wine.SetImageBitmap(imageBitmap);
            var place1 = new RelativeLayout.LayoutParams(520, 520);
            // var place = new RelativeLayout.LayoutParams(520, 620);
            place1.LeftMargin = parent.Resources.DisplayMetrics.WidthPixels / 2 - 430;
            Wine.LayoutParameters = place1;

            var place2 = new RelativeLayout.LayoutParams(520, 520);
            place2.LeftMargin = parent.Resources.DisplayMetrics.WidthPixels / 2 - 430;
            // place.LayoutParameters = place2;
            //imgPlaceHolder.LayoutParameters = new RelativeLayout.LayoutParams(520, 520);
            //imgWine.LayoutParameters = new RelativeLayout.LayoutParams(520, 520);



            Name.Focusable = false;
            Vintage.Focusable = false;
            Price.Focusable = false;
            Wine.Focusable = false;

            NotifyDataSetChanged();
            return row;
        }
        private int ConvertPixelsToDp(float pixelValue)
        {
            var dp = (int)((pixelValue) / myContext.Resources.DisplayMetrics.Density);
            return dp;
        }
    }
}