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

   
    class ReviewPopup
    {

        Context Parent;
        private int WineId;

        Review _editObj;

        public ReviewPopup(Context parent, Review EditObj)
        {
            Parent = parent;
            _editObj = EditObj;
            WineId = EditObj.WineId;
        }

        public void CreatePopup(object sender, RatingBar.RatingBarChangeEventArgs e)
        {
            //e.Rating

            Dialog editDialog = new Dialog(Parent);
            var rat = e.Rating;
            //editDialog.Window.RequestFeature(WindowFeatures.NoTitle);
            //editDialog.Window.SetBackgroundDrawable(new Android.Graphics.Drawables.ColorDrawable(Android.Graphics.Color.White));// (Android.Graphics.Color.Transparent));
            editDialog.SetContentView(Resource.Layout.EditReviewPopup);
            //editDialog.SetTitle();
            ServiceWrapper sw = new ServiceWrapper();
            Review review = new Review();
            ImageButton ibs = editDialog.FindViewById<ImageButton>(Resource.Id.ratingimage);
            ImageButton close = editDialog.FindViewById<ImageButton>(Resource.Id.close);
            Button btnSubmitReview = editDialog.FindViewById<Button>(Resource.Id.btnSubmitReview);
            TextView Comments = editDialog.FindViewById<TextView>(Resource.Id.txtReviewComments);
            RatingBar custRating = editDialog.FindViewById<RatingBar>(Resource.Id.rating);
            custRating.Rating = rat;


            ibs.SetImageResource(Resource.Drawable.wine_review);
            ibs.SetScaleType(ImageView.ScaleType.CenterCrop);
            //close.SetImageResource(Resource.Drawable.Close);
            close.SetScaleType(ImageView.ScaleType.CenterCrop);
            editDialog.Window.SetBackgroundDrawable(new Android.Graphics.Drawables.ColorDrawable(Android.Graphics.Color.Transparent));
            editDialog.Show();
            close.Click += delegate
            {
                editDialog.Dismiss();
            };
            btnSubmitReview.Click += async delegate
            {

                review.ReviewDate = DateTime.Now;
                review.ReviewUserId = Convert.ToInt32(CurrentUser.getUserId());
                review.Username = CurrentUser.getUserName();
                review.RatingText = Comments.Text;
                review.RatingStars = Convert.ToInt32(custRating.Rating);
                review.IsActive = true;

                review.WineId = WineId;
                await sw.InsertUpdateReview(review);
                ((IPopupParent)Parent).RefreshParent();
                editDialog.Dismiss();

            };

        }
        
        public void EditPopup(object sender, EventArgs e)
        {
            Dialog editDialog = new Dialog(Parent);

            //editDialog.Window.RequestFeature(WindowFeatures.NoTitle);
            //editDialog.Window.SetBackgroundDrawable(new Android.Graphics.Drawables.ColorDrawable(Android.Graphics.Color.White));// (Android.Graphics.Color.Transparent));
            editDialog.SetContentView(Resource.Layout.EditReviewPopup);
            //editDialog.SetTitle();
            ServiceWrapper sw = new ServiceWrapper();
            Review review = new Review();

            ImageButton ibs = editDialog.FindViewById<ImageButton>(Resource.Id.ratingimage);
            ImageButton close = editDialog.FindViewById<ImageButton>(Resource.Id.close);
            Button btnSubmitReview = editDialog.FindViewById<Button>(Resource.Id.btnSubmitReview);
            TextView Comments = editDialog.FindViewById<TextView>(Resource.Id.txtReviewComments);
            RatingBar custRating = editDialog.FindViewById<RatingBar>(Resource.Id.rating);


            Comments.Text = _editObj.RatingText;
            custRating.Rating = _editObj.RatingStars;

            ibs.SetImageResource(Resource.Drawable.wine_review);
            ibs.SetScaleType(ImageView.ScaleType.CenterCrop);
            //close.SetImageResource(Resource.Drawable.Close);
            close.SetScaleType(ImageView.ScaleType.CenterCrop);
            editDialog.Window.SetBackgroundDrawable(new Android.Graphics.Drawables.ColorDrawable(Android.Graphics.Color.Transparent));
            editDialog.Show();
            close.Click += delegate
            {
                editDialog.Dismiss();
            };
            btnSubmitReview.Click += async delegate
            {

                review.ReviewDate = DateTime.Now;
                review.ReviewUserId = Convert.ToInt32(CurrentUser.getUserId());
                review.RatingText = Comments.Text;
                review.RatingStars = Convert.ToInt32(custRating.Rating);
                review.IsActive = true;
                review.WineId = WineId;
                await sw.InsertUpdateReview(review);
                ((IPopupParent)Parent).RefreshParent();
                editDialog.Dismiss();
            };

        }
    }

    public interface IPopupParent
    {
        void RefreshParent();
    }
}