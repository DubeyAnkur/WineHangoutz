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

namespace HelloGridView
{
    [Activity(Label = "detailViewActivity", MainLauncher = false, Icon = "@drawable/icon")]
    public class detailViewActivity : Activity
    {
        //List<Wine> myArr;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.detailedView);
            //var listview = FindViewById<ListView>(Resource.Id.listView1);
            //myArr = SampleData();
            string[] arr1 = new string[] { "Silver Oak Napa Valley", "Cabernet ", "2011"," This is the description about wine" };
            TextView TopName = FindViewById<TextView>(Resource.Id.textView6); //Assigning to respected Textfield
            TextView TopBrand = FindViewById<TextView>(Resource.Id.textView7);
            TextView TopVintage = FindViewById<TextView>(Resource.Id.textView8);
            TextView WineDescription = FindViewById<TextView>(Resource.Id.textView36);

            TopName.Focusable = false;
            TopBrand.Focusable = false;
            TopVintage.Focusable = false;
            WineDescription.Focusable = false;

            TopName.Text = arr1[0]; //Assigning value
            TopBrand.Text = arr1[1];
            TopVintage.Text = arr1[2];
            WineDescription.Text = arr1[3];

        }
          
    public List<Wine> SampleData()
    {
        List<Wine> myArr = new List<Wine>();
        Wine w1 = new Wine();
        w1.Name = "Silver Oak Napa Valley Cabernet Sauvignon 2011";
        w1.Ratings = "Ratings";
        w1.UserRatings = "User Ratings";
        w1.Price = "Prise$";
        w1.Vintage = "WS: TOP 100";
        w1.imageURL = "http://cdn.fluidretail.net/customers/c1477/13/97/48/_s/pi/n/139748_spin_spin2/main_variation_na_view_01_204x400.jpg";

        Wine w2 = new Wine();
        w2.Name = "Bodega Norton Reserve Malbec 2013";
        w2.Price = "$19.99";
        w2.Ratings = "15";
        w2.UserRatings = "12";
        w2.Vintage = "WS: TOP 100";
        w2.imageURL = "http://cdn.fluidretail.net/customers/c1477/13/97/48/_s/pi/n/139748_spin_spin2/main_variation_na_view_01_204x400.jpg";


        Wine w3 = new Wine();
        w3.Name = "Bodega Norton Reserve Malbec 2013";
        w3.Ratings = "15";
        w3.UserRatings = "12";
        w3.Price = "$19.99";
        w3.Vintage = "WS: TOP 100";
        w3.imageURL = "http://www.savvyitsol.com/placeholder.jpeg";

        Wine w4 = new Wine();
        w4.Name = "Bodega Norton Reserve Malbec 2013";
        w4.Price = "$19.99";
        w4.Ratings = "15";
        w4.UserRatings = "12";
        w4.Vintage = "WS: TOP 100";
        w4.imageURL = "http://www.savvyitsol.com/placeholder.jpeg";

        Wine w5 = new Wine();
        w5.Name = "Silver Oak Napa Valley Cabernet Sauvignon 2011";
        w5.Price = "$15.99";
        w5.Ratings = "15";
        w5.UserRatings = "12";
        w5.Vintage = "WS: TOP 100";
        w5.imageURL = "http://www.savvyitsol.com/placeholder.jpeg";


        Wine w6 = new Wine();
        w6.Name = "Bodega Norton Reserve Malbec 2013";
        w6.Price = "$19.99";
        w6.Ratings = "15";
        w6.UserRatings = "12";
        w6.Vintage = "WS: TOP 100";
        w6.imageURL = "http://www.savvyitsol.com/placeholder.jpeg";


        Wine w7 = new Wine();
        w7.Name = "Bodega Norton Reserve Malbec 2013";
        w7.Price = "$19.99";
        w7.Ratings = "15";
        w7.UserRatings = "12";
        w7.Vintage = "WS: TOP 100";
        w7.imageURL = "http://www.savvyitsol.com/placeholder.jpeg";

        Wine w8 = new Wine();
        w8.Name = "Bodega Norton Reserve Malbec 2013";
        w8.Price = "$19.99";
        w8.Ratings = "15";
        w8.UserRatings = "12";
        w8.Vintage = "WS: TOP 100";
        w8.imageURL = "http://www.savvyitsol.com/placeholder.jpeg";

        myArr.Add(w1);
        myArr.Add(w2);
        myArr.Add(w3);
        myArr.Add(w4);
        myArr.Add(w5);
        myArr.Add(w6);
        myArr.Add(w7);
        myArr.Add(w8);
        return myArr;
    }
    }
}