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
using System.Net;
using Android.Graphics;

namespace Tabs
{
    public class WineDetailsActivity : Activity
    {
        ListView wineList;
        List<Review> myArr;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            SetContentView(Resource.Layout.WineDetailAndReview);

            ImageView imgBigWine = FindViewById<ImageView>(Resource.Id.imgBigWine);

            WineDetails curr = SampleData();
            var imageBitmap = GetImageBitmapFromUrl(curr.imageURL);
            imgBigWine.SetImageBitmap(imageBitmap);

            CreatePopups(curr);

            wineList = FindViewById<ListView>(Resource.Id.lstWineReviews);
            myArr = SampleReviewData();

            ReviewListAdapter adapter = new ReviewListAdapter(this, myArr);
            wineList.Adapter = adapter;
        }

        private void CreatePopups(WineDetails curr)
        {
            Button btnDesc = FindViewById<Button>(Resource.Id.btnDesc);
            btnDesc.Click += (object sender, EventArgs e) =>
            {
                // On "Call" button click, try to dial phone number.
                var callDialog = new AlertDialog.Builder(this);
                callDialog.SetTitle("Description");
                callDialog.SetMessage(curr.Description);

                callDialog.SetNegativeButton("Close", delegate { });

                // Show the alert dialog to the user and wait for response.
                callDialog.Show();
            };

            Button btnWineNotes = FindViewById<Button>(Resource.Id.btnWineNotes);
            btnWineNotes.Click += (object sender, EventArgs e) =>
            {
                // On "Call" button click, try to dial phone number.
                var callDialog = new AlertDialog.Builder(this);
                callDialog.SetTitle("Wine maker Notes");
                callDialog.SetMessage(curr.WinemakerNotes);

                callDialog.SetNegativeButton("Close", delegate { });

                // Show the alert dialog to the user and wait for response.
                callDialog.Show();
            };

            Button btnTechNotes = FindViewById<Button>(Resource.Id.btnTechNotes);
            btnTechNotes.Click += (object sender, EventArgs e) =>
            {
                // On "Call" button click, try to dial phone number.
                var callDialog = new AlertDialog.Builder(this);
                callDialog.SetTitle("Technical Notes");
                callDialog.SetMessage(curr.TechnicalNotes);

                callDialog.SetNegativeButton("Close", delegate { });

                // Show the alert dialog to the user and wait for response.
                callDialog.Show();
            };

            Button btnFoorPair = FindViewById<Button>(Resource.Id.btnFoorPair);
            btnFoorPair.Click += (object sender, EventArgs e) =>
            {
                // On "Call" button click, try to dial phone number.
                var callDialog = new AlertDialog.Builder(this);
                callDialog.SetTitle("Food Pairing");
                callDialog.SetMessage(curr.FoodPairing);

                callDialog.SetNegativeButton("Close", delegate { });

                // Show the alert dialog to the user and wait for response.
                callDialog.Show();
            };

            Button btnProducer = FindViewById<Button>(Resource.Id.btnProducer);
            btnProducer.Click += (object sender, EventArgs e) =>
            {
                // On "Call" button click, try to dial phone number.
                var callDialog = new AlertDialog.Builder(this);
                callDialog.SetTitle("Producer");
                callDialog.SetMessage(curr.Producer);

                callDialog.SetNegativeButton("Close", delegate { });

                // Show the alert dialog to the user and wait for response.
                callDialog.Show();
            };

            //Create Rating popup.
            Button btnReview = FindViewById<Button>(Resource.Id.btnReview);
            btnReview.Click += (object sender, EventArgs e) =>
            {
                //Pull up Dialog
                FragmentTransaction trans = FragmentManager.BeginTransaction();
                DialogReview dr = new DialogReview();
                dr.Show(trans, "Wine Review");

                dr.newReviewCreated += Dr_newReviewCreated;
            };
        }

        private void Dr_newReviewCreated(object sender, NewReviewEventArgs e)
        {
            myArr.Insert(0, e.newReview);
            ReviewListAdapter adapter = new ReviewListAdapter(this, myArr);
            wineList.Adapter = adapter;
        }

        private Android.Graphics.Bitmap GetImageBitmapFromUrl(string url)
        {
            Bitmap imageBitmap = null;

            using (var webClient = new WebClient())
            {
                var imageBytes = webClient.DownloadData(url);
                if (imageBytes != null && imageBytes.Length > 0)
                {
                    imageBitmap = BitmapFactory.DecodeByteArray(imageBytes, 0, imageBytes.Length);
                }
            }

            return imageBitmap;
        }

        private WineDetails SampleData()
        {
            WineDetails w1 = new WineDetails();
            w1.Description = "Description";
            w1.FoodPairing = "FoodPairing";
            w1.imageURL = "https://www.wineoutlet.com/labels/P23578.jpg";
            w1.Name = "King Estate Pinot Gris";
            w1.Producer = "Producer";
            w1.TechnicalNotes = "TechnicalNotes";
            w1.WinemakerNotes = "WinemakerNotes";

            return w1;
        }
        private List<Review> SampleReviewData()
        {
            List<Review> rList = new List<Review>();

            Review r1 = new Review();
            r1.Title = "Nice wine, Good taste";
            r1.ReviewStars = 4;
            r1.ReviewText = "Generous at the onset, this wine has the scent of pear slices spritzed with lemon juice. Those fresh fruit flavors get a warm toasted lees accent on the palate, along with a mineral finish. A versatile gris for a meal, whether a grilled chicken or stuffed squid.";
            r1.Reviewer = "Ankur Dubey";
            rList.Add(r1);

            return rList;
        }
    }
}