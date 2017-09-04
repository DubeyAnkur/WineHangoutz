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
using System;
using System.Collections.Generic;
using System.Globalization;

namespace WineHangouts
{
    [Activity(Label = "MyTastingAdapter")]
    public class MyTastingAdapter : BaseAdapter<Tastings>
    {
        private List<Tastings> myItems;
        private Context myContext;
		private int screenid = 24;

		public override Tastings this[int position]
        {
            get
            {
                return myItems[position];
            }
        }

        public MyTastingAdapter(Context con, List<Tastings> strArr)
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
        public EventHandler Edit_Click;
        public EventHandler Delete_Click;
        public override long GetItemId(int position)
        {
            return position;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            View row = convertView;
            if (myItems.Count == 0)

            {
                row = LayoutInflater.From(myContext).Inflate(Resource.Layout.EmptyTaste, null, false);
                TextView te =row. FindViewById<TextView>(Resource.Id.textView123a);

            }
            else
            {
                if (row == null)
                    
                    row = LayoutInflater.From(myContext).Inflate(Resource.Layout.MyTastingView, null, false);
                    TextView txtName = row.FindViewById<TextView>(Resource.Id.SkuName);
					TextView txtYear = row.FindViewById<TextView>(Resource.Id.Vintage);
					//TextView txtDescription = row.FindViewById<TextView>(Resource.Id.TastingNotes);
					TextView txtDate = row.FindViewById<TextView>(Resource.Id.Date);
					TextView txtPrice = row.FindViewById<TextView>(Resource.Id.Price);
					ImageView heartImg = row.FindViewById<ImageView>(Resource.Id.imageButton4);
					ImageButton wineimage = row.FindViewById<ImageButton>(Resource.Id.imageButton2);
					//RatingBar rb = row.FindViewById<RatingBar>(Resource.Id.AvgRating);
					heartImg.SetImageResource(Resource.Drawable.heart_empty);
					txtDate.SetTextSize(Android.Util.ComplexUnitType.Dip, 11);
                txtPrice.SetTextSize(Android.Util.ComplexUnitType.Dip, 11);
                txtName.Text = myItems[position].Name;
					txtYear.Text = myItems[position].Vintage.ToString();
                if(txtYear.Text==null|| txtYear.Text=="0")
                { txtYear.Text = ""; }
                else { txtYear.Text = myItems[position].Vintage.ToString(); }
					//txtDescription.Text = myItems[position].Description;
					txtDate.Text = "Tasted on :"+ myItems[position].TastingDate.ToString("yyyy/MM/dd");
                //txtPrice.Text = myItems[position].SalePrice.ToString("C", GridViewAdapter.Cultures.UnitedState);
                txtPrice.Text = myItems[position].PlantFinal.ToString();
                if(txtPrice.Text=="1")
                {
                    txtPrice.Text = "Tasted at :" + " Wall";
                }
                else if(txtPrice.Text == "2")
                {
                    txtPrice.Text = "Tasted at :" + " Pt.Pleasant Beach";
                }
                //rb.Rating = (float)myItems[position].AverageRating;
                //Bitmap imageBitmap = bvb.Bottleimages(myItems[position].WineId);
                ProfilePicturePickDialog pppd = new ProfilePicturePickDialog();
					string path = pppd.CreateDirectoryForPictures();
					//string path = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
					var filePath = System.IO.Path.Combine(path + "/" + myItems[position].Barcode + ".jpg");

					bool count = Convert.ToBoolean(myItems[position].IsLike);
					if (count == true)
					{
						heartImg.SetImageResource(Resource.Drawable.heart_full);
					}
					else
					{
						heartImg.SetImageResource(Resource.Drawable.heart_empty);
					}
					heartImg.Tag = position;

					if (convertView == null)
					{
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
							myItems[actualPosition].IsLike = x;
							like.BarCode = myItems[actualPosition].Barcode;
							LoggingClass.LogInfo("Liked an item", screenid);
							ServiceWrapper sw = new ServiceWrapper();
							await sw.InsertUpdateLike(like);
						};
					}





					Bitmap imageBitmap;
                //if (System.IO.File.Exists(filePath))
                //{
                //	imageBitmap = BitmapFactory.DecodeFile(filePath);
                //	wineimage.SetImageBitmap(imageBitmap);
                //}
                string url = myItems[position].SmallImageUrl;
                if (url == null)
                {
                    url = myItems[position].Barcode + ".jpg";
                }
                imageBitmap = BlobWrapper.Bottleimages(url, myItems[position].PlantFinal);

                if (imageBitmap == null)
                {
                    wineimage.SetImageResource(Resource.Drawable.bottle);
                }
                else { wineimage.SetImageBitmap(imageBitmap); }
                //wineimage.SetScaleType(ImageView.ScaleType.CenterCrop);
                

                txtName.Focusable = false;
					txtYear.Focusable = false;
					//txtDescription.Focusable = false;
					txtDate.Focusable = false;
                txtPrice.Focusable = false;
                wineimage.Focusable = false;
                wineimage.FocusableInTouchMode = false;
                wineimage.Clickable = true;
            }
			
			LoggingClass.LogInfo("Entered into My tastings Adapter", screenid);
			return row;
        }
        private int ConvertPixelsToDp(float pixelValue)
        {
            var dp = (int)((pixelValue) / myContext.Resources.DisplayMetrics.Density);
            return dp;
        }
    }
}