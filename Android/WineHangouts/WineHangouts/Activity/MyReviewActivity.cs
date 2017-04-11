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
using Android.Util;
using Hangout.Models;
using static Android.Widget.AdapterView;

namespace WineHangouts
{
    [Activity(Label = "My Reviews")]
    public class MyReviewActivity : Activity, IPopupParent
    {
        public int uid;
        Context parent;
        public int x;
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            uid = Convert.ToInt32(CurrentUser.getUserId());
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Tasting);
            try
            {
                ActionBar.SetHomeButtonEnabled(true);
                ActionBar.SetDisplayHomeAsUpEnabled(true);

                ServiceWrapper svc = new ServiceWrapper();
                ItemReviewResponse uidreviews = new ItemReviewResponse();
                // ItemRatingResponse irr = svc.GetItemReviewUID(uid).Result;

                uidreviews = svc.GetItemReviewUID(uid).Result;

                //if (uidreviews.Reviews.Count == 0)
                //{
                //    SetContentView(Resource.Layout.Dummy);
                //}
                List<Review> myArr1;
                myArr1 = uidreviews.Reviews.ToList();
           

                var wineList = FindViewById<ListView>(Resource.Id.listView1);
                // myArr1 = SampleData1();
                Review edit = new Review();
                ReviewPopup editPopup = new ReviewPopup(this, edit);
                MyReviewAdapter adapter = new MyReviewAdapter(this, myArr1);
                //if (adapter.Count == 0)
                //{
                //    TextView infoText = FindViewById<TextView>(Resource.Id.txtInfo);
                //    infoText.Text = "You haven't reviewed anything";
                //}
                //adapter.Edit_Click += editPopup.EditPopup;
             
                wineList.Adapter = adapter;

                // wineList.ItemClick += listView_ItemClick;

                wineList.ItemClick += delegate (object sender, AdapterView.ItemClickEventArgs args)
                {
                    int WineID = myArr1[args.Position].WineId;
                    var intent = new Intent(this, typeof(detailViewActivity));
                    intent.PutExtra("WineID", WineID);
                    StartActivity(intent);
                };
                ProgressIndicator.Hide();
            }
            catch (Exception exe)
            {
                AlertDialog.Builder aler = new AlertDialog.Builder(this);
                aler.SetTitle("Sorry");
                aler.SetMessage("We're under maintainence");
                aler.SetNegativeButton("Ok", delegate { });
                Dialog dialog = aler.Create();
                dialog.Show();
            }
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            if (item.ItemId == Android.Resource.Id.Home)
            {
                Finish();
                return false;
            }
            return base.OnOptionsItemSelected(item);
        }
        private void Close_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private int ConvertPixelsToDp(float pixelValue)
        {
            var dp = (int)((pixelValue) / Resources.DisplayMetrics.Density);
            return dp;
        }

        private int PixelsToDp(int pixels)
        {
            return (int)TypedValue.ApplyDimension(ComplexUnitType.Dip, pixels, Resources.DisplayMetrics);
        }


        public void RefreshParent()
        {
            ServiceWrapper svc = new ServiceWrapper();

            var uidreviews = svc.GetItemReviewUID(uid).Result;
            ListView wineList = FindViewById<ListView>(Resource.Id.listView1);
            Review edit = new Review();
            ReviewPopup editPopup = new ReviewPopup(this, edit);
            MyReviewAdapter adapter = new MyReviewAdapter(this, uidreviews.Reviews.ToList());
            //adapter.Edit_Click += editPopup.EditPopup;

            wineList.Adapter = adapter;
            adapter.NotifyDataSetChanged();
        }

        //void listView_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        //{
        //    //Get our item from the list adapter
        //    int WineID = e.Position;
        //        //var intent = new Intent(this, typeof(detailViewActivity));
        //        //intent.PutExtra("WineID", WineID);
        //        //StartActivity(intent);

        //}

        
    }

}

