using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using System.Collections.Generic;
using Hangout.Models;
using System.Linq;

namespace WineHangouts
{
    [Activity(Label = "MyFavoriteAvtivity")]
    public class MyFavoriteAvtivity : Activity
    {
        public string StoreName = "";
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            if (StoreName == "")
                StoreName = Intent.GetStringExtra("MyData");
            this.Title = StoreName;

            int StoreId = 0;
            if (StoreName == "My Favorite")
                StoreId = 1;
            else if (StoreName == "Point Pleasent Store")
                StoreId = 2;
            else if (StoreName == "Wall Store")
                StoreId = 3;
            int userId = Convert.ToInt32(CurrentUser.getUserId());
            ServiceWrapper sw = new ServiceWrapper();
            var output = sw.GetItemFavsUID(userId).Result;


            SetContentView(Resource.Layout.MyFavoriteGridView);
            ActionBar.SetHomeButtonEnabled(true);
            ActionBar.SetDisplayHomeAsUpEnabled(true);
            //var listview = FindViewById<ListView>(Resource.Id.gridview);
            List<Item> myArr;
            //myArr = SampleData();
           // myArr = 

            var gridview = FindViewById<GridView>(Resource.Id.gridviewfav);
            //myArr = SampleData();

            MyFavoriteAdapter adapter = new MyFavoriteAdapter(this, output.ItemList.ToList());
            gridview.SetNumColumns(2);
            gridview.Adapter = adapter;
            //gridview.SetColumnWidth(1);

            //gridview.SetBackgroundColor(new Android.Graphics.Color(110, 145, 141));
            //gridview.SetVerticalSpacing(5);
            //gridview.SetHorizontalSpacing(5);

            gridview.ItemClick += delegate (object sender, AdapterView.ItemClickEventArgs args)
            {
                var intent = new Intent(this, typeof(MyFavoriteAvtivity));
                StartActivity(intent);
            };


        }
        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            if (item.ItemId == Android.Resource.Id.Home)
            {
                base.OnBackPressed();
                return false;
            }
            return base.OnOptionsItemSelected(item);
        }
        public class ImageAdapter : BaseAdapter
        {
            Context context;

            public ImageAdapter(Context c)
            {
                context = c;
            }

            public override int Count
            {
                get { return thumbIds.Length; }
            }

            public override Java.Lang.Object GetItem(int position)
            {
                return null;
            }

            public override long GetItemId(int position)
            {
                return 0;
            }

            // create a new ImageView for each item referenced by the Adapter
            public override View GetView(int position, View convertView, ViewGroup parent)
            {
                ImageView imageView;

                if (convertView == null)
                {  // if it's not recycled, initialize some attributes
                    imageView = new ImageView(context);
                    imageView.LayoutParameters = new GridView.LayoutParams(450, 450);
                    imageView.SetScaleType(ImageView.ScaleType.FitCenter);
                    imageView.SetPadding(8, 8, 8, 8);
                }
                else
                {
                    imageView = (ImageView)convertView;
                }

                imageView.SetImageResource(thumbIds[position]);
                return imageView;
            }

            // references to our images
            int[] thumbIds = {
        Resource.Drawable.placeholder_bottiglia_lista, Resource.Drawable.placeholder_bottiglia_lista,
        Resource.Drawable.placeholder_bottiglia_lista, Resource.Drawable.placeholder_bottiglia_lista,
        Resource.Drawable.placeholder_bottiglia_lista, Resource.Drawable.placeholder_bottiglia_lista,
        Resource.Drawable.placeholder_bottiglia_lista, Resource.Drawable.placeholder_bottiglia_lista,
        Resource.Drawable.placeholder_bottiglia_lista, Resource.Drawable.placeholder_bottiglia_lista,

    };


        }

    }
}