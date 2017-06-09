using System;
using System.Collections.Generic;
using System.Net;
using System.IO;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;
using Hangout.Models;
using System.Timers;
using System.Windows;
using System.Diagnostics;


namespace WineHangouts
{
    public class ServiceWrapper
    {
        HttpClient client;
        private int screenid = 19;
		public string error;
        public ServiceWrapper()
        {
            client = new HttpClient();
            //client.MaxResponseContentBufferSize = 256000;
        }
	

        public string ServiceURL
        {
            get
            {

                string host = "https://hangoutz.azurewebsites.net/";
                return host + "api/Item/";
            }

        }
		//private static System.Timers.Timer aTimer;
		//private static void SetTimer()
		//{
		//	// Create a timer with a two second interval.
		//	aTimer = new System.Timers.Timer(2000);
		//	// Hook up the Elapsed event for the timer. 
		//	aTimer.Elapsed += OnTimedEvent;
		//	aTimer.AutoReset = true;
		//	aTimer.Enabled = true;
		//}

		//private static void OnTimedEvent(Object source, ElapsedEventArgs e)
		//{
		//	Console.WriteLine("The Elapsed event was raised at {0:HH:mm:ss.fff}",
		//					  e.SignalTime);
		//}


		Stopwatch sw = new Stopwatch();
		
		public async Task<string> GetDataAsync()
        {
            var uri = new Uri(ServiceURL + "TestService/1");
            var response = await client.GetAsync(uri);
            string output = "";
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                output = JsonConvert.DeserializeObject<string>(content);
            }
            return output;
        }
		
        public async Task<ItemListResponse> GetItemList(int storeId, int userId)
        {
			
            ItemListResponse output = null;
			//SetTimer();
			sw.Start();
			
			
			LoggingClass.LogServiceInfo("Service called", "GetItemList");

            try
            {
                var uri = new Uri(ServiceURL + "GetItemList/" + storeId + "/user/" + userId);
                var response = await client.GetStringAsync(uri).ConfigureAwait(false);
                output = JsonConvert.DeserializeObject<ItemListResponse>(response);
                LoggingClass.LogServiceInfo("Service Response", "GetItemList");
            }
            catch (Exception exe)
            {
                LoggingClass.LogError(exe.Message, screenid, exe.StackTrace.ToString());
            }
			//aTimer.Stop();Console.WriteLine("The timer ran for " + sw.Elapsed.TotalSeconds);
			//aTimer.Dispose();
			sw.Stop();
			
			LoggingClass.LogTime("The total time to  start and end the service getItemList", "The timer ran for " + sw.Elapsed.TotalSeconds);
			return output;
			
		}

        public async Task<ItemDetailsResponse> GetItemDetails(int wineid,int storeid)
        {
            ItemDetailsResponse output = null;
			sw.Start();

			LoggingClass.LogServiceInfo("Service Called", "GetItemDetails");
            try
            {
                var uri = new Uri(ServiceURL + "GetItemDetails/" + wineid+"/user/"+storeid);
                var response = await client.GetStringAsync(uri).ConfigureAwait(false);
                output = JsonConvert.DeserializeObject<ItemDetailsResponse>(response);
                LoggingClass.LogServiceInfo("Service Response", "GetItemDetails");
            }
            catch (Exception exe)
            {
                LoggingClass.LogError(exe.Message, screenid, exe.StackTrace.ToString());
            }
			sw.Stop();

			LoggingClass.LogTime("The total time to  start and end the service GetItemDetails", "The timer ran for " + sw.Elapsed.TotalSeconds);

			return output;
        }
        public async Task<int> InsertUpdateLike(SKULike skuLike)
        {
			sw.Start();
			try
            {
                LoggingClass.LogServiceInfo("service called", "InsertUpdateLike");
                var uri = new Uri(ServiceURL + "InsertUpdateLike/");
                var content = JsonConvert.SerializeObject(skuLike);
                var cont = new StringContent(content, System.Text.Encoding.UTF8, "application/json");
                var response = await client.PostAsync(uri, cont); // In debug mode it do not work, Else it works
                //var result = response.Content.ReadAsStringAsync().Result;
                LoggingClass.LogServiceInfo("service response", "InsertUpdateLike");
            }
            catch (Exception exe)
            {
                LoggingClass.LogError(exe.Message, screenid, exe.StackTrace.ToString());
            }
			sw.Stop();

			LoggingClass.LogTime("The total time to  start and end the service InsertUpdateLike", "The timer ran for " + sw.Elapsed.TotalSeconds);

			return 1;
        }

