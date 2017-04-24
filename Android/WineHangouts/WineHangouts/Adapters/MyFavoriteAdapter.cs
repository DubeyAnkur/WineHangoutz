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
using Android.Graphics.Drawables;
using System.Globalization;
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
                row = LayoutInflater.From(myContext).Inflate(Resource.Layout.MyFavorite, null, false);
            //else
            //    return row;


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


            Heart.SetImageResource(Resource.Drawable.Heart_emp);
            var heartLP = new FrameLayout.LayoutParams(80, 80);

            var metrics = myContext.Resources.DisplayMetrics;
            var widthInDp = ConvertPixelsToDp(metrics.WidthPixels);
            var heightInDp = ConvertPixelsToDp(metrics.HeightPixels);
            heartLP.LeftMargin = parent.Resources.DisplayMetrics.WidthPixels / 2-110;
            heartLP.TopMargin = 5;
            Heart.LayoutParameters = heartLP;
            Heart.Layout(50, 50, 50, 50);



            bool count = Convert.ToBoolean(myItems[position].IsLike);
            if (count == true)
            {
                Heart.SetImageResource(Resource.Drawable.HeartFull);
            }
            else
            {
                Heart.SetImageResource(Resource.Drawable.Heart_emp);
            }
            Heart.Tag = position;
            if (convertView == null)
            {
                Heart.Click += async delegate
            {
                int actualPosition = Convert.ToInt32(Heart.Tag);
                bool x;
                if (count == false)
                {
                    Heart.SetImageResource(Resource.Drawable.HeartFull);
                    x = true;
                    count = true;
                }
                else
                {
                    Heart.SetImageResource(Resource.Drawable.Heart_emp);
                    x = false;
                    count = false;
                }
                SKULike like = new SKULike();
                like.UserID = Convert.ToInt32(CurrentUser.getUserId());
                like.SKU = Convert.ToInt32(myItems[actualPosition].SKU);
                like.Liked = x;
                myItems[actualPosition].IsLike = x;
                ServiceWrapper sw = new ServiceWrapper();
                like.WineId = myItems[actualPosition].WineId;
                await sw.InsertUpdateLike(like);
            };
            }
            ////////////Bitmap imageBitmap = bvb.Bottleimages(myItems[position].WineId);
            //////////// place.SetImageResource(Resource.Drawable.placeholder);
            //////////ProfilePicturePickDialog pppd = new ProfilePicturePickDialog();
            //////////string path = pppd.CreateDirectoryForPictures();
            ////////////string path = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            //////////var filePath = System.IO.Path.Combine(path + "/" + myItems[position].WineId + ".jpg");
            //////////Bitmap imageBitmap;
            //////////if (System.IO.File.Exists(filePath))
            //////////{
            //////////    imageBitmap = BitmapFactory.DecodeFile(filePath);
            //////////    Wine.SetImageBitmap(imageBitmap);
            //////////}
            //////////else
            //////////{
            //////////    imageBitmap = BlobWrapper.Bottleimages(myItems[position].WineId);
            //////////    Wine.SetImageBitmap(imageBitmap);
            //////////}
            ////////////Wine.SetImageBitmap(imageBitmap);
            //////////var place1 = new RelativeLayout.LayoutParams(520, 520);
            //////////// var place = new RelativeLayout.LayoutParams(520, 620);
            //////////place1.LeftMargin = parent.Resources.DisplayMetrics.WidthPixels / 2 - 430;
            //////////Wine.LayoutParameters = place1;

            //////////var place2 = new RelativeLayout.LayoutParams(520, 520);
            //////////place2.LeftMargin = parent.Resources.DisplayMetrics.WidthPixels / 2 - 430;
            //////////// place.LayoutParameters = place2;
            ////////////imgPlaceHolder.LayoutParameters = new RelativeLayout.LayoutParams(520, 520);
            ////////////imgWine.LayoutParameters = new RelativeLayout.LayoutParams(520, 520);

            Bitmap imageBitmap;



            //if (System.IO.File.Exists(filePath))
            //{
            //    imageBitmap = BitmapFactory.DecodeFile(filePath);
            //}
            //else
            //{
            imageBitmap = BlobWrapper.Bottleimages(myItems[position].WineId);
            //}
            //var place1 = new FrameLayout.LayoutParams(650, 650);
            //imgPlaceHolder.SetImageResource(Resource.Drawable.placeholder);
            //place1.LeftMargin = -70;
            //imgPlaceHolder.LayoutParameters = place1;



            var place = new FrameLayout.LayoutParams(650, 650);
            place.LeftMargin = -70; //-650 + (parent.Resources.DisplayMetrics.WidthPixels - imageBitmap.Width) / 2;
            Wine.LayoutParameters = place;


            if (imageBitmap != null)
            {
                float ratio = (float)650 / imageBitmap.Height;
                imageBitmap = Bitmap.CreateScaledBitmap(imageBitmap, Convert.ToInt32(imageBitmap.Width * ratio), 650, true);
                //imageBitmap.Recycle();
                //Canvas canvas = new Canvas(imageBitmap);
                //imageBitmap.EraseColor(Color.White);
                //canvas.DrawColor(Color.Transparent, PorterDuff.Mode.Clear);
                //canvas.DrawBitmap(imageBitmap, 0, 0, null);
                Wine.SetImageBitmap(imageBitmap);

                imageBitmap.Dispose();

            }
            else
            {
                Wine.SetImageResource(Resource.Drawable.wine7);
            }



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