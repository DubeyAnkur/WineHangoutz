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
using Hangout.Models;

namespace WineHangouts
{
   
    class DeleteReview : DialogFragment
    {
	 
        //Review _editObj;
        public Dialog myDialog;
        private int WineId;
        private int screenid = 12;
        Context Parent;
        public DeleteReview(Context parent,Review _editObj)
        {
            Parent = parent;
            WineId=  _editObj.WineId;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
			Stopwatch st=new Stopwatch();
			st.Start();
			base.OnCreateView(inflater, container, savedInstanceState);
            var view = inflater.Inflate(Resource.Layout.DeleteReviewPop, container, false);
			LoggingClass.LogInfo("Entered into Delete review popup with" + WineId, screenid);
			ServiceWrapper sw = new ServiceWrapper();
            Review review = new Review();
            Button Delete = view.FindViewById<Button>(Resource.Id.button1);
            Button Cancel = view.FindViewById<Button>(Resource.Id.button2);
            try
            {
                Delete.Click += async delegate
                {
                    review.WineId = WineId;
                    ProgressIndicator.Show(Parent);
                    review.ReviewUserId = Convert.ToInt32(CurrentUser.getUserId());
					
					await sw.DeleteReview(review);
                    ((IPopupParent)Parent).RefreshParent();
                    ProgressIndicator.Hide();
                    myDialog.Dismiss();
					LoggingClass.LogInfoEx("User deleted winereview" + WineId +review.PlantFinal, screenid);


				};
            }
            catch(Exception exe)
            {
                LoggingClass.LogError(exe.Message, screenid, exe.StackTrace.ToString());
            }
            Cancel.Click += delegate
            {
				LoggingClass.LogInfo("clicked on cancel" + WineId + review.PlantFinal, screenid);
				myDialog.Dismiss();
            };
			st.Stop();
			LoggingClass.LogTime("Deletereview time", st.Elapsed.TotalSeconds.ToString());
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