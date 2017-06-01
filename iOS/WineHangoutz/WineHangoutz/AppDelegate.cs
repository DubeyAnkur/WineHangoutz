using Foundation;
using UIKit;
using CoreGraphics;
using System.Collections.Generic;
using BigTed;
using WindowsAzure.Messaging;
using System;
using Security;

namespace WineHangoutz
{
	// The UIApplicationDelegate for the application. This class is responsible for launching the
	// User Interface of the application, as well as listening (and optionally responding) to application events from iOS.
	[Register("AppDelegate")]
	public class AppDelegate : UIApplicationDelegate
	{
		protected string deviceToken = string.Empty;
		protected int screenid = 1;
		public string DeviceToken {get { return deviceToken; } }
		public override UIWindow Window
		{
			get;
			set;
		}
		UINavigationController nav;
		public override bool FinishedLaunching(UIApplication application, NSDictionary launchOptions)
		{
			BlobWrapper.DownloadAllImages();
			//string uid_device = UIKit.UIDevice.CurrentDevice.IdentifierForVendor.AsString();

			// Override point for customization after application launch.
			// If not required for your application you can safely delete this method
			UITabBarController RootTab = (UITabBarController)Window.RootViewController;

			//CurrentUser.Clear();
			//CurrentUser.Store("1", "Tester");//for direct login

			UIImage profile = UIImage.FromFile("profile.png");
			profile = ResizeImage(profile, 25, 25);

			UIImage info = UIImage.FromFile("Info.png");
			info = ResizeImage(info, 25, 25);

			var topBtn = new UIBarButtonItem(profile, UIBarButtonItemStyle.Plain, (sender, args) =>
			{
				BTProgressHUD.Show("Loading,,,");
				nav.PushViewController(new ProfileViewController(nav), false);
				nav.NavigationBar.TopItem.Title = "Profile";
				BTProgressHUD.Dismiss();
			});
			var optbtn = new UIBarButtonItem(info, UIBarButtonItemStyle.Plain, (sender, args) =>
			{
				BTProgressHUD.Show("Loading,,,");
				nav.PushViewController(new AboutController1(nav), false);
				nav.NavigationBar.TopItem.Title = "About";
				BTProgressHUD.Dismiss();
			});
			ManageTabBar(RootTab);

			nav = new UINavigationController(Window.RootViewController);
			nav.NavigationBar.TopItem.SetRightBarButtonItem(optbtn, true);
			nav.NavigationBar.TopItem.SetLeftBarButtonItem(topBtn, true);

			Window.RootViewController = nav;

			if (UIDevice.CurrentDevice.CheckSystemVersion(8, 0))
			{
				var pushSettings = UIUserNotificationSettings.GetSettingsForTypes(
					   UIUserNotificationType.Alert | UIUserNotificationType.Badge | UIUserNotificationType.Sound,
					   new NSSet());

				UIApplication.SharedApplication.RegisterUserNotificationSettings(pushSettings);
				UIApplication.SharedApplication.RegisterForRemoteNotifications();
			}
			else
			{
				UIRemoteNotificationType notificationTypes = UIRemoteNotificationType.Alert | UIRemoteNotificationType.Badge | UIRemoteNotificationType.Sound;
				UIApplication.SharedApplication.RegisterForRemoteNotificationTypes(notificationTypes);
			}


			return true;
		}

		public override async void RegisteredForRemoteNotifications(UIApplication application, NSData deviceToken)
		{
			var DeviceToken = deviceToken.Description;
			if (!string.IsNullOrWhiteSpace(DeviceToken))
			{
				DeviceToken = DeviceToken.Trim('<').Trim('>');
				CurrentUser.SetToken(DeviceToken);
			}
			ServiceWrapper svc = new ServiceWrapper();
			int DeviceType = 2;
			await svc.InsertUpdateToken(CurrentUser.GetToken(), CurrentUser.RetreiveUserId().ToString(),DeviceType);
			// Get previous device token
			var oldDeviceToken = NSUserDefaults.StandardUserDefaults.StringForKey("PushDeviceToken");

			// Has the token changed?
			if (string.IsNullOrEmpty(	oldDeviceToken) || !oldDeviceToken.Equals(DeviceToken))
			{
				//TODO: Put your own logic here to notify your server that the device token has changed/been created!
			}

			// Save new device token 
			NSUserDefaults.StandardUserDefaults.SetString(DeviceToken, "PushDeviceToken");

		}
		public override void ReceivedRemoteNotification(UIApplication application, NSDictionary userInfo)
		{
			ProcessNotification(userInfo, false);
		}
		void ProcessNotification(NSDictionary options, bool fromFinishedLaunching)
		{
			if (null != options && options.ContainsKey(new NSString("aps")))
			{
				
				NSDictionary aps = options.ObjectForKey(new NSString("aps")) as NSDictionary;

				string wine = options.ObjectForKey(new NSString("wineid")).ToString();

				string alert = string.Empty;
				string wineid = string.Empty;

				if (aps.ContainsKey(new NSString("alert")))
					alert = (aps[new NSString("alert")] as NSString).ToString();
				wineid = wine;
				if (!fromFinishedLaunching)
				{
					if (wineid == "")
					{
						alert = "Sorry";
						UIAlertView avAlert = new UIAlertView("we were unable to find tasted wines", alert, null, "Ok", null);
						avAlert.Show();
					}
					else
					{
						int storeid = 2;
						nav.PushViewController(new SKUDetailView(wineid,storeid.ToString()), false);
						BTProgressHUD.Dismiss();
					}
				}
			}
		}

