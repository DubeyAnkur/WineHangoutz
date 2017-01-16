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
using Android.Graphics;
using Android.Util;
using Hangout.Models;

namespace WineHangouts
{

    //public class EditReview : DialogFragment
    //{


    //    //RatingBar userRatingBar;
    //    //EditText txtReviewComments;

    //    //RatingBar userRatingBar;
    //    //EditText txtReviewComments;
    //    public Dialog myDialog;

    //    public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
    //    {
    //        base.OnCreateView(inflater, container, savedInstanceState);
    //        var view = inflater.Inflate(Resource.Layout.EditReviewPopup, container, false);


    //        //userRatingBar = view.FindViewById<RatingBar>(Resource.Id.rtbProductRating);
    //        //txtReviewComments = view.FindViewById<EditText>(Resource.Id.txtReviewComments);
    //        //Button btnSubmitReview = view.FindViewById<Button>(Resource.Id.btnSubmitReview);

    //        //userRatingBar = view.FindViewById<RatingBar>(Resource.Id.rtbProductRating);
    //        //txtReviewComments = view.FindViewById<EditText>(Resource.Id.txtReviewComments);
    //        Button btnSubmitReview = view.FindViewById<Button>(Resource.Id.btnSubmitReview);
    //        //ImageButton ibs = view.FindViewById<ImageButton>(Resource.Id.imageButton1);
    //        ImageButton close = view.FindViewById<ImageButton>(Resource.Id.imageButton2);
    //        //ibs.SetImageResource(Resource.Drawable.wine_review);
    //        //ibs.SetScaleType(ImageView.ScaleType.CenterCrop);
    //        close.SetImageResource(Resource.Drawable.Close);
    //        close.SetScaleType(ImageView.ScaleType.CenterCrop);
    //        //  btnSubmitReview.Click += BtnSubmitReview_Click;
    //        return view;
    //    }

    //    public override Dialog OnCreateDialog(Bundle Saved)
    //    {
    //        Dialog dialog = base.OnCreateDialog(Saved);
    //        dialog.Window.RequestFeature(WindowFeatures.NoTitle);
    //        //dialog.Window.RequestFeature(wi);
    //        var metrics = Resources.DisplayMetrics;
    //        var widthInDp = ConvertPixelsToDp(metrics.WidthPixels);
    //        myDialog = dialog;

    //        return dialog;
    //    }
    //    private int ConvertPixelsToDp(float pixelValue)
    //    {
    //        var dp = (int)((pixelValue) / Resources.DisplayMetrics.Density);
    //        return dp;
    //    }
    //}
    class ReviewPopup
    {

        Context Parent;
        public int SKU;
        public ReviewPopup(Context parent)
        {
            Parent = parent;
        }

        public void CreatePopup(object sender, RatingBar.RatingBarChangeEventArgs e)
        {
            Dialog editDialog = new Dialog(Parent);

            //editDialog.Window.RequestFeature(WindowFeatures.NoTitle);
            //editDialog.Window.SetBackgroundDrawable(new Android.Graphics.Drawables.ColorDrawable(Android.Graphics.Color.White));// (Android.Graphics.Color.Transparent));
            editDialog.SetContentView(Resource.Layout.EditReviewPopup);
            //editDialog.SetTitle();

            ImageButton ibs = editDialog.FindViewById<ImageButton>(Resource.Id.ratingimage);
            ImageButton close = editDialog.FindViewById<ImageButton>(Resource.Id.close);
            Button btnSubmitReview = editDialog.FindViewById<Button>(Resource.Id.btnSubmitReview);
            TextView Comments = editDialog.FindViewById<TextView>(Resource.Id.txtReviewComments);
            RatingBar custRating = editDialog.FindViewById<RatingBar>(Resource.Id.rating);
            ibs.SetImageResource(Resource.Drawable.wine_review);
            ibs.SetScaleType(ImageView.ScaleType.CenterCrop);
            close.SetImageResource(Resource.Drawable.Close);
            close.SetScaleType(ImageView.ScaleType.CenterCrop);
            editDialog.Window.SetBackgroundDrawable(new Android.Graphics.Drawables.ColorDrawable(Android.Graphics.Color.Transparent));
            editDialog.Show();
            close.Click += delegate
            {
                editDialog.Dismiss();
            };
            btnSubmitReview.Click += async delegate
            {
                ServiceWrapper sw = new ServiceWrapper();
                Review review = new Review();
                review.ReviewDate = DateTime.Now;
                review.ReviewUserId = Convert.ToInt32(CurrentUser.getUserId());
                review.RatingText = Comments.Text;
                review.RatingStars = Convert.ToInt32( custRating.Rating);
                review.IsActive = true;
                review.SKU = SKU;
                await sw.InsertUpdateReview(review);
            };

        }
        public void EditPopup(object sender, EventArgs e)
        {
            Dialog editDialog = new Dialog(Parent);
            
            //editDialog.Window.RequestFeature(WindowFeatures.NoTitle);
            //editDialog.Window.SetBackgroundDrawable(new Android.Graphics.Drawables.ColorDrawable(Android.Graphics.Color.White));// (Android.Graphics.Color.Transparent));
            editDialog.SetContentView(Resource.Layout.EditReviewPopup);
            //editDialog.SetTitle();
           

            ImageButton ibs = editDialog.FindViewById<ImageButton>(Resource.Id.ratingimage);
            ImageButton close = editDialog.FindViewById<ImageButton>(Resource.Id.close);
           

            ibs.SetImageResource(Resource.Drawable.wine_review);
            ibs.SetScaleType(ImageView.ScaleType.CenterCrop);
            close.SetImageResource(Resource.Drawable.Close);
            close.SetScaleType(ImageView.ScaleType.CenterCrop);
            editDialog.Window.SetBackgroundDrawable(new Android.Graphics.Drawables.ColorDrawable(Android.Graphics.Color.Transparent));
            editDialog.Show();
            close.Click += delegate
            {
                editDialog.Dismiss();
            };
           
        }
    }
}