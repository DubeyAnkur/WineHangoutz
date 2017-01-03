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

namespace WineHangouts
{
    [Activity(Label = "reviewAdapter")]
    public class reviewAdapter : BaseAdapter<Review>
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
            ImageButton Image = row.FindViewById<ImageButton>(Resource.Id.imageButton2);

            Name.Text = myItems[position].Name;
            Comments.Text = myItems[position].Comments;
            Image.SetImageResource(Resource.Drawable.user);
            Image.SetScaleType(ImageView.ScaleType.CenterCrop);
            return row;
        }
    }
}