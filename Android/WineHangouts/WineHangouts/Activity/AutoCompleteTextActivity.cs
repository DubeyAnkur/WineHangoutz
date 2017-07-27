using System;
using Android.App;
using Android.OS;
using Android.Widget;
using System.IO;
using System.Collections.Generic;
using Android.Views;

namespace WineHangouts
{
    [Activity(Label = "Search Helper", MainLauncher = false, Icon = "@drawable/logo5", ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    public class AutoCompleteTextActivity : Activity {
        protected override void OnCreate (Bundle bundle)
        {
            base.OnCreate (bundle);
            Window.RequestFeature(WindowFeatures.NoTitle);
            SetContentView(Resource.Layout.AutoCompleteTextLayout);

            //var autoCompleteOptions = new String[]
            //{
            //    "Hello",
            //    "Hey",
            //    "Hej",
            //    "Hi",
            //    "Hola",
            //    "Bonjour",
            //    "Gday",
            //    "Goodbye",
            //    "Sayonara",
            //    "Farewell",
            //    "Adios"
            //};
            //ArrayAdapter autoCompleteAdapter = new ArrayAdapter(this, Android.Resource.Layout.SimpleDropDownItem1Line
            //    , autoCompleteOptions);

            var autocompleteTextView = FindViewById<AutoCompleteTextView>(Resource.Id.AutoCompleteInput);
            //autocompleteTextView.Adapter = autoCompleteAdapter;
           

            #region Additional Information - use a file to populate autocomplete array

            // instead of the small array of greetings, use a large dictionary of words loaded from a file
            Stream seedDataStream = Assets.Open(@"WineList.txt");
            List<string> lines = new List<string>();
            using (StreamReader reader = new StreamReader(seedDataStream)) {
                string line;
                while ((line = reader.ReadLine()) != null) {
                    lines.Add(line);
                }
            }
            ProgressIndicator.Hide();
            string[] wordlist = lines.ToArray();
            ArrayAdapter dictionaryAdapter = new ArrayAdapter(this, Android.Resource.Layout.SimpleDropDownItem1Line, wordlist);
            autocompleteTextView.Adapter = dictionaryAdapter;
            
            //ArrayAdapter autoCompleteAdapter = new ArrayAdapter(this, Android.Resource.Layout.SimpleDropDownItem1Line
            //    , dictionaryAdapter);

            #endregion
        }
    }
}


