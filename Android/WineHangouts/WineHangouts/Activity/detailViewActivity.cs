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
using Android.Graphics;
using Android.Util;
using Hangout.Models;
using System.Net;
using System.Threading.Tasks;
using System.IO;
using Android.Support;

namespace WineHangouts
{
    [Activity(Label = "Wine Details", MainLauncher = false, Icon = "@drawable/logo5")]
    public class DetailViewActivity : Activity, IPopupParent
    {
        public int sku;
        private int screenid = 4; 
        //Button downloadButton;
        WebClient webClient;
        ImageView HighImageWine;
        //ImageView imgWine;
        int wineid;
        //LinearLayout progressLayout;
        //List<ItemDetails> DetailsArray;
        List<Review> ReviewArray;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.detailedView);
            wineid = Intent.GetIntExtra("WineID", 123);
            ActionBar.SetHomeButtonEnabled(true);
            ActionBar.SetDisplayHomeAsUpEnabled(true);
            ServiceWrapper svc = new ServiceWrapper();
            ItemDetailsResponse myData = new ItemDetailsResponse();
            ItemReviewResponse SkuRating = new ItemReviewResponse();
            this.Title = "Wine Details";
            var commentsView = FindViewById<ListView>(Resource.Id.listView2);
            TextView WineName = FindViewById<TextView>(Resource.Id.txtWineName); //Assigning values to respected Textfields
            WineName.Focusable = false;
            TextView WineProducer = FindViewById<TextView>(Resource.Id.txtProducer);
            WineProducer.Focusable = false;
            TextView Vintage = FindViewById<TextView>(Resource.Id.txtVintage);
            Vintage.Focusable = false;
            TextView WineDescription = FindViewById<TextView>(Resource.Id.txtWineDescription);
            WineDescription.Focusable = false;
            RatingBar AvgRating = FindViewById<RatingBar>(Resource.Id.avgrating);
            AvgRating.Focusable = false;
            TableRow tr5 = FindViewById<TableRow>(Resource.Id.tableRow5);
            try
            {
                DownloadAsync(this, System.EventArgs.Empty, wineid);
                myData = svc.GetItemDetails(wineid).Result;
                SkuRating = svc.GetItemReviewsByWineID(wineid).Result;
                ReviewArray = SkuRating.Reviews.ToList();
                reviewAdapter comments = new reviewAdapter(this, ReviewArray);
                commentsView.Adapter = comments;
                setListViewHeightBasedOnChildren1(commentsView);
                WineName.Text = myData.ItemDetails.Name;
                WineName.InputType = Android.Text.InputTypes.TextFlagNoSuggestions;
                Vintage.Text = myData.ItemDetails.Vintage.ToString();
				if (myData.ItemDetails.Producer == null || myData.ItemDetails.Producer == "")
				{
					WineProducer.Text = "Not Available";
				}
				else
				{
					WineProducer.Text = myData.ItemDetails.Producer;
				}
				if (myData.ItemDetails.Description == null || myData.ItemDetails.Description == "")
				{
					WineDescription.Text = "Not Available";
				}
				else
				{
					WineDescription.Text = myData.ItemDetails.Producer;
				}
				AvgRating.Rating = (float)myData.ItemDetails.AverageRating;
                Review edit = new Review()
                {
                    WineId = wineid,
                    RatingText = ""
                };
                var tempReview = ReviewArray.Find(x => x.ReviewUserId == Convert.ToInt32(CurrentUser.getUserId()));
                if(tempReview != null)
                    edit.RatingText = tempReview.RatingText;
                ReviewPopup editPopup = new ReviewPopup(this, edit);
                RatingBar RatingInput = FindViewById<RatingBar>(Resource.Id.ratingInput);//Taking rating stars input
                RatingInput.RatingBarChange +=   editPopup.CreatePopup;                   
                var metrics = Resources.DisplayMetrics;
                var widthInDp = ConvertPixelsToDp(metrics.WidthPixels);
                var heightInDp = ConvertPixelsToDp(metrics.HeightPixels);

               
                HighImageWine = FindViewById<ImageView>(Resource.Id.WineImage);
                
                BitmapFactory.Options options = new BitmapFactory.Options
                {
                    InJustDecodeBounds = false,
                    OutHeight = 75,
                    OutWidth = 75

                };
                ProgressIndicator.Hide();
                LoggingClass.LogInfo("Entered into ", screenid);
                Bitmap result = BitmapFactory.DecodeResource(Resources, Resource.Drawable.placeholder_re, options);
            }
            catch (Exception exe)
            {
                LoggingClass.LogError(exe.Message, screenid, exe.StackTrace.ToString());
                AlertDialog.Builder alert = new AlertDialog.Builder(this);
                alert.SetTitle("Sorry");
                alert.SetMessage("We're under maintainence");
                alert.SetNegativeButton("Ok", delegate { });
                Dialog dialog = alert.Create();
                dialog.Show();

            }
            //downloadButton = FindViewById<Button>(Resource.Id.Download);
            //try
            //{
            //    //downloadButton.Enabled = true;
            //    downloadButton.Click += downloadAsync;
            //    //downloadButton.Enabled = false;

            //}

            //catch (Exception e) { }

        }


        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            if (item.ItemId == Android.Resource.Id.Home)
            {
                base.OnBackPressed();
                LoggingClass.LogInfo("Exited from ", screenid);
                return false;
            }
            return base.OnOptionsItemSelected(item);
        }

        private int ConvertPixelsToDp(float pixelValue)
        {
            var dp = (int)((pixelValue) / Resources.DisplayMetrics.Density);
            return dp;
        }
        public void setListViewHeightBasedOnChildren(ListView listView)
        {
            DetailsViewAdapter listAdapter = (DetailsViewAdapter)listView.Adapter;
            if (listAdapter == null)
                return;

            int desiredWidth = View.MeasureSpec.MakeMeasureSpec(listView.Width, MeasureSpecMode.Unspecified);
            int heightMeasureSpec = View.MeasureSpec.MakeMeasureSpec(ViewGroup.LayoutParams.WrapContent, MeasureSpecMode.Exactly);
            int totalHeight = 0;
            View view = null;
            for (int i = 0; i < listAdapter.Count; i++)
            {
                view = listAdapter.GetView(i, view, listView);
                if (i == 0)
                    view.LayoutParameters = new ViewGroup.LayoutParams(desiredWidth, WindowManagerLayoutParams.WrapContent);

                view.Measure(desiredWidth, heightMeasureSpec);
                totalHeight += view.MeasuredHeight;
            }
            ViewGroup.LayoutParams params1 = listView.LayoutParameters;
            params1.Height = totalHeight + (listView.DividerHeight * (listAdapter.Count - 1));
            listView.LayoutParameters = params1;
        }
        public void setListViewHeightBasedOnChildren1(ListView listView1)
        {
            reviewAdapter listAdapter = (reviewAdapter)listView1.Adapter;
            if (listAdapter == null)
                return;

            int desiredWidth = View.MeasureSpec.MakeMeasureSpec(listView1.Width, MeasureSpecMode.Unspecified);
            int heightMeasureSpec = View.MeasureSpec.MakeMeasureSpec(ViewGroup.LayoutParams.WrapContent, MeasureSpecMode.Exactly);
            int totalHeight = 0;
            View view = null;
            for (int i = 0; i < listAdapter.Count; i++)
            {
                view = listAdapter.GetView(i, view, listView1);
                if (i == 0)
                    view.LayoutParameters = new ViewGroup.LayoutParams(desiredWidth, WindowManagerLayoutParams.WrapContent);

                view.Measure(desiredWidth, heightMeasureSpec);
                totalHeight += view.MeasuredHeight;
            }
            ViewGroup.LayoutParams params1 = listView1.LayoutParameters;
            params1.Height = totalHeight + (listView1.DividerHeight * (listAdapter.Count - 1));
            listView1.LayoutParameters = params1;
        }

        public int PixelsToDp(int pixels)
        {
            return (int)TypedValue.ApplyDimension(ComplexUnitType.Dip, pixels, Resources.DisplayMetrics);
        }
        public void RefreshParent()
        {
            ServiceWrapper svc = new ServiceWrapper();
            int wineid = Intent.GetIntExtra("WineID", 138);
            ItemDetailsResponse myData = svc.GetItemDetails(wineid).Result;
            var SkuRating = svc.GetItemReviewsByWineID(wineid).Result;
            this.Title = "Wine Details";
            var commentsView = FindViewById<ListView>(Resource.Id.listView2);
            reviewAdapter comments = new reviewAdapter(this, SkuRating.Reviews.ToList());
            commentsView.Adapter = comments;
            comments.NotifyDataSetChanged();
        }

        public async void DownloadAsync(object sender, System.EventArgs ea, int wineid)
        {
            webClient = new WebClient();
            var url = new Uri("https://icsintegration.blob.core.windows.net/bottleimagesdetails/" + wineid + ".jpg");
            byte[] imageBytes = null;
            //progressLayout.Visibility = ViewStates.Visible;
            try
            {
                imageBytes = await webClient.DownloadDataTaskAsync(url);

            }
            catch (TaskCanceledException)
            {
                //this.progressLayout.Visibility = ViewStates.Gone;
                return;
            }
            catch (Exception exe)
            {
                LoggingClass.LogError("while downloading image "+exe.Message, screenid, exe.StackTrace.ToString());
                //progressLayout.Visibility = ViewStates.Gone;
                //downloadButton.Click += downloadAsync;
                //downloadButton.Text = "Download Image";
                Bitmap imgWine=BlobWrapper.Bottleimages(wineid);
                HighImageWine.SetImageBitmap(imgWine);
                return;
            }

            try
            {
                string documentsPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
                string localFilename = "Wine.png";
                string localPath = System.IO.Path.Combine(documentsPath, localFilename);

                FileStream fs = new FileStream(localPath, FileMode.OpenOrCreate);
                await fs.WriteAsync(imageBytes, 0, imageBytes.Length);
                fs.Close();

                BitmapFactory.Options options = new BitmapFactory.Options()
                {
                    InJustDecodeBounds = true
                };
                await BitmapFactory.DecodeFileAsync(localPath, options);
                
                Bitmap bitmap = await BitmapFactory.DecodeFileAsync(localPath);
                if (bitmap == null)
                {
                    HighImageWine.SetImageResource(Resource.Drawable.wine7);
                }
                HighImageWine.SetImageBitmap(bitmap);
            }
            catch (Exception exe)
            {
                LoggingClass.LogError("While setting High resulution image" + exe.Message, screenid, exe.StackTrace.ToString());
                
            }

            //progressLayout.Visibility = ViewStates.Gone;
            //downloadButton.Click += downloadAsync;
            //downloadButton.Enabled = false;
            HighImageWine.Dispose();
            //downloadButton.Text = "Download Image";
        }
    }

}

