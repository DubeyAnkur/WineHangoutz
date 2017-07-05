using System;
using System.Collections.Generic;
using System.Net;
using System.IO;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;
using Hangout.Models;
using System.Runtime;
using Foundation;
using System.Text;
using System.Net.Http.Headers;
using UIKit;
using System.Diagnostics;
using BigTed;

namespace WineHangoutz
{
    public class ServiceWrapper
    {
        HttpClient client;
		private int screenid=13;
		public string error = null;
        public ServiceWrapper()
        {
            client = new HttpClient();

            //client.MaxResponseContentBufferSize = 256000;
        }
		Stopwatch sw = new Stopwatch();
        public string ServiceURL
        {
            get
            {
                
                string host = "https://hangoutz.azurewebsites.net/";
                return host + "api/Item/";
            }

        }

        public async Task<string> GetDataAsync()
        {
			
				var uri = new Uri(ServiceURL + "TestService/1");
				var response = await client.GetAsync(uri);
				string output = "";
				if (response.IsSuccessStatusCode)
				{
					var content = await response.Content.ReadAsStringAsync();
					output = JsonConvert.DeserializeObject<string>(content); // JsonConvert.DeserializeObject<string>(content);
				}
				return output;

        }

   //     public async Task<ItemListResponse> GetItemList(int storeId,int userId)
   //     {
			//ItemListResponse output=null;
			//LoggingClass.LogServiceInfo("Service Call", "ItemList");
			//try
			//{

			//	var uri = new Uri(ServiceURL + "GetItemList/" + storeId + "/user/" + userId);
			//	var response = await client.GetStringAsync(uri).ConfigureAwait(false);
			//	output = JsonConvert.DeserializeObject<ItemListResponse>(response);
			//	LoggingClass.LogServiceInfo("Service Response", "ItemList");

			//}
			//catch (Exception ex)
			//{
			//	LoggingClass.LogError(ex.ToString(), screenid, ex.StackTrace);
			//}
			//return output;
   //     }
		public async Task<ItemListResponse> GetItemLists(int storeId, int userId)
		{
			sw.Start();
			ItemListResponse output = null;
			try
			{

				var uri = new Uri(ServiceURL + "GetItemLists/" + storeId + "/user/" + userId);
				string Token = CurrentUser.GetAuthToken();
				client.DefaultRequestHeaders.Accept.Clear();
				client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Token);
				var response = await client.GetStringAsync(uri).ConfigureAwait(false);
				output = JsonConvert.DeserializeObject<ItemListResponse>(response);


			}
			catch (Exception ex)
			{
				LoggingClass.LogError(ex.ToString(), screenid, ex.StackTrace);
			}
			sw.Stop();
			LoggingClass.LogServiceInfo("GetItemList service Time Elapsed"+sw.Elapsed.TotalSeconds, "ItemList");
			return output;  
		}


   //     public async Task<ItemDetailsResponse> GetItemDetails(int wineid,int storeid)
   //     {
			//ItemDetailsResponse output = null;
			//LoggingClass.LogServiceInfo("Service Call", "GetItemDetails");
			//try
			//{

			//	var uri = new Uri(ServiceURL + "GetItemDetails/" + wineid+"/user/"+storeid);
			//	var response = await client.GetStringAsync(uri).ConfigureAwait(false);
			//	output = JsonConvert.DeserializeObject<ItemDetailsResponse>(response);
			//	LoggingClass.LogServiceInfo("Service Response", "GetItemDetails");
			//}
			//catch (Exception ex)
			//{
			//	LoggingClass.LogError(ex.ToString(), screenid, ex.StackTrace);
			//}
			//return output;
   //     }

		public async Task<ItemDetailsResponse> GetItemDetailsBarcode(string wineid, int storeid)
		{
			sw.Start();
			ItemDetailsResponse output = null;
			try
			{

				var uri = new Uri(ServiceURL + "GetItemDetailsBarcode/" + wineid + "/user/" + storeid);
				string Token = CurrentUser.GetAuthToken();
				client.DefaultRequestHeaders.Accept.Clear();
				client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Token);
				var response = await client.GetStringAsync(uri).ConfigureAwait(false);
				output = JsonConvert.DeserializeObject<ItemDetailsResponse>(response);

			}
			catch (Exception ex)
			{
				LoggingClass.LogError(ex.ToString(), screenid, ex.StackTrace);
			}
			sw.Stop();
			LoggingClass.LogServiceInfo("Service "+sw.Elapsed.TotalSeconds, "GetItemDetails");
			return output;
		}

        public async Task<int> InsertUpdateLike(SKULike skuLike)
        {
			sw.Start();
            try
            {

                var uri = new Uri(ServiceURL + "InsertUpdateLike/");
                var content = JsonConvert.SerializeObject(skuLike);
                var cont = new StringContent(content, System.Text.Encoding.UTF8, "application/json");
				string Token = CurrentUser.GetAuthToken();
				client.DefaultRequestHeaders.Accept.Clear();
				client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Token);
                var response = await client.PostAsync(uri, cont); // In debug mode it do not work, Else it works
                //var result = response.Content.ReadAsStringAsync().Result;


            }
            //catch (Exception ex)
           catch (Exception ex)
			{
				LoggingClass.LogError(ex.ToString(), screenid, ex.StackTrace);
			}
			sw.Stop();
			LoggingClass.LogServiceInfo("Service "+sw.Elapsed.TotalSeconds, "InsertUpdateLike");
            return 1;
        }

		public async Task<CustomerResponse> AuthencateUser(string Email,string CardId,string uid)
        {
			sw.Start();
			CustomerResponse output = null;
			try
			{
				var uri = new Uri(ServiceURL + "AuthenticateUser/" + CardId + "/email/" + Email + "/DeviceId/" + uid);
				var byteArray = new UTF8Encoding().GetBytes(CardId + ":password");
				client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));
				client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
				var response = await client.GetAsync(uri).ConfigureAwait(false);
				if (response.IsSuccessStatusCode)
				{
					string Token = response.RequestMessage.Headers.Authorization.Parameter;
					CurrentUser.SaveAuthToken(Token);
					output = JsonConvert.DeserializeObject<CustomerResponse>(response.Content.ReadAsStringAsync().Result);
					//var response = await client.GetStringAsync(uri).ConfigureAwait(false);
					output = JsonConvert.DeserializeObject<CustomerResponse>(response.Content.ReadAsStringAsync().Result);
				}
			}
			catch (Exception ex)
 			{
				LoggingClass.LogError(ex.ToString(), screenid, ex.StackTrace);
			}
			sw.Stop();
			LoggingClass.LogServiceInfo("Service "+sw.Elapsed.TotalSeconds, "Authencate Card and sending mail");
			return output;
        }

		//public async Task<CustomerResponse> AuthencateUser1(string cardnumber)
		//{
		//	CustomerResponse output = null;
		//	LoggingClass.LogServiceInfo("Service Call", "AuthencateEmail");
		//	LoggingClass.LogServiceInfo("Tried to login with " + cardnumber+" From IOS Device", "");
		//	try
		//	{
		//		var uri = new Uri(ServiceURL + "AuthenticateUser1/" + cardnumber);
		//		var response = await client.GetStringAsync(uri).ConfigureAwait(false);
		//		output = JsonConvert.DeserializeObject<CustomerResponse>(response);
		//		LoggingClass.LogServiceInfo("Service Response", "AuthencateUser");
		//	}
		//	catch (Exception ex)
		//	{
		//		LoggingClass.LogError(ex.ToString(), screenid, ex.StackTrace);
		//	}
		//	//Boolean output = true;
		//	return output;
		//}

		public async Task<DeviceToken> VerifyMail(string Uid)
		{
			sw.Start();
			DeviceToken output=null;
			try
			{
				var uri = new Uri(ServiceURL + "GetVerificationStatus/" + Uid);
				var response = await client.GetStringAsync(uri).ConfigureAwait(false);
				output = JsonConvert.DeserializeObject<DeviceToken>(response);
			}
			catch (Exception ex)
			{
				LoggingClass.LogError(ex.ToString(), screenid, ex.StackTrace);
			}
			sw.Stop();
			LoggingClass.LogServiceInfo("Service "+sw.Elapsed.TotalSeconds, "Verify");
			return  output;
		}

		public async Task<ItemReviewResponse> GetItemReviewsByWineID(string WineID)
        {
			sw.Start();
			ItemReviewResponse output = null;
			try
			{

				var uri = new Uri(ServiceURL + "/GetReviewsBarcode/" + WineID);
				var response = await client.GetStringAsync(uri).ConfigureAwait(false);
				output = JsonConvert.DeserializeObject<ItemReviewResponse>(response);

			}
			catch (Exception ex)
			{
				LoggingClass.LogError(ex.ToString(), screenid, ex.StackTrace);
			}
			sw.Stop();
			LoggingClass.LogServiceInfo("Service "+sw.Elapsed.TotalSeconds, "GetItemReviewsByWineID");
			return output;
        }

        public async Task<ItemReviewResponse> GetItemReviewUID(int userId)
        {
			sw.Start();
			ItemReviewResponse output = null;
			try
			{

				var uri = new Uri(ServiceURL + "GetReviewUID/" + userId);
				var response = await client.GetStringAsync(uri).ConfigureAwait(false);
				output = JsonConvert.DeserializeObject<ItemReviewResponse>(response);
			}
			catch (Exception ex)
			{
				LoggingClass.LogError(ex.ToString(), screenid, ex.StackTrace);
			}
			sw.Stop();
			LoggingClass.LogServiceInfo("Service "+sw.Elapsed.TotalSeconds, "GetItemReviewUID");
			return output;

        }

        public async Task<int> InsertUpdateReview(Review review)
        {
			sw.Start();
			try
			{
				var uri = new Uri(ServiceURL + "InsertUpdateReview/");
				var content = JsonConvert.SerializeObject(review);
				var cont = new StringContent(content, System.Text.Encoding.UTF8, "application/json");
				string Token = CurrentUser.GetAuthToken();
				client.DefaultRequestHeaders.Accept.Clear();
				client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Token);
				var response = await client.PostAsync(uri, cont);
			}
			catch (Exception ex)
			{
				LoggingClass.LogError(ex.ToString(), screenid, ex.StackTrace);
			}
			sw.Stop();
			LoggingClass.LogServiceInfo("Service "+sw.Elapsed.TotalSeconds, "InsertUpdateReview");
            return 1;
        }
        public async Task<int> DeleteReview(Review review)
        {
			sw.Start();
			try
			{
				var uri = new Uri(ServiceURL + "DeleteReview/");
				var content = JsonConvert.SerializeObject(review);
				var cont = new StringContent(content, System.Text.Encoding.UTF8, "application/json");
				string Token = CurrentUser.GetAuthToken();
				client.DefaultRequestHeaders.Accept.Clear();
				client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Token);
				var response = await client.PostAsync(uri, cont); // In debug mode it do not work, Else it works
				//var result = response.Content.ReadAsStringAsync().Result;
			}
			catch (Exception ex)
			{
				LoggingClass.LogError(ex.ToString(), screenid, ex.StackTrace);
			}
			sw.Stop();
			LoggingClass.LogServiceInfo("Service "+sw.Elapsed.TotalSeconds, "DeleteReview");
            return 1;
        }
        public async Task<int> UpdateCustomer(Customer customer)
        {
			sw.Start();
			try
            {
                var uri = new Uri(ServiceURL + "UpdateCustomer/");
                var content = JsonConvert.SerializeObject(customer);
                var cont = new StringContent(content, System.Text.Encoding.UTF8, "application/json");
				string Token = CurrentUser.GetAuthToken();
				client.DefaultRequestHeaders.Accept.Clear();
				client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Token);
                var response = await client.PostAsync(uri, cont); // In debug mode it do not work, Else it works
               	//var result = response.Content.ReadAsStringAsync().Result;
           				
			}
            catch (Exception ex)
			{
				LoggingClass.LogError(ex.ToString(), screenid, ex.StackTrace);
			}
			sw.Stop();
			LoggingClass.LogServiceInfo("Service "+sw.Elapsed.TotalSeconds, "UpdateCustomer");
            return 1;
        }
		public async Task<int> InsertUpdateToken(string token,string user_id,int DeviceType)
		{
			sw.Start();
			try
			{
				var uri = new Uri(ServiceURL + "UpdateDeviceToken1/" + user_id + "/token/" + token.Replace(" ", "")+"/DeviceType/"+DeviceType);
				var content = JsonConvert.SerializeObject(token);
				var cont = new StringContent(content, System.Text.Encoding.UTF8, "application/json");
				string Token = CurrentUser.GetAuthToken();
				client.DefaultRequestHeaders.Accept.Clear();
				client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Token);
				var response = await client.PostAsync(uri, cont); // In debug mode it do not work, Else it works
																  //var result = response.Content.ReadAsStringAsync().Result;
			}
			catch (Exception ex)
			{
				LoggingClass.LogError(ex.ToString(), screenid, ex.StackTrace);
			}
			sw.Stop();
			LoggingClass.LogServiceInfo("Service "+sw.Elapsed.TotalSeconds, "InsertUpdateToken");
			return 1;
		}
        public async Task<ItemListResponse> GetItemFavsUID(int userId)
        {
			sw.Start();
			ItemListResponse output = null;
			try
			{

				var uri = new Uri(ServiceURL + "GetItemFavUID/" + userId);
				var response = await client.GetStringAsync(uri).ConfigureAwait(false);
				output = JsonConvert.DeserializeObject<ItemListResponse>(response);
			}
			catch (Exception ex)
			{
				LoggingClass.LogError(ex.ToString(), screenid, ex.StackTrace);
			}
			sw.Stop();
			LoggingClass.LogServiceInfo("Service "+sw.Elapsed.TotalSeconds, "InsertUpdateToken");
			return output;
        }
        public async Task<CustomerResponse> GetCustomerDetails(int userID)
        {
			sw.Start();
			CustomerResponse output=null;
			try
			{

				var uri = new Uri(ServiceURL + "GetCustomerDetails/" + userID);
				var response = await client.GetStringAsync(uri).ConfigureAwait(false);
				output = JsonConvert.DeserializeObject<CustomerResponse>(response);
			}
			catch (Exception ex)
			{
				LoggingClass.LogError(ex.ToString(), screenid, ex.StackTrace);
			}
			sw.Stop();
			LoggingClass.LogServiceInfo("Service "+sw.Elapsed.TotalSeconds, "GetCustomerDetails");
			return output;
        }
        public async Task<TastingListResponse> GetMyTastingsList(int customerid)
        {
			sw.Start();
			TastingListResponse output = null;
			try
			{

				//customerid = 38691;
				var uri = new Uri(ServiceURL + "GetMyTastingsList/" + customerid);
				var response = await client.GetStringAsync(uri).ConfigureAwait(false);
				output = JsonConvert.DeserializeObject<TastingListResponse>(response);
			}catch (Exception ex)
			{
				LoggingClass.LogError(ex.ToString(), screenid, ex.StackTrace);
			}
			sw.Stop();
			LoggingClass.LogServiceInfo("Service "+sw.Elapsed.TotalSeconds, "GetMyTastingsList");
			return output;
        }
		public async Task<CustomerResponse> InsertUpdateGuest(string token)
		{
			sw.Start();
			CustomerResponse output = null;
			try
			{

				var uri = new Uri(ServiceURL + "InsertUpdateguests/" + token+"/token/1");
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

    }
}