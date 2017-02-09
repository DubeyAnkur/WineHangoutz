using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Hangout.Models;
using Android.Widget;
using Android.Graphics;
using System.Net;

namespace WineHangouts
{
    [Activity(Label = "reviewAdapter")]
    public class reviewAdapter : BaseAdapter<Review>
    {
        private List<Review> myItems;
        private Context myContext;
        int userId = Convert.ToInt32(CurrentUser.getUserId());
        public override Review this[int position]
        {
            get
            {
                return myItems[position];
            }
        }

        public reviewAdapter(Context con, List<Review> strArr)
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
                row = LayoutInflater.From(myContext).Inflate(Resource.Layout.CommentsCell, null, false);
            TextView Name = row.FindViewById<TextView>(Resource.Id.textView64);
            TextView Comments = row.FindViewById<TextView>(Resource.Id.textView66);
            TextView date = row.FindViewById<TextView>(Resource.Id.textView67);
            RatingBar rb = row.FindViewById<RatingBar>(Resource.Id.rtbProductRating);
            ImageButton Image = row.FindViewById<ImageButton>(Resource.Id.imageButton2);
            var imageBitmap = GetImageBitmapFromUrl("https://icsintegration.blob.core.windows.net/profileimages/1.jpg");
            if (imageBitmap == null)
            {
                Image.SetImageResource(Resource.Drawable.ic_action_person);
            }
            Name.Text = myItems[position].Username;
            Comments.Text = myItems[position].RatingText;
            date.Text = myItems[position].Date.ToString("dd/MM/yyyy");
            rb.Rating = myItems[position].RatingStars;
            Image.SetImageBitmap(imageBitmap);
            Image.SetScaleType(ImageView.ScaleType.CenterCrop);
            return row;
        }

        private Bitmap GetImageBitmapFromUrl(string p)
        {
            Bitmap imageBitmap = null;

            using (var webClient = new WebClient())
            {
                if (webClient.DownloadData(p).ToString() == null)
                {
                    var imageBytes = webClient.DownloadData(p);
                    if (imageBytes != null && imageBytes.Length > 0)
                    {
                        imageBitmap = BitmapFactory.DecodeByteArray(imageBytes, 0, imageBytes.Length);
                    }
                }
               
            }

            return imageBitmap;
        }
    }
}