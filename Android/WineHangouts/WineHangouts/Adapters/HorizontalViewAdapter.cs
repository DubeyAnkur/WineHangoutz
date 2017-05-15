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

namespace WineHangouts
{
    class HorizontalViewAdapter : BaseAdapter<Item>
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

        public HorizontalViewAdapter(Context con, List<Item> strArr)
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
                row = LayoutInflater.From(myContext).Inflate(Resource.Layout.LandscapeCell, null, false);
            //else
            //    return row;

            TextView txtName = row.FindViewById<TextView>(Resource.Id.txtWinename);
            ImageView imgWine = row.FindViewById<ImageView>(Resource.Id.imgWine);
            //Bitmap imagebitmap = BlobWrapper.Bottleimages(myItems[position].WineId);
            //imgWine.SetImageBitmap(imagebitmap);
            txtName.Text = myItems[position].Name;
            //TextView txtRatings = row.FindViewById<TextView>(Resource.Id.txtRatings);
            //TextView txtVintage = row.FindViewById<TextView>(Resource.Id.txtVintage);
            ////TextView txtUserRatings = row.FindViewById<TextView>(Resource.Id.txtUserRatings);
            //TextView txtPrice = row.FindViewById<TextView>(Resource.Id.txtPrice);

            //ImageView imgPlaceHolder = row.FindViewById<ImageView>(Resource.Id.placeholder);
            //ImageView heartImg = row.FindViewById<ImageView>(Resource.Id.imgHeart);
            //RelativeLayout rel = row.FindViewById<RelativeLayout>(Resource.Id.relative);
            //var place11 = new RelativeLayout.LayoutParams(520, 620)
            //rel.LayoutParameters = place11;
            //rel.LayoutParameters = new RelativeLayout.LayoutParams(520, 520);

            //txtRatings.Text = myItems[position].Ratings;
            //txtUserRatings.Text = myItems[position].UserRatings;
            //txtPrice.Text = myItems[position].RegPrice.ToString();
            //txtPrice.Text = "$ " + txtPrice.Text;
            //txtVintage.Text = myItems[position].Vintage.ToString();
            //heartImg.t = myItems[position].s;

            // heartImg.SetImageResource(Resource.Drawable.heart_empty);
            //var heartLP = new RelativeLayout.LayoutParams(80, 80);

            //var metrics = myContext.Resources.DisplayMetrics;
            //var widthInDp = ConvertPixelsToDp(metrics.WidthPixels);
            //var heightInDp = ConvertPixelsToDp(metrics.HeightPixels);
            //heartLP.LeftMargin = parent.Resources.DisplayMetrics.WidthPixels / 2;
            //heartImg.LayoutParameters = heartLP;





            //heartImg.Layout(50, 50, 50, 50);
            //bool count = Convert.ToBoolean(myItems[position].IsLike);
            //if (count == true)
            //{
            //    heartImg.SetImageResource(Resource.Drawable.heart_full);
            //}
            //else
            //{
            //    heartImg.SetImageResource(Resource.Drawable.heart_empty);
            //}
            //if (convertView == null)
            //{
            //    heartImg.Click += async delegate
            //    {
            //        bool x;
            //        if (count == false)
            //        {
            //            heartImg.SetImageResource(Resource.Drawable.heart_full);
            //            x = true;
            //            count = true;
            //        }
            //        else
            //        {
            //            heartImg.SetImageResource(Resource.Drawable.heart_empty);
            //            x = false;
            //            count = false;
            //        }
            //        SKULike like = new SKULike();
            //        like.UserID = Convert.ToInt32(CurrentUser.getUserId());
            //        like.SKU = Convert.ToInt32(myItems[position].SKU);
            //        like.Liked = x;
            //        ServiceWrapper sw = new ServiceWrapper();
            //        await sw.InsertUpdateLike(like);
            //    };
            //}

            //imgPlaceHolder.SetImageResource(Resource.Drawable.placeholder);

            //var place = new RelativeLayout.LayoutParams(heightInDp, heightInDp);
            // var place = new RelativeLayout.LayoutParams(520, 620);
            //place.LeftMargin = parent.Resources.DisplayMetrics.WidthPixels / 2 - 530;
            //imgWine.LayoutParameters = place;

            //var place1 = new RelativeLayout.LayoutParams(heightInDp, heightInDp);
            //place1.LeftMargin = parent.Resources.DisplayMetrics.WidthPixels / 2 - 530;
            //imgPlaceHolder.LayoutParameters = place1;
            //imgPlaceHolder.LayoutParameters = new RelativeLayout.LayoutParams(520, 520);
            //imgWine.LayoutParameters = new RelativeLayout.LayoutParams(520, 520);


            txtName.Focusable = false;
            //txtRatings.Focusable = false;
            //txtUserRatings.Focusable = false;
            //txtVintage.Focusable = false;
            //txtPrice.Focusable = false;
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