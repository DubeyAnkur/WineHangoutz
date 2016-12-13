using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Widget;
using Android.Graphics;
using System.Net;

using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
namespace HelloGridView
{
    class ListViewAdapter : BaseAdapter<WineDetails>
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

        public ListViewAdapter(Context con, List<WineDetails> strArr)
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
                row = LayoutInflater.From(myContext).Inflate(Resource.Layout.ListView, null, false);

            TextView txtName = row.FindViewById<TextView>(Resource.Id.textView12);
            
            TextView txtName1 = row.FindViewById<TextView>(Resource.Id.textView13);
            //TextView txtRatings = row.FindViewById<TextView>(Resource.Id.txtRatings);
            //TextView description = row.FindViewById<TextView>(Resource.Id.textView33);
            //TextView txtPrice = row.FindViewById<TextView>(Resource.Id.txtPrice);


            txtName.Text = myItems[position].Name;
            txtName1.Text = myItems[position].Price;
            //description.Text = myItems[position].UserRatings;
            //txtPrice.Text = myItems[position].Price;
            //imgWine.SetImageURI(new Uri(myItems[position].imageURL));

            //var imageBitmap = GetImageBitmapFromUrl(myItems[position].imageURL);
            //imgWine.SetImageBitmap(imageBitmap);

            txtName.Focusable = false;
          txtName1.Focusable = false;
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