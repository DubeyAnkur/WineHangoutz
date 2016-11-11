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

namespace Tabs
{
    class WineDetails
    {
        public string Name { get; set; }
        public string imageURL { get; set; }
        public string SKU { get; set; }
        public string Description { get; set; }
        public string WinemakerNotes { get; set; }
        public string TechnicalNotes { get; set; }
        public string FoodPairing { get; set; }
        public string Producer { get; set; }
    }
}