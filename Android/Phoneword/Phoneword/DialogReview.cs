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

namespace Phoneword
{
    public class NewReviewEventArgs : EventArgs
    {
        public Review newReview { get; set; }
        public NewReviewEventArgs(Review revw) : base()
        {
            newReview = revw;
        }
    }
    class DialogReview : DialogFragment
    {
        public event EventHandler<NewReviewEventArgs> newReviewCreated;
        RatingBar userRatingBar;
        EditText txtRatingTitle;
        EditText txtReviewComments;
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);
            var view = inflater.Inflate(Resource.Layout.ReviewPopup, container, false);

            userRatingBar = view.FindViewById<RatingBar>(Resource.Id.rtbProductRating);
            txtRatingTitle = view.FindViewById<EditText>(Resource.Id.txtRatingTitle);
            txtReviewComments = view.FindViewById<EditText>(Resource.Id.txtReviewComments);
            Button btnSubmitReview = view.FindViewById<Button>(Resource.Id.btnSubmitReview);

            btnSubmitReview.Click += BtnSubmitReview_Click;
            return view;
        }

        private void BtnSubmitReview_Click(object sender, EventArgs e)
        {
            Review newReview = new Review();
            newReview.ReviewStars = Convert.ToInt32(userRatingBar.Rating);
            newReview.Title = txtRatingTitle.Text;
            newReview.ReviewText = txtReviewComments.Text;

            newReviewCreated.Invoke(this, new NewReviewEventArgs(newReview));
            this.Dismiss();
        }
    }
}