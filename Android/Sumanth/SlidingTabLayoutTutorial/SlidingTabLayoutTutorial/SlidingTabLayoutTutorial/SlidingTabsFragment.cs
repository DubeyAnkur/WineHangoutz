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
using Android.Support.V4.View;

namespace SlidingTabLayoutTutorial
{
    public class SlidingTabsFragment : Fragment
    {
        private SlidingTabScrollView mSlidingTabScrollView;
        private ViewPager mViewPager;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            return inflater.Inflate(Resource.Layout.fragment_sample, container, false);
        }

        public override void OnViewCreated(View view, Bundle savedInstanceState)
        {
            mSlidingTabScrollView = view.FindViewById<SlidingTabScrollView>(Resource.Id.sliding_tabs);
            mViewPager = view.FindViewById<ViewPager>(Resource.Id.viewpager);
            mViewPager.Adapter = new SamplePagerAdapter();

            mSlidingTabScrollView.ViewPager = mViewPager;
        }

        public class SamplePagerAdapter : PagerAdapter
        {
            List<string> items = new List<string>();

            public SamplePagerAdapter() : base()
            {
                items.Add("Xamarin");
                items.Add("Android");
                items.Add("Tutorial");

            }

            public override int Count
            {
                get { return items.Count; }
            }

            public override bool IsViewFromObject(View view, Java.Lang.Object obj)
            {
                return view == obj;
            }

            public override Java.Lang.Object InstantiateItem(ViewGroup container, int position)
            {
                View view = LayoutInflater.From(container.Context).Inflate(Resource.Layout.Fragment1Layout, container, false);
                container.AddView(view);

                return view;
            }

            public string GetHeaderTitle(int position)
            {
                return items[position];
            }

            public override void DestroyItem(ViewGroup container, int position, Java.Lang.Object obj)
            {
                container.RemoveView((View)obj);
            }
        }
        class SampleTabFragment : Fragment
        {
            public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
            {
                base.OnCreateView(inflater, container, savedInstanceState);

                /*  var view = inflater.Inflate (Resource.Layout.Tab, container, false);
                  var sampleTextView = view.FindViewById<TextView> (Resource.Id.sampleTextView);             
                  sampleTextView.Text = "sample fragment text";

                  return view;*/
                var view = inflater.Inflate(Resource.Layout.Fragment1Layout, null);
                Button txtName = view.FindViewById<Button>(Resource.Id.btnWall);
                Button txtName1 = view.FindViewById<Button>(Resource.Id.btnPP);
                Button txtName2 = view.FindViewById<Button>(Resource.Id.btnSec);


                var param = txtName.LayoutParameters;
                var param1 = txtName1.LayoutParameters;
                var param2 = txtName2.LayoutParameters;
                param.Height = PixelsToDp(160);
                param1.Height = PixelsToDp(160);
                param2.Height = PixelsToDp(160);
                return view;
            }
            private int PixelsToDp(int pixels)
            {
                return (int)TypedValue.ApplyDimension(ComplexUnitType.Dip, pixels, Resources.DisplayMetrics);
            }
        }
    }
}