        public async Task<CustomerResponse> AuthencateUser(string CardID,string Email,string DeviceID)
        {
			sw.Start();
			CustomerResponse output = null;
            try
            {
                LoggingClass.LogServiceInfo("service called", "AuthencateUser");
				var uri = new Uri(ServiceURL + "AuthenticateUser/" + CardID + "/email/" + Email + "/DeviceId/" + DeviceID);
				//var uri = new Uri(ServiceURL + "AuthenticateUser/" + UserName);
                var response = await client.GetStringAsync(uri).ConfigureAwait(false);
				//{ "customer":{ "CustomerID"0,"FirstName""UNKNOWN","LastName"null,"PhoneNumber"null,
				//		"Phone2"null,"Email"null,"Address1"null,"Address2"null,"City"null,"State"null,
				//		"CustomerType":null,"CustsomerAdded":"0001-01-01T00:00:00","CardNumber":null,"Notes1":
				//		null,"IsUpdated":0,"PreferredStore":0,"IsMailSent":0},"ErorCode":0,"ErrorDescription":The credentials you entered did not match our records. \nPlease double-check and try again.
				//	"The credentials you entered did not match our records. \nPlease double-check and try again."}
			//	error = response.ToString();
			//	error = error.Replace(":", "");
			//	error = error.Replace(",", "");
			//error = error.Replace('"', ' ');
				
			//	error = error.Replace('{', ' ');
			//	error = error.Replace('}', ' ');
			//	error = error.Replace("customer    CustomerID  0  FirstName   UNKNOWN   LastName  null  PhoneNumber  null  Phone2  null  Email  null  Address1  null", "");
			//	error = error.Replace("Address2  null  City  null  State  null  CustomerType  null  CustsomerAdded   0001-01-01T00 00 00   CardNumber  null  Notes1  null  IsUpdated  0  PreferredStore  0  IsMailSent  0   ErorCode  0  ErrorDescription  ","" ); ;
			//	error = error.Replace(' ',' ');
			
					
				output = JsonConvert.DeserializeObject<CustomerResponse>(response);
				error = output.ErrorDescription;

				LoggingClass.LogServiceInfo("service response", "AuthencateUser");
            }
            catch (Exception exe)
            {
                LoggingClass.LogError(exe.Message, screenid, exe.StackTrace.ToString());
            }
			sw.Stop();

			LoggingClass.LogTime("The total time to  start and end the service AuthencateUser", "The timer ran for " + sw.Elapsed.TotalSeconds);

			return output;
        }
        public async Task<CustomerResponse> AuthencateUser1(string email)
        {
			sw.Start();
			CustomerResponse output = null;
            try
            {
                LoggingClass.LogServiceInfo("service called", "AuthencateUser1");
                var uri = new Uri(ServiceURL + "AuthenticateUser1/" + email);
                var response = await client.GetStringAsync(uri).ConfigureAwait(false);
                output = JsonConvert.DeserializeObject<CustomerResponse>(response);
                LoggingClass.LogServiceInfo("service response"+email, "AuthencateUser1");
            }
            catch (Exception exe)
            {
                LoggingClass.LogError(exe.Message, screenid, exe.StackTrace.ToString());
            }
			sw.Stop();

			LoggingClass.LogTime("The total time to  start and end the service AuthencateUser1", "The timer ran for " + sw.Elapsed.TotalSeconds);

			return output;
        }
        public async Task<DeviceToken> CheckMail(string uid)
        {
			sw.Start();
			DeviceToken output = null;
            try
            {
                LoggingClass.LogServiceInfo("service called", "CheckMail");
                var uri = new Uri(ServiceURL + "GetVerificationStatus/" + uid);
                var response = await client.GetStringAsync(uri).ConfigureAwait(false);
                output = JsonConvert.DeserializeObject<DeviceToken>(response);
                LoggingClass.LogServiceInfo("service response", "CheckMail");
            }
            catch (Exception exe)
            {
                LoggingClass.LogError(exe.Message, screenid, exe.StackTrace.ToString());
            }
			sw.Stop();

			LoggingClass.LogTime("The total time to  start and end the service CheckMail", "The timer ran for " + sw.Elapsed.TotalSeconds);

			return output;
        }

