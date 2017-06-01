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
            if (row == null)
                row = LayoutInflater.From(myContext).Inflate(Resource.Layout.MyTastingView, null, false);
            else
                return convertView;
			if (myItems.Count == 0)
			{
				AlertDialog.Builder aler = new AlertDialog.Builder(myContext);
				//aler.SetTitle("No Reviews Avalilable");
				aler.SetMessage("Sorry you haven't Tasted our wines");
				aler.SetNegativeButton("Ok", delegate { });
				LoggingClass.LogInfo("Clicked on Secaucus", screenid);
				Dialog dialog = aler.Create();
				dialog.Show();
			}
			else
			{

				TextView txtName = row.FindViewById<TextView>(Resource.Id.SkuName);
				TextView txtYear = row.FindViewById<TextView>(Resource.Id.Vintage);
				TextView txtDescription = row.FindViewById<TextView>(Resource.Id.TastingNotes);
				TextView txtDate = row.FindViewById<TextView>(Resource.Id.Date);
				TextView txtPrice = row.FindViewById<TextView>(Resource.Id.Price);

				ImageButton wineimage = row.FindViewById<ImageButton>(Resource.Id.imageButton2);
				RatingBar rb = row.FindViewById<RatingBar>(Resource.Id.AvgRating);



				txtDate.SetTextSize(Android.Util.ComplexUnitType.Dip, 12);
				txtName.Text = myItems[position].Name;
				txtName.InputType = Android.Text.InputTypes.TextFlagNoSuggestions;
				txtYear.Text = myItems[position].Vintage.ToString();
				txtDescription.Text = myItems[position].Description;
				txtDate.Text = myItems[position].TastingDate.ToString();
				txtPrice.Text = myItems[position].SalePrice.ToString("C", GridViewAdapter.Cultures.UnitedState);
				rb.Rating = (float)myItems[position].AverageRating;
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
					imageBitmap = BlobWrapper.Bottleimages(myItems[position].WineId, myItems[position].PlantFinal);

					wineimage.SetImageBitmap(imageBitmap);
				}
				wineimage.SetScaleType(ImageView.ScaleType.CenterCrop);


				txtName.Focusable = false;
				txtYear.Focusable = false;
				txtDescription.Focusable = false;
				txtDate.Focusable = false;


				
			}
			LoggingClass.LogInfo("Entered into My tastings Adapter", screenid);
			return row;
        }
    }
}