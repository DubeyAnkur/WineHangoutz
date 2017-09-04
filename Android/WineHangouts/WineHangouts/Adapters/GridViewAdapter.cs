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
using System.Collections;
using System.Threading.Tasks;
using System.Globalization;
using Android.Graphics.Drawables;
using Newtonsoft.Json;



namespace WineHangouts
{
    class GridViewAdapter : BaseAdapter<Item>
    {
        private List<Item> myItems;
        private Context myContext;
        private int screenid = 15;
		public int storeid;
        //private Hashtable wineImages;

        public void ClearData()
        {
            myItems.Clear();
            NotifyDataSetChanged();
        }
        public void FeedData(IEnumerable<Item> newData)
        {
            myItems.AddRange(newData);
            NotifyDataSetChanged();
        }
        public override Item this[int position]
        {
            get
            {
                return myItems[position];
            }
        }

        public GridViewAdapter(Context con, List<Item> strArr,int storeId)
        {
		
			myContext = con;
            myItems = strArr;
			storeid = storeId;
			
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
            View row = convertView;
			if (row == null)
			
				row = LayoutInflater.From(myContext).Inflate(Resource.Layout.cell, null, false);
				//else
				//    return row;

				TextView txtName = row.FindViewById<TextView>(Resource.Id.txtName);

				TextView txtVintage = row.FindViewById<TextView>(Resource.Id.txtVintage);
				TextView AmountLeft = row.FindViewById<TextView>(Resource.Id.txtAmountLeft);
				TextView txtPrice = row.FindViewById<TextView>(Resource.Id.txtPrice);
				ImageView imgWine = row.FindViewById<ImageView>(Resource.Id.imgWine);
				ImageView heartImg = row.FindViewById<ImageView>(Resource.Id.imgHeart);
				RatingBar rating = row.FindViewById<RatingBar>(Resource.Id.rtbProductRating);
				rating.Rating = (float)myItems[position].AverageRating;
				txtName.Text = myItems[position].Name;
				txtPrice.Text = myItems[position].SalePrice.ToString("C", Cultures.UnitedState);
				AmountLeft.Text = "Wine left in bottle: " + myItems[position].AvailableVolume.ToString() + " ml";
				txtVintage.Text = myItems[position].Vintage.ToString();
                if(txtVintage.Text=="0"||txtVintage.Text==null)
            {
                txtVintage.Text = "";
            }else
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
               
                    heartImg.Click +=  delegate
                    {
                        if (CurrentUser.getUserId() == null)
                        {
                            AlertDialog.Builder aler = new AlertDialog.Builder(myContext, Resource.Style.MyDialogTheme);
                            aler.SetTitle("Sorry");
                            aler.SetMessage("This Feature is available for VIP Users only");
                            aler.SetNegativeButton("Ok", delegate
                            {
                                LoggingClass.LogInfo("Closed PoPup", screenid);
                            });
                            Dialog dialog1 = aler.Create();
                            dialog1.Show();
                        }
                        else
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
                            //await sw.InsertUpdateLike(like);
                        }
                    };
               // }
            }
            

				Bitmap imageBitmap;
				imageBitmap = BlobWrapper.Bottleimages(myItems[position].SmallImageUrl, storeid);
				var place = new FrameLayout.LayoutParams(650, 650);

				//-650 + (parent.Resources.DisplayMetrics.WidthPixels - imageBitmap.Width) / 2;
				imgWine.LayoutParameters = place;
			//var place1 = new FrameLayout.LayoutParams(600, 500);

			////-650 + (parent.Resources.DisplayMetrics.WidthPixels - imageBitmap.Width) / 2;
			//imgWine.LayoutParameters = place1;
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

					imageBitmap.Dispose();

				}
				else
				{
				if (heartLP.LeftMargin <= 250)
				{
					place.LeftMargin = -140;
					imgWine.SetImageResource(Resource.Drawable.bottle);
				}
				else
				{
					place.LeftMargin = -70;
					imgWine.SetImageResource(Resource.Drawable.bottle);
				}
			}

				txtName.Focusable = false;
				AmountLeft.Focusable = false;
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