using System;
using System.Collections.Generic;
using System.Net;
using System.IO;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;
using Hangout.Models;
using System.Text;

namespace WineHangoutz
{
	public class ServiceWrapper
	{
		HttpClient client;
		public ServiceWrapper()
		{
			client = new HttpClient();
			//client.MaxResponseContentBufferSize = 256000;
		}

		public string ServiceURL
		{
			get
			{
				//string host = "http://localhost:65442/";
				string host = "http://hangoutz.azurewebsites.net/";
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
				output = JsonConvert.DeserializeObject<string>(content);
			}
			return output;
		}

		public async Task<ItemListResponse> GetItemList(int storeId)
		{
			var uri = new Uri(ServiceURL + "GetItemList/" + storeId);
			var response = await client.GetStringAsync(uri).ConfigureAwait(false);
			var output = JsonConvert.DeserializeObject<ItemListResponse>(response);
			return output;
		}

		public async Task<ItemDetailsResponse> GetItemDetails(int sku)
		{
			var uri = new Uri(ServiceURL + "GetItemDetails/" + sku);
			var response = await client.GetStringAsync(uri).ConfigureAwait(false);
			var output = JsonConvert.DeserializeObject<ItemDetailsResponse>(response);
			return output;
		}
		public async Task<int> InsertUpdateLike(SKULike skuLike)
		{
			try
			{

				var uri = new Uri(ServiceURL + "InsertUpdateLike/");
				var content = JsonConvert.SerializeObject(skuLike);
				var cont = new StringContent(content, System.Text.Encoding.UTF8, "application/json");
				var response = await client.PostAsync(uri, cont); // In debug mode it do not work, Else it works
																  //var result = response.Content.ReadAsStringAsync().Result;
			}
			catch (Exception ex)
			{
				throw ex;
			}
			return 1;
		}

		public async Task<UserResponse> AuthencateUser(string UserName)
		{
			var uri = new Uri(ServiceURL + "AuthenticateUser/" + UserName);
			var response = await client.GetStringAsync(uri).ConfigureAwait(false);
			var output = JsonConvert.DeserializeObject<UserResponse>(response);
			return output;
		}

		public async Task<ItemRatingResponse> GetItemRatingsSKU(int sku)
		{
			var uri = new Uri(ServiceURL + "/GetItemRatingsSKU/" + sku);
			var response = await client.GetStringAsync(uri).ConfigureAwait(false);
			var output = JsonConvert.DeserializeObject<ItemRatingResponse>(response);
			return output;
		}

		public async Task<ItemRatingResponse> GetItemRatingsUID(int userId)
		{
			var uri = new Uri(ServiceURL + "GetItemRatingsUID/" + userId);
			var response = await client.GetStringAsync(uri).ConfigureAwait(false);
			var output = JsonConvert.DeserializeObject<ItemRatingResponse>(response);
			return output;
		}

		public async Task<int> InsertUpdateReview(Review review)
		{
			try
			{
				var uri = new Uri(ServiceURL + "InsertUpdateReview/");
				var content = JsonConvert.SerializeObject(review);
				var cont = new StringContent(content, System.Text.Encoding.UTF8, "application/json");
				var response = await client.PostAsync(uri, cont); // In debug mode it do not work, Else it works
																  //var result = response.Content.ReadAsStringAsync().Result;
			}
			catch (Exception ex)
			{
				throw ex;
			}
			return 1;
		}
	}
}