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

        public MyReviewAdapter(Context con, List<Review> strArr )
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
        //public EventHandler Edit_Click;
        //public EventHandler Delete_Click;

       
        public override long GetItemId(int position)
        {
            return position;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            View row = convertView;
			if (myItems.Count == 0)

			{

				AlertDialog.Builder aler = new AlertDialog.Builder(myContext);
				//aler.SetTitle("No Reviews Avalilable");
				aler.SetMessage("Sorry you haven't Reviewed our wines");
				aler.SetNegativeButton("Ok", delegate { });
				LoggingClass.LogInfo("Clicked on Secaucus", screenid);
				Dialog dialog = aler.Create();
				dialog.Show();
			}
			else
			{
				if (row == null)
				{
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
					RatingBar rb = row.FindViewById<RatingBar>(Resource.Id.rating);
					ImageView heartImg = row.FindViewById<ImageView>(Resource.Id.imageButton4);
					heartImg.SetImageResource(Resource.Drawable.Heart_emp);
					//edit.SetScaleType(ImageView.ScaleType.Center);
					//delete.SetScaleType(ImageView.ScaleType.Center);
					//edit.SetImageResource(Resource.Drawable.edit);
					//delete.SetImageResource(Resource.Drawable.delete);
					edit.Focusable = false;
					//edit.FocusableInTouchMode = false;
					edit.Clickable = true;
					delete.Focusable = false;
					//delete.FocusableInTouchMode = false;
					delete.Clickable = true;
					wineimage.Focusable = false;
					wineimage.FocusableInTouchMode = false;
					wineimage.Clickable = true;
					//TextView txtPrice = row.FindViewById<TextView>(Resource.Id.txtPrice);
					//ImageView imgWine = row.FindViewById<ImageView>(Resource.Id.imgWine);
					//edit.SetTag(1, 5757);
					edit.Click += (sender, args) =>
					{

						string WineBarcode = myItems[position].Barcode;
						Review _review = new Review();
						_review.Barcode = WineBarcode;
						_review.RatingStars = myItems[position].RatingStars;
						_review.RatingText = myItems[position].RatingText;
                        _review.PlantFinal = myItems[position].PlantFinal;
						LoggingClass.LogInfo("clicked on edit  an item---->"+ WineBarcode + "----->"+ _review.RatingStars+"---->"+_review.RatingText, screenid);
						PerformItemClick(sender, args, _review);
					};
					//delete.Click += Delete_Click;
					delete.Click += (sender, args) =>
					{
						string WineBarcode = myItems[position].Barcode;

						Review _review = new Review();
						_review.Barcode = WineBarcode;
						LoggingClass.LogInfo("clicked on delete item--->" + WineBarcode, screenid);
						PerformdeleteClick(sender, args, _review);

					};
					wineimage.Click += (sender, args) => Console.WriteLine("ImageButton {0} clicked", position);
					txtDate.SetTextSize(Android.Util.ComplexUnitType.Dip, 12);
					txtName.Text = myItems[position].Name;
					// txtName.InputType = Android.Text.InputTypes.TextFlagNoSuggestions;
					// txtPrice.Text= myItems[position].
					txtYear.Text = myItems[position].Vintage;
					txtDescription.Text = myItems[position].RatingText;
					txtDescription.InputType = Android.Text.InputTypes.TextFlagNoSuggestions;
					txtDate.Text = myItems[position].Date.ToString("dd/MM/yyyy");
					rb.Rating = myItems[position].RatingStars;
					//Bitmap imageBitmap = bvb.Bottleimages(myItems[position].WineId);
					ProfilePicturePickDialog pppd = new ProfilePicturePickDialog();
					string path = pppd.CreateDirectoryForPictures();
					//string path = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
					var filePath = System.IO.Path.Combine(path + "/" + myItems[position].Barcode + ".jpg");


					bool count = true;//Convert.ToBoolean(myItems[position].IsLike);
					if (count == true)
					{
						heartImg.SetImageResource(Resource.Drawable.HeartFull);
					}
					else
					{
						heartImg.SetImageResource(Resource.Drawable.Heart_emp);
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
								heartImg.SetImageResource(Resource.Drawable.HeartFull);
								LoggingClass.LogInfoEx("Liked an item------>" + myItems[position].Barcode, screenid);
								x = true;
								count = true;
							}
							else
							{
								heartImg.SetImageResource(Resource.Drawable.Heart_emp);
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
					if (System.IO.File.Exists(filePath))
					{
						imageBitmap = BitmapFactory.DecodeFile(filePath);
						wineimage.SetImageBitmap(imageBitmap);
					}
					else
					{
						imageBitmap = BlobWrapper.Bottleimages(myItems[position].Barcode, Convert.ToInt32(myItems[position].PlantFinal));

						wineimage.SetImageBitmap(imageBitmap);
					}
					//wineimage.SetImageBitmap(imageBitmap);
					//wineimage.SetImageResource(Resource.Drawable.wine7);
					wineimage.SetScaleType(ImageView.ScaleType.CenterCrop);

					txtName.Focusable = false;
					txtYear.Focusable = false;
					txtDescription.Focusable = false;
					txtDate.Focusable = false;

				}
				
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

       
    }
}