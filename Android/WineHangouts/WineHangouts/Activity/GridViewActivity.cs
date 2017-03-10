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
        public int WineID;
        public string StoreName = "";
        /// <summary>
        /// 
        /// </summary>
        /// <param name="bundle"></param>
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            try
            {
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

            int userId = Convert.ToInt32(CurrentUser.getUserId());
            ServiceWrapper sw = new ServiceWrapper();
            ItemListResponse output = new ItemListResponse();
          
                output = sw.GetItemList(StoreId, userId).Result;
            
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
               

                WineID = myArr[args.Position].WineId;
                //detailViewActivity dva = new detailViewActivity();
                //dva.downloadAsync(sender, args, WineID);
                var intent = new Intent(this, typeof(detailViewActivity));
                intent.PutExtra("WineID", WineID);
                StartActivity(intent);
                //    ProgressDialog progressdialog = ProgressDialog.Show(this, "Please Wait", "We are loading it");
                //    new Thread(new ThreadStart(delegate
                //{
                //        RunOnUiThread(() => progressdialog.Show());
                //    Thread.Sleep(10000); 


                //    RunOnUiThread(() => progressdialog.Dismiss());
                //        //RunOnUiThread(() => progressDialog.Wait(1000));
                //        //RunOnUiThread(() => progressDialog.Hide());
                //    })).Start();


            };
            ProgressIndicator.Hide();
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

