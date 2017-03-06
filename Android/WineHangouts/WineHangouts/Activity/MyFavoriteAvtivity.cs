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
    [Activity(Label = "My Favorites")]
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
            ItemListResponse output = new ItemListResponse();
            try
            {
                output = sw.GetItemFavsUID(userId).Result;
            }
            catch (Exception ex)
            {
                AlertDialog.Builder aler = new AlertDialog.Builder(this);
                aler.SetTitle("Sorry");
                aler.SetMessage("We're under maintainence");
                aler.SetNegativeButton("Ok", delegate { });
                Dialog dialog = aler.Create();
                dialog.Show();
                
            }
            SetContentView(Resource.Layout.MyFavoriteGridView);
            ActionBar.SetHomeButtonEnabled(true);
            ActionBar.SetDisplayHomeAsUpEnabled(true);
            List<Item> myArr;
            myArr= output.ItemList.ToList();
            var gridview = FindViewById<GridView>(Resource.Id.gridviewfav);
            MyFavoriteAdapter adapter = new MyFavoriteAdapter(this, myArr);
            gridview.SetNumColumns(2);
            gridview.Adapter = adapter;
            

            gridview.ItemClick += delegate (object sender, AdapterView.ItemClickEventArgs args)
            {
                int WineID = myArr[args.Position].WineId;
                var intent = new Intent(this, typeof(detailViewActivity));
                intent.PutExtra("WineID", WineID);
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
        //public class ImageAdapter : BaseAdapter
        //{
        //    Context context;

        //    public ImageAdapter(Context c)
        //    {
        //        context = c;
        //    }

        //    public override int Count
        //    {
        //        get { return thumbIds.Length; }
        //    }

        //    public override Java.Lang.Object GetItem(int position)
        //    {
        //        return null;
        //    }

        //    public override long GetItemId(int position)
        //    {
        //        return 0;
        //    }

        //    // create a new ImageView for each item referenced by the Adapter
        //    public override View GetView(int position, View convertView, ViewGroup parent)
        //    {
        //        ImageView imageView;

        //        if (convertView == null)
        //        {  // if it's not recycled, initialize some attributes
        //            imageView = new ImageView(context);
        //            imageView.LayoutParameters = new GridView.LayoutParams(450, 450);
        //            imageView.SetScaleType(ImageView.ScaleType.FitCenter);
        //            imageView.SetPadding(8, 8, 8, 8);
        //        }
        //        else
        //        {
        //            imageView = (ImageView)convertView;
        //        }

        //        imageView.SetImageResource(thumbIds[position]);
        //        return imageView;
        //    }

        //    // references to our images
        //    int[] thumbIds = {    };


        //}

    }
}