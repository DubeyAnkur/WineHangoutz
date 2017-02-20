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
using System.Threading;

namespace WineHangouts
{

    [Activity(Label = "GridViewActivity", MainLauncher = false)]
    public class GridViewActivity : Activity
    {

        public string StoreName = "";
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            if (StoreName == "")
                StoreName = Intent.GetStringExtra("MyData");
            this.Title = StoreName;
            this.ActionBar.SetHomeButtonEnabled(true);
            this.ActionBar.SetDisplayShowTitleEnabled(true);//  ToolbartItems.Add(new ToolbarItem { Text = "BTN 1", Icon = "myicon.png" });

            int StoreId = 0;
            if (StoreName == "Wall Store")
                StoreId = 1;
            else if (StoreName == "Point Pleasant Store")
                StoreId = 2;
            else
                StoreId = 3;
            //var progressDialog = ProgressDialog.Show(this, "Please wait...", "We are loading available wines...", true);
            //new Thread(new ThreadStart(delegate
            //{
            //    //LOAD METHOD TO GET ACCOUNT INFO

            //    //HIDE PROGRESS DIALOG
            //    RunOnUiThread(() => progressDialog.Show());
            //    Thread.Sleep(10000);
            //    RunOnUiThread(() => progressDialog.Dismiss());
            //    //RunOnUiThread(() => progressDialog.Wait(1000));
            //    //RunOnUiThread(() => progressDialog.Hide());
            //})).Start();
            int userId = Convert.ToInt32(CurrentUser.getUserId());
            ServiceWrapper sw = new ServiceWrapper();
            var output = sw.GetItemList(StoreId,userId).Result;


            SetContentView(Resource.Layout.Main);
            ActionBar.SetHomeButtonEnabled(true);
            ActionBar.SetDisplayHomeAsUpEnabled(true);
            //var listview = FindViewById<ListView>(Resource.Id.gridview);
            List<Item> myArr;
            //myArr = SampleData();
            myArr = output.ItemList.ToList();

            var gridview = FindViewById<GridView>(Resource.Id.gridview);
            //myArr = SampleData();

            GridViewAdapter adapter = new GridViewAdapter(this, myArr);
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


    }





}

