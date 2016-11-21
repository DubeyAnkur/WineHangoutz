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

namespace Tabs
{
    [Activity(Label = "EnoListing")]
    public class EnoListing : Activity
    {
        List<Wine> myArr;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            SetContentView(Resource.Layout.EnoListing);

            var storeName = Intent.Extras.GetStringArrayList("storeName") ?? new string[0];

            TextView txtStoreName = FindViewById<TextView>(Resource.Id.txtStoreName);
            txtStoreName.Text = "Welcome to " + storeName[0] + " store !!!";

            ListView wineList = FindViewById<ListView>(Resource.Id.wineList);
            myArr = SampleData();

            ListViewAdapter adapter = new ListViewAdapter(this, myArr);
            wineList.Adapter = adapter;

            //wineList.ItemClick += WineList_ItemClick;
            wineList.ItemClick += new EventHandler<AdapterView.ItemClickEventArgs>(WineList_ItemClick);
        }

        private void WineList_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            var intent = new Intent(this, typeof(WineDetailsActivity));
            List<string> sku = new List<string>();
            sku.Add(myArr[e.Position].SKU);
            intent.PutStringArrayListExtra("sku", sku);
            StartActivity(intent);
        }

        public List<Wine> SampleData()
        {
            List<Wine> myArr = new List<Wine>();
            Wine w1 = new Wine();
            w1.Name = "Silver Oak Napa Valley Cabernet Sauvignon 2011";
            w1.Price = "$15.99";
            w1.Ratings = "WS: 95 pts";
            w1.UserRatings = "4.5";
            w1.imageURL = "http://cdn.fluidretail.net/customers/c1477/13/97/48/_s/pi/n/139748_spin_spin2/main_variation_na_view_01_204x400.jpg";
            w1.SKU = "1";

            Wine w2 = new Wine();
            w2.Name = "Bodega Norton Reserve Malbec 2013";
            w2.Price = "$19.99";
            w2.Ratings = "WS: TOP 100";
            w2.UserRatings = "4.8";
            w2.imageURL = "http://www.wineoutlet.com/labels/B24718.jpg";
            w2.SKU = "24718";

            Wine w3 = new Wine();
            w3.Name = "Carpano Vermouth Antica Formula";
            w3.Price = "$20.99";
            w3.Ratings = "WE: 95 pts, UBC: 90";
            w3.UserRatings = "3.8";
            w3.imageURL = "http://library.bevnetwork.com/bottles/680/320985.jpg";
            w3.SKU = "1778668";

            Wine w4 = new Wine();
            w4.Name = "Breca Old Vine Garnacha 2013";
            w4.Price = "$11.99";
            w4.Ratings = "No Ratings";
            w4.UserRatings = "Not rated yet";
            w4.imageURL = "http://library.bevnetwork.com/bottles/645/172959.jpg";
            w4.SKU = "01247";

            myArr.Add(w1);
            myArr.Add(w2);
            myArr.Add(w3);
            myArr.Add(w4);
            return myArr;
        }
    }
}