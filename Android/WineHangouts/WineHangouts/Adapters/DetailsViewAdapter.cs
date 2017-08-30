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
using Android.Content.Res;
using Android.Util;
using static Android.Text.BoringLayout;
using Hangout.Models;

namespace WineHangouts
{
    class DetailsViewAdapter : BaseAdapter<Item>
    {
		private int screenid = 22;
        private List<Item> myItems;
        private Context myContext;
        public override Item this[int position]
        {
            get
            {
                return myItems[position];
            }
        }

        public DetailsViewAdapter(Context con, List<Item> strArr)
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
                row = LayoutInflater.From(myContext).Inflate(Resource.Layout.WinePropertiesCell, null, false);
			LoggingClass.LogInfo("Entered into DetailsViewAdapter", screenid);
            TextView Type = row.FindViewById<TextView>(Resource.Id.textView12);
            TextView Value = row.FindViewById<TextView>(Resource.Id.textView13);
            Type.LayoutParameters.Width = 550;
            Type.Text = myItems[position].Name;
            Value.Text = myItems[position].Vintage.ToString();
            if (Value.Text == null || Value.Text == "0") { Value.Text = ""; } else { Value.Text = myItems[position].Vintage.ToString(); }
            Type.Focusable = false;
            Value.Focusable = false;
            

            return row;
        }

       
    }
}