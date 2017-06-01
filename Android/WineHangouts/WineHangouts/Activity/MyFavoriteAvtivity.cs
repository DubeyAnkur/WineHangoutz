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

namespace WineHangouts
{
	[Activity(Label = "My Favorites")]
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
				SetContentView(Resource.Layout.MyFavoriteGridView);
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
					AlertDialog.Builder aler = new AlertDialog.Builder(this);
					//aler.SetTitle("No Reviews Avalilable");
					aler.SetMessage("Sorry you didn't tell us your Favourite wines");
					LoggingClass.LogInfo("Sorry you didn't tell us your Favourite wines", screenid);
					aler.SetNegativeButton("Ok", delegate { Finish(); });
					LoggingClass.LogInfo("Clicked on Secaucus", screenid);
					Dialog dialog = aler.Create();
					dialog.Show();
				}
				else
				{

					var gridview = FindViewById<GridView>(Resource.Id.gridviewfav);
					MyFavoriteAdapter adapter = new MyFavoriteAdapter(this, myArr);
					LoggingClass.LogInfo("Entered into Favourite Adapter", screenid);
					gridview.SetNumColumns(2);
					gridview.Adapter = adapter;
					gridview.ItemClick += delegate (object sender, AdapterView.ItemClickEventArgs args)
					{
						int WineID = myArr[args.Position].WineId;
						int storeid = myArr[args.Position].PlantFinal;
						ProgressIndicator.Show(this);
						var intent = new Intent(this, typeof(DetailViewActivity));
						LoggingClass.LogInfo("Clicked on " + myArr[args.Position].WineId + " to enter into wine details", screenid);
						intent.PutExtra("WineID", WineID);
						intent.PutExtra("storeid", storeid);
						StartActivity(intent);
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