        public async Task<int> InsertUpdateToken1(TokenModel token)
        {
			sw.Start();
			try
            {
                LoggingClass.LogServiceInfo("service called", "InsertUpdateToken1");
                var uri = new Uri(ServiceURL + "UpdateDeviceToken1/" + token.User_id + "/token/" + token.DeviceToken.Replace(":", ",") + "/DeviceType/" + token.DeviceType);
                var content = JsonConvert.SerializeObject(token);
                var cont = new StringContent(content, System.Text.Encoding.UTF8, "application/json");
                var response = await client.PostAsync(uri, cont); // In debug mode it do not work, Else it works
                LoggingClass.LogServiceInfo("service responce", "InsertUpdateToken1");
                //var result = response.Content.ReadAsStringAsync().Result;
            }
            catch (Exception exe)
            {
                LoggingClass.LogError(exe.Message, screenid, exe.StackTrace.ToString());
            }
			sw.Stop();

			LoggingClass.LogTime("The total time to  start and end the service InsertUpdateToken1", "The timer ran for " + sw.Elapsed.TotalSeconds);

			return 1;
        }

        public async Task<ItemReviewResponse> GetItemReviewsByWineID(int WineID)
        {
			sw.Start();
			LoggingClass.LogServiceInfo("service called", "GetItemReviewsByWineID");
            var uri = new Uri(ServiceURL + "/GetItemReviewsWineID/" + WineID);
            var response = await client.GetStringAsync(uri).ConfigureAwait(false);
            var output = JsonConvert.DeserializeObject<ItemReviewResponse>(response);
            LoggingClass.LogServiceInfo("service responce", "GetItemReviewsByWineID");
			sw.Stop();

			LoggingClass.LogTime("The total time to  start and end the service GetItemReviewsByWineID", "The timer ran for " + sw.Elapsed.TotalSeconds);

			return output;

        }

        public async Task<ItemReviewResponse> GetItemReviewUID(int userId)
        {
			sw.Start();
			LoggingClass.LogServiceInfo("service called", "GetItemReviewUID");
            var uri = new Uri(ServiceURL + "GetItemReviewsUID/" + userId);
            var response = await client.GetStringAsync(uri).ConfigureAwait(false);
            var output = JsonConvert.DeserializeObject<ItemReviewResponse>(response);
            LoggingClass.LogServiceInfo("service responce", "GetItemReviewUID");
			sw.Stop();

			LoggingClass.LogTime("The total time to  start and end the service GetItemReviewUID", "The timer ran for " + sw.Elapsed.TotalSeconds);

			return output;
        }

        public async Task<int> InsertUpdateReview(Review review)
        {
			sw.Start();
			try
            {
                LoggingClass.LogServiceInfo("service called", "InsertUpdateReview");
                var uri = new Uri(ServiceURL + "InsertUpdateReview/");
                var content = JsonConvert.SerializeObject(review);
                var cont = new StringContent(content, System.Text.Encoding.UTF8, "application/json");
                var response = await client.PostAsync(uri, cont);
                LoggingClass.LogServiceInfo("service responce", "InsertUpdateReview");
                // In debug mode it do not work, Else it works
                //var result = response.Content.ReadAsStringAsync().Result;
            }
            catch (Exception exe)
            {
                LoggingClass.LogError(exe.Message, screenid, exe.StackTrace.ToString());
            }
			sw.Stop();

			LoggingClass.LogTime("The total time to  start and end the service InsertUpdateReview", "The timer ran for " + sw.Elapsed.TotalSeconds);

			return 1;
        }
        public async Task<int> DeleteReview(Review review)
        {
			sw.Start();
			try
            {
                LoggingClass.LogServiceInfo("service called", "DeleteReview");
                var uri = new Uri(ServiceURL + "DeleteReview/");
                var content = JsonConvert.SerializeObject(review);
                var cont = new StringContent(content, System.Text.Encoding.UTF8, "application/json");
                var response = await client.PostAsync(uri, cont); // In debug mode it do not work, Else it works
                LoggingClass.LogServiceInfo("service responce", "DeleteReview");
                //var result = response.Content.ReadAsStringAsync().Result;
            }
            catch (Exception exe)
            {
                LoggingClass.LogError(exe.Message, screenid, exe.StackTrace.ToString());
            }
			sw.Stop();

			LoggingClass.LogTime("The total time to  start and end the service DeleteReview", "The timer ran for " + sw.Elapsed.TotalSeconds);

			return 1;
        }
        public async Task<int> UpdateCustomer(Customer customer)
        {
			sw.Start();
			try
            {
                LoggingClass.LogServiceInfo("service called", "UpdateCustomer");
                var uri = new Uri(ServiceURL + "UpdateCustomer/");
                var content = JsonConvert.SerializeObject(customer);
                var cont = new StringContent(content, System.Text.Encoding.UTF8, "application/json");
                var response = await client.PostAsync(uri, cont); // In debug mode it do not work, Else it works
                LoggingClass.LogServiceInfo("service responce", "UpdateCustomer");
                //var result = response.Content.ReadAsStringAsync().Result;
            }
            catch (Exception exe)
            {
                LoggingClass.LogError(exe.Message, screenid, exe.StackTrace.ToString());
            }
			sw.Stop();

			LoggingClass.LogTime("The total time to  start and end the service UpdateCustomer", "The timer ran for " + sw.Elapsed.TotalSeconds);

			return 1;
        }
        public async Task<ItemListResponse> GetItemFavsUID(int userId)
        {
			sw.Start();
			LoggingClass.LogServiceInfo("service called", "csfavs");
            var uri = new Uri(ServiceURL + "GetItemFavsUID/" + userId);
            var response = await client.GetStringAsync(uri).ConfigureAwait(false);
            var output = JsonConvert.DeserializeObject<ItemListResponse>(response);
            LoggingClass.LogServiceInfo("service responce", "csfavs");
			sw.Stop();

			LoggingClass.LogTime("The total time to  start and end the service GetItemFavsUID", "The timer ran for " + sw.Elapsed.TotalSeconds);

			return output;
        }
        public async Task<CustomerResponse> GetCustomerDetails(int userID)
        {
			sw.Start();
			LoggingClass.LogServiceInfo("service called", "GetCustomerDetails");
            var uri = new Uri(ServiceURL + "GetCustomerDetails/" + userID);
            var response = await client.GetStringAsync(uri).ConfigureAwait(false);
            var output = JsonConvert.DeserializeObject<CustomerResponse>(response);
            LoggingClass.LogServiceInfo("service responce", "GetCustomerDetails");
			sw.Stop();

			LoggingClass.LogTime("The total time to  start and end the service GetCustomerDetails", "The timer ran for " + sw.Elapsed.TotalSeconds);


			return output;
        }
        public async Task<TastingListResponse> GetMyTastingsList(int customerid)
        {
			sw.Start();
			//customerid = 38691;
			LoggingClass.LogServiceInfo("service called", "GetMyTastingsList");
            var uri = new Uri(ServiceURL + "GetMyTastingsList/" + customerid);
            var response = await client.GetStringAsync(uri).ConfigureAwait(false);
            var output = JsonConvert.DeserializeObject<TastingListResponse>(response);
			sw.Stop();

			LoggingClass.LogTime("The total time to  start and end the service GetMyTastingsList", "The timer ran for " + sw.Elapsed.TotalSeconds);

			LoggingClass.LogServiceInfo("service responce", "GetMyTastingsList");
            return output;
        }
    }
}