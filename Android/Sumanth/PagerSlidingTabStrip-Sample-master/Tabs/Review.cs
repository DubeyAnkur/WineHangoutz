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
    public class Review
    {
        public int ReviewStars { get; set; }
        public string Title { get; set; }
        public string ReviewText { get; set; }
        public string Reviewer { get; set; }
        public DateTime ReviewDate { get; set; }
    }
}