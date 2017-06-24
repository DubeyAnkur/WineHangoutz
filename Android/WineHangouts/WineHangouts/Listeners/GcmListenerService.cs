using Android.App;
using Android.Content;
using Android.OS;
using Android.Gms.Gcm;
using Android.Util;

namespace WineHangouts
{
    [Service (Exported = false), IntentFilter (new [] { "com.google.android.c2dm.intent.RECEIVE" })]
    public class MyGcmListenerService : GcmListenerService
    {
        public override void OnMessageReceived (string from, Bundle data)
        {
            // Extract the message received from GCM:
            var message = data.GetString("message");
            string WineBarcode = data.GetString("barcode");
            Log.Debug("MyGcmListenerService", "From:    " + from);
            Log.Debug("MyGcmListenerService", "Message: " + message);
            //var intent = new Intent(this, typeof(detailViewActivity));
            //intent.PutExtra("WineID", wineId);
            // Forward the received message in a local notification:
            SendNotification(message, WineBarcode);
        }

        // Use Notification Builder to create and launch the notification:
        void SendNotification(string message, string WineBarcode)
        {
            var intent = new Intent(this, typeof(DetailViewActivity));
            intent.PutExtra("WineBarcode", WineBarcode);
            intent.AddFlags(ActivityFlags.ClearTop);
            var pendingIntent = PendingIntent.GetActivity(this, 0, intent, PendingIntentFlags.OneShot);

            var notificationBuilder = new Notification.Builder(this)
                .SetSmallIcon(Resource.Drawable.logo5)
                .SetContentTitle("Wine Hangouts")
                .SetContentText(message)
                .SetAutoCancel(false)
                .SetContentIntent(pendingIntent);

            var notificationManager = (NotificationManager)GetSystemService(Context.NotificationService);
            notificationManager.Notify(0, notificationBuilder.Build());
        }

    }
}
