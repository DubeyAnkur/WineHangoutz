using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Hangout.Models;
using Android.Widget;
using Android.Graphics;
using System.Net;

namespace WineHangouts
{
	[Activity(Label = "Review Adapter")]
	public class reviewAdapter : BaseAdapter<Review>
	{
		private List<Review> myItems;
		private Context myContext;
		private int screenid = 24;

		int userId = Convert.ToInt32(CurrentUser.getUserId());
		public override Review this[int position]
		{
			get
			{
				return myItems[position];
			}
		}

		public reviewAdapter(Context con, List<Review> strArr)
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

			
				if (row == null)
					row = LayoutInflater.From(myContext).Inflate(Resource.Layout.CommentsCell, null, false);
				TextView Name = row.FindViewById<TextView>(Resource.Id.textView64);
				TextView Comments = row.FindViewById<TextView>(Resource.Id.textView66);
				TextView date = row.FindViewById<TextView>(Resource.Id.textView67);
				RatingBar rb = row.FindViewById<RatingBar>(Resource.Id.rtbProductRating);
				ImageView Image = row.FindViewById<ImageView>(Resource.Id.imageButton2);
				Image.SetScaleType(ImageView.ScaleType.CenterCrop);
				Bitmap imageBitmap = BlobWrapper.ProfileImages(myItems[position].ReviewUserId);
				if (imageBitmap == null)
				{
					Image.SetImageResource(Resource.Drawable.ProfileEmpty);

				}

				else
				{
					Image.SetImageBitmap(imageBitmap);

				}
				///imageBitmap.Dispose();
				//ProfilePicturePickDialog pppd = new ProfilePicturePickDialog();
				//string path = pppd.CreateDirectoryForPictures();
				////string path = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
				////It's taking lot of time to load user images so giving wine id, after completing compressing image we will give reviewuserid
				//var filePath = System.IO.Path.Combine(path + "/" + myItems[position].ReviewUserId + ".jpg");
				//if (System.IO.File.Exists(filePath))
				//{
				//    imageBitmap = BitmapFactory.DecodeFile(filePath);
				//    Image.SetImageBitmap(imageBitmap);
				//}
				//else
				//{
				//    //It's taking lot of time to load user images so giving wine id, after completing compressing image we will give reviewuserid
				//    imageBitmap = BlobWrapper.ProfileImages(myItems[position].ReviewUserId);
				//    if(imageBitmap==null)
				//    {
				//        Image.SetImageResource(Resource.Drawable.user1);
				//    }
				//    else
				//    { 
				//    Image.SetImageBitmap(imageBitmap);
				//    }
				//}
				Name.Text = myItems[position].Username;
				Name.InputType = Android.Text.InputTypes.TextFlagNoSuggestions;
				Comments.Text = myItems[position].RatingText;
				date.Text = myItems[position].Date.ToString("yyyy/MM/dd");
				rb.Rating = myItems[position].RatingStars;

            //Image.SetImageBitmap(imageBitmap);

            return row;


        }
			
			
		


	}
}