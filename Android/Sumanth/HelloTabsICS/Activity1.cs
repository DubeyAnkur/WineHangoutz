using System;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Util;

namespace HelloTabsICS
{
    [Activity (Label = "WineHangOuts", MainLauncher = true)]
    public class Activity1 : Activity
    {   
        protected override void OnCreate (Bundle bundle)
        {
            base.OnCreate (bundle);
         
            // Set our view from the "main" layout resource
            SetContentView (Resource.Layout.Main);
            
   
            this.ActionBar.NavigationMode = ActionBarNavigationMode.Tabs;
                 
            AddTab ("SHOP", Resource.Drawable.ic_tab_white1, new SampleTabFragment ());
            AddTab ("TASTE", Resource.Drawable.ic_tab_white, new SampleTabFragment2 ());
            AddTab("EXPLORE", Resource.Drawable.ic_tab_white, new SampleTabFragment3());

            if (bundle != null)
                this.ActionBar.SelectTab(this.ActionBar.GetTabAt(bundle.GetInt("tab")));

        }

        protected override void OnSaveInstanceState(Bundle outState)
        {
            outState.PutInt("tab", this.ActionBar.SelectedNavigationIndex);

            base.OnSaveInstanceState(outState);
        }
        
        void AddTab (string tabText, int iconResourceId, Fragment view)
        {
            var tab = this.ActionBar.NewTab ();            
            tab.SetText (tabText);
            
            
            // must set event handler before adding tab
            tab.TabSelected += delegate(object sender, ActionBar.TabEventArgs e)
            {
                var fragment = this.FragmentManager.FindFragmentById(Resource.Id.fragmentContainer);
                if (fragment != null)
                    e.FragmentTransaction.Remove(fragment);         
                e.FragmentTransaction.Add (Resource.Id.fragmentContainer, view);
            };
            tab.TabUnselected += delegate(object sender, ActionBar.TabEventArgs e) {
                e.FragmentTransaction.Remove(view);
            };
            
            this.ActionBar.AddTab (tab);
        }
        
        class SampleTabFragment: Fragment
        {            
            public override View OnCreateView (LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
            {
                base.OnCreateView (inflater, container, savedInstanceState);
                
              /*  var view = inflater.Inflate (Resource.Layout.Tab, container, false);
                var sampleTextView = view.FindViewById<TextView> (Resource.Id.sampleTextView);             
                sampleTextView.Text = "sample fragment text";

                return view;*/
                var view = inflater.Inflate(Resource.Layout.Fragment1Layout, null);
                Button txtName = view.FindViewById<Button>(Resource.Id.button);
                Button txtName1 = view.FindViewById<Button>(Resource.Id.button1);
                Button txtName2 = view.FindViewById<Button>(Resource.Id.button2);


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

        class SampleTabFragment2 : Fragment
        {
            public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
            {
                base.OnCreateView(inflater, container, savedInstanceState);

                /*var view = inflater.Inflate(Resource.Layout.Tab, container, false);
                var sampleTextView = view.FindViewById<TextView>(Resource.Id.sampleTextView);
                sampleTextView.Text = "sample fragment text 2";

                return view;*/
                var view = inflater.Inflate(Resource.Layout.Fragment1Layout2, null);
                Button txtName = view.FindViewById<Button>(Resource.Id.button);
                Button txtName1 = view.FindViewById<Button>(Resource.Id.button1);
                Button txtName2 = view.FindViewById<Button>(Resource.Id.button2);


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
        class SampleTabFragment3 : Fragment
        {
            public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
            {
                base.OnCreateView(inflater, container, savedInstanceState);
                /*
                                var view = inflater.Inflate(Resource.Layout.Tab, container, false);
                                var sampleTextView = view.FindViewById<TextView>(Resource.Id.sampleTextView);
                                sampleTextView.Text = "sample fragment text 2";

                                return view;*/
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
}


