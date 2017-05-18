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
using System.Threading.Tasks;
using System.Collections.Concurrent;
using System.Net.Http;
using Newtonsoft.Json;


namespace WineHangouts
{

    [Activity(Label = "Available Wines", MainLauncher = false)]
    public class GridViewActivity : Android.Support.V4.App.FragmentActivity
    {
       // bool loading;
        public int WineID;
        public string StoreName = "";
        private int screenid = 3;
        GridViewAdapter adapter;

       // SwipeRefreshLayout refresher1;


      
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

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
                    LoggingClass.LogInfo("Refreshed grid view",screenid);
                    await SomeAsync();
                    mSwipeRefreshLayout.Refreshing = false;
                };

                ActionBar.SetHomeButtonEnabled(true);
                ActionBar.SetDisplayHomeAsUpEnabled(true);
               
                ProgressIndicator.Hide();
                LoggingClass.LogInfo("Entered into gridview activity", screenid);
            }
            catch (Exception exe)
            {
                LoggingClass.LogError(exe.Message, screenid, exe.StackTrace.ToString());
                ProgressIndicator.Hide();
                AlertDialog.Builder aler = new AlertDialog.Builder(this);
                aler.SetTitle("Sorry");
                aler.SetMessage("We're under maintainence");
                aler.SetNegativeButton("Ok", delegate { });
                Dialog dialog = aler.Create();
                dialog.Show();

            }


        }

        public async Task SomeAsync()
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
            try
            { 
                int userId = Convert.ToInt32(CurrentUser.getUserId());
                ServiceWrapper sw = new ServiceWrapper();
                ItemListResponse output = sw.GetItemList(StoreId, userId).Result;

                List<Item> myArr = output.ItemList.ToList();

                var gridview = FindViewById<GridView>(Resource.Id.gridview);
                adapter = new GridViewAdapter(this, myArr,StoreId);
                gridview.SetNumColumns(2);
                gridview.Adapter = adapter;

                gridview.ItemClick += delegate (object sender, AdapterView.ItemClickEventArgs args)
                {
                    WineID = myArr[args.Position].WineId;
                    ProgressIndicator.Show(this);
                    var intent = new Intent(this, typeof(DetailViewActivity));
                    LoggingClass.LogInfo("Clicked on " + myArr[args.Position].WineId + " to enter into wine details",screenid);
                    intent.PutExtra("WineID", WineID);
                    intent.PutExtra("storeid", StoreId);
                    StartActivity(intent);
                };
            }
            catch(Exception exe)
            {
                LoggingClass.LogError(exe.Message, screenid, exe.StackTrace.ToString());
            }
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
                LoggingClass.LogInfo("Exited from Gridview Activity",screenid);
                return false;
            }
            return base.OnOptionsItemSelected(item);
        }
       
    }
    
}

