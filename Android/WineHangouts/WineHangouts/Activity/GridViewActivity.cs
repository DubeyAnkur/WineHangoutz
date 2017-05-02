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
using Android.Util;
using System.Threading;
using Android.Support.V4.Widget;
using AppseeAnalytics.Android;
using System.Threading.Tasks;

using System.Collections.Concurrent;
using System.Net.Http;
using Newtonsoft.Json;


namespace WineHangouts
{

    [Activity(Label = "Available Wines", MainLauncher = false)]
    public class GridViewActivity : Android.Support.V4.App.FragmentActivity
    {
        bool loading;
        public int WineID;
        public string StoreName = "";
        GridViewAdapter adapter;
        SwipeRefreshLayout refresher1;


        /// <summary>
        /// 
        /// </summary>
        /// <param name="bundle"></param>
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            Appsee.StartScreen("Grid");
            SetContentView(Resource.Layout.Main);

            try
            {
                if (StoreName == "")
                    StoreName = Intent.GetStringExtra("MyData");
                this.Title = StoreName;
                this.ActionBar.SetHomeButtonEnabled(true);
                this.ActionBar.SetDisplayShowTitleEnabled(true);//  ToolbartItems.Add(new ToolbarItem { Text = "BTN 1", Icon = "myicon.png" });
                
                BindGridData();

                SwipeRefreshLayout mSwipeRefreshLayout = FindViewById<SwipeRefreshLayout>(Resource.Id.PullDownRefresh);

                //mSwipeRefreshLayout.Refresh += MSwipeRefreshLayout_Refresh;
                //mSwipeRefreshLayout.SetColorScheme(Resource.Color.abc_background_cache_hint_selector_material_dark, Resource.Color.abc_background_cache_hint_selector_material_light);

                mSwipeRefreshLayout.Refresh  += async delegate {
                    //BindGridData();
                    
                    await someAsync();
                    mSwipeRefreshLayout.Refreshing = false;
                };

                ActionBar.SetHomeButtonEnabled(true);
                ActionBar.SetDisplayHomeAsUpEnabled(true);
               
                ProgressIndicator.Hide();
            }
            catch (Exception ex)
            {
                Log.Error("Hangouts Exception", ex.Message);
                ProgressIndicator.Hide();
                AlertDialog.Builder aler = new AlertDialog.Builder(this);
                aler.SetTitle("Sorry");
                aler.SetMessage("We're under maintainence");
                aler.SetNegativeButton("Ok", delegate { });
                Dialog dialog = aler.Create();
                dialog.Show();

            }


        }

        public async Task someAsync()
        {
            await Task.Delay(TimeSpan.FromSeconds(1));
            BindGridData();
        }

        private void BindGridData()
        {
            int StoreId = 0;
            if (StoreName == "Wall Store")
                StoreId = 1;
            else if (StoreName == "Point Pleasant Store")
                StoreId = 2;
            else
                StoreId = 3;

            int userId = Convert.ToInt32(CurrentUser.getUserId());
            ServiceWrapper sw = new ServiceWrapper();
            ItemListResponse output = sw.GetItemList(StoreId, userId).Result;

            List<Item> myArr = output.ItemList.ToList();

            var gridview = FindViewById<GridView>(Resource.Id.gridview);
            adapter = new GridViewAdapter(this, myArr);
            gridview.SetNumColumns(2);
            gridview.Adapter = adapter;

            gridview.ItemClick += delegate (object sender, AdapterView.ItemClickEventArgs args)
            {
                WineID = myArr[args.Position].WineId;
                ProgressIndicator.Show(this);
                var intent = new Intent(this, typeof(detailViewActivity));
                intent.PutExtra("WineID", WineID);
                StartActivity(intent);
            };
        }

        private void MSwipeRefreshLayout_Refresh(object sender, EventArgs e)
        {
            BindGridData();
            SwipeRefreshLayout mSwipeRefreshLayout = FindViewById<SwipeRefreshLayout>(Resource.Id.PullDownRefresh);
            mSwipeRefreshLayout.Refreshing =false;
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
        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Drawable.options_menu_1, menu);
            return true;
        }


    }






}

