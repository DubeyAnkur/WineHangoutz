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

namespace HelloGridView
{
    [Activity(Label = "TasteFrag")]
    public class TasteFrag : Android.Support.V4.App.Fragment
    {
        void StartNewActivity(object sender, EventArgs e)
        {
            Intent intent = new Intent(this.Activity, typeof(MainActivity));


            StartActivity(intent);
        }
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);
            View view = inflater.Inflate(Resource.Layout.TasteFragment, container, false);
            Button _button = view.FindViewById<Button>(Resource.Id.button);
            var param = _button.LayoutParameters;
            param.Height = PixelsToDp(160);
            _button.Click += StartNewActivity;
            return view;




           
        }
        private int PixelsToDp(int pixels)
        {
            return (int)TypedValue.ApplyDimension(ComplexUnitType.Dip, pixels, Resources.DisplayMetrics);
        }
    }
}