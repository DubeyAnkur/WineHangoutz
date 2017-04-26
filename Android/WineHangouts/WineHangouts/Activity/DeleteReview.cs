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
   
    class DeleteReview : DialogFragment
    {
        Review _editObj;
        public Dialog myDialog;
        private int WineId;
        Context Parent;
        public DeleteReview(Context parent,Review _editObj)
        {
            Parent = parent;
            WineId=  _editObj.WineId;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);
            var view = inflater.Inflate(Resource.Layout.DeleteReviewPop, container, false);
            ServiceWrapper sw = new ServiceWrapper();
            Review review = new Review();
            Button Delete = view.FindViewById<Button>(Resource.Id.button1);
            Button Cancel = view.FindViewById<Button>(Resource.Id.button2);
            
            Delete.Click += async delegate
            {
                review.WineId = WineId;
				ProgressIndicator.Show(Parent);
				review.ReviewUserId = Convert.ToInt32(CurrentUser.getUserId());
                await sw.DeleteReview(review);
                 ((IPopupParent)Parent).RefreshParent();
				ProgressIndicator.Hide();
				myDialog.Dismiss();
				
			};
			
            Cancel.Click += delegate
            {
                myDialog.Dismiss();
            };

            return view;
        }

        public override Dialog OnCreateDialog(Bundle Saved)
        {
            Dialog dialog = base.OnCreateDialog(Saved);
            dialog.Window.RequestFeature(WindowFeatures.NoTitle);
            myDialog = dialog;
            return dialog;
        }
   }

}