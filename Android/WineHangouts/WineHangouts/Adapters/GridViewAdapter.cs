﻿using System;
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

namespace WineHangouts
{
    class GridViewAdapter : BaseAdapter<Wine>
    {
        private List<Wine> myItems;
        private Context myContext;
        public override Wine this[int position]
        {
            get
            {
                return myItems[position];
            }
        }

        public GridViewAdapter(Context con, List<Wine> strArr)
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
                row = LayoutInflater.From(myContext).Inflate(Resource.Layout.cell, null, false);

            TextView txtName = row.FindViewById<TextView>(Resource.Id.txtName);
            //TextView txtRatings = row.FindViewById<TextView>(Resource.Id.txtRatings);
            TextView txtVintage = row.FindViewById<TextView>(Resource.Id.txtVintage);
            //TextView txtUserRatings = row.FindViewById<TextView>(Resource.Id.txtUserRatings);
            TextView txtPrice = row.FindViewById<TextView>(Resource.Id.txtPrice);
            ImageView imgWine = row.FindViewById<ImageView>(Resource.Id.imgWine);
            ImageView imgPlaceHolder = row.FindViewById<ImageView>(Resource.Id.placeholder);
            ImageView heartImg = row.FindViewById<ImageView>(Resource.Id.imgHeart);
            //RelativeLayout rel = row.FindViewById<RelativeLayout>(Resource.Id.relative);
            //var place11 = new RelativeLayout.LayoutParams(520, 620)
            //rel.LayoutParameters = place11;
            //rel.LayoutParameters = new RelativeLayout.LayoutParams(520, 520);
            txtName.Text = myItems[position].Name;
            //txtRatings.Text = myItems[position].Ratings;
            //txtUserRatings.Text = myItems[position].UserRatings;
            txtPrice.Text = myItems[position].Price;
            txtVintage.Text = myItems[position].Vintage;
            heartImg.SetImageResource(Resource.Drawable.heart_empty);
            var heartLP = new RelativeLayout.LayoutParams(80, 80);

            var metrics = myContext.Resources.DisplayMetrics;
            var widthInDp = ConvertPixelsToDp(metrics.WidthPixels);
            var heightInDp = ConvertPixelsToDp(metrics.HeightPixels);
            heartLP.LeftMargin = parent.Resources.DisplayMetrics.WidthPixels/2 - 110 ;
            heartImg.LayoutParameters = heartLP;
            
            //heartImg.Layout(50, 50, 50, 50);
            int count=0;
            heartImg.Click += delegate
            {
                if ((count % 2) == 0)
                {
                    heartImg.SetImageResource(Resource.Drawable.heart_full);
                }
                else
                {
                    heartImg.SetImageResource(Resource.Drawable.heart_empty);
                }
                count++;
            };             
            imgPlaceHolder.SetImageResource(Resource.Drawable.placeholder);
            imgWine.SetImageResource(Resource.Drawable.wine1);
            var place = new RelativeLayout.LayoutParams(520, 520);
           // var place = new RelativeLayout.LayoutParams(520, 620);
            place.LeftMargin = parent.Resources.DisplayMetrics.WidthPixels / 2 - 530;
            imgWine.LayoutParameters = place;

            var place1 = new RelativeLayout.LayoutParams(520, 520);
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