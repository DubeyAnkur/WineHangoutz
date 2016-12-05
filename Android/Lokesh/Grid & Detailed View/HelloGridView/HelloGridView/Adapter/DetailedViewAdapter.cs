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

namespace HelloGridView
{
    class DetailedViewAdapter : BaseAdapter<WineDetails>
    {
        private List<WineDetails> myItems;
        private Context myContext;
        public override WineDetails this[int position]
        {
            get
            {
                return myItems[position];
            }
        }

        public DetailedViewAdapter(Context con, List<WineDetails> strArr)
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

        public static DetailedViewAdapter Adapter { get; internal set; }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override View GetView(int position, View convertView1, ViewGroup parent)
        {
            View row = convertView1;
            if (row == null)
                row = LayoutInflater.From(myContext).Inflate(Resource.Layout.detailedView, null, false);

            TextView Value = row.FindViewById<TextView>(Resource.Id.textView15);
            //TextView txtRatings = row.FindViewById<TextView>(Resource.Id.textView15);
            //TextView description = row.FindViewById<TextView>(Resource.Id.textView33);
            //TextView txtPrice = row.FindViewById<TextView>(Resource.Id.txtPrice);
            //ImageView imgWine = row.FindViewById<ImageView>(Resource.Id.imageView2);


            Value.Text = myItems[position].Name;
            //txtRatings.Text = myItems[position].Ratings;
            //description.Text = myItems[position].UserRatings;
            //txtPrice.Text = myItems[position].Price;
            //imgWine.SetImageURI(new Uri(myItems[position].imageURL));

            //var imageBitmap = GetImageBitmapFromUrl(myItems[position].imageURL);
            //imgWine.SetImageBitmap(imageBitmap);

            Value.Focusable = false;
            //txtRatings.Focusable = false;
            //description.Focusable = false;
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