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

namespace WineHangouts
{
    class DetailsViewAdapter : BaseAdapter<WineDetails>
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

        public DetailsViewAdapter(Context con, List<WineDetails> strArr)
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

        //public override WineDetails this[int position]
        //{
        //    get
        //    {
        //        throw new NotImplementedException();
        //    }
        //}

        public override long GetItemId(int position)
        {
            return position;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            View row = convertView;
            if (row == null)
                row = LayoutInflater.From(myContext).Inflate(Resource.Layout.WinePropertiesCell, null, false);
            
            TextView Type = row.FindViewById<TextView>(Resource.Id.textView12);
            TextView Value = row.FindViewById<TextView>(Resource.Id.textView13);
            Type.LayoutParameters.Width = 550;
            //TextView txtUserRatings = row.FindViewById<TextView>(Resource.Id.txtUserRatings);
            //TextView txtPrice = row.FindViewById<TextView>(Resource.Id.txtPrice);
            //ImageView imgWine = row.FindViewById<ImageView>(Resource.Id.imgWine);


            Type.Text = myItems[position].Type;
            Value.Text = myItems[position].Value;
            //Type.Text = myItems[position].Classification;
            //Value.Text = myItems[position].ClassificationValue;
            //Type.Text = myItems[position].Grapetype;
            //Value.Text = myItems[position].GrapeTypeValue;
            //Type.Text = myItems[position].Alcohol;
            //Value.Text = myItems[position].AlcoholValue;
            //Type.Text = myItems[position].Vintage;
            //Value.Text = myItems[position].VintageValue;
            //Type.Text = myItems[position].Aromas;
            //Value.Text = myItems[position].AromasValue;
            //Type.Text = myItems[position].FoodPairings;
            //Value.Text = myItems[position].FoodPairingsValue;
            //Type.Text = myItems[position].Bottlesize;
            //Value.Text = myItems[position].BottleSizeValue;
            //Type.Text = myItems[position].ServingAt;
            //Value.Text = myItems[position].ServingAtValue;
            //imgWine.SetImageURI(new Uri(myItems[position].imageURL));

            //var imageBitmap = GetImageBitmapFromUrl(myItems[position].imageURL);
            //imgWine.SetImageBitmap(imageBitmap);

            Type.Focusable = false;
            Value.Focusable = false;
            //txtUserRatings.Focusable = false;
            //txtPrice.Focusable = false;
            //imgWine.Focusable = false;


            return row;
        }

       
    }
}