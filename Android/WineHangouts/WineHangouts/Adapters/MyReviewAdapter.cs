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
					int WineId = myItems[position].WineId;
					Review _review = new Review();
					_review.WineId = WineId;
					_review.RatingStars = myItems[position].RatingStars;
					_review.RatingText = myItems[position].RatingText;
					PerformItemClick(sender, args, _review);
				};
				//delete.Click += Delete_Click;
				delete.Click += (sender, args) =>
				{
					int WineId = myItems[position].WineId;

					Review _review = new Review();
					_review.WineId = WineId;

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
				var filePath = System.IO.Path.Combine(path + "/" + myItems[position].WineId + ".jpg");
				Bitmap imageBitmap;
				if (System.IO.File.Exists(filePath))
				{
					imageBitmap = BitmapFactory.DecodeFile(filePath);
					wineimage.SetImageBitmap(imageBitmap);
				}
				else
				{
					imageBitmap = BlobWrapper.Bottleimages(myItems[position].WineId,Convert.ToInt32( myItems[position].PlantFinal));

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