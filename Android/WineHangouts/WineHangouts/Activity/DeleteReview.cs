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
using AndroidHUD;

namespace WineHangouts
{
   
    class DeleteReview : DialogFragment
    {
	 
        //Review _editObj;
        public Dialog myDialog;
        private string WineBarcode;
        private int screenid = 12;
        Context Parent;
        public DeleteReview(Context parent,Review _editObj)
        {
            Parent = parent;
            WineBarcode =  _editObj.Barcode;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
			Stopwatch st=new Stopwatch();
			st.Start();
			base.OnCreateView(inflater, container, savedInstanceState);
            var view = inflater.Inflate(Resource.Layout.DeleteReviewPop, container, false);
			LoggingClass.LogInfo("Entered into Delete review popup with" + WineBarcode, screenid);
			ServiceWrapper sw = new ServiceWrapper();
            Review review = new Review();
            Button Delete = view.FindViewById<Button>(Resource.Id.button1);
            Button Cancel = view.FindViewById<Button>(Resource.Id.button2);
            try
            {
                Delete.Click += async delegate
                {
                    AndHUD.Shared.Show(Parent, "Deleting  Review...", Convert.ToInt32(MaskType.Clear));
                    review.Barcode = WineBarcode;
                   // ProgressIndicator.Show(Parent);
                    review.ReviewUserId = Convert.ToInt32(CurrentUser.getUserId());
					
					await sw.DeleteReview(review);
                    ((IPopupParent)Parent).RefreshParent();
                    ProgressIndicator.Hide();
                    myDialog.Dismiss();
                    AndHUD.Shared.Dismiss();
                    AndHUD.Shared.ShowSuccess(Parent, "Sucessfully Deleted", MaskType.Clear, TimeSpan.FromSeconds(2));

                    LoggingClass.LogInfoEx("User deleted winereview" + WineBarcode + "from "+review.PlantFinal+"st Store", screenid);


				};
            }
            catch(Exception exe)
            {
                LoggingClass.LogError(exe.Message, screenid, exe.StackTrace.ToString());
            }
            Cancel.Click += delegate
            {
				LoggingClass.LogInfo("clicked on cancel" + WineBarcode + review.PlantFinal, screenid);
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