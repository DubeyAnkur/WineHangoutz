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
    public class TokenModel
    {
       public int User_id { get; set; }
       public string DeviceToken { get; set; }
        public int DeviceType { get; set; }
    }
}