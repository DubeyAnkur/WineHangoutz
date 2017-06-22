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

        public async Task<ItemListResponse> GetItemList(int storeId,int userId)
        {
			ItemListResponse output=null;
			LoggingClass.LogServiceInfo("Service Call", "ItemList");
			try
			{

				var uri = new Uri(ServiceURL + "GetItemList/" + storeId + "/user/" + userId);
				var response = await client.GetStringAsync(uri).ConfigureAwait(false);
				output = JsonConvert.DeserializeObject<ItemListResponse>(response);
				LoggingClass.LogServiceInfo("Service Response", "ItemList");

			}
			catch (Exception ex)
			{
				LoggingClass.LogError(ex.ToString(), screenid, ex.StackTrace);
			}
			return output;
        }
		public async Task<ItemListResponse> GetItemLists(int storeId, int userId)
		{
			ItemListResponse output = null;
			LoggingClass.LogServiceInfo("Service Call", "ItemList");
			try
			{

				var uri = new Uri(ServiceURL + "GetItemLists/" + storeId + "/user/" + userId);
				var response = await client.GetStringAsync(uri).ConfigureAwait(false);
				output = JsonConvert.DeserializeObject<ItemListResponse>(response);
				LoggingClass.LogServiceInfo("Service Response", "ItemList");

			}
			catch (Exception ex)
			{
				LoggingClass.LogError(ex.ToString(), screenid, ex.StackTrace);
			}
			return output;  
		}


        public async Task<ItemDetailsResponse> GetItemDetails(int wineid,int storeid)
        {
			ItemDetailsResponse output = null;
			LoggingClass.LogServiceInfo("Service Call", "GetItemDetails");
			try
			{

				var uri = new Uri(ServiceURL + "GetItemDetails/" + wineid+"/user/"+storeid);
				var response = await client.GetStringAsync(uri).ConfigureAwait(false);
				output = JsonConvert.DeserializeObject<ItemDetailsResponse>(response);
				LoggingClass.LogServiceInfo("Service Response", "GetItemDetails");
			}
			catch (Exception ex)
			{
				LoggingClass.LogError(ex.ToString(), screenid, ex.StackTrace);
			}
			return output;
        }

		public async Task<ItemDetailsResponse> GetItemDetailsBarcode(string wineid, int storeid)
		{
			ItemDetailsResponse output = null;
			LoggingClass.LogServiceInfo("Service Call", "GetItemDetails");
			try
			{

				var uri = new Uri(ServiceURL + "GetItemDetailsBarcode/" + wineid + "/user/" + storeid);
				var response = await client.GetStringAsync(uri).ConfigureAwait(false);
				output = JsonConvert.DeserializeObject<ItemDetailsResponse>(response);
				LoggingClass.LogServiceInfo("Service Response", "GetItemDetails");
			}
			catch (Exception ex)
			{
				LoggingClass.LogError(ex.ToString(), screenid, ex.StackTrace);
			}
			return output;
		}

        public async Task<int> InsertUpdateLike(SKULike skuLike)
        {
			LoggingClass.LogServiceInfo("Service Call", "InsertUpdateLike");
            try
            {

                var uri = new Uri(ServiceURL + "InsertUpdateLike/");
                var content = JsonConvert.SerializeObject(skuLike);
                var cont = new StringContent(content, System.Text.Encoding.UTF8, "application/json");
                var response = await client.PostAsync(uri, cont); // In debug mode it do not work, Else it works
                //var result = response.Content.ReadAsStringAsync().Result;

				LoggingClass.LogServiceInfo("Service Response", "InsertUpdateLike");
            }
            //catch (Exception ex)
           catch (Exception ex)
			{
				LoggingClass.LogError(ex.ToString(), screenid, ex.StackTrace);
			}
            return 1;
        }

		public async Task<CustomerResponse> AuthencateUser(string Email,string CardId,string uid)
        {
			CustomerResponse output = null;

			LoggingClass.LogServiceInfo("Service Call", "AuthencateUser");
			try
			{
				var uri = new Uri(ServiceURL + "AuthenticateUser/" + CardId+"/email/"+Email+"/DeviceId/"+uid);
				var response = await client.GetStringAsync(uri).ConfigureAwait(false);
				output = JsonConvert.DeserializeObject<CustomerResponse>(response);
				LoggingClass.LogServiceInfo("Service Response", "AuthencateUser");
			}
			catch (Exception ex)
			{
				LoggingClass.LogError(ex.ToString(), screenid, ex.StackTrace);
			}
			return output;
        }

		public async Task<CustomerResponse> AuthencateUser1(string cardnumber)
		{
			CustomerResponse output = null;

			LoggingClass.LogServiceInfo("Service Call", "AuthencateEmail");
			LoggingClass.LogServiceInfo("Tried to login with " + cardnumber+" From IOS Device", "");
			try
			{
				var uri = new Uri(ServiceURL + "AuthenticateUser1/" + cardnumber);
				var response = await client.GetStringAsync(uri).ConfigureAwait(false);
				output = JsonConvert.DeserializeObject<CustomerResponse>(response);
				LoggingClass.LogServiceInfo("Service Response", "AuthencateUser");
			}
			catch (Exception ex)
			{
				LoggingClass.LogError(ex.ToString(), screenid, ex.StackTrace);
			}
			//Boolean output = true;
			return output;
		}
		public async Task<DeviceToken> VerifyMail(string Uid)
		{
			DeviceToken output=null;

			LoggingClass.LogServiceInfo("Service Call", "VerifyEmail");
			try
			{
				var uri = new Uri(ServiceURL + "GetVerificationStatus/" + Uid);
				var response = await client.GetStringAsync(uri).ConfigureAwait(false);
				output = JsonConvert.DeserializeObject<DeviceToken>(response);
				LoggingClass.LogServiceInfo("Service Response", "Verify");
			}
			catch (Exception ex)
			{
				LoggingClass.LogError(ex.ToString(), screenid, ex.StackTrace);
			}
			//Boolean output = true;
			return  output;
		}

		public async Task<ItemReviewResponse> GetItemReviewsByWineID(string WineID)
        {
			ItemReviewResponse output = null;
			LoggingClass.LogServiceInfo("Service Call", "GetItemReviewsByWineID");
			try
			{

				var uri = new Uri(ServiceURL + "/GetReviewsBarcode/" + WineID);
				var response = await client.GetStringAsync(uri).ConfigureAwait(false);
				output = JsonConvert.DeserializeObject<ItemReviewResponse>(response);
				LoggingClass.LogServiceInfo("Service Response", "GetItemReviewsByWineID");

			}
			catch (Exception ex)
			{
				LoggingClass.LogError(ex.ToString(), screenid, ex.StackTrace);
			}
			return output;
        }

        public async Task<ItemReviewResponse> GetItemReviewUID(int userId)
        {
			ItemReviewResponse output = null;
			LoggingClass.LogServiceInfo("Service Call", "GetItemReviewUID");
			try
			{

				var uri = new Uri(ServiceURL + "GetItemReviewsUID/" + userId);
				var response = await client.GetStringAsync(uri).ConfigureAwait(false);
				output = JsonConvert.DeserializeObject<ItemReviewResponse>(response);
				LoggingClass.LogServiceInfo("Service Response", "GetItemReviewUID");

			}
			catch (Exception ex)
			{
				LoggingClass.LogError(ex.ToString(), screenid, ex.StackTrace);
			}
			return output;

        }

        public async Task<int> InsertUpdateReview(Review review)
        {
			LoggingClass.LogServiceInfo("Service Call", "InsertUpdateReview");
            try
            {
                var uri = new Uri(ServiceURL + "InsertUpdateReview/");
                var content = JsonConvert.SerializeObject(review);
                var cont = new StringContent(content, System.Text.Encoding.UTF8, "application/json");
                var response = await client.PostAsync(uri, cont); // In debug mode it do not work, Else it works
                //var result = response.Content.ReadAsStringAsync().Result;
				LoggingClass.LogServiceInfo("Service Response", "InsertUpdateReview");
            }
            //catch (Exception ex)
            //{
            //    throw ex;
            //}
			catch (Exception ex)
			{
				LoggingClass.LogError(ex.ToString(), screenid, ex.StackTrace);
			}
            return 1;
        }
        public async Task<int> DeleteReview(Review review)
        {
			LoggingClass.LogServiceInfo("Service Call", "DeleteReview");
			
            try
            {
                var uri = new Uri(ServiceURL + "DeleteReview/");
                var content = JsonConvert.SerializeObject(review);
                var cont = new StringContent(content, System.Text.Encoding.UTF8, "application/json");
                var response = await client.PostAsync(uri, cont); // In debug mode it do not work, Else it works
                //var result = response.Content.ReadAsStringAsync().Result;
				LoggingClass.LogServiceInfo("Service Response", "DeleteReview");
            }
            //catch (Exception ex)
            //{
            //    throw ex;
            //}
			catch (Exception ex)
			{
				LoggingClass.LogError(ex.ToString(), screenid, ex.StackTrace);
			}
            return 1;
        }
        public async Task<int> UpdateCustomer(Customer customer)
        {
			LoggingClass.LogServiceInfo("Service Call", "UpdateCustomer");

			try
            {
                var uri = new Uri(ServiceURL + "UpdateCustomer/");
                var content = JsonConvert.SerializeObject(customer);
                var cont = new StringContent(content, System.Text.Encoding.UTF8, "application/json");
                var response = await client.PostAsync(uri, cont); // In debug mode it do not work, Else it works
               	//var result = response.Content.ReadAsStringAsync().Result;
           				LoggingClass.LogServiceInfo("Service Response", "UpdateCustomer");
			}
            catch (Exception ex)
			{
				LoggingClass.LogError(ex.ToString(), screenid, ex.StackTrace);
			}
            return 1;
        }
		public async Task<int> InsertUpdateToken(string token,string user_id,int DeviceType)
		{
			LoggingClass.LogServiceInfo("Service Call", "InsertUpdateToken");

			try
			{
				var uri = new Uri(ServiceURL + "UpdateDeviceToken1/" + user_id + "/token/" + token.Replace(" ", "")+"/DeviceType/"+DeviceType);
				var content = JsonConvert.SerializeObject(token);
				var cont = new StringContent(content, System.Text.Encoding.UTF8, "application/json");
				var response = await client.PostAsync(uri, cont); // In debug mode it do not work, Else it works
																  //var result = response.Content.ReadAsStringAsync().Result;
			LoggingClass.LogServiceInfo("Service Response", "InsertUpdateToken");
			}
			catch (Exception ex)
			{
				LoggingClass.LogError(ex.ToString(), screenid, ex.StackTrace);
			}
			return 1;
		}
        public async Task<ItemListResponse> GetItemFavsUID(int userId)
        {
			LoggingClass.LogServiceInfo("Service Call", "GetItemFavsUID");
			ItemListResponse output = null;
			try
			{

				var uri = new Uri(ServiceURL + "GetItemFavUID/" + userId);
				var response = await client.GetStringAsync(uri).ConfigureAwait(false);
				output = JsonConvert.DeserializeObject<ItemListResponse>(response);
				LoggingClass.LogServiceInfo("Service Call", "GetItemFavsUID");
			
			}
			catch (Exception ex)
			{
				LoggingClass.LogError(ex.ToString(), screenid, ex.StackTrace);
			}
			return output;
        }
        public async Task<CustomerResponse> GetCustomerDetails(int userID)
        {
			CustomerResponse output=null;
			LoggingClass.LogServiceInfo("Service Call", "GetCustomerDetails");
			try
			{

				var uri = new Uri(ServiceURL + "GetCustomerDetails/" + userID);
				var response = await client.GetStringAsync(uri).ConfigureAwait(false);
				output = JsonConvert.DeserializeObject<CustomerResponse>(response);
				LoggingClass.LogServiceInfo("Service Call", "GetCustomerDetails");

			}
			catch (Exception ex)
			{
				LoggingClass.LogError(ex.ToString(), screenid, ex.StackTrace);
			}
			return output;
        }
        public async Task<TastingListResponse> GetMyTastingsList(int customerid)
        {
			LoggingClass.LogServiceInfo("Service Call", "GetMyTastingsList");
			TastingListResponse output = null;
			try
			{

				//customerid = 38691;
				var uri = new Uri(ServiceURL + "GetMyTastingsList/" + customerid);
				var response = await client.GetStringAsync(uri).ConfigureAwait(false);
			output = JsonConvert.DeserializeObject<TastingListResponse>(response);
				LoggingClass.LogServiceInfo("Service Call", "GetMyTastingsList");

			}catch (Exception ex)
			{
				LoggingClass.LogError(ex.ToString(), screenid, ex.StackTrace);
			}
			return output;
        }
    }
}