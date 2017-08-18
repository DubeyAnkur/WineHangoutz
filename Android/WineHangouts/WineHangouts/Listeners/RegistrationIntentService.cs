using System;
using Android.App;
using Android.Content;
using Android.Util;
using Android.Gms.Gcm;
using Android.Gms.Iid;
using Firebase.Messaging;
using Firebase;
using Android.Widget;

namespace WineHangouts
{
    // This intent service receives the registration token from GCM:
    [Service (Exported = false)]
    public class RegistrationIntentService : IntentService
    {
        // Notification topics that I subscribe to:
        static readonly string[] Topics = { "global" };

        // Create the IntentService, name the worker thread for debugging purposes:
        public RegistrationIntentService() : base ("RegistrationIntentService")
        { }

        // OnHandleIntent is invoked on a worker thread:
        protected override void OnHandleIntent (Intent intent)
        {
            try
            {
                Log.Info("RegistrationIntentService", "Calling InstanceID.GetAuthToken");
                //FirebaseApp.InitializeApp(this);
                // Ensure that the request is atomic:
                lock (this)
                {
                    // Request a registration token:
                    var instanceID = InstanceID.GetInstance(this);
                    var token = instanceID.GetToken("509579216493", GoogleCloudMessaging.InstanceIdScope, null);
                    //Toast.MakeText(this, token, ToastLength.Short).Show();
                    // Log the registration token that was returned from GCM:
                    Log.Info("RegistrationIntentService", "GCM Registration Token: " + token);
                    CurrentUser.PutDeviceToken(token);
                    // Send to the app server (if it requires it):
                    //SendRegistrationToAppServer(token);
                   
                    //FirebaseMessaging.Instance.SubscribeToTopic("global");
                    // Subscribe to receive notifications:
                    SubscribeToTopics(token, Topics);
                }
            }
            catch (Exception e)
            {
                Log.Debug("RegistrationIntentService", "Failed to get a registration token",e.Message);
            }
        }

        //public async void SendRegistrationToAppServer(string token)
        //{
        //    TokenModel _token = new TokenModel();
        //    _token.User_id = Convert.ToInt32(CurrentUser.getUserId());
        //    _token.DeviceToken = token;
        //    ServiceWrapper svc = new ServiceWrapper();
        //    int x= await svc.InsertUpdateToken(_token);
        //    // Add custom implementation here as needed.
        //}

        // Subscribe to topics to receive notifications from the app server:
       private void SubscribeToTopics (string token, string[] topics)
        {
            foreach (var topic in topics)
            {
                var pubSub = GcmPubSub.GetInstance(this);
                pubSub.Subscribe(token, "/topics/" + topic, null);
            }
        }
    }
}
