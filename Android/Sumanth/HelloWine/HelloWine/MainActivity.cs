using Android.App;
using Android.Widget;
using Android.OS;
using com.refractored;
using Android.Support.V4.View;
using Android.Util;
using Android.Support.V7.App;

namespace HelloWine
{
    [Activity(Label = "HwlloWine", MainLauncher = true, Icon = "@drawable/icon", Theme = "@style/Theme.AppCompat.Light")]
    public class MainActivity : AppCompatActivity
    {
        // int count = 1;
        PagerSlidingTabStrip _tabs;
        ViewPager _pager;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.Main);

            //Android.Support.V7.Widget.Toolbar myToolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.my_toolbar);
            //SetSupportActionBar(myToolbar);

            _pager = FindViewById<ViewPager>(Resource.Id.pager);
            _tabs = FindViewById<PagerSlidingTabStrip>(Resource.Id.tabs);

            _pager.PageMargin = (int)TypedValue.ApplyDimension(ComplexUnitType.Dip, 5, Resources.DisplayMetrics);
            _pager.CurrentItem = 0;

            string[] titles = new string[]
                {
                    "SHOP",
                    "TASTE",
                    "EXPLORE"
                };

            var adapter = new PagerAdapter(SupportFragmentManager, titles);
            _pager.Adapter = adapter;
            _pager.OffscreenPageLimit = titles.Length;

            _tabs.SetViewPager(_pager);
        }
    }
}


