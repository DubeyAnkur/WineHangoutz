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

namespace Tabs
{
    class ReviewListAdapter : BaseAdapter<Review>
    {
        private List<Review> myItems;
        private Context myContext;
        public override Review this[int position]
        {
            get
            {
                return myItems[position];
            }
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
                row = LayoutInflater.From(myContext).Inflate(Resource.Layout.ReviewList, null, false);

            TextView txtReviewText = row.FindViewById<TextView>(Resource.Id.txtReviewText);
            TextView txtRevier = row.FindViewById<TextView>(Resource.Id.txtRevier);
            TextView txtTitle = row.FindViewById<TextView>(Resource.Id.txtTitle);
            RatingBar lstRatingBar = row.FindViewById<RatingBar>(Resource.Id.lstRatingBar);

            txtReviewText.Text = myItems[position].ReviewText;
            //txtRevier.Text = myItems[position].ReviewText;
            txtTitle.Text = myItems[position].Title;
            lstRatingBar.Rating = myItems[position].ReviewStars;

            return row;
        }

        public ReviewListAdapter(Context con, List<Review> strArr)
        {
            myContext = con;
            myItems = strArr;
        }
    }
}