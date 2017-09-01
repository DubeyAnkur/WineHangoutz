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
using System.Diagnostics;
using Hangout.Models;
using AndroidHUD;

namespace WineHangouts
{


    class ReviewPopup
    {

        Context Parent;
		Stopwatch st;
        private string WineBarcode;
        private int ParentScreenId=17;
        Review _editObj;
        string storeid;
        //List<Review> ReviewArray;
        public ReviewPopup(Context parent, Review EditObj)
        {
            Parent = parent;
            _editObj = EditObj;
            WineBarcode = EditObj.Barcode;
            storeid = _editObj.PlantFinal;
        }
        public void CreatePopup(object sender, RatingBar.RatingBarChangeEventArgs e)
        {

            if (CurrentUser.getUserId() == null)
            {
                AlertDialog.Builder aler = new AlertDialog.Builder(Parent, Resource.Style.MyDialogTheme);
                aler.SetTitle("Sorry");
                aler.SetMessage("This Feature is available for VIP Users only");
                aler.SetNegativeButton("Ok", delegate {
                   
                });
                Dialog dialog1 = aler.Create();
                dialog1.Show();
            }
            else
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
                close.SetScaleType(ImageView.ScaleType.CenterCrop);
                editDialog.Window.SetBackgroundDrawable(new Android.Graphics.Drawables.ColorDrawable(Android.Graphics.Color.Transparent));
                editDialog.Show();
                LoggingClass.LogInfo("Entered into CreatePopup", screenid);
                close.Click += delegate
                {
					LoggingClass.LogInfo("Closed PoPup", screenid);
					editDialog.Dismiss();
                };
                btnSubmitReview.Click += async delegate
                {
                   
     //               if (CurrentUser.getUserId() == null)
					//{
					//	AlertDialog.Builder aler = new AlertDialog.Builder(Parent, Resource.Style.MyDialogTheme);
					//	aler.SetTitle("Sorry");
					//	aler.SetMessage("This Feature is available for VIP Users only");
					//	aler.SetNegativeButton("Ok", delegate {
					//		LoggingClass.LogInfo("Closed PoPup", screenid);
					//		editDialog.Dismiss();
					//	});
					//	Dialog dialog1 = aler.Create();
					//	dialog1.Show();
					//}
					//else
					//{
                        AndHUD.Shared.Show(Parent, "Saving Review...", Convert.ToInt32(MaskType.Clear));
                        ProgressIndicator.Show(Parent);
                        review.ReviewDate = DateTime.Now;
						review.ReviewUserId = Convert.ToInt32(CurrentUser.getUserId());
						review.Username = CurrentUser.getUserName();
						review.RatingText = Comments.Text;
						review.RatingStars = Convert.ToInt32(custRating.Rating);
						review.IsActive = true;
						review.Barcode =WineBarcode ;
						review.PlantFinal = storeid;
						LoggingClass.LogInfo("Submitted review---->" + review.RatingStars + " ---->" + review.RatingText + "---->" + review.PlantFinal + "---->" + review.Barcode, screenid);
						await sw.InsertUpdateReview(review);
						((IPopupParent)Parent).RefreshParent();
						ProgressIndicator.Hide();
						editDialog.Dismiss();
                        AndHUD.Shared.Dismiss();
                        AndHUD.Shared.ShowSuccess(Parent, "Sucessfully Saved", MaskType.Clear, TimeSpan.FromSeconds(2));
                    //}
                   

                };
            }
            catch (Exception exe)
            {
                LoggingClass.LogError(exe.Message, ParentScreenId, exe.StackTrace.ToString());
            }
		
			//LoggingClass.LogTime("create popup",st.Elapsed.TotalSeconds.ToString());
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
				
				LoggingClass.LogInfo("Entered into EditPopup", screenid);

				close.SetScaleType(ImageView.ScaleType.CenterCrop);
                editDialog.Window.SetBackgroundDrawable(new Android.Graphics.Drawables.ColorDrawable(Android.Graphics.Color.Transparent));
                editDialog.Show();
                close.Click += delegate
                {
					LoggingClass.LogInfo("Closed PoPup", screenid);
					editDialog.Dismiss();
                };
                btnSubmitReview.Click += async delegate
                {
                    AndHUD.Shared.Show(Parent, "Saving Review...", Convert.ToInt32(MaskType.Clear));

                   // ProgressIndicator.Show(Parent);
                    review.ReviewDate = DateTime.Now;
                    review.ReviewUserId = Convert.ToInt32(CurrentUser.getUserId());
                    review.RatingText = Comments.Text;
                    review.RatingStars = Convert.ToInt32(custRating.Rating);
                    review.IsActive = true;
                    review.PlantFinal =storeid;
                    review.Barcode = WineBarcode;
                    try
                    {
                        await sw.InsertUpdateReview(review);
                        LoggingClass.LogInfo("Edited Review-----> "+ review.RatingText+"-----> "+review.RatingStars+ "----->"+review.Barcode+ "----->"+review.PlantFinal+"submitted",screenid);
                    }

                    catch (Exception exe)
                    {
                        LoggingClass.LogError(exe.Message, screenid, exe.StackTrace.ToString());
                    }
                    ((IPopupParent)Parent).RefreshParent();
                    // ProgressIndicator.Hide();
                    AndHUD.Shared.Dismiss();
                    AndHUD.Shared.ShowSuccess(Parent, "Sucessfully Saved", MaskType.Clear, TimeSpan.FromSeconds(2));
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