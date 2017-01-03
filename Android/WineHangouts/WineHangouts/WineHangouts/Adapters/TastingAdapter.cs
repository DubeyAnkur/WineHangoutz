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
    class ListViewAdapter1 : BaseAdapter<Wine1>
    {
        private List<Wine1> myItems;
        private Context myContext;
        public override Wine1 this[int position]
        {
            get
            {
                return myItems[position];
            }
        }

        public ListViewAdapter1(Context con, List<Wine1> strArr)
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
                row = LayoutInflater.From(myContext).Inflate(Resource.Layout.TastingListview, null, false);

            TextView txtName = row.FindViewById<TextView>(Resource.Id.textView64);
            TextView txtYear = row.FindViewById<TextView>(Resource.Id.textView65);
            TextView txtDescription = row.FindViewById<TextView>(Resource.Id.textView66);
            TextView txtDate = row.FindViewById<TextView>(Resource.Id.textView67);
            ImageButton edit = row.FindViewById<ImageButton>(Resource.Id.imageButton3);
            ImageButton delete = row.FindViewById<ImageButton>(Resource.Id.imageButton4);
            edit.SetScaleType(ImageView.ScaleType.Center);
            delete.SetScaleType(ImageView.ScaleType.Center);
            //TextView txtPrice = row.FindViewById<TextView>(Resource.Id.txtPrice);
            //ImageView imgWine = row.FindViewById<ImageView>(Resource.Id.imgWine);
            edit.Click += Edit_Click;
            delete.Click += Delete_Click;
            txtName.Text = myItems[position].Name;
            txtYear.Text = myItems[position].Vintage;
            txtDescription.Text = myItems[position].UserRatings;
            txtDate.Text = myItems[position].Date;
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