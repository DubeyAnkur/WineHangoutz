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
        public static void SaveCardNumber(string CardNumber)
        {
            _edit.PutString("CardNumber", CardNumber);
            _edit.Apply();
        }
        public static string GetCardNumber()
        {
            string CardNumber = _pref.GetString("CardNumber", null);
            return CardNumber;
        }
		public static void SavePrefered(int StoreName)
		{
			_edit.PutString("StoreName", StoreName.ToString());
			_edit.Apply();
		}
		public static string GetPrefered()
		{
			string StoreName = _pref.GetString("StoreName", null);
			return StoreName;
		}


		public static void PutDeviceToken(string count)
        {
            _edit.PutString("token", count);
            _edit.Apply();

        }

        public static string getDeviceToken()
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
		public static void SaveAuthToken(string Token)
		{
			_edit.PutString("Token", Token);
			_edit.Apply();
		}
		public static string GetAuthToken()
		{
			string Token = _pref.GetString("Token", null);
			return Token;
		}
		public static void SaveDeviceID(string DeviceID)
		{
			_edit.PutString("DeviceID", DeviceID);
			_edit.Apply();
		}
		public static string GetDeviceID()
		{
			string DeviceID = _pref.GetString("DeviceID", null);
			return DeviceID;
		}
       
        public static string GuestId { get; set; }
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
            progress.Window.SetBackgroundDrawable(new ColorDrawable(Android.Graphics.Color.Purple));
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