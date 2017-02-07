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
using System.Globalization;

namespace WineHangouts
{
    [Activity(Label = "MyTastingAdapter")]
    public class MyTastingAdapter : BaseAdapter<Item>
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

        public MyTastingAdapter(Context con, List<Item> strArr)
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
                row = LayoutInflater.From(myContext).Inflate(Resource.Layout.MyTastingView, null, false);
            else
                return convertView;

            TextView txtName = row.FindViewById<TextView>(Resource.Id.SkuName);
            TextView txtYear = row.FindViewById<TextView>(Resource.Id.Vintage);
            TextView txtDescription = row.FindViewById<TextView>(Resource.Id.TastingNotes);
            TextView txtDate = row.FindViewById<TextView>(Resource.Id.Date);
            
            ImageButton wineimage = row.FindViewById<ImageButton>(Resource.Id.imageButton2);
            RatingBar rb = row.FindViewById<RatingBar>(Resource.Id.AvgRating);
           
            //TextView txtPrice = row.FindViewById<TextView>(Resource.Id.txtPrice);
            //ImageView imgWine = row.FindViewById<ImageView>(Resource.Id.imgWine);
            //edit.SetTag(1, 5757);
           
           
            txtDate.SetTextSize(Android.Util.ComplexUnitType.Dip, 12);
            txtName.Text = myItems[position].Name;
            txtYear.Text = myItems[position].Vintage.ToString();
            txtDescription.Text = myItems[position].Description;
            txtDate.Text = myItems[position].RegPrice.ToString("C");
            rb.Rating = (float)myItems[position].AverageRating;
            wineimage.SetImageResource(Resource.Drawable.wine7);
            wineimage.SetScaleType(ImageView.ScaleType.CenterCrop);
            //txtPrice.Text = myItems[position].Price;
            //imgWine.SetImageURI(new Uri(myItems[position].imageURL));

            //var imageBitmap = GetImageBitmapFromUrl(myItems[position].imageURL);
            //imgWine.SetImageBitmap(imageBitmap);

            txtName.Focusable = false;
            txtYear.Focusable = false;
            txtDescription.Focusable = false;
            txtDate.Focusable = false;
            //txtRatings.Focusable = false;
            //txtUserRatings.Focusable = false;
            //txtPrice.Focusable = false;
            //imgWine.Focusable = false;


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
    }
}