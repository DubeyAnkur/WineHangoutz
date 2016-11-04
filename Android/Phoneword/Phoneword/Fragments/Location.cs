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


namespace Phoneword
{
    class LocationFragment : Fragment
    {
        public override View OnCreateView(LayoutInflater inflater,
            ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);

            var view = inflater.Inflate(
                Resource.Layout.Tab, container, false);

            var sampleTextView =
                view.FindViewById<TextView>(Resource.Id.sampleTextView);
            sampleTextView.Text = "sample fragment text";


            return view;
        }
    }
}