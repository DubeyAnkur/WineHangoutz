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
using System.Collections;
using System.Threading.Tasks;

namespace WineHangouts
{
    class GridViewAdapter : BaseAdapter<Item>
    {
        private List<Item> myItems;
        private Context myContext;
        private Hashtable wineImages;
        public override Item this[int position]
        {
            get
            {
                return myItems[position];
            }
        }

        public GridViewAdapter(Context con, List<Item> strArr)
        {
            myContext = con;
            myItems = strArr;
            wineImages = new Hashtable();
            foreach (var item in myItems)
            {
                if (!wineImages.ContainsKey(item.WineId))
                {
                    var imageBitmap = GetImageBitmapFromUrl("https://icsintegration.blob.core.windows.net/bottleimages/" + item.WineId + ".jpg");
                    wineImages.Add(item.WineId, imageBitmap);
                }
            }
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

        public async Task bindImages()
        {
            await 
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            View row = convertView;
            if (row == null)
                row = LayoutInflater.From(myContext).Inflate(Resource.Layout.cell, null, false);
            //else
            //    return row;

            TextView txtName = row.FindViewById<TextView>(Resource.Id.txtName);
            //TextView txtRatings = row.FindViewById<TextView>(Resource.Id.txtRatings);
            TextView txtVintage = row.FindViewById<TextView>(Resource.Id.txtVintage);
            //TextView txtUserRatings = row.FindViewById<TextView>(Resource.Id.txtUserRatings);
            TextView txtPrice = row.FindViewById<TextView>(Resource.Id.txtPrice);
            ImageView imgWine = row.FindViewById<ImageView>(Resource.Id.imgWine);
            ImageView imgPlaceHolder = row.FindViewById<ImageView>(Resource.Id.placeholder);
            ImageView heartImg = row.FindViewById<ImageView>(Resource.Id.imgHeart);
            RatingBar rating = row.FindViewById<RatingBar>(Resource.Id.rtbProductRating);
            rating.Rating = (float)myItems[position].AverageRating;
            //RelativeLayout rel = row.FindViewById<RelativeLayout>(Resource.Id.relative);
            //var place11 = new RelativeLayout.LayoutParams(520, 620)
            //rel.LayoutParameters = place11;
            //rel.LayoutParameters = new RelativeLayout.LayoutParams(520, 520);
            txtName.Text = myItems[position].Name;
            //txtRatings.Text = myItems[position].Ratings;
            //txtUserRatings.Text = myItems[position].UserRatings;
            txtPrice.Text = myItems[position].RegPrice.ToString("C");

            txtVintage.Text = myItems[position].Vintage.ToString();
            //heartImg.t = myItems[position].s;

            heartImg.SetImageResource(Resource.Drawable.heart_empty);
            var heartLP = new RelativeLayout.LayoutParams(80, 80);

            var metrics = myContext.Resources.DisplayMetrics;
            var widthInDp = ConvertPixelsToDp(metrics.WidthPixels);
            var heightInDp = ConvertPixelsToDp(metrics.HeightPixels);
            heartLP.LeftMargin = parent.Resources.DisplayMetrics.WidthPixels / 2;
            heartImg.LayoutParameters = heartLP;

            heartImg.Layout(50, 50, 50, 50);
            bool count = Convert.ToBoolean(myItems[position].IsLike);
            if (count == true)
            {
                heartImg.SetImageResource(Resource.Drawable.heart_full);
            }
            else
            {
                heartImg.SetImageResource(Resource.Drawable.heart_empty);
            }
            if (convertView == null)
            {
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
                    like.WineId = myItems[position].WineId;
                    ServiceWrapper sw = new ServiceWrapper();
                    await sw.InsertUpdateLike(like);
                };
            }

            imgPlaceHolder.SetImageResource(Resource.Drawable.placeholder);
            //if (convertView == null)
            {
                //var imageBitmap = GetImageBitmapFromUrl("https://icsintegration.blob.core.windows.net/bottleimages/" + myItems[position].WineId + ".jpg");
                Bitmap imageBitmap = (Bitmap)wineImages[myItems[position].WineId];
                imgWine.SetImageBitmap(imageBitmap);

                //imgWine.SetImageResource(Resource.Drawable.wine1);
                //var place = new RelativeLayout.LayoutParams(heightInDp, heightInDp);
                var place = new RelativeLayout.LayoutParams(520, 520);
                place.LeftMargin = parent.Resources.DisplayMetrics.WidthPixels / 2 - 430;
                imgWine.LayoutParameters = place;

                var place1 = new RelativeLayout.LayoutParams(520, 520);
                place1.LeftMargin = parent.Resources.DisplayMetrics.WidthPixels / 2 - 430;
                imgPlaceHolder.LayoutParameters = place1;
                //   imgPlaceHolder.LayoutParameters = new RelativeLayout.LayoutParams(520, 520);
                //imgWine.LayoutParameters = new RelativeLayout.LayoutParams(520, 520);


                txtName.Focusable = false;
                //txtRatings.Focusable = false;
                //txtUserRatings.Focusable = false;
                txtVintage.Focusable = false;
                txtPrice.Focusable = false;
                imgWine.Focusable = false;
            }

            return row;
        }

        private Bitmap GetImageBitmapFromUrl(string url)
        {
            Bitmap imageBitmap = null;
            try
            {

                using (var webClient = new WebClient())
                {
                    var imageBytes = webClient.DownloadData(url);
                    if (imageBytes != null && imageBytes.Length > 0)
                    {
                        imageBitmap = BitmapFactory.DecodeByteArray(imageBytes, 0, imageBytes.Length);
                    }
                }

            }
            catch (Exception)
            {
                return null;
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