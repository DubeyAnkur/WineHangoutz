using System;
using System.Collections.Generic;
using System.Net;
using System.IO;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;

namespace WineHangoutz
{
	public class ServiceWrapper
	{
		HttpClient client;
  		public ServiceWrapper()
		{
			client = new HttpClient();
			client.MaxResponseContentBufferSize = 256000;
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

	}
}