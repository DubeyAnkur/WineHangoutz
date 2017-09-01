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
using System;
using System.Collections.Generic;

namespace WineHangouts
{
    [Activity(Label = "PotraitAdapter")]
    public class PotraitAdapter : BaseAdapter<Item>
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

        public PotraitAdapter(Context con, List<Item> strArr)
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
        public EventHandler Edit_Click;
        public EventHandler Delete_Click;
        public override long GetItemId(int position)
        {
            return position;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            View row = convertView;
            if (row == null)
                row = LayoutInflater.From(myContext).Inflate(Resource.Layout.PotraitListView, null, false);
            TextView txtName = row.FindViewById<TextView>(Resource.Id.SkuName);
            TextView txtYear = row.FindViewById<TextView>(Resource.Id.Vintage);
           
            TextView txtDate = row.FindViewById<TextView>(Resource.Id.Date);
            ImageButton edit = row.FindViewById<ImageButton>(Resource.Id.imageButton3);
            ImageButton delete = row.FindViewById<ImageButton>(Resource.Id.imageButton4);
            ImageButton wineimage = row.FindViewById<ImageButton>(Resource.Id.imageButton2);
            RatingBar rb = row.FindViewById<RatingBar>(Resource.Id.AvgRating);
            edit.SetScaleType(ImageView.ScaleType.CenterCrop);
            
            //TextView txtPrice = row.FindViewById<TextView>(Resource.Id.txtPrice);
            //ImageView imgWine = row.FindViewById<ImageView>(Resource.Id.imgWine);
            //edit.SetTag(1, 5757);
            //edit.Click += (sender, args) => {
            //    PerformItemClick(sender, args, position, 5757);
            //};
           
            edit.SetImageResource(Resource.Drawable.heart_empty);
            
          
            bool count = Convert.ToBoolean(myItems[position].IsLike);
            if (count == true)
            {
                edit.SetImageResource(Resource.Drawable.heart_full);
            }
            else
            {
                edit.SetImageResource(Resource.Drawable.heart_empty);
            }
            if (convertView == null)
            {
                edit.Click += async delegate
                {
                    bool x;
                    if (count == false)
                    {
                        edit.SetImageResource(Resource.Drawable.heart_full);
                        x = true;
                        count = true;
                    }
                    else
                    {
                        edit.SetImageResource(Resource.Drawable.heart_empty);
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
            }




            txtDate.SetTextSize(Android.Util.ComplexUnitType.Dip, 12);
            txtName.Text = myItems[position].Name;
            txtYear.Text = myItems[position].Vintage.ToString(); ;

            txtDate.Text = myItems[position].RegPrice.ToString();
            txtDate.Text = "$ " + txtDate.Text;
            rb.Rating = (float)myItems[position].AverageRating;
            wineimage.SetImageResource(Resource.Drawable.Tes);
            wineimage.SetScaleType(ImageView.ScaleType.CenterCrop);
            //txtPrice.Text = myItems[position].Price;
            //imgWine.SetImageURI(new Uri(myItems[position].imageURL));

       

            txtName.Focusable = false;
            txtYear.Focusable = false;
            rb.Focusable = false;
            txtDate.Focusable = false;
            //txtRatings.Focusable = false;
            //txtUserRatings.Focusable = false;
            //txtPrice.Focusable = false;
            //imgWine.Focusable = false;


            return row;
        }

        //public void PerformItemClick(object sender, EventArgs e, int position, int SKU)
        //{
        //    ReviewPopup editPopup = new ReviewPopup(myContext);
        //    editPopup.SKU = SKU;
        //    editPopup.EditPopup(sender, e);
        //}

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
    }
}