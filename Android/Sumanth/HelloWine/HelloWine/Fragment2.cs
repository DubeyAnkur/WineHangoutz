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
    public class Fragment2 : Android.Support.V4.App.Fragment
    {
        static readonly List<string> storeName = new List<string>();
        Button button21, button22, button23;
        void StartNewActivity(object sender, EventArgs e)
        {
            Intent intent = new Intent(this.Activity, typeof(GridLayout));
            storeName.Add("MyTastings");
            storeName.Add("Point Pleasent");
            storeName.Add("SEACACUS");
            intent.PutStringArrayListExtra("storeName", storeName);

            StartActivity(intent);
        }
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);

            var view = inflater.Inflate(Resource.Layout.LayoutFragment1, null);
            button21 = view.FindViewById<Button>(Resource.Id.button);
            var param21 = button21.LayoutParameters;
            param21.Height = PixelsToDp(160);
            button21.Click += StartNewActivity;
            button22 = view.FindViewById<Button>(Resource.Id.button1);           
            button22.Click += StartNewActivity;
            button23 = view.FindViewById<Button>(Resource.Id.button2);
            button23.Click += StartNewActivity;
            return view;
            
        }
        private int PixelsToDp(int pixels)
        {
            return (int)TypedValue.ApplyDimension(ComplexUnitType.Dip, pixels, Resources.DisplayMetrics);
        }
    }
}