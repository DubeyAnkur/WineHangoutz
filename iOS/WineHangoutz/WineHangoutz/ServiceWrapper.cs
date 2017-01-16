﻿using System;
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

		public bool ServiceEnabled { 
			get {
				return true;
			}

		}

		public async Task<List<string>> RefreshDataAsync()
		{
  			var uri = new Uri(string.Format("http://developer.xamarin.com:8081/api/todoitems{0}", string.Empty));
			List<string> Items = new List<string>();
  			var response = await client.GetAsync(uri);

			if (response.IsSuccessStatusCode)
			{
				var content = await response.Content.ReadAsStringAsync();
				Items = JsonConvert.DeserializeObject<List<string>>(content);
			}
			return Items;
		}

		public async Task<string> GetDataAsync()
		{
			var uri = new Uri("http://hangoutz.azurewebsites.net/api/Item/TestService/1");
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
			var uri = new Uri("http://hangoutz.azurewebsites.net/api/item/GetItemList/" + storeId);
			var response = await client.GetStringAsync(uri).ConfigureAwait(false);
			var	output = JsonConvert.DeserializeObject<ItemListResponse>(response);
			return output;
		}

		public async Task<ItemDetailsResponse> GetItemDetails(int sku)
		{
			var uri = new Uri("http://hangoutz.azurewebsites.net/api/Item/GetItemDetails/" + sku);
			var response = await client.GetStringAsync(uri).ConfigureAwait(false);
			var output = JsonConvert.DeserializeObject<ItemDetailsResponse>(response);
			return output;
		}

		public async Task<int> AuthencateUser(string UserName)
		{
			var uri = new Uri("http://hangoutz.azurewebsites.net/api/Item/AuthenticateUser/" + UserName);
			var response = await client.GetStringAsync(uri).ConfigureAwait(false);
			var output = JsonConvert.DeserializeObject<int>(response);
			return output;
		}

		public async Task<ItemRatingResponse> GetItemRatingsSKU(int sku)
		{
			var uri = new Uri("http://hangoutz.azurewebsites.net/api/Item/GetItemRatingsSKU/" + sku);
			var response = await client.GetStringAsync(uri).ConfigureAwait(false);
			var output = JsonConvert.DeserializeObject<ItemRatingResponse>(response);
			return output;
		}

		public async Task<ItemRatingResponse> GetItemRatingsUID(int userId)
		{
			var uri = new Uri("http://hangoutz.azurewebsites.net/api/Item/GetItemRatingsUID/" + userId);
			var response = await client.GetStringAsync(uri).ConfigureAwait(false);
			var output = JsonConvert.DeserializeObject<ItemRatingResponse>(response);
			return output;
		}

		public async Task<int> InsertUpdateLike(SKULike skuLike)
		{
			var uri = new Uri("http://hangoutz.azurewebsites.net/api/Item/InsertUpdateLike/");

			var content = JsonConvert.SerializeObject(skuLike);

			var response = await client.PostAsync(
				uri,
				new StringContent(content)).ConfigureAwait(false);

			return 1;
		}

	}
}