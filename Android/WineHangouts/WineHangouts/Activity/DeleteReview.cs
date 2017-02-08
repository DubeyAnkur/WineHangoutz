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
   
    class DeleteReview : DialogFragment
    {


       
        public Dialog myDialog;
 

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);
            var view = inflater.Inflate(Resource.Layout.DeleteReviewPop, container, false);


            Button btnSubmitReview = view.FindViewById<Button>(Resource.Id.btnSubmitReview);


          
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