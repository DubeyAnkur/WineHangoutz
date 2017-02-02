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
    [Activity(Label = "WineHangouts", MainLauncher = false)]
    public class TabActivity : Activity
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

            public SampleTabFragment()
            {
                tabName = "Location";
            }
            public SampleTabFragment(string Name)
            {
                tabName = Name;
            }
            public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
            {
                base.OnCreateView(inflater, container, savedInstanceState);
                var view = inflater.Inflate(Resource.Layout.LocationLayout, null);


                // The result will be null because InJustDecodeBounds == true.


                Button Top = view.FindViewById<Button>(Resource.Id.btnTop);
                Button Middle = view.FindViewById<Button>(Resource.Id.btnMiddle);
                Button Bottom = view.FindViewById<Button>(Resource.Id.btnBottom);
                var metrics = Resources.DisplayMetrics;
                int height = metrics.HeightPixels; // ConvertPixelsToDp(metrics.HeightPixels);
                ////int heightInDp = ConvertPixelsToDp(metrics.HeightPixels);
                height = height - (int)((380 * metrics.Density) / 3);
                height = height / 3;
                Top.LayoutParameters.Height = height;
                Middle.LayoutParameters.Height = height;
                Bottom.LayoutParameters.Height = height;


                if (tabName == "Location")
                {
                   
                   
                    Top.SetBackgroundResource(Resource.Drawable.city);
                    Top.Text = "Wall";
                    Top.SetTextColor(Color.White);
                    Top.TextSize = 25;
                    Middle.SetBackgroundResource(Resource.Drawable.beach );
                    Middle.Text = "Point Pleasent";
                    Middle.SetTextColor(Color.White);
                    Middle.TextSize = 25;
                    Bottom.SetBackgroundResource(Resource.Drawable.city1);
                    Bottom.Text = "Seacucas";
                    Bottom.SetTextColor(Color.White);
                    Bottom.TextSize = 25;
                    //var param = Top.LayoutParameters;
                    //var param1 = Middle.LayoutParameters;
                    //var param2 = Bottom.LayoutParameters;
                    //param.Height = PixelsToDp(160);
                    //param1.Height = PixelsToDp(160);
                    //param2.Height = PixelsToDp(160);
                    Top.Click += (sender, e) =>
                    {
                        var intent = new Intent(Activity, typeof(MainActivity));
                        intent.PutExtra("MyData", "Wall Store");
                        StartActivity(intent);
                    };
                    Middle.Click += (sender, e) =>
                    {
                        var intent = new Intent(Activity, typeof(MainActivity));
                        intent.PutExtra("MyData", "Point Pleasent Store");
                        StartActivity(intent);
                    };
                    Bottom.Click += (sender, e) =>
                    {
                        // var intent = new Intent(Activity, typeof(MainActivity));
                        //intent.PutExtra("MyData", "Seacucas Store");
                        // StartActivity(intent);
                        AlertDialog.Builder aler = new AlertDialog.Builder(Activity);
                        aler.SetTitle("Secacus Store");
                        aler.SetMessage("Coming Soon");
                        aler.SetNegativeButton("Ok", delegate { });
                        Dialog dialog = aler.Create();
                        dialog.Show();
                    };
                }
                if (tabName == "TASTE")
                {
                  
                    Top.SetBackgroundResource(Resource.Drawable.winereviews);
                    Top.Text = "My Reviews";
                    Top.SetTextColor(Color.White);
                    Top.TextSize = 25;
                    Middle.SetBackgroundResource(Resource.Drawable.winetasting);
                    Middle.Text = "My Tastings";
                    Middle.SetTextColor(Color.White);
                    Middle.TextSize = 25;
                    Bottom.SetBackgroundResource(Resource.Drawable.myfavorate);
                    Bottom.Text = "My Favorite";
                    Bottom.SetTextColor(Color.White);
                    Bottom.TextSize = 25;
                    //var param = Top.LayoutParameters;
                    //var param1 = Middle.LayoutParameters;
                    //var param2 = Bottom.LayoutParameters;
                    //param.Height = PixelsToDp(160);
                    //param1.Height = PixelsToDp(160);
                    //param2.Height = PixelsToDp(160);
                    Top.Click += (sender, e) =>
                    {
                        var intent = new Intent(Activity, typeof(TastingActivity));
                        intent.PutExtra("MyData", "My Reviews");
                        StartActivity(intent);
                    };
                    Middle.Click += (sender, e) =>
                    {
                        var intent = new Intent(Activity, typeof(MyTastingActivity));
                        intent.PutExtra("MyData", "My Tastings");
                        StartActivity(intent);
                    };
                    Bottom.Click += (sender, e) =>
                    {
                        var intent = new Intent(Activity, typeof(MyFavoriteAvtivity));
                        intent.PutExtra("MyData", "My Favorite");
                        StartActivity(intent);
                    };

                    //};
                }
                if (tabName == "EXPLORE")
                {
                  
                    Top.SetBackgroundResource(Resource.Drawable.myprofile);
                    Top.Text = "My Profile";
                    Top.SetTextColor(Color.White);
                    Top.TextSize = 25;
                    //Top.SetTextColor(Color.Red);

                    Middle.SetBackgroundResource(Resource.Drawable.sfondo_cantine);
                    Middle.Text = "Wineries";
                    Middle.SetTextColor(Color.White);
                    Middle.TextSize = 25;
                    Bottom.SetBackgroundResource(Resource.Drawable.sfondo_regioni);
                    Bottom.Text = "Regions";
                    Bottom.SetTextColor(Color.White);
                    Bottom.TextSize = 25;

                    //var param = Top.LayoutParameters;
                    //var param1 = Middle.LayoutParameters;
                    //var param2 = Bottom.LayoutParameters;
                    //param.Height = PixelsToDp(160);
                    //param1.Height = PixelsToDp(160);
                    //param2.Height = PixelsToDp(160);
                    Top.Click += (sender, e) =>
                    {
                        var intent = new Intent(Activity, typeof(ProfileActivity));
                        intent.PutExtra("MyData", "My Profile");
                        StartActivity(intent);
                    };
                    Middle.Click += (sender, e) =>
                    {
                        var intent = new Intent(Activity, typeof(TastingActivity));
                        intent.PutExtra("MyData", "Wineries");
                        StartActivity(intent);
                    };
                    Bottom.Click += (sender, e) =>
                    {
                        var intent = new Intent(Activity, typeof(PotraitActivity));
                        intent.PutExtra("MyData", "Regions");
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
        public override void OnBackPressed()
        {
            MoveTaskToBack(true);
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
            //        Button Top = view.FindViewById<Button>(Resource.Id.button);
            //        Button Middle = view.FindViewById<Button>(Resource.Id.button1);
            //        Button Bottom = view.FindViewById<Button>(Resource.Id.button2);


            //        var param = Top.LayoutParameters;
            //        var param1 = Middle.LayoutParameters;
            //        var param2 = Bottom.LayoutParameters;
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
            //        Button Top = view.FindViewById<Button>(Resource.Id.button);
            //        Button Middle = view.FindViewById<Button>(Resource.Id.button1);
            //        Button Bottom = view.FindViewById<Button>(Resource.Id.button2);


            //        var param = Top.LayoutParameters;
            //        var param1 = Middle.LayoutParameters;
            //        var param2 = Bottom.LayoutParameters;
            //        param.Height = PixelsToDp(160);
            //        param1.Height = PixelsToDp(160);
            //        param2.Height = PixelsToDp(160);

            //        //Top.wei = 100;

            //        return view;
            //    }
            //    private int PixelsToDp(int pixels)
            //    {
            //        return (int)TypedValue.ApplyDimension(ComplexUnitType.Dip, pixels, Resources.DisplayMetrics);
            //    }

            //}
        }

}


