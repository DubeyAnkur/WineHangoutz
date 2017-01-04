using System;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Util;
using Android.Graphics;

namespace WineHangouts
{
    [Activity(Label = "WineHangOuts", MainLauncher = true)]
    public class Activity1 : Activity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Fragment);

            this.ActionBar.NavigationMode = ActionBarNavigationMode.Tabs;


            AddTab("Location", Resource.Drawable.ic_tab_white, new SampleTabFragment("Location"));
            AddTab("TASTE", Resource.Drawable.ic_tab_white, new SampleTabFragment("TASTE"));
            AddTab("EXPLORE", Resource.Drawable.ic_tab_white, new SampleTabFragment("EXPLORE"));

            if (bundle != null)
                this.ActionBar.SelectTab(this.ActionBar.GetTabAt(bundle.GetInt("tab")));

        }

        protected override void OnSaveInstanceState(Bundle outState)
        {
            outState.PutInt("tab", this.ActionBar.SelectedNavigationIndex);

            base.OnSaveInstanceState(outState);
        }

        void AddTab(string tabText, int iconResourceId, Fragment view)
        {
            var tab = this.ActionBar.NewTab();
            tab.SetText(tabText);


            // must set event handler before adding tab
            tab.TabSelected += delegate (object sender, ActionBar.TabEventArgs e)
            {
                var fragment = this.FragmentManager.FindFragmentById(Resource.Id.fragmentContainer);
                if (fragment != null)
                    e.FragmentTransaction.Remove(fragment);
                e.FragmentTransaction.Add(Resource.Id.fragmentContainer, view);
            };
            tab.TabUnselected += delegate (object sender, ActionBar.TabEventArgs e) {
                e.FragmentTransaction.Remove(view);
            };
            
            this.ActionBar.AddTab(tab);
        }
        

        class SampleTabFragment : Fragment
        {
            string tabName;
            
            public SampleTabFragment(string Name)
            {
                tabName = Name;
            }
            public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
            {
                base.OnCreateView(inflater, container, savedInstanceState);
                var view = inflater.Inflate(Resource.Layout.Fragment1Layout, null);


                // The result will be null because InJustDecodeBounds == true.


                Button txtName = view.FindViewById<Button>(Resource.Id.button);
                Button txtName1 = view.FindViewById<Button>(Resource.Id.button1);
                Button txtName2 = view.FindViewById<Button>(Resource.Id.button2);
                var metrics = Resources.DisplayMetrics;
                int height = metrics.HeightPixels; // ConvertPixelsToDp(metrics.HeightPixels);
                ////int heightInDp = ConvertPixelsToDp(metrics.HeightPixels);
                height = height - (int)((380 * metrics.Density) / 3);
                height = height / 3;
                txtName1.LayoutParameters.Height = height;
                txtName2.LayoutParameters.Height = height;
                txtName.LayoutParameters.Height = height;


                if (tabName == "Location")
                {
                   
                   
                    txtName.SetBackgroundResource(Resource.Drawable.sfondo_catalogo_vini);
                    txtName.Text = "Wall";
                    txtName1.SetBackgroundResource(Resource.Drawable.sfondo_promozioni);
                    txtName1.Text = "PointPleasent";
                    txtName2.SetBackgroundResource(Resource.Drawable.sfondo_selezioni);
                    txtName2.Text = "Seacucas";
                    //var param = txtName.LayoutParameters;
                    //var param1 = txtName1.LayoutParameters;
                    //var param2 = txtName2.LayoutParameters;
                    //param.Height = PixelsToDp(160);
                    //param1.Height = PixelsToDp(160);
                    //param2.Height = PixelsToDp(160);
                    txtName.Click += (sender, e) =>
                    {
                        var intent = new Intent(Activity, typeof(MainActivity));
                        intent.PutExtra("MyData", "Wall Store");
                        StartActivity(intent);
                    };
                    txtName1.Click += (sender, e) =>
                    {
                        var intent = new Intent(Activity, typeof(MainActivity));
                        intent.PutExtra("MyData", "Point Pleasent Store");
                        StartActivity(intent);
                    };
                    txtName2.Click += (sender, e) =>
                    {
                        var intent = new Intent(Activity, typeof(MainActivity));
                        intent.PutExtra("MyData", "Seacucas Store");
                        StartActivity(intent);
                    };
                }
                if (tabName == "TASTE")
                {
                  
                    txtName.SetBackgroundResource(Resource.Drawable.sfondo_mierecensioni);
                    txtName.Text = "My Tastings";
                    txtName1.SetBackgroundResource(Resource.Drawable.sfondo_nuoverecensioni);
                    txtName1.Text = "New Tastings";
                    txtName2.SetBackgroundResource(Resource.Drawable.sfondo_topvini);
                    txtName2.Text = "Top Wines";
                    //var param = txtName.LayoutParameters;
                    //var param1 = txtName1.LayoutParameters;
                    //var param2 = txtName2.LayoutParameters;
                    //param.Height = PixelsToDp(160);
                    //param1.Height = PixelsToDp(160);
                    //param2.Height = PixelsToDp(160);
                    txtName.Click += (sender, e) =>
                    {
                        var intent = new Intent(Activity, typeof(TastingActivity));
                       
                        StartActivity(intent);
                    };
                    txtName1.Click += (sender, e) =>
                    {
                        var intent = new Intent(Activity, typeof(TastingActivity));
                       
                        StartActivity(intent);
                    };
                    txtName2.Click += (sender, e) =>
                    {
                        var intent = new Intent(Activity, typeof(TastingActivity));
                       
                        StartActivity(intent);
                    };
                 
                    //};
                }
                if (tabName == "EXPLORE")
                {
                  
                    txtName.SetBackgroundResource(Resource.Drawable.sfondo_blog);
                    txtName.Text = "Blog";
                    //txtName.SetTextColor(Color.Red);

                    txtName1.SetBackgroundResource(Resource.Drawable.sfondo_cantine);
                    txtName1.Text = "Wineries";
                    txtName2.SetBackgroundResource(Resource.Drawable.sfondo_regioni);
                    txtName2.Text = "Regions";

                    //var param = txtName.LayoutParameters;
                    //var param1 = txtName1.LayoutParameters;
                    //var param2 = txtName2.LayoutParameters;
                    //param.Height = PixelsToDp(160);
                    //param1.Height = PixelsToDp(160);
                    //param2.Height = PixelsToDp(160);
                    txtName.Click += (sender, e) =>
                    {
                        var intent = new Intent(Activity, typeof(TastingActivity));
                        StartActivity(intent);
                    };
                    txtName1.Click += (sender, e) =>
                    {
                        var intent = new Intent(Activity, typeof(TastingActivity));
                        StartActivity(intent);
                    };
                    txtName2.Click += (sender, e) =>
                    {
                        var intent = new Intent(Activity, typeof(TastingActivity));
                        StartActivity(intent);
                    };
                }

                return view;
                /*  var view = inflater.Inflate (Resource.Layout.Tab, container, false);
                  var sampleTextView = view.FindViewById<TextView> (Resource.Id.sampleTextView);             
                  sampleTextView.Text = "sample fragment text";

                  return view;*/

            }
            private int PixelsToDp(int pixels)
            {
                return (int)TypedValue.ApplyDimension(ComplexUnitType.Dip, pixels, Resources.DisplayMetrics);
            }
        

            
        }
    

        //class SampleTabFragment2 : Fragment
        //{
        //    public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        //    {
        //        base.OnCreateView(inflater, container, savedInstanceState);

        //        /*var view = inflater.Inflate(Resource.Layout.Tab, container, false);
        //        var sampleTextView = view.FindViewById<TextView>(Resource.Id.sampleTextView);
        //        sampleTextView.Text = "sample fragment text 2";

        //        return view;*/
        //        var view = inflater.Inflate(Resource.Layout.Fragment1Layout2, null);
        //        Button txtName = view.FindViewById<Button>(Resource.Id.button);
        //        Button txtName1 = view.FindViewById<Button>(Resource.Id.button1);
        //        Button txtName2 = view.FindViewById<Button>(Resource.Id.button2);


        //        var param = txtName.LayoutParameters;
        //        var param1 = txtName1.LayoutParameters;
        //        var param2 = txtName2.LayoutParameters;
        //        param.Height = PixelsToDp(160);
        //        param1.Height = PixelsToDp(160);
        //        param2.Height = PixelsToDp(160);
        //        return view;
        //    }
        //    private int PixelsToDp(int pixels)
        //    {
        //        return (int)TypedValue.ApplyDimension(ComplexUnitType.Dip, pixels, Resources.DisplayMetrics);
        //    }
        //}
        //class SampleTabFragment3 : Fragment
        //{
        //    public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        //    {
        //        base.OnCreateView(inflater, container, savedInstanceState);
        //        /*
        //                        var view = inflater.Inflate(Resource.Layout.Tab, container, false);
        //                        var sampleTextView = view.FindViewById<TextView>(Resource.Id.sampleTextView);
        //                        sampleTextView.Text = "sample fragment text 2";

        //                        return view;*/
        //        var view = inflater.Inflate(Resource.Layout.Fragment1Layout3, null);
        //        Button txtName = view.FindViewById<Button>(Resource.Id.button);
        //        Button txtName1 = view.FindViewById<Button>(Resource.Id.button1);
        //        Button txtName2 = view.FindViewById<Button>(Resource.Id.button2);


        //        var param = txtName.LayoutParameters;
        //        var param1 = txtName1.LayoutParameters;
        //        var param2 = txtName2.LayoutParameters;
        //        param.Height = PixelsToDp(160);
        //        param1.Height = PixelsToDp(160);
        //        param2.Height = PixelsToDp(160);

        //        //txtName.wei = 100;

        //        return view;
        //    }
        //    private int PixelsToDp(int pixels)
        //    {
        //        return (int)TypedValue.ApplyDimension(ComplexUnitType.Dip, pixels, Resources.DisplayMetrics);
        //    }

        //}
    }
}


