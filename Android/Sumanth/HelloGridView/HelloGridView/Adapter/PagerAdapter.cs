using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.Support.V4.App;
using Java.Lang;

//using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace HelloGridView
{
    public class PagerAdapter : FragmentPagerAdapter
    {
        private string[] Titles;

        public PagerAdapter(FragmentManager fm, string[] titles)
            : base(fm)
        {
            Titles = titles;
        }

        public override ICharSequence GetPageTitleFormatted(int position)
        {

            return new Java.Lang.String(Titles[position]);
        }

        public override int Count
        {
            get { return Titles.Length; }
        }

        public override Fragment GetItem(int position)
        {


            switch (position)
            {
                case 0:
                    return new LocationFragment();
                case 1:
                    return new TasteFrag();
                case 2:
                    return new ExploreFrag();
                case 3:
                default:
                    return new LocationFragment();
            }
        }
    }
}