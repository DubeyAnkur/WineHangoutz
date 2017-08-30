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
using System.Diagnostics;
using System.Net;
using AndroidHUD;

namespace WineHangouts
{

    [Activity(Label = "Available Wines", MainLauncher = false, ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    public class GridViewActivity : Android.Support.V4.App.FragmentActivity
    {
       // bool loading;
        public string  WineBarcode;
        public string StoreName = "";
		
		private int screenid = 3;
        GridViewAdapter adapter;

       // SwipeRefreshLayout refresher1;


      
        protected override void OnCreate(Bundle bundle)
        {
			Stopwatch st = new Stopwatch();
			st.Start();
			base.OnCreate(bundle);

            SetContentView(Resource.Layout.Main);
        
            try
            {
				
				CheckInternetConnection();
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

			st.Stop();
			LoggingClass.LogTime("Gridactivity",st.Elapsed.TotalSeconds.ToString());
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
				LoggingClass.LogInfo("entered into "+StoreName, screenid);
				var gridview = FindViewById<GridView>(Resource.Id.gridview);
                adapter = new GridViewAdapter(this, myArr,StoreId);
				LoggingClass.LogInfoEx("Entered into Grid View Adapter", screenid);
				gridview.SetNumColumns(2);
                gridview.Adapter = adapter;

                gridview.ItemClick += delegate (object sender, AdapterView.ItemClickEventArgs args)
                {
                    WineBarcode = myArr[args.Position].Barcode;
                    // ProgressIndicator.Show(this);
                    AndHUD.Shared.Show(this, "Loading...", Convert.ToInt32(MaskType.Clear));
                    var intent = new Intent(this, typeof(DetailViewActivity));
                    LoggingClass.LogInfo("Clicked on " + myArr[args.Position].Barcode + " to enter into wine details",screenid);
                    intent.PutExtra("WineBarcode", WineBarcode);
                    intent.PutExtra("storeid", StoreId);
                    StartActivity(intent);
                };
				//TokenModel devInfo = new TokenModel();
				//var activityManager = (ActivityManager)this.GetSystemService(Context.ActivityService);

				//ActivityManager.MemoryInfo memInfo = new ActivityManager.MemoryInfo();
				//activityManager.GetMemoryInfo(memInfo);

				//System.Diagnostics.Debug.WriteLine("GetDeviceInfo - Avail {0} - {1} MB", memInfo.AvailMem, memInfo.AvailMem / 1024 / 1024);
				//System.Diagnostics.Debug.WriteLine("GetDeviceInfo - Low {0}", memInfo.LowMemory);
				//System.Diagnostics.Debug.WriteLine("GetDeviceInfo - Total {0} - {1} MB", memInfo.TotalMem, memInfo.TotalMem / 1024 / 1024);

				//devInfo.AvailableMainMemory = memInfo.AvailMem;
				//devInfo.IsLowMainMemory = memInfo.LowMemory;
				//devInfo.TotalMainMemory = memInfo.TotalMem;
			}
            catch(Exception exe)
            {
                LoggingClass.LogError(exe.Message, screenid, exe.StackTrace.ToString());
            }
        }
		protected override void OnPause()
		{
			base.OnPause();
			LoggingClass.LogInfo("OnPause state in Gridview activity"+StoreName, screenid);

		}
		public  bool CheckInternetConnection()
		{
			
			string CheckUrl = "http://google.com";

			try
			{
				HttpWebRequest iNetRequest = (HttpWebRequest)WebRequest.Create(CheckUrl);

				iNetRequest.Timeout = 5000;

				WebResponse iNetResponse = iNetRequest.GetResponse();

				// Console.WriteLine ("...connection established..." + iNetRequest.ToString ());
				iNetResponse.Close();

				return true;

			}
			catch (WebException ex)
			{
				AlertDialog.Builder aler = new AlertDialog.Builder(this,Resource.Style.MyDialogTheme);
				aler.SetTitle("Sorry");
				LoggingClass.LogInfo("Please check your internet connection", screenid);
				aler.SetMessage("Please check your internet connection");
				aler.SetNegativeButton("Ok", delegate { });
				Dialog dialog = aler.Create();
				dialog.Show();
				return false;
			}
		}


		protected override void OnResume()
		{
			base.OnResume();
			LoggingClass.LogInfo("OnResume state in Gridview activity"+StoreName, screenid);
		}

		private void MSwipeRefreshLayout_Refresh(object sender, EventArgs e)
        {
            BindGridData();
            SwipeRefreshLayout mSwipeRefreshLayout = FindViewById<SwipeRefreshLayout>(Resource.Id.PullDownRefresh);
			LoggingClass.LogInfo("Refreshed GridView", screenid);
			mSwipeRefreshLayout.Refreshing =false;
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            if (item.ItemId == Android.Resource.Id.Home)
            {
				// base.OnBackPressed();
				var intent = new Intent(this, typeof(TabActivity));
				LoggingClass.LogInfo("Clicked on options menu About", screenid);
				StartActivity(intent);
				LoggingClass.LogInfo("Exited from Gridview Activity",screenid);
				TokenModel devInfo = new TokenModel();
				var activityManager = (ActivityManager)this.GetSystemService(Context.ActivityService);
				return false;
            }
            return base.OnOptionsItemSelected(item);
        }
       
    }
    
}

