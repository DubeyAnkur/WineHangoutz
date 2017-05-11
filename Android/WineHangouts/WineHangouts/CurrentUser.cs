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
using Android.Graphics.Drawables;

namespace WineHangouts
{
    class CurrentUser
    {
        private static string _perf = "userInfo";
        private static ISharedPreferencesEditor _edit;
        private static ISharedPreferences _pref;
        static CurrentUser()
        {
            _pref = Application.Context.GetSharedPreferences(_perf, FileCreationMode.Private);
            _edit = _pref.Edit();

        }

        public static void SaveUserName(string UserName, string UserId)
        {

            _edit.PutString("UserName", UserName);
            _edit.PutString("UserId", UserId);
            _edit.Apply();
        }

        public static void SaveMailId(string email)
        {
            _edit.PutString("UserEmail", email);
            _edit.Apply();
        }
        public static string GetMailId()
        {
            string MailId = _pref.GetString("UserEmail", null);
            return MailId;
        }
        
        public static void PutToken(string count)
        {
            _edit.PutString("token", count);
            _edit.Apply();
           
        }

        public static string getToken()
        {
            string countVal = _pref.GetString("token", null);
            return countVal;
        }

        public static string getUserName()
        {


            string value1 = _pref.GetString("UserName", null);
            return value1;

        }
        public static string getUserId()
        {

            string value1 = _pref.GetString("UserId", null);
            return value1;
        }
        public static void ClearDetails()
        {
            _edit.Clear();
        }
    }

    public class ProgressIndicator
    {
        // There will be only one instance of ProgressDialog across application.
        static ProgressDialog progress;
        static ProgressIndicator()
        {
            
        }

        public static void Show(Context _parent)
        {
            progress = new Android.App.ProgressDialog(_parent);
            progress.Indeterminate = true;
			progress.Window.SetBackgroundDrawable(new ColorDrawable(Android.Graphics.Color.Transparent));
			progress.SetProgressStyle(Android.App.ProgressDialogStyle.Spinner);
			//progress.SetProgressStyle(ProgressDialogStyle.Spinner);
			
			progress.SetMessage("Loading... Please Wait...");
            progress.SetCancelable(false);
            progress.Show();
        }

        public static void Hide()
        {
            //progress should not be null & should we called for every show.
            progress.Dismiss();
        }
    }
}