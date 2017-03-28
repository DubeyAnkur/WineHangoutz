using System;
using System.Collections.Generic;
using System.Net;
using System.IO;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;
using Hangout.Models;
using System.Runtime;

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
            var uri = new Uri(ServiceURL + "GetItemList/" + storeId + "/user/" + userId);
            var response = await client.GetStringAsync(uri).ConfigureAwait(false);
            var output = JsonConvert.DeserializeObject<ItemListResponse>(response);
            return output;
        }

        public async Task<ItemDetailsResponse> GetItemDetails(int wineid)
        {
            var uri = new Uri(ServiceURL + "GetItemDetails/" + wineid);
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

        public async Task<CustomerResponse> AuthencateUser(string UserName)
        {
            var uri = new Uri(ServiceURL + "AuthenticateUser/" + UserName);
            var response = await client.GetStringAsync(uri).ConfigureAwait(false);
            var output = JsonConvert.DeserializeObject<CustomerResponse>(response);
            return output;
        }

        //public async Task<ItemReviewResponse> GetItemReviewSKU(int sku)
        //{
        //    var uri = new Uri(ServiceURL + "/GetItemReviewsSKU/" + sku);
        //    var response = await client.GetStringAsync(uri).ConfigureAwait(false);
        //    var output = JsonConvert.DeserializeObject<ItemReviewResponse>(response);
        //    return output;
        //}

        public async Task<ItemReviewResponse> GetItemReviewsByWineID(int WineID)
        {
            var uri = new Uri(ServiceURL + "/GetItemReviewsWineID/" + WineID);
            var response = await client.GetStringAsync(uri).ConfigureAwait(false);
            var output = JsonConvert.DeserializeObject<ItemReviewResponse>(response);
            return output;
        }

        public async Task<ItemReviewResponse> GetItemReviewUID(int userId)
        {
            var uri = new Uri(ServiceURL + "GetItemReviewsUID/" + userId);
            var response = await client.GetStringAsync(uri).ConfigureAwait(false);
            var output = JsonConvert.DeserializeObject<ItemReviewResponse>(response);
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
        public async Task<int> DeleteReview(Review review)
        {
            try
            {
                var uri = new Uri(ServiceURL + "DeleteReview/");
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
        public async Task<int> UpdateCustomer(Customer customer)
        {
            try
            {
                var uri = new Uri(ServiceURL + "UpdateCustomer/");
                var content = JsonConvert.SerializeObject(customer);
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
        public async Task<ItemListResponse> GetItemFavsUID(int userId)
        {
            var uri = new Uri(ServiceURL + "GetItemFavsUID/" + userId);
            var response = await client.GetStringAsync(uri).ConfigureAwait(false);
            var output = JsonConvert.DeserializeObject<ItemListResponse>(response);
            return output;
        }
        public async Task<CustomerResponse> GetCustomerDetails(int userID)
        {
            var uri = new Uri(ServiceURL + "GetCustomerDetails/" + userID);
            var response = await client.GetStringAsync(uri).ConfigureAwait(false);
            var output = JsonConvert.DeserializeObject<CustomerResponse>(response);
            return output;
        }
        public async Task<TastingListResponse> GetMyTastingsList(int customerid)
        {
            //customerid = 38691;
            var uri = new Uri(ServiceURL + "GetMyTastingsList/" + customerid);
            var response = await client.GetStringAsync(uri).ConfigureAwait(false);
            var output = JsonConvert.DeserializeObject<TastingListResponse>(response);
            return output;
        }
    }
}