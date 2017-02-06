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
    [Activity(Label = "LandscapeActivity")]
    public class LandscapeActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Landscape);
            HorizontalScrollView hsw = FindViewById<HorizontalScrollView>(Resource.Id.HorizontalScrollView1);
            
        ///    listner x = new listner();
         ///   hsw.SetOnScrollChangeListener(x);

            int StoreId = 2;
            int userId = Convert.ToInt32(CurrentUser.getUserId());
            List<Item> myArr;
            ServiceWrapper sw = new ServiceWrapper();
            var output = sw.GetItemList(StoreId, userId).Result;
            myArr = output.ItemList.ToList();
            var gridview = FindViewById<GridView>(Resource.Id.gridview);
            HorizontalViewAdapter adapter = new HorizontalViewAdapter(this, myArr);
            gridview.SetNumColumns(10);
            gridview.Adapter = adapter;
            // Create your application here
        }
    }

    public class listner : View.IOnScrollChangeListener
    {
        IntPtr _handler;
      
        public IntPtr Handle
        {
            get
            {
                //throw new NotImplementedException();
                return IntPtr.Zero; 
            }
        }

        public void Dispose()
        {
            //throw new NotImplementedException();
        }

        public void OnScrollChange(View v, int scrollX, int scrollY, int oldScrollX, int oldScrollY)
        {
            //throw new NotImplementedException();
            int x = scrollX;
        }
    }
}