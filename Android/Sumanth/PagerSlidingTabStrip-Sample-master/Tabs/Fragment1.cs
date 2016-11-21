
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;

namespace Tabs
{
    public class Fragment1 : Android.Support.V4.App.Fragment
    {
        static readonly List<string> storeName = new List<string>();
        Button _button, _button1, _button2;
        void StartNewActivity(object sender, EventArgs e)
        {
            Intent intent = new Intent(this.Activity, typeof(GridViewActivity));
            storeName.Add("Wall");
            storeName.Add("Point Pleasent");
                intent.PutStringArrayListExtra("storeName", storeName);
        
            StartActivity(intent);


            //Intent intent1 = new Intent(this.Activity, typeof(EnoListing));
            //storeName.Add("Point Pleasant");
            //intent.PutStringArrayListExtra("storeName", storeName);

            //StartActivity(intent1);

            //Intent intent2 = new Intent(this.Activity, typeof(EnoListing));
            //storeName.Add("Secaucas");
            //intent.PutStringArrayListExtra("storeName", storeName);

            //StartActivity(intent2);

        }


        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);

          //  var view = inflater.Inflate(Resource.Layout.Fragment1Layout, null);
            View view = inflater.Inflate(Resource.Layout.Fragment1Layout, container, false);
            _button = view.FindViewById<Button>(Resource.Id.btnWall);
            var param =_button.LayoutParameters;
            param.Height = PixelsToDp(160);
            _button.Click += StartNewActivity;
            return view;
          // View view1 = inflater.Inflate(Resource.Layout.Fragment1Layout, container, false);
            _button1 = view.FindViewById<Button>(Resource.Id.btnPP);
           // var param1 = _button.LayoutParameters;
           // param1.Height = PixelsToDp(160);
            _button1.Click += StartNewActivity;
            return view;
             _button2 = view.FindViewById<Button>(Resource.Id.btnSec);
            _button2.Click += StartNewActivity;
            return view;
            //View view2 = inflater.Inflate(Resource.Layout.Fragment1Layout, container, false);
            // Button _button2 = view.FindViewById<Button>(Resource.Id.btnSec);
            //var param2 = _button.LayoutParameters;
            //param2.Height = PixelsToDp(160);
            //_button2.Click += StartNewActivity;
            //return view;

            ////Button btnWall = view.FindViewById<Button>(Resource.Id.btnWall);
            ////Button btnPP = view.FindViewById<Button>(Resource.Id.btnPP);
            ////Button btnSec = view.FindViewById<Button>(Resource.Id.btnSec);


            ////var param = btnWall.LayoutParameters;
            ////var param1 = btnPP.LayoutParameters;
            ////var param2 = btnSec.LayoutParameters;
            ////param.Height = PixelsToDp(160);
            ////param1.Height = PixelsToDp(160);
            ////param2.Height = PixelsToDp(160);
            ////return view;
            ////btnWall.Click += (sender, e) =>

            ////    {
            ////        var intent = new Intent(this.Activity, typeof(EnoListing));
            ////        storeName.Add("Wall");
            ////        intent.PutStringArrayListExtra("storeName", storeName);
            ////        StartActivity(intent);
            ////    };
            ////btnPP.Click += (sender, e) =>
            ////{
            ////    var intent = new Intent(this.Activity, typeof(EnoListing));
            ////    storeName.Add("Secaucas");
            ////    intent.PutStringArrayListExtra("storeName", storeName);
            ////    StartActivity(intent);
            ////};
            ////btnPP.Click += (sender, e) =>
            //// {
            ////     var intent = new Intent(this.Activity, typeof(EnoListing));
            ////     storeName.Add("Point Pleasant");
            ////     intent.PutStringArrayListExtra("storeName", storeName);
            ////     StartActivity(intent);
            //// };
        }
        private int PixelsToDp(int pixels)
        {
            return (int)TypedValue.ApplyDimension(ComplexUnitType.Dip, pixels, Resources.DisplayMetrics);
        }
    }
}
