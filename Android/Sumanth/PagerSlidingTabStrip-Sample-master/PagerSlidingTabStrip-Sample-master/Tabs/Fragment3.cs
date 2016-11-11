
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
    public class Fragment3 : Android.Support.V4.App.Fragment
    {
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);

        
  var view = inflater.Inflate(Resource.Layout.Fragment1Layout3, null);
            Button txtName = view.FindViewById<Button>(Resource.Id.button);
            Button txtName1 = view.FindViewById<Button>(Resource.Id.button1);
            Button txtName2 = view.FindViewById<Button>(Resource.Id.button2);


            var param = txtName.LayoutParameters;
            var param1 = txtName1.LayoutParameters;
            var param2 = txtName2.LayoutParameters;
            param.Height = PixelsToDp(160);
            param1.Height = PixelsToDp(160);
            param2.Height = PixelsToDp(160);

            //txtName.wei = 100;

            return view;
        }
        private int PixelsToDp(int pixels)
        {
            return (int)TypedValue.ApplyDimension(ComplexUnitType.Dip, pixels, Resources.DisplayMetrics);
        }
    }
}