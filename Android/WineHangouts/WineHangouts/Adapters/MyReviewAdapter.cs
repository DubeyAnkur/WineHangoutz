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

using System.Net;
using Hangout.Models;
using Java.Util;
using AndroidHUD;

namespace WineHangouts
{
    class MyReviewAdapter : BaseAdapter<Review>
    {
        private List<Review> myItems;
        private Context myContext;
        private int screenid = 23;

        public override Review this[int position]
        {
            get
            {
                return myItems[position];
            }
        }

        public MyReviewAdapter(Context con, List<Review> strArr)
        {
            myContext = con;
            myItems = strArr;


        }
        public override int Count
        {
            get
            {
                return myItems.Count;

            }
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            View row = convertView;
            if (myItems.Count == 0)

            {
                row = LayoutInflater.From(myContext).Inflate(Resource.Layout.ReviewEmpty, null, false);
                TextView txtName = row.FindViewById<TextView>(Resource.Id.textView1);
                ImageView Imag = row.FindViewById<ImageView>(Resource.Id.imageView1);

            }
            else
            {
                if (row == null)
                    row = LayoutInflater.From(myContext).Inflate(Resource.Layout.MyReviewsCell, null, false);

                //else
                //    return convertView;

                TextView txtName = row.FindViewById<TextView>(Resource.Id.textView64);
                TextView txtYear = row.FindViewById<TextView>(Resource.Id.textView65);
                TextView txtDescription = row.FindViewById<TextView>(Resource.Id.textView66);
                TextView txtDate = row.FindViewById<TextView>(Resource.Id.textView67);
                //TextView txtPrice = row.FindViewById<TextView>(Resource.Id.txtPrice);
                ImageButton edit = row.FindViewById<ImageButton>(Resource.Id.imageButton3);
                ImageButton delete = row.FindViewById<ImageButton>(Resource.Id.imageButton4);
                ImageButton wineimage = row.FindViewById<ImageButton>(Resource.Id.imageButton2);
                var metrics = myContext.Resources.DisplayMetrics;
                var widthInDp = ConvertPixelsToDp(metrics.WidthPixels);
                var heightInDp = ConvertPixelsToDp(metrics.HeightPixels);


                RatingBar rb = row.FindViewById<RatingBar>(Resource.Id.rating);
                ImageView heartImg = row.FindViewById<ImageView>(Resource.Id.imageButton44);
                heartImg.SetImageResource(Resource.Drawable.heart_empty);

                edit.Focusable = false;

                edit.Clickable = true;
                delete.Focusable = false;

                delete.Clickable = true;
                wineimage.Focusable = false;
                wineimage.FocusableInTouchMode = false;
                wineimage.Clickable = true;

                //if (convertView == null)
                //{ edit.Tag = position; }

                //if (convertView == null)
                //{
                //    edit.Click += (sender, args) =>
                //    {
                //        int tempPosition = Convert.ToInt32(((ImageButton)sender).Tag);
                //        string WineBarcode = myItems[tempPosition].Barcode;
                //        Review _review = new Review();
                //        _review.Barcode = WineBarcode;
                //        _review.RatingStars = myItems[tempPosition].RatingStars;
                //        _review.RatingText = myItems[tempPosition].RatingText;
                //        _review.PlantFinal = myItems[tempPosition].PlantFinal;
                //        LoggingClass.LogInfo("clicked on edit  an item---->" + WineBarcode + "----->" + _review.RatingStars + "---->" + _review.RatingText, screenid);
                //        PerformItemClick(sender, args, _review);
                //    };
                //    //delete.Click += Delete_Click;
                //    delete.Click += (sender, args) =>
                //    {
                //        string WineBarcode = myItems[position].Barcode;

                //        Review _review = new Review();
                //        _review.Barcode = WineBarcode;
                //        LoggingClass.LogInfo("clicked on delete item--->" + WineBarcode, screenid);
                //        PerformdeleteClick(sender, args, _review);

                //    };
                //}

                wineimage.Click += (sender, args) => Console.WriteLine("ImageButton {0} clicked", position);
                txtDate.SetTextSize(Android.Util.ComplexUnitType.Dip, 12);
                txtName.Text = myItems[position].Name;

                txtYear.Text = myItems[position].Vintage;
                if (txtYear.Text == null || txtYear.Text == "0")
                { txtYear.Text = ""; }
                else { txtYear.Text = myItems[position].Vintage; }
                txtDescription.Text = myItems[position].RatingText;

                txtDate.Text = myItems[position].Date.ToString("yyyy/MM/dd");
                rb.Rating = myItems[position].RatingStars;

                ProfilePicturePickDialog pppd = new ProfilePicturePickDialog();
                string path = pppd.CreateDirectoryForPictures();

                var filePath = System.IO.Path.Combine(path + "/" + myItems[position].Barcode + ".jpg");


                bool count = Convert.ToBoolean(myItems[position].Liked);
                if (count == true)
                {
                    heartImg.SetImageResource(Resource.Drawable.heart_full);
                }
                else
                {
                    heartImg.SetImageResource(Resource.Drawable.heart_empty);
                }
                heartImg.Tag = position;
                edit.Tag = position;
               delete.Tag = position;
                if (convertView == null)
                {
                    edit.Click += (sender, args) =>
                        {
                            int tempPosition = Convert.ToInt32(edit.Tag);
                            string WineBarcode = myItems[tempPosition].Barcode;
                            Review _review = new Review();
                            _review.Barcode = WineBarcode;
                            _review.RatingStars = myItems[tempPosition].RatingStars;
                            _review.RatingText = myItems[tempPosition].RatingText;
                            _review.PlantFinal = myItems[tempPosition].PlantFinal;
                            LoggingClass.LogInfo("clicked on edit  an item---->" + WineBarcode + "----->" + _review.RatingStars + "---->" + _review.RatingText, screenid);
                            PerformItemClick(sender, args, _review);
                        };
        
                    delete.Click += (sender, args) =>
                    {
                        int tempPositio1n = Convert.ToInt32(edit.Tag);
                        string WineBarcode = myItems[tempPositio1n].Barcode;

                        Review _review = new Review();
                        _review.Barcode = WineBarcode;
                        LoggingClass.LogInfo("clicked on delete item--->" + WineBarcode, screenid);
                        PerformdeleteClick(sender, args, _review);

                    };
                    heartImg.Click += async delegate
                    {
                        int actualPosition = Convert.ToInt32(heartImg.Tag);
                        bool x;
                        if (count == false)
                        {
                            heartImg.SetImageResource(Resource.Drawable.heart_full);
                            LoggingClass.LogInfoEx("Liked an item------>" + myItems[position].Barcode, screenid);
                            x = true;
                            count = true;
                        }
                        else
                        {
                            heartImg.SetImageResource(Resource.Drawable.heart_empty);
                            LoggingClass.LogInfoEx("UnLiked an item" + "----->" + myItems[position].Barcode, screenid);
                            x = false;
                            count = false;
                        }
                        SKULike like = new SKULike();
                        like.UserID = Convert.ToInt32(CurrentUser.getUserId());
                        like.SKU = Convert.ToInt32(myItems[actualPosition].SKU);
                        like.Liked = x;
                            //myItems[actualPosition].IsLike = x;
                            like.BarCode = myItems[actualPosition].Barcode;
                        LoggingClass.LogInfo("Liked an item", screenid);
                        ServiceWrapper sw = new ServiceWrapper();
                        await sw.InsertUpdateLike(like);
                    };
                }


                Bitmap imageBitmap;
                //if (System.IO.File.Exists(filePath))
                //{
                //    imageBitmap = BitmapFactory.DecodeFile(filePath);
                //    wineimage.SetImageBitmap(imageBitmap);
                //}
                //else
                //{
                //    imageBitmap = BlobWrapper.Bottleimages(myItems[position].Barcode, Convert.ToInt32(myItems[position].PlantFinal));

                //    wineimage.SetImageBitmap(imageBitmap);
                //}
                string url = myItems[position].SmallImageURL;
                if (url == null)
                {
                    url = myItems[position].Barcode + ".jpg";
                }
                imageBitmap = BlobWrapper.Bottleimages(url, Convert.ToInt32(myItems[position].PlantFinal));
                if (imageBitmap == null)
                {
                    wineimage.SetImageResource(Resource.Drawable.bottle);
                }
                else { wineimage.SetImageBitmap(imageBitmap); }

                txtName.Focusable = false;
                txtYear.Focusable = false;
                txtDescription.Focusable = false;
                txtDate.Focusable = false;

            }


            LoggingClass.LogInfo("Entered into My Review Adapter", screenid);
            return row;
        }

        public void PerformItemClick(object sender, EventArgs e, Review edit)
        {
            ReviewPopup editPopup = new ReviewPopup(myContext, edit);
            editPopup.EditPopup(sender, e);
        }
        public void PerformdeleteClick(object sender, EventArgs e, Review edit)
        {

            DeleteReview dr = new DeleteReview(myContext, edit);
            dr.Show(((Activity)myContext).FragmentManager, "");

        }
        private int ConvertPixelsToDp(float pixelValue)
        {
            var dp = (int)((pixelValue) / myContext.Resources.DisplayMetrics.Density);
            return dp;
        }

    }
}