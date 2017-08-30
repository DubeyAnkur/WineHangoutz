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
using System.Diagnostics;
using Android.Content.PM;

namespace WineHangouts
{
    [Activity(Label = "About Us" ,ScreenOrientation = ScreenOrientation.Portrait)]
    public class AboutActivity : Activity
    {
        private int screenid = 11;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            try
            {
                base.OnCreate(savedInstanceState);
                ActionBar.SetHomeButtonEnabled(true);
                ActionBar.SetDisplayHomeAsUpEnabled(true);
                SetContentView(Resource.Layout.About);
               // ImageView imh = FindViewById<ImageView>(Resource.Id.imageView1);
               //// ImageView img = new ImageView(this);
               
               // TextView txt1 = FindViewById<TextView>(Resource.Id.textView1);
               // TextView txt2 = FindViewById<TextView>(Resource.Id.textView2);
               // TextView txt3 = FindViewById<TextView>(Resource.Id.textView3);
               // TextView txt4 = FindViewById<TextView>(Resource.Id.textView4);
               // TextView txt5 = FindViewById<TextView>(Resource.Id.textView5);
               // TextView txt6 = FindViewById<TextView>(Resource.Id.textView6);
               // TextView txt7 = FindViewById<TextView>(Resource.Id.textView7);
               // TextView txt8 = FindViewById<TextView>(Resource.Id.textView8);
               // TextView txt9 = FindViewById<TextView>(Resource.Id.textView9);
               // TextView txt10 = FindViewById<TextView>(Resource.Id.textView10);
               // TextView txt11 = FindViewById<TextView>(Resource.Id.textView11);
               // TextView txt12 = FindViewById<TextView>(Resource.Id.textView12);

               // TextView txt13 = FindViewById<TextView>(Resource.Id.textView13);
               // TextView txt14 = FindViewById<TextView>(Resource.Id.textView14);
               // TextView txt15 = FindViewById<TextView>(Resource.Id.textView15);

               // TextView txt16 = FindViewById<TextView>(Resource.Id.textView16);
               // TextView txt17 = FindViewById<TextView>(Resource.Id.textView17);
               // TextView txt18 = FindViewById<TextView>(Resource.Id.textView18);
               // TextView txt19 = FindViewById<TextView>(Resource.Id.textView19);

                LoggingClass.LogInfo("Entered into About Us", screenid);
            }
            catch(Exception ex)
            { }
        }
        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            if (item.ItemId == Android.Resource.Id.Home)
            {
                Finish();
                LoggingClass.LogInfo("Exited from About Us", screenid);
                return false;
            }
            return base.OnOptionsItemSelected(item);
        }
		//protected override void OnPause()
		//{
		//	base.OnPause();
		//	LoggingClass.LogInfo("OnPause state in About activity", screenid);

		//}
		//protected override void OnResume()
		//{
		//	base.OnResume();
		//	LoggingClass.LogInfo("OnResume state in About activity", screenid);
		//}

	}
}