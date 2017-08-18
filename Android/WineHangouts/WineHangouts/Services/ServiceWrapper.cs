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
using System.Text;
using System.Net.Http.Headers;
using Android.App;

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
			sw.Start();
			LoggingClass.LogServiceInfo("Service called", "GetItemList");
            try
            {
                var uri = new Uri(ServiceURL + "GetItemLists/" + storeId + "/user/" + userId);
				var response = await client.GetStringAsync(uri).ConfigureAwait(false);
                output = JsonConvert.DeserializeObject<ItemListResponse>(response);
                LoggingClass.LogServiceInfo("Service Response", "GetItemList");
            }
            catch (Exception exe)
            {
                LoggingClass.LogError(exe.Message, screenid, exe.StackTrace.ToString());
            }
			sw.Stop();
			LoggingClass.LogTime("The total time to  start and end the service getItemList", "The timer ran for " + sw.Elapsed.TotalSeconds);
			return output;
			
		}

        public async Task<ItemDetailsResponse> GetItemDetails(string WineBarcode, int storeid)
        {
            ItemDetailsResponse output = null;
			sw.Start();
			LoggingClass.LogServiceInfo("Service Called", "GetItemDetails");
            try
            {
                var uri = new Uri(ServiceURL + "GetItemDetailsBarcode/" + WineBarcode + "/user/"+storeid);
				var response = await client.GetStringAsync(uri). ConfigureAwait(false);
				output = JsonConvert.DeserializeObject<ItemDetailsResponse>(response);
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
				string Token = CurrentUser.GetAuthToken();
				client.DefaultRequestHeaders.Accept.Clear();
				client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Token);
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

		public async Task<CustomerResponse> InsertUpdateGuest(string token)
		{
			sw.Start();
			CustomerResponse output = null;
			try
			{

				var uri = new Uri(ServiceURL + "InsertUpdateguests/" + token + "/token/1");
				var content = JsonConvert.SerializeObject(token);
				var cont = new StringContent(content, System.Text.Encoding.UTF8, "application/json");
				var response = await client.GetStringAsync(uri).ConfigureAwait(false);
				output = JsonConvert.DeserializeObject<CustomerResponse>(response);
				CurrentUser.GuestId = output.customer.CustomerID.ToString();
			}
			catch (Exception ex)
			{
				LoggingClass.LogError(ex.ToString(), screenid, ex.StackTrace);
			}
			sw.Stop();
			LoggingClass.LogServiceInfo("Service " + sw.Elapsed.TotalSeconds, "Guest Service");
			return output;
		}

		public async Task<CustomerResponse> AuthencateUser(string Email, string CardId, string uid)
        {
            sw.Start();
            CustomerResponse output = null;
            LoggingClass.LogServiceInfo("Service Call", "AuthencateUser");
            try
            {
                var uri = new Uri(ServiceURL + "AuthenticateUserBeta/" + CardId + "/email/" + Email + "/DeviceId/" + uid);
				var byteArray = new UTF8Encoding().GetBytes(CardId + ":password");
				client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));
				client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
				var response = await client.GetAsync(uri).ConfigureAwait(false);
				if (response.IsSuccessStatusCode)
				{
					string Token = response.RequestMessage.Headers.Authorization.Parameter;
					CurrentUser.SaveAuthToken(Token);
					output = JsonConvert.DeserializeObject<CustomerResponse>(response.Content.ReadAsStringAsync().Result);
					LoggingClass.LogServiceInfo("Service Response", "AuthencateUser");
				}
				else {
					AlertActivity a = new AlertActivity();
					a.UnAuthourised();
					
				}
            }
            catch (Exception ex)
            {
                LoggingClass.LogError(ex.ToString(), screenid, ex.StackTrace);
            }
            sw.Stop();
            LoggingClass.LogServiceInfo("Service responce "+sw.Elapsed.TotalSeconds, "AuthencateUser");
            return output;
        }

        public async Task<CustomerResponse> ContinueService(CustomerResponse customer)
        {
            sw.Start();

            CustomerResponse output = null;
            try
            {
                var uri = new Uri(ServiceURL + "ContinueClick/");
                var content = JsonConvert.SerializeObject(customer);
                var cont = new StringContent(content, System.Text.Encoding.UTF8, "application/json");
                string Token = CurrentUser.GetAuthToken();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Token);
                var response = await client.PostAsync(uri, cont).ConfigureAwait(false);
                if (response.IsSuccessStatusCode)
                {
                    var tokenJson = await response.Content.ReadAsStringAsync();
                    output = JsonConvert.DeserializeObject<CustomerResponse>(tokenJson);
                }
                //var result = response.Content.ReadAsStringAsync().Result;
            }
            catch (Exception ex)
            {
                LoggingClass.LogError(ex.ToString(),screenid , ex.StackTrace);
            }
            sw.Stop();
            LoggingClass.LogServiceInfo("Service " + sw.Elapsed.TotalSeconds, "Continue Service");
            //Console.WriteLine("UpdateCustomer service Time Elapsed"+sw.Elapsed.TotalSeconds);
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
				string Token = CurrentUser.GetAuthToken();
				client.DefaultRequestHeaders.Accept.Clear();
				client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Token);
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

		public async Task<ItemReviewResponse> GetItemReviewsByWineBarcode(string WineBarcode)
        {
			sw.Start();
			LoggingClass.LogServiceInfo("service called", "GetItemReviewsByWineBarcode");
            var uri = new Uri(ServiceURL + "/GetReviewsBarcode/" + WineBarcode);
			var response = await client.GetStringAsync(uri).ConfigureAwait(false);
            var output = JsonConvert.DeserializeObject<ItemReviewResponse>(response);
            LoggingClass.LogServiceInfo("service responce", "GetItemReviewsByWineBarcode");
			sw.Stop();
			LoggingClass.LogTime("The total time to  start and end the service GetItemReviewsByWineBarcode", "The timer ran for " + sw.Elapsed.TotalSeconds);
			return output;

        }

        public async Task<ItemReviewResponse> GetItemReviewUID(int userId)
        {
			sw.Start();
			LoggingClass.LogServiceInfo("service called", "GetItemReviewUID");
            var uri = new Uri(ServiceURL + "GetReviewUID/" + userId);
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
				string Token = CurrentUser.GetAuthToken();
				client.DefaultRequestHeaders.Accept.Clear();
				client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Token);
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
				string Token = CurrentUser.GetAuthToken();
				client.DefaultRequestHeaders.Accept.Clear();
				client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Token);
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
				string Token = CurrentUser.GetAuthToken();
				client.DefaultRequestHeaders.Accept.Clear();
				client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Token);
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
			var uri = new Uri(ServiceURL + "GetItemFavUID/" + userId);
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
		public async Task<int> ResendEMail(string CardNumber)
		{
			sw.Start();
			int output = 0;
			try
			{
				var uri = new Uri(ServiceURL + "ResendEmail/" + CardNumber);
				//var content = JsonConvert.SerializeObject(token);
				//var cont = new StringContent(content, System.Text.Encoding.UTF8, "application/json");
				var response = await client.GetStringAsync(uri).ConfigureAwait(false);
				output = JsonConvert.DeserializeObject<int>(response);
			}
			catch (Exception ex)
			{
				LoggingClass.LogError(ex.ToString(), screenid, ex.StackTrace);
			}
			sw.Stop();
			LoggingClass.LogServiceInfo("Service " + sw.Elapsed.TotalSeconds, "Resend Email Service");
			return output;
		}
        public async Task<CustomerResponse> UpdateMail(string email, string userid)
        {
            sw.Start();
            CustomerResponse output = null;
            try
            {

                var uri = new Uri(ServiceURL + "UpdateEmailAddress/" + email + "/user/" + userid);
                var content = JsonConvert.SerializeObject(email);
                var cont = new StringContent(content, System.Text.Encoding.UTF8, "application/json");
                string Token = CurrentUser.GetAuthToken();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Token);
                var response = await client.GetStringAsync(uri).ConfigureAwait(false);
                output = JsonConvert.DeserializeObject<CustomerResponse>(response);
            }
            catch (Exception ex)
            {
                LoggingClass.LogError(ex.ToString(), screenid, ex.StackTrace);
            }
            sw.Stop();
            LoggingClass.LogServiceInfo("Service " + sw.Elapsed.TotalSeconds, "Email Update Service");
            return output;
        }
    }
}