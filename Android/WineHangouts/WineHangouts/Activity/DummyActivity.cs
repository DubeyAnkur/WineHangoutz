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
using Hangout.Models;

namespace WineHangouts
{
    [Activity(Label = "Testing App", MainLauncher =false, Icon = "@drawable/update")]
    public class DummyActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            
            //RelativeLayout ll1 = FindViewById<RelativeLayout>(Resource.Id.linearLayout_gridtableLayout);
            //ll1.LayoutParameters = new RelativeLayout.LayoutParams(1500, 1500);
            //EditText ed = FindViewById<EditText>(Resource.Id.edittext);
            //ed.Text = "lokesh";

            //ActionBar.NavigationMode = ActionBarNavigationMode.Tabs;
            //ActionBar.Tab tab = ActionBar.NewTab();

            //tab.SetIcon(Resource.Drawable.user);
            //tab.TabSelected += (sender, args) =>
            //{
            //    // Do something when tab is selected
            //};
            //ActionBar.AddTab(tab);

            //tab = ActionBar.NewTab();
            ////tab.SetText(Resources.GetString(Resource.String.tab2_text));
            //tab.SetIcon(Resource.Drawable.user);
            //tab.TabSelected += (sender, args) =>
            //{
            //    // Do something when tab is selected
            //};
            //ActionBar.AddTab(tab);
            ////TableRow tr= FindViewById<TableRow>(Resource.Id.tableRow1);
            ////tr.SetBackgroundColor(Android.Graphics.Color.Aqua);
            ////Button bn = new Button(this);
            ////var metrics = Resources.DisplayMetrics;
            ////var param = bn.LayoutParameters;
            ////param.Width = 320;
            //////var heightInDp = ConvertPixelsToDp(metrics.HeightPixels);
            //////bn.LayoutParameters = new LinearLayout.LayoutParams(widthInDp, widthInDp);

            ////bn.SetText("Hellojsjdfff".ToArray(), 2,3);
            ////tr.AddView(bn);
            //// Create your application here
        }
    }
}