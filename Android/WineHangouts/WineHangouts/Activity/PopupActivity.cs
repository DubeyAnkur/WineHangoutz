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
        private int ParentScreenId=17;
        Review _editObj;
        string storeid;
        //List<Review> ReviewArray;
        public ReviewPopup(Context parent, Review EditObj)
        {
            Parent = parent;
            _editObj = EditObj;
            WineId = EditObj.WineId;
            storeid = _editObj.PlantFinal;
        }
        public void CreatePopup(object sender, RatingBar.RatingBarChangeEventArgs e)
        {

            try
            {
                Dialog editDialog = new Dialog(Parent);
                var rat = e.Rating;
                //editDialog.Window.RequestFeature(WindowFeatures.NoTitle);
                //editDialog.Window.SetBackgroundDrawable(new Android.Graphics.Drawables.ColorDrawable(Android.Graphics.Color.White));// (Android.Graphics.Color.Transparent));
                editDialog.SetContentView(Resource.Layout.EditReviewPopup);
                //editDialog.SetTitle();
                ServiceWrapper sw = new ServiceWrapper();
                Review review = new Review();
                ImageButton close = editDialog.FindViewById<ImageButton>(Resource.Id.close);
                Button btnSubmitReview = editDialog.FindViewById<Button>(Resource.Id.btnSubmitReview);
                TextView Comments = editDialog.FindViewById<TextView>(Resource.Id.txtReviewComments);
                RatingBar custRating = editDialog.FindViewById<RatingBar>(Resource.Id.rating);
                custRating.Rating = rat;
                Comments.Text = _editObj.RatingText;
                 int screenid = 9;
        //ImageButton ibs = editDialog.FindViewById<ImageButton>(Resource.Id.ratingimage);
        //ItemReviewResponse SkuRating = new ItemReviewResponse();
        //SkuRating = sw.GetItemReviewsByWineID(WineId).Result;
        //ReviewArray = SkuRating.Reviews.ToList();
        //for (int i = 0; i < ReviewArray.Count(); i++)
        //{
        //    if (Convert.ToInt32(CurrentUser.getUserId()) == ReviewArray[i].ReviewUserId)
        //    {
        //        ItemReviewResponse uidreviews = new ItemReviewResponse();
        //        uidreviews = sw.GetItemReviewUID(Convert.ToInt32(CurrentUser.getUserId())).Result;
        //        List<Review> myArr1;
        //        myArr1 = uidreviews.Reviews.ToList();
        //        for (int j = 0; j < myArr1.Count; j++)
        //        {
        //            if (ReviewArray[i].Name == myArr1[i].Name)
        //                Comments.Text = myArr1[i].RatingText.ToString();
        //        }
        //    }
        //    else
        //    {
        //        CreatePopup(sender, e);
        //    }
        //}



        //ibs.SetImageResource(Resource.Drawable.wine_review);
        //ibs.SetScaleType(ImageView.ScaleType.CenterCrop);
        //close.SetImageResource(Resource.Drawable.Close);
        close.SetScaleType(ImageView.ScaleType.CenterCrop);
                editDialog.Window.SetBackgroundDrawable(new Android.Graphics.Drawables.ColorDrawable(Android.Graphics.Color.Transparent));
                editDialog.Show();
                LoggingClass.LogInfo("Entered into ", screenid);
                close.Click += delegate
                {
                    editDialog.Dismiss();
                };
                btnSubmitReview.Click += async delegate
                {
                    ProgressIndicator.Show(Parent);
                    review.ReviewDate = DateTime.Now;
                    review.ReviewUserId = Convert.ToInt32(CurrentUser.getUserId());
                    review.Username = CurrentUser.getUserName();
                    review.RatingText = Comments.Text;
                    review.RatingStars = Convert.ToInt32(custRating.Rating);
                    review.IsActive = true;
                    review.WineId = WineId;
                    review.PlantFinal = storeid;
                    LoggingClass.LogInfo("Submitted review",screenid);
                    await sw.InsertUpdateReview(review);
                    ((IPopupParent)Parent).RefreshParent();
                    ProgressIndicator.Hide();
                    editDialog.Dismiss();

                };
            }
            catch (Exception exe)
            {
                LoggingClass.LogError(exe.Message, ParentScreenId, exe.StackTrace.ToString());
            }

        }

        public void EditPopup(object sender, EventArgs e)
        {
            try
            {
                Dialog editDialog = new Dialog(Parent);
                int screenid = 18;
                //editDialog.Window.RequestFeature(WindowFeatures.NoTitle);
                //editDialog.Window.SetBackgroundDrawable(new Android.Graphics.Drawables.ColorDrawable(Android.Graphics.Color.White));// (Android.Graphics.Color.Transparent));
                editDialog.SetContentView(Resource.Layout.EditReviewPopup);
                //editDialog.SetTitle();
                ServiceWrapper sw = new ServiceWrapper();
                Review review = new Review();
                //ImageButton ibs = editDialog.FindViewById<ImageButton>(Resource.Id.ratingimage);
                ImageButton close = editDialog.FindViewById<ImageButton>(Resource.Id.close);
                Button btnSubmitReview = editDialog.FindViewById<Button>(Resource.Id.btnSubmitReview);
                TextView Comments = editDialog.FindViewById<TextView>(Resource.Id.txtReviewComments);
                RatingBar custRating = editDialog.FindViewById<RatingBar>(Resource.Id.rating);
                Comments.Text = _editObj.RatingText;
                custRating.Rating = _editObj.RatingStars;
                //ibs.SetImageResource(Resource.Drawable.wine_review);
                //ibs.SetScaleType(ImageView.ScaleType.CenterCrop);
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
                    ProgressIndicator.Show(Parent);
                    review.ReviewDate = DateTime.Now;
                    review.ReviewUserId = Convert.ToInt32(CurrentUser.getUserId());
                    review.RatingText = Comments.Text;
                    review.RatingStars = Convert.ToInt32(custRating.Rating);
                    review.IsActive = true;
                    review.WineId = WineId;
                    try
                    {
                        await sw.InsertUpdateReview(review);
                        LoggingClass.LogInfo("Edited Review submitted",screenid);
                    }

                    catch (Exception exe)
                    {
                        LoggingClass.LogError(exe.Message, screenid, exe.StackTrace.ToString());
                    }
                    ((IPopupParent)Parent).RefreshParent();
                    ProgressIndicator.Hide();
                    editDialog.Dismiss();
                };
            }
            catch (Exception exe)
            {
                LoggingClass.LogError(exe.Message, ParentScreenId, exe.StackTrace.ToString());
            }
        }
    }
    public interface IPopupParent
    {
        void RefreshParent();
    }
}