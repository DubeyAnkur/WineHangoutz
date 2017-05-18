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

namespace WineHangouts
{
	[Activity(Label = "My Favorites")]
	public class MyFavoriteAvtivity : Activity
	{
		public string StoreName = "";
        private int screenid = 7;
		protected override void OnCreate(Bundle bundle)
		{
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
				
				var gridview = FindViewById<GridView>(Resource.Id.gridviewfav);
				MyFavoriteAdapter adapter = new MyFavoriteAdapter(this, myArr);

				gridview.SetNumColumns(2);
				gridview.Adapter = adapter;
				gridview.ItemClick += delegate (object sender, AdapterView.ItemClickEventArgs args)
				{
					int WineID = myArr[args.Position].WineId;
                    int storeid = myArr[args.Position].PlantFinal;
                    ProgressIndicator.Show(this);
					var intent = new Intent(this, typeof(DetailViewActivity));
					intent.PutExtra("WineID", WineID);
                    intent.PutExtra("storeid", storeid);
                    StartActivity(intent);
				};
				ProgressIndicator.Hide();
                LoggingClass.LogInfo("Entered into My Favorites Activity", screenid);
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