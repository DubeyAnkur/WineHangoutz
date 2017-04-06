using System;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Util;
using Android.Graphics;
using Android.Graphics.Drawables;
using System.Threading;
using System.Threading.Tasks;

namespace WineHangouts
{
    [Activity(Label = "@string/ApplicationName", MainLauncher = false, Theme = "@style/Base.Widget.Design.TabLayout")]
    public class TabActivity : Activity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            this.TitleColor = Color.LightGray;


            SetContentView(Resource.Layout.Fragment);


            this.ActionBar.NavigationMode = ActionBarNavigationMode.Tabs;
           

            AddTab("Location", 1, new SampleTabFragment("Location", this));
            AddTab("My Hangouts",1, new SampleTabFragment("My Hangouts", this));
            AddTab("Explore",1, new SampleTabFragment("Explore", this));

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



            tab.TabSelected += delegate (object sender, ActionBar.TabEventArgs e)
            {
                var fragment = this.FragmentManager.FindFragmentById(Resource.Id.fragmentContainer);
                if (fragment != null)
                    e.FragmentTransaction.Remove(fragment);
                e.FragmentTransaction.Add(Resource.Id.fragmentContainer, view);
            };
            tab.TabUnselected += delegate (object sender, ActionBar.TabEventArgs e)
            {
                e.FragmentTransaction.Remove(view);
            };

            this.ActionBar.AddTab(tab);
        }


      public  class SampleTabFragment : Fragment
        {

            string tabName;
            Activity _parent;
            ProgressDialog progress;
            public SampleTabFragment()
            {
                tabName = "Location";
            }
            public SampleTabFragment(string Name, Activity parent)
            {
                tabName = Name;
                _parent = parent;
                // progress = new Android.App.ProgressDialog(_parent);

            }

            public override void OnViewCreated(View view, Bundle savedInstanceState)
            {
                
            }
            public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
            {
                base.OnCreateView(inflater, container, savedInstanceState);
                var view = inflater.Inflate(Resource.Layout.LocationLayout, null);
                Button Top = view.FindViewById<Button>(Resource.Id.btnTop);
                Button Middle = view.FindViewById<Button>(Resource.Id.btnMiddle);
                Button Bottom = view.FindViewById<Button>(Resource.Id.btnBottom);
                var metrics = Resources.DisplayMetrics;
                int height = metrics.HeightPixels;
                height = height - (int)((360 * metrics.Density) / 3);
                height = height / 3;
                Top.LayoutParameters.Height = height;
                Middle.LayoutParameters.Height = height;
                Bottom.LayoutParameters.Height = height;


                if (tabName == "Location")
                {


                    Top.SetBackgroundResource(Resource.Drawable.city);
                    Top.Text = "Wall";
                    Top.SetTextColor(Color.White);
                    Top.TextSize = 20;
                    Middle.SetBackgroundResource(Resource.Drawable.beach);
                    Middle.Text = "Point Pleasant";
                    Middle.SetTextColor(Color.White);
                    Middle.TextSize = 20;
                    Bottom.SetBackgroundResource(Resource.Drawable.city1);
                    Bottom.Text = "Seacucas";
                    Bottom.SetTextColor(Color.White);
                    Bottom.TextSize = 20;

                    

                    Top.Click += (sender, e) =>
                    {
                        ProgressIndicator.Show(_parent);

                        var intent = new Intent(Activity, typeof(GridViewActivity));
                        intent.PutExtra("MyData", "Wall Store");
                        
                        StartActivity(intent);

                    };
                    Middle.Click += (sender, e) =>
                    {
                        ProgressIndicator.Show(_parent);

                        var intent = new Intent(Activity, typeof(GridViewActivity));
                        intent.PutExtra("MyData", "Point Pleasant Store");
                        StartActivity(intent);
                    };
                    Bottom.Click += (sender, e) =>
                    {
                        AlertDialog.Builder aler = new AlertDialog.Builder(Activity);
                        aler.SetTitle("Secacus Store");
                        aler.SetMessage("Coming Soon");
                        aler.SetNegativeButton("Ok", delegate { });
                        Dialog dialog = aler.Create();
                        dialog.Show();
                    };
                }
                if (tabName == "My Hangouts")
                {

                    Top.SetBackgroundResource(Resource.Drawable.winereviews);
                    Top.Text = "My Reviews";
                    Top.SetTextColor(Color.White);
                    Top.TextSize = 20;
                    Middle.SetBackgroundResource(Resource.Drawable.winetasting);
                    Middle.Text = "My Tastings";
                    Middle.SetTextColor(Color.White);
                    Middle.TextSize = 20;
                    Bottom.SetBackgroundResource(Resource.Drawable.myfavorate);
                    Bottom.Text = "My Favorites";
                    Bottom.SetTextColor(Color.White);
                    Bottom.TextSize = 20;

                    Top.Click += (sender, e) =>
                    {
                        ProgressIndicator.Show(_parent);
                        var intent = new Intent(Activity, typeof(MyReviewActivity));
                        intent.PutExtra("MyData", "My Reviews");
                        StartActivity(intent);
                    };
                    Middle.Click += (sender, e) =>
                    {
                        ProgressIndicator.Show(_parent);
                        var intent = new Intent(Activity, typeof(MyTastingActivity));
                        intent.PutExtra("MyData", "My Tastings");
                        StartActivity(intent);
                    };
                    Bottom.Click += (sender, e) =>
                    {
                        ProgressIndicator.Show(_parent);
                        var intent = new Intent(Activity, typeof(MyFavoriteAvtivity));
                        intent.PutExtra("MyData", "My Favorites");
                        StartActivity(intent);
                    };

                    //};
                }
                if (tabName == "Explore")
                {
                   
                    Top.SetBackgroundResource(Resource.Drawable.myprofile);
                    Top.Text = "My Profile";
                    
                    Top.SetTextColor(Color.White);
                    Top.TextSize = 20;


                    Middle.SetBackgroundResource(Resource.Drawable.sfondo_cantine);
                    Middle.Text = "Wineries/Search Helper";
                    Middle.TextSize = 20;
                    Middle.SetTextColor(Color.White);


                    Bottom.SetBackgroundResource(Resource.Drawable.sfondo_regioni);
                    Bottom.Text = "Regions";
                    Bottom.TextSize = 20;
                    Bottom.SetTextColor(Color.White);
                    Bottom.SetTextAppearance(Resource.Drawable.abc_btn_borderless_material);

                    Top.Click += (sender, e) =>
                    {
                        ProgressIndicator.Show(_parent);

                        var intent = new Intent(Activity, typeof(ProfileActivity));
                        intent.PutExtra("MyData", "My Profile");
                        StartActivity(intent);



                    };
                    Middle.Click += (sender, e) =>
                    {
                        //AlertDialog.Builder aler = new AlertDialog.Builder(Activity);
                        //aler.SetTitle("Wineries Section");
                        //aler.SetMessage("Coming Soon");
                        //aler.SetNegativeButton("Ok", delegate { });
                        //Dialog dialog = aler.Create();
                        //dialog.Show();
                        var intent = new Intent(Activity, typeof(LandscapeActivity));
                        intent.PutExtra("MyData", "Wineries");
                        StartActivity(intent);
                        //var intent = new Intent(Activity, typeof(AutoCompleteTextActivity));
                        ////intent.PutExtra("MyData", "Wineries");
                        //StartActivity(intent);
                    };
                    Bottom.Click += (sender, e) =>
                    {
                        AlertDialog.Builder aler = new AlertDialog.Builder(Activity);
                        aler.SetTitle("Regions Section");
                        aler.SetMessage("Coming Soon");
                        aler.SetNegativeButton("Ok", delegate { });
                        Dialog dialog = aler.Create();
                        dialog.Show();
                        //var intent = new Intent(Activity, typeof(PotraitActivity));
                        //intent.PutExtra("MyData", "Regions");
                        //StartActivity(intent);
                    };
                }

                return view;


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

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
        

            switch (item.ItemId)
            {
                case Resource.Id.action_settings:
                    ProgressIndicator.Show(this);
                    var intent = new Intent(this, typeof(ProfileActivity));
                    intent.PutExtra("MyData", "Wall Store");

                    StartActivity(intent);
                    break;
                case Resource.Id.action_settings1:
                    ProgressIndicator.Show(this);
                    var intent1 = new Intent(this, typeof(AboutActivity));
                    intent1.PutExtra("MyData", "Wall Store");

                    StartActivity(intent1);
                    break;

                case Resource.Id.action_settings2:
                    MoveTaskToBack(true);
                    break;
            }
            try
            {
                if (item.ItemId == Resource.Id.action_settings3)
                {
                    ProgressIndicator.Show(this);
                    StartActivity(typeof(AutoCompleteTextActivity));
                }
            }

            catch (Exception ex)
            {
                throw new Exception();
            }
           
            if (item.ItemId == Android.Resource.Id.Home)
            {
                Finish();
                return false;
            }
            return base.OnOptionsItemSelected(item);
        }
        public override bool OnCreateOptionsMenu(IMenu menu)
        {


            MenuInflater.Inflate(Resource.Drawable.options_menu, menu);

            return true;
        }
    }

}


