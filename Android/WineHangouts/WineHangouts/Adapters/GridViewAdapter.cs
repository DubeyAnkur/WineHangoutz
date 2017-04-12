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



namespace WineHangouts
{
    class GridViewAdapter : BaseAdapter<Item>
    {
        private List<Item> myItems;
        private Context myContext;
        //private Hashtable wineImages;


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
            //wineImages = new Hashtable();
            //foreach (var item in myItems)
            //{
            //    if (!wineImages.ContainsKey(item.WineId))
            //    {
            //        BlobWrapper bvb = new BlobWrapper();
            //        var imageBitmap = bvb.GetImageBitmapFromUrl("https://icsintegration.blob.core.windows.net/bottleimages/" + item.WineId + ".jpg");
            //        wineImages.Add(item.WineId, imageBitmap);
            //    }
            //}
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

        //public async Task bindImages()
        //{
        //    await 
        //}

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
           // ImageView imgPlaceHolder = row.FindViewById<ImageView>(Resource.Id.Placeholder);
            ImageView heartImg = row.FindViewById<ImageView>(Resource.Id.imgHeart);
            RatingBar rating = row.FindViewById<RatingBar>(Resource.Id.rtbProductRating);
            rating.Rating = (float)myItems[position].AverageRating;
            //RelativeLayout rel = row.FindViewById<RelativeLayout>(Resource.Id.relative);
            //var place11 = new RelativeLayout.LayoutParams(520, 620)
            //rel.LayoutParameters = place11;
            //rel.LayoutParameters = new RelativeLayout.LayoutParams(520, 520);
            txtName.Text = myItems[position].Name;
            //txtName.InputType = Android.Text.InputTypes.TextFlagNoSuggestions;
            //txtRatings.Text = myItems[position].Ratings;
            //txtUserRatings.Text = myItems[position].UserRatings;

            txtPrice.Text = myItems[position].SalePrice.ToString("C", Cultures.UnitedState);

            txtVintage.Text = myItems[position].Vintage.ToString();
            //heartImg.t = myItems[position].s;

            heartImg.SetImageResource(Resource.Drawable.heart_empty);
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
                heartImg.SetImageResource(Resource.Drawable.heart_full);
            }
            else
            {
                heartImg.SetImageResource(Resource.Drawable.heart_empty);
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
                    like.SKU = Convert.ToInt32(myItems[actualPosition].SKU);
                    like.Liked = x;
                    myItems[actualPosition].IsLike = x;
                    like.WineId = myItems[actualPosition].WineId;
                    ServiceWrapper sw = new ServiceWrapper();
                    await sw.InsertUpdateLike(like);
                };
            }


            //ProfilePicturePickDialog pppd = new ProfilePicturePickDialog();
            //string path = pppd.CreateDirectoryForPictures();
            //var filePath = System.IO.Path.Combine(path + "/" + myItems[position].WineId + ".jpg");
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
            imgWine.LayoutParameters = place;


            if (imageBitmap != null)
            {
                float ratio = (float)650 / imageBitmap.Height;
                imageBitmap = Bitmap.CreateScaledBitmap(imageBitmap, Convert.ToInt32(imageBitmap.Width * ratio), 650, true);
                //imageBitmap.Recycle();
                //Canvas canvas = new Canvas(imageBitmap);
                //imageBitmap.EraseColor(Color.White);
                //canvas.DrawColor(Color.Transparent, PorterDuff.Mode.Clear);
                //canvas.DrawBitmap(imageBitmap, 0, 0, null);
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
            //imgPlaceHolder.Focusable = false;
            return row;
        }



        private int ConvertPixelsToDp(float pixelValue)
        {
            var dp = (int)((pixelValue) / myContext.Resources.DisplayMetrics.Density);
            return dp;
        }
    }
}