using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using System.Collections.Generic;
using Hangout.Models;
using System.Linq;
using Android.Util;
using System.Diagnostics;
using AndroidHUD;

namespace WineHangouts
{
	[Activity(Label = "My Favorites", ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
	public class MyFavoriteAvtivity : Activity
	{
		public string StoreName = "";
		private int screenid = 7;
		public Context parent;
		protected override void OnCreate(Bundle bundle)
		{
			Stopwatch st = new Stopwatch();
			st.Start();
			base.OnCreate(bundle);
			try
			{
				//SetContentView(Resource.Layout.MyFavoriteGridView);
				ActionBar.SetHomeButtonEnabled(true);
				ActionBar.SetDisplayHomeAsUpEnabled(true);
				if (StoreName == "")
					StoreName = Intent.GetStringExtra("MyData");
				this.Title = StoreName;
                int userId = Convert.ToInt32(CurrentUser.getUserId());
				ServiceWrapper sw = new ServiceWrapper();
				ItemListResponse output = new ItemListResponse();
				output = sw.GetItemFavsUID(userId).Result;
				List<Item> myArr;
				myArr = output.ItemList.ToList();
				if (output.ItemList.Count == 0)
				{
                    SetContentView(Resource.Layout.FavEmp);
                    TextView txtName = FindViewById<TextView>(Resource.Id.textView1);
                    ImageView Imag = FindViewById<ImageView>(Resource.Id.imageView1);
                    //AlertDialog.Builder aler = new AlertDialog.Builder(this, Resource.Style.MyDialogTheme);
                    ////aler.SetTitle("No Reviews Avalilable");
                    //aler.SetMessage("Sorry you didn't tell us your Favourite wines");
                    //LoggingClass.LogInfo("Sorry you didn't tell us your Favourite wines", screenid);
                    //aler.SetNegativeButton("Ok", delegate { Finish(); });
                    //LoggingClass.LogInfo("Clicked on Secaucus", screenid);
                    //Dialog dialog = aler.Create();
                    //dialog.Show();
                }
				else
				{
                    SetContentView(Resource.Layout.MyFavoriteGridView);
                    var gridview = FindViewById<GridView>(Resource.Id.gridviewfav);
					MyFavoriteAdapter adapter = new MyFavoriteAdapter(this, myArr);
					LoggingClass.LogInfo("Entered into Favourite Adapter", screenid);
					gridview.SetNumColumns(2);
					gridview.Adapter = adapter;
					gridview.ItemClick += delegate (object sender, AdapterView.ItemClickEventArgs args)
                    {
                        try
                        {
                            string WineBarcode = myArr[args.Position].Barcode;
                            int storeid =1;// myArr[args.Position].PlantFinal;
                            //ProgressIndicator.Show(this);
                            AndHUD.Shared.Show(this, "Loading...", Convert.ToInt32(MaskType.Clear));
                            var intent = new Intent(this, typeof(DetailViewActivity));
                            LoggingClass.LogInfo("Clicked on " + myArr[args.Position].Barcode + " to enter into wine details", screenid);
                            intent.PutExtra("WineBarcode", WineBarcode);
                            intent.PutExtra("storeid", storeid);
                            StartActivity(intent);
                        }
                        catch (Exception)
                        {
                            string WineBarcode = myArr[args.Position].Barcode;
                            int storeid = myArr[args.Position].PlantFinal;
                            ProgressIndicator.Show(this);
                            AndHUD.Shared.Dismiss();
                            var intent = new Intent(this, typeof(DetailViewActivity));
                            LoggingClass.LogInfo("Clicked on " + myArr[args.Position].Barcode + " to enter into wine details", screenid);
                            intent.PutExtra("WineBarcode", WineBarcode);
                            intent.PutExtra("storeid", storeid);
                            StartActivity(intent);
                        }
					};

					LoggingClass.LogInfo("Entered into My Favorites Activity", screenid);
				}
				st.Stop();
				LoggingClass.LogTime("Favouriteactivity", st.Elapsed.TotalSeconds.ToString());
				ProgressIndicator.Hide();
			}

			catch (Exception exe)
			{
				LoggingClass.LogError(exe.Message, screenid, exe.StackTrace.ToString());
				ProgressIndicator.Hide();
				AlertDialog.Builder aler = new AlertDialog.Builder(this);
				aler.SetTitle("Sorry");
				aler.SetMessage("We're under maintainence");
				aler.SetNegativeButton("Ok", delegate { });
				Dialog dialog = aler.Create();
				dialog.Show();
			}
		}
		protected override void OnPause()
		{
			base.OnPause();
			LoggingClass.LogInfo("OnPause state in Favourite activity---->" + StoreName, screenid);

		}

		protected override void OnResume()
		{
			base.OnResume();
			LoggingClass.LogInfo("OnResume state in Favourite activity--->" + StoreName, screenid);
		}

		public override bool OnOptionsItemSelected(IMenuItem item)
		{
			if (item.ItemId == Android.Resource.Id.Home)
			{
				base.OnBackPressed();
				LoggingClass.LogInfo("Exited from My Favorites", screenid);
				return false;
			}
			return base.OnOptionsItemSelected(item);
		}
	}
}