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
using System.Globalization;

namespace WineHangouts
{
	[Activity(Label = "MyFavoriteAdapter")]
	public class MyFavoriteAdapter : BaseAdapter<Item>
	{
		private List<Item> myItems;
		private Context myContext;
        private int screenid = 16;
        public int storeid;
        public override Item this[int position]
		{
			get
			{
				return myItems[position];
			}
		}

		public MyFavoriteAdapter(Context con, List<Item> strArr)
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

		public static class Cultures
		{
			public static readonly CultureInfo UnitedState =
				CultureInfo.GetCultureInfo("en-US");
		}

		public override long GetItemId(int position)
		{
			return position;
		}


		public override View GetView(int position, View convertView, ViewGroup parent)
		{
			//////////////			View row = convertView;
			//////////////			if (row == null)
			//////////////				row = LayoutInflater.From(myContext).Inflate(Resource.Layout.MyFavorite, null, false);
			//////////////			else
			//////////////				return row;
			//////////////			if (myItems.Count == 0)
			//////////////			{

			//////////////				AlertDialog.Builder aler = new AlertDialog.Builder(myContext);
			//////////////				aler.SetTitle("No Reviews Avalilable");
			//////////////				aler.SetMessage("Sorry you didn't tell us your Favourite wines");
			//////////////				aler.SetNegativeButton("Ok", delegate { });
			//////////////				LoggingClass.LogInfo("Clicked on Secaucus", screenid);
			//////////////				Dialog dialog = aler.Create();
			//////////////				dialog.Show();
			//////////////			}
			//////////////			else
			//////////////			{

			//////////////				TextView Name = row.FindViewById<TextView>(Resource.Id.txtNamefav);
			//////////////				TextView Vintage = row.FindViewById<TextView>(Resource.Id.txtVintagefav);
			//////////////				ImageView Wine = row.FindViewById<ImageView>(Resource.Id.imgWinefav);
			//////////////				TextView Price = row.FindViewById<TextView>(Resource.Id.txtPricefav);
			//////////////				RatingBar Avgrating = row.FindViewById<RatingBar>(Resource.Id.rtbProductRatingfav);
			//////////////				//ImageView place = row.FindViewById<ImageView>(Resource.Id.placeholdefavr);
			//////////////				ImageView Heart = row.FindViewById<ImageView>(Resource.Id.imgHeartfav);

			//////////////				String str = "lokesh";
			//////////////				Name.Text = myItems[position].Name;

			//////////////				Price.Text = myItems[position].SalePrice.ToString("C", GridViewAdapter.Cultures.UnitedState);

			//////////////				Avgrating.Rating = (float)myItems[position].AverageRating;
			//////////////				Vintage.Text = myItems[position].Vintage.ToString();


			//////////////				Heart.SetImageResource(Resource.Drawable.Heart_emp);
			//////////////				var heartLP = new RelativeLayout.LayoutParams(80, 80);

			//////////////				var metrics = myContext.Resources.DisplayMetrics;
			//////////////				var widthInDp = ConvertPixelsToDp(metrics.WidthPixels);
			//////////////				var heightInDp = ConvertPixelsToDp(metrics.HeightPixels);
			//////////////				heartLP.LeftMargin = parent.Resources.DisplayMetrics.WidthPixels / 2 - 110; // 110 = 80 + 30

			//////////////				heartLP.TopMargin = 5;
			//////////////				Heart.LayoutParameters = heartLP;
			//////////////				bool count = Convert.ToBoolean(myItems[position].IsLike);
			//////////////				if (count == true)
			//////////////				{
			//////////////					Heart.SetImageResource(Resource.Drawable.HeartFull);
			//////////////				}
			//////////////				else
			//////////////				{
			//////////////					Heart.SetImageResource(Resource.Drawable.Heart_emp);
			//////////////				}

			//////////////				Heart.Click += async delegate
			//////////////				{
			//////////////					bool x;
			//////////////					if (count == false)
			//////////////					{
			//////////////						Heart.SetImageResource(Resource.Drawable.HeartFull);
			//////////////						LoggingClass.LogInfo("Liked an item" + myItems[position].WineId, screenid);
			//////////////						x = true;
			//////////////						count = true;
			//////////////					}
			//////////////					else
			//////////////					{
			//////////////						Heart.SetImageResource(Resource.Drawable.heart_empty);
			//////////////						LoggingClass.LogInfo("UnLiked an item" + myItems[position].WineId, screenid);
			//////////////						x = false;
			//////////////						count = false;
			//////////////					}
			//////////////					SKULike like = new SKULike();
			//////////////					like.UserID = Convert.ToInt32(CurrentUser.getUserId());
			//////////////					like.SKU = Convert.ToInt32(myItems[position].SKU);
			//////////////					like.Liked = x;
			//////////////					ServiceWrapper sw = new ServiceWrapper();
			//////////////					LoggingClass.LogInfo("Liked an item" + myItems[position].WineId, screenid);
			//////////////					like.WineId = myItems[position].WineId;
			//////////////					await sw.InsertUpdateLike(like);
			//////////////				};
			//////////////				////////////////Bitmap imageBitmap = bvb.Bottleimages(myItems[position].WineId);
			//////////////				//////////////// place.SetImageResource(Resource.Drawable.placeholder);
			//////////////				//////////////ProfilePicturePickDialog pppd = new ProfilePicturePickDialog();
			//////////////				//////////////string path = pppd.CreateDirectoryForPictures();
			//////////////				////////////////string path = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
			//////////////				//////////////var filePath = System.IO.Path.Combine(path + "/" + myItems[position].WineId + ".jpg");
			//////////////				//Bitmap imageBitmap;
			//////////////				Bitmap imageBitmap;
			//////////////				imageBitmap = BlobWrapper.Bottleimages(myItems[position].WineId, myItems[position].PlantFinal);
			//////////////				//if (System.IO.File.Exists(filePath))
			//////////////				//{
			//////////////				//	imageBitmap = BitmapFactory.DecodeFile(filePath);
			//////////////				//	Wine.SetImageBitmap(imageBitmap);
			//////////////				//}
			//////////////				//else
			//////////////				{
			//////////////					imageBitmap = BlobWrapper.Bottleimages(myItems[position].WineId, myItems[position].PlantFinal);
			//////////////					Wine.SetImageBitmap(imageBitmap);
			//////////////				}
			//////////////				//Wine.SetImageBitmap(imageBitmap);
			//////////////				var place1 = new RelativeLayout.LayoutParams(620, 620);
			//////////////				var place = new RelativeLayout.LayoutParams(620, 620);
			//////////////				//place1.LeftMargin = parent.Resources.DisplayMetrics.WidthPixels / 2 - 430;
			//////////////				Wine.LayoutParameters = place1;

			//////////////				//var place2 = new RelativeLayout.LayoutParams(620, 620);
			//////////////				//place2.LeftMargin = parent.Resources.DisplayMetrics.WidthPixels / 2 - 430;
			//////////////				//place.LayoutParameters = place2;
			//////////////				//imgPlaceHolder.LayoutParameters = new RelativeLayout.LayoutParams(520, 520);
			//////////////				//Wine.LayoutParameters = new RelativeLayout.LayoutParams(520, 520);
			//////////////				if (imageBitmap != null)
			//////////////				{
			//////////////					if (heartLP.LeftMargin <= 250)
			//////////////					{
			//////////////						place1.LeftMargin = -140;
			//////////////						float ratio = (float)500 / imageBitmap.Height;
			//////////////						imageBitmap = Bitmap.CreateScaledBitmap(imageBitmap, Convert.ToInt32(imageBitmap.Width * ratio), 550, true);
			//////////////					}
			//////////////					else
			//////////////					{
			//////////////						place1.LeftMargin = -70;
			//////////////						float ratio = (float)650 / imageBitmap.Height;
			//////////////						imageBitmap = Bitmap.CreateScaledBitmap(imageBitmap, Convert.ToInt32(imageBitmap.Width * ratio), 650, true);
			//////////////					}


			//////////////					Wine.SetImageBitmap(imageBitmap);

			//////////////					imageBitmap.Dispose();

			//////////////				}
			//////////////				else
			//////////////				{
			//////////////					Wine.SetImageResource(Resource.Drawable.wine7);
			//////////////				}



			//////////////				Name.Focusable = false;
			//////////////				Vintage.Focusable = false;
			//////////////				Price.Focusable = false;
			//////////////				Wine.Focusable = false;
			//////////////				Wine.Dispose();
			//////////////				Heart.Dispose();
			//////////////				heartLP.Dispose();

			//////////////				NotifyDataSetChanged();
			//////////////				LoggingClass.LogInfo("Entered into my fav Adapter", screenid);
			//////////////			}
			//////////////			return row;
			//////////////		}
			//////////////		private int ConvertPixelsToDp(float pixelValue)
			//////////////		{
			//////////////			var dp = (int)((pixelValue) / myContext.Resources.DisplayMetrics.Density);
			//////////////			return dp;
			//////////////		}
			//////////////	}
			//////////////}
			View row = convertView;
			if (row == null)
			
				row = LayoutInflater.From(myContext).Inflate(Resource.Layout.MyFavorite, null, false);
				//else
				//	return row;

				TextView txtName = row.FindViewById<TextView>(Resource.Id.txtName);

				TextView txtVintage = row.FindViewById<TextView>(Resource.Id.txtVintage);

				TextView txtPrice = row.FindViewById<TextView>(Resource.Id.txtPrice);
				ImageView imgWine = row.FindViewById<ImageView>(Resource.Id.imgWine);
         
            ImageView heartImg = row.FindViewById<ImageView>(Resource.Id.imgHeart);
				RatingBar rating = row.FindViewById<RatingBar>(Resource.Id.rtbProductRating);
				rating.Rating = (float)myItems[position].AverageRating;
				txtName.Text = myItems[position].Name;
				txtPrice.Text = myItems[position].SalePrice.ToString("C", Cultures.UnitedState);
         
            txtVintage.Text = myItems[position].Vintage.ToString();
            if (txtVintage.Text == "0" || txtVintage.Text == null)
            {
                txtVintage.Text = "";
            }
            else
            {
                txtVintage.Text = myItems[position].Vintage.ToString();
            }
            heartImg.SetImageResource(Resource.Drawable.heart_empty);
				var heartLP = new FrameLayout.LayoutParams(80, 80);
				var metrics = myContext.Resources.DisplayMetrics;
				var widthInDp = ConvertPixelsToDp(metrics.WidthPixels);
				var heightInDp = ConvertPixelsToDp(metrics.HeightPixels);
				heartLP.LeftMargin = parent.Resources.DisplayMetrics.WidthPixels / 2 - 110; // 110 = 80 + 30

				heartLP.TopMargin = 5;
				heartImg.LayoutParameters = heartLP;
				heartImg.Layout(50, 50, 50, 50);
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
							LoggingClass.LogInfoEx("Liked an item----->" + myItems[position].Barcode, screenid);
							x = true;
							count = true;
						}
						else
						{
							heartImg.SetImageResource(Resource.Drawable.heart_empty);
							LoggingClass.LogInfoEx("UnLiked an item---->" + myItems[position].Barcode, screenid);
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
                        //myItems[position].pl

                        ServiceWrapper sw = new ServiceWrapper();
						await sw.InsertUpdateLike(like);
					};
				}
				Bitmap imageBitmap;
            string url = myItems[position].SmallImageUrl;
            if (url == null)
            {
                url = myItems[position].Barcode + ".jpg";
            }

                imageBitmap = BlobWrapper.Bottleimages(url, storeid);
				var place = new FrameLayout.LayoutParams(650, 650);

				//-650 + (parent.Resources.DisplayMetrics.WidthPixels - imageBitmap.Width) / 2;
				imgWine.LayoutParameters = place;
				if (imageBitmap != null)
				{
					if (heartLP.LeftMargin <= 250)
					{
						place.LeftMargin = -140;
						float ratio = (float)500 / imageBitmap.Height;
						imageBitmap = Bitmap.CreateScaledBitmap(imageBitmap, Convert.ToInt32(imageBitmap.Width * ratio), 550, true);
					}
					else
					{
						place.LeftMargin = -70;
						float ratio = (float)650 / imageBitmap.Height;
						imageBitmap = Bitmap.CreateScaledBitmap(imageBitmap, Convert.ToInt32(imageBitmap.Width * ratio), 650, true);
					}


					imgWine.SetImageBitmap(imageBitmap);


				}
				else
				{
					imgWine.SetImageResource(Resource.Drawable.bottle);
				}

				txtName.Focusable = false;
           
            txtVintage.Focusable = false;
				txtPrice.Focusable = false;
				imgWine.Focusable = false;
				imgWine.Dispose();
			
			
			return row;

		}

		private int ConvertPixelsToDp(float pixelValue)
		{
			var dp = (int)((pixelValue) / myContext.Resources.DisplayMetrics.Density);
			return dp;
		}
	}
}