		private void ManageTabBar(UITabBarController RootTab)
		{
			UITabBar tabBar = RootTab.TabBar;
			//UITabBar.Appearance.BackgroundColor = UIColor.Red;
			//UITabBar.Appearance.BackgroundImage = UIImage.FromFile("Star4.png");
			UITabBarItem t0 = tabBar.Items[0];
			t0.Title = "Shop";
			UIImage shop = UIImage.FromFile("shop.png");
			shop = ResizeImage(shop, 32, 32);
			t0.Image = shop;
			t0.SelectedImage = shop;

			UITabBarItem t1 = tabBar.Items[1];
			//t1 = new UITabBarItem();
			t1.Title = "My Hangouts";
			UIImage Taste = UIImage.FromFile("taste.png");
			Taste = ResizeImage(Taste, 35, 35);
			t1.Image = Taste;
			t1.SelectedImage = Taste;

			UITabBarItem t2 = tabBar.Items[2];
			t2.Title = "Explore";
			UIImage explore = UIImage.FromFile("explore.png");
			explore = ResizeImage(explore, 35, 35);
			t2.Image = explore;
			t2.SelectedImage = explore;
			t2.Enabled = false;
			//RootTab[2].RemoveFromParentViewController();


		}

		public UIImage ResizeImage(UIImage sourceImage, float width, float height)
		{
			UIGraphics.BeginImageContext(new CGSize(width, height));
			sourceImage.Draw(new CGRect(0, 0, width, height));
			var resultImage = UIGraphics.GetImageFromCurrentImageContext();
			UIGraphics.EndImageContext();
			return resultImage;
		}

		//public override string UniqueID
		//{
		//	get
		//	{
		//		var query = new SecRecord(SecKind.GenericPassword);
		//		query.Service = NSBundle.MainBundle.BundleIdentifier;
		//		query.Account = "UniqueID";

		//		NSData uniqueId = SecKeyChain.QueryAsData(query);
		//		if (uniqueId == null)
		//		{
		//			query.ValueData = NSData.FromString(System.Guid.NewGuid().ToString());
		//			var err = SecKeyChain.Add(query);
		//			if (err != SecStatusCode.Success && err != SecStatusCode.DuplicateItem)
		//				throw new Exception("Cannot store Unique ID");

		//			return query.ValueData.ToString();
		//		}
		//		else
		//		{
		//			return uniqueId.ToString();
		//		}
		//	}
		//}


		public override void OnResignActivation(UIApplication application)
		{
			// Invoked when the application is about to move from active to inactive state.
			// This can occur for certain types of temporary interruptions (such as an incoming phone call or SMS message) 
			// or when the user quits the application and it begins the transition to the background state.
			// Games should use this method to pause the game.
		}

		public override void DidEnterBackground(UIApplication application)
		{
			// Use this method to release shared resources, save user data, invalidate timers and store the application state.
			// If your application supports background exection this method is called instead of WillTerminate when the user quits.
		}

		public override void WillEnterForeground(UIApplication application)
		{
			// Called as part of the transiton from background to active state.
			// Here you can undo many of the changes made on entering the background.
		}

		public override void OnActivated(UIApplication application)
		{
			// Restart any tasks that were paused (or not yet started) while the application was inactive. 
			// If the application was previously in the background, optionally refresh the user interface.
		}

		public override void WillTerminate(UIApplication application)
		{
			// Called when the application is about to terminate. Save data, if needed. See also DidEnterBackground.
		}

	}
}

