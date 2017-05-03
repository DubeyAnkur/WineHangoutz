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
using System.Globalization;
using Android.Graphics.Drawables;
using Newtonsoft.Json;



namespace WineHangouts
{
    class GridViewAdapter : BaseAdapter<Item>
    {
        private List<Item> myItems;
        private Context myContext;
        //private Hashtable wineImages;

        public void ClearData()
        {
            myItems.Clear();
            NotifyDataSetChanged();
        }
        public void FeedData(IEnumerable<Item> newData)
        {
            myItems.AddRange(newData);
            NotifyDataSetChanged();
        }
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
           
        }
        public override int Count
        {
            get
            {
                return myItems.Count;
            }
        }

        public static class Cultures
        {
            public static readonly CultureInfo UnitedState =
                CultureInfo.GetCultureInfo("en-US");
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
            //else
            //    return row;

            TextView txtName = row.FindViewById<TextView>(Resource.Id.txtName);
          
            TextView txtVintage = row.FindViewById<TextView>(Resource.Id.txtVintage);
         
            TextView txtPrice = row.FindViewById<TextView>(Resource.Id.txtPrice);
            ImageView imgWine = row.FindViewById<ImageView>(Resource.Id.imgWine);
        
            ImageView heartImg = row.FindViewById<ImageView>(Resource.Id.imgHeart);
            RatingBar rating = row.FindViewById<RatingBar>(Resource.Id.rtbProductRating);
            rating.Rating = (float)myItems[position].AverageRating;
            txtName.Text = myItems[position].Name;
            txtPrice.Text = myItems[position].SalePrice.ToString("C", Cultures.UnitedState);
            txtVintage.Text = myItems[position].Vintage.ToString();
            heartImg.SetImageResource(Resource.Drawable.Heart_emp);
            var heartLP = new FrameLayout.LayoutParams(80, 80);
            var metrics = myContext.Resources.DisplayMetrics;
            var widthInDp = ConvertPixelsToDp(metrics.WidthPixels);
            var heightInDp = ConvertPixelsToDp(metrics.HeightPixels);
            heartLP.LeftMargin = parent.Resources.DisplayMetrics.WidthPixels / 2 - 110; // 110 = 80 + 30
            heartLP.TopMargin = 5;
            heartImg.LayoutParameters = heartLP;
            heartImg.Layout(50, 50, 50, 50);
            bool count = Convert.ToBoolean(myItems[position].IsLike);
            if (count == true)
            {
                heartImg.SetImageResource(Resource.Drawable.HeartFull);
            }
            else
            {
                heartImg.SetImageResource(Resource.Drawable.Heart_emp);
            }

            heartImg.Tag = position;

            if (convertView == null)
            {
                heartImg.Click += async delegate
                {
                    int actualPosition = Convert.ToInt32(heartImg.Tag);
                    bool x;
                    if (count == false)
                    {
                        heartImg.SetImageResource(Resource.Drawable.HeartFull);
                        x = true;
                        count = true;
                    }
                    else
                    {
                        heartImg.SetImageResource(Resource.Drawable.Heart_emp);
                        x = false;
                        count = false;
                    }
                    SKULike like = new SKULike();
                    like.UserID = Convert.ToInt32(CurrentUser.getUserId());
                    like.SKU = Convert.ToInt32(myItems[actualPosition].SKU);
                    like.Liked = x;
                    myItems[actualPosition].IsLike = x;
                    like.WineId = myItems[actualPosition].WineId;
                    LoggingClass.LogInfo("Liked an item");
                    ServiceWrapper sw = new ServiceWrapper();
                    await sw.InsertUpdateLike(like);
                };
            }
            Bitmap imageBitmap;
            imageBitmap = BlobWrapper.Bottleimages(myItems[position].WineId);
            var place = new FrameLayout.LayoutParams(650, 650);
            place.LeftMargin = -70; //-650 + (parent.Resources.DisplayMetrics.WidthPixels - imageBitmap.Width) / 2;
            imgWine.LayoutParameters = place;
            if (imageBitmap != null)
            {
                float ratio = (float)650 / imageBitmap.Height;
                imageBitmap = Bitmap.CreateScaledBitmap(imageBitmap, Convert.ToInt32(imageBitmap.Width * ratio), 650, true);
               
                imgWine.SetImageBitmap(imageBitmap);

                imageBitmap.Dispose();

            }
            else
            {
                imgWine.SetImageResource(Resource.Drawable.wine7);
            }

            txtName.Focusable = false;

            txtVintage.Focusable = false;
            txtPrice.Focusable = false;
            imgWine.Focusable = false;
        
            return row;
        }
     
        private int ConvertPixelsToDp(float pixelValue)
        {
            var dp = (int)((pixelValue) / myContext.Resources.DisplayMetrics.Density);
            return dp;
        }
    }
}