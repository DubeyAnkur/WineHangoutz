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

        public static void SaveUserName(string x, string y)
        {

            _edit.PutString("user", x);
            _edit.PutString("pass", y);
            _edit.Apply();
        }

        public static string getUserName()
        {


            string value1 = _pref.GetString("user", null);
            return value1;

        }
        public static string getPassword()
        {

            string value1 = _pref.GetString("pass", null);
            return value1;
        }
    }
}