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
            progress.SetProgressStyle(Android.App.ProgressDialogStyle.Spinner);
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