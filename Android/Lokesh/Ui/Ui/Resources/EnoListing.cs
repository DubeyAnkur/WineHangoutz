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

namespace Ui
{
    [Activity(Label = "EnoListing",MainLauncher = true, Icon = "@drawable/icon")]
    public class EnoListing : Activity
    {
        List<Wine> myArr;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            SetContentView(Resource.Layout.listView_row);
            ListView wineList = FindViewById<ListView>(Resource.Id.wineList);
            myArr = SampleData();

            ListViewAdapter adapter = new ListViewAdapter(this, myArr);
            wineList.Adapter = adapter;
        }

           

        public List<Wine> SampleData()
        {
            List<Wine> myArr = new List<Wine>();
            Wine w1 = new Wine();
            w1.Name = "Silver Oak Napa Valley Cabernet Sauvignon 2011";
            w1.Price = "$15.99";
            w1.Vintage = "WS: TOP 100";
            w1.imageURL = "http://cdn.fluidretail.net/customers/c1477/13/97/48/_s/pi/n/139748_spin_spin2/main_variation_na_view_01_204x400.jpg";
           

            Wine w2 = new Wine();
            w2.Name = "Bodega Norton Reserve Malbec 2013";
            w2.Price = "$19.99";
            w2.Vintage = "WS: TOP 100";
            w2.imageURL = "http://www.wineoutlet.com/labels/B24718.jpg";


            Wine w3 = new Wine();
            w3.Name = "Bodega Norton Reserve Malbec 2013";
            w3.Price = "$19.99";
            w3.Vintage = "WS: TOP 100";
            w3.imageURL = "http://www.wineoutlet.com/labels/B24718.jpg";

            Wine w4 = new Wine();
            w4.Name = "Bodega Norton Reserve Malbec 2013";
            w4.Price = "$19.99";
            w4.Vintage = "WS: TOP 100";
            w4.imageURL = "http://www.wineoutlet.com/labels/B24718.jpg";

            myArr.Add(w1);
            myArr.Add(w2);
            myArr.Add(w3);
            myArr.Add(w4);
            return myArr;
        }
    }
}