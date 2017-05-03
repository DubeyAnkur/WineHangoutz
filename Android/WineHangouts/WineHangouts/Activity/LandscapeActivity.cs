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
using Hangout.Models;

namespace WineHangouts
{
    [Activity(Label = "EnoMachine Bottles")]
    public class LandscapeActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Landscape);
            HorizontalScrollView hsw = FindViewById<HorizontalScrollView>(Resource.Id.HorizontalScrollView1);
            int StoreId = 2;
            int userId = Convert.ToInt32(CurrentUser.getUserId());
            List<Item> myArr;
            ServiceWrapper sw = new ServiceWrapper();
            var output = sw.GetItemList(StoreId, userId).Result;
            myArr = output.ItemList.ToList();
            var gridview = FindViewById<GridView>(Resource.Id.gridview);
            HorizontalViewAdapter adapter = new HorizontalViewAdapter(this, myArr);
            gridview.SetNumColumns(myArr.Count);
            gridview.Adapter = adapter;
            //ListView lv = FindViewById<ListView>(Resource.Id.listView1);
            //lv.Adapter=adapter;

        }
    }

    public class listner : View.IOnScrollChangeListener
    {
        //IntPtr _handler;
      
        public IntPtr Handle
        {
            get
            {
                
                return IntPtr.Zero; 
            }
        }

        public void Dispose()
        {
            
        }

        public void OnScrollChange(View v, int scrollX, int scrollY, int oldScrollX, int oldScrollY)
        {
            
            int x = scrollX;
        }
    }
}