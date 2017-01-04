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

namespace WineHangouts
{
   
    class EditReview : DialogFragment
    {


        //RatingBar userRatingBar;
        //EditText txtReviewComments;

        //RatingBar userRatingBar;
        //EditText txtReviewComments;
        public Dialog myDialog;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);
            var view = inflater.Inflate(Resource.Layout.EditReviewPopup, container, false);


            //userRatingBar = view.FindViewById<RatingBar>(Resource.Id.rtbProductRating);
            //txtReviewComments = view.FindViewById<EditText>(Resource.Id.txtReviewComments);
            //Button btnSubmitReview = view.FindViewById<Button>(Resource.Id.btnSubmitReview);

            //userRatingBar = view.FindViewById<RatingBar>(Resource.Id.rtbProductRating);
            //txtReviewComments = view.FindViewById<EditText>(Resource.Id.txtReviewComments);
            Button btnSubmitReview = view.FindViewById<Button>(Resource.Id.btnSubmitReview);
            ImageButton ibs = view.FindViewById<ImageButton>(Resource.Id.imageButton1);
            ImageButton close = view.FindViewById<ImageButton>(Resource.Id.imageButton2);
            ibs.SetImageResource(Resource.Drawable.wine_review);
            ibs.SetScaleType(ImageView.ScaleType.CenterCrop);
            close.SetImageResource(Resource.Drawable.Close);
            close.SetScaleType(ImageView.ScaleType.CenterCrop);
            //  btnSubmitReview.Click += BtnSubmitReview_Click;
            return view;
        }

        public override Dialog OnCreateDialog(Bundle Saved)
        {
            Dialog dialog = base.OnCreateDialog(Saved);
            dialog.Window.RequestFeature(WindowFeatures.NoTitle);
            //dialog.Window.RequestFeature(wi);
            var metrics = Resources.DisplayMetrics;
            var widthInDp = ConvertPixelsToDp(metrics.WidthPixels);
            myDialog = dialog;

            return dialog;
        }
        private int ConvertPixelsToDp(float pixelValue)
        {
            var dp = (int)((pixelValue) / Resources.DisplayMetrics.Density);
            return dp;
        }
    }
}