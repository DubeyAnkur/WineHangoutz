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
using Android.Util;

namespace HelloWine
{
    public class Fragment1 : Android.Support.V4.App.Fragment
    {
        static readonly List<string> storeName = new List<string>();
        Button button11, button12, button13;
        void StartNewActivity(object sender, EventArgs e)
        {
            Intent intent = new Intent(this.Activity, typeof(GridViewActivity));
            storeName.Add("MyTastings");
            storeName.Add("Point Pleasent");
            storeName.Add("SEACACUS");
            intent.PutStringArrayListExtra("storeName", storeName);

            StartActivity(intent);
        }
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);

            var view = inflater.Inflate(Resource.Layout.LayoutFragment, null);
            button11 = view.FindViewById<Button>(Resource.Id.btnWall);
            var param1 = button11.LayoutParameters;
            param1.Height = PixelsToDp(160);
            button11.Click += StartNewActivity;
            button12 = view.FindViewById<Button>(Resource.Id.btnPP);
      
            button12.Click += StartNewActivity;
            button13= view.FindViewById<Button>(Resource.Id.btnSec);
            
            button13.Click += StartNewActivity;
            return view;
        }
        private int PixelsToDp(int pixels)
        {
            return (int)TypedValue.ApplyDimension(ComplexUnitType.Dip, pixels, Resources.DisplayMetrics);
        }
    }
}