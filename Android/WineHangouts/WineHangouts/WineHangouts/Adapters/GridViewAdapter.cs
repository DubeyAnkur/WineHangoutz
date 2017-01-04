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
            TextView txtRatings = row.FindViewById<TextView>(Resource.Id.txtRatings);
            TextView txtVintage = row.FindViewById<TextView>(Resource.Id.txtVintage);
            //TextView txtUserRatings = row.FindViewById<TextView>(Resource.Id.txtUserRatings);
            TextView txtPrice = row.FindViewById<TextView>(Resource.Id.txtPrice);
            //ImageView imgWine = row.FindViewById<ImageView>(Resource.Id.imgWine);
            //ImageView imgWine1 = row.FindViewById<ImageView>(Resource.Id.imgWine1);

            txtName.Text = myItems[position].Name;
            txtRatings.Text = myItems[position].Ratings;
            //txtUserRatings.Text = myItems[position].UserRatings;
            txtPrice.Text = myItems[position].Price;
            txtVintage.Text = myItems[position].Vintage;
            //imgWine.SetImageResource(Resource.Drawable.wine1);
            //imgWine.SetImageResource(Resource.Drawable.placeholder);
            //imgWine.SetImageResource(Resource.Drawable.wine1);


            RelativeLayout layout = row.FindViewById<RelativeLayout>(Resource.Id.relLayout);
            //ImageView Setup
            ImageView imageView = new ImageView(this.myContext);
            ImageView imageView2 = new ImageView(this.myContext);
            imageView.SetImageResource(Resource.Drawable.placeholder);
            imageView2.SetImageResource(Resource.Drawable.wine1);
            imageView.SetPadding(1,2,3,4);
            //imageView.SetMaxHeight(250);
            //imageView.SetMaxWidth(45);

            layout.AddView(imageView);
            layout.AddView(imageView2);



            //imgWine.SetImageURI(new Uri(myItems[position].imageURL));

            //var imageBitmap = GetImageBitmapFromUrl(myItems[position].imageURL);
            //imgWine.SetImageBitmap(imageBitmap);

            txtName.Focusable = false;
            txtRatings.Focusable = false;
            //txtUserRatings.Focusable = false;
            txtVintage.Focusable = false;
            txtPrice.Focusable = false;
            imageView.Focusable = false;
            imageView2.Focusable = false;
            // imgWine1.Focusable = false;


            return row;
        }


        private int PixelsToDp(object widthInDp)
        {
            throw new NotImplementedException();
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