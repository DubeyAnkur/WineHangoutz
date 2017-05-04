using System;
using System.Collections.Generic;
using System.Net;
using System.IO;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;
using Hangout.Models;

namespace WineHangouts
{
    public class ServiceWrapper
    {
        HttpClient client;
        private int screenid = 19;
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
                output = JsonConvert.DeserializeObject<string>(content);
            }
            return output;
        }

        public async Task<ItemListResponse> GetItemList(int storeId, int userId)
        {
            ItemListResponse output = null;

            LoggingClass.LogServiceInfo("Service called", "itemlist");
            try
            {
                var uri = new Uri(ServiceURL + "GetItemList/" + storeId + "/user/" + userId);
                var response = await client.GetStringAsync(uri).ConfigureAwait(false);
                output = JsonConvert.DeserializeObject<ItemListResponse>(response);
                LoggingClass.LogServiceInfo("Service Response", "itemlist");
            }
            catch (Exception exe)
            {
                LoggingClass.LogError(exe.Message, screenid, exe.StackTrace.ToString());
            }
            return output;
        }

        public async Task<ItemDetailsResponse> GetItemDetails(int wineid)
        {
            ItemDetailsResponse output = null;
            LoggingClass.LogServiceInfo("Service Called", "itemdetails");
            try
            {
                var uri = new Uri(ServiceURL + "GetItemDetails/" + wineid);
                var response = await client.GetStringAsync(uri).ConfigureAwait(false);
                output = JsonConvert.DeserializeObject<ItemDetailsResponse>(response);
                LoggingClass.LogServiceInfo("Service Response", "itemdetails");
            }
            catch (Exception exe)
            {
                LoggingClass.LogError(exe.Message, screenid, exe.StackTrace.ToString());
            }
            return output;
        }
        public async Task<int> InsertUpdateLike(SKULike skuLike)
        {
            try
            {
                LoggingClass.LogServiceInfo("service called", "like");
                var uri = new Uri(ServiceURL + "InsertUpdateLike/");
                var content = JsonConvert.SerializeObject(skuLike);
                var cont = new StringContent(content, System.Text.Encoding.UTF8, "application/json");
                var response = await client.PostAsync(uri, cont); // In debug mode it do not work, Else it works
                //var result = response.Content.ReadAsStringAsync().Result;
                LoggingClass.LogServiceInfo("service response", "like");
            }
            catch (Exception exe)
            {
                LoggingClass.LogError(exe.Message, screenid, exe.StackTrace.ToString());
            }
            return 1;
        }

        public async Task<CustomerResponse> AuthencateUser(string UserName)
        {
            CustomerResponse output = null;
            try
            {
                LoggingClass.LogServiceInfo("service called", "authen");
                var uri = new Uri(ServiceURL + "AuthenticateUser/" + UserName);
                var response = await client.GetStringAsync(uri).ConfigureAwait(false);
                output = JsonConvert.DeserializeObject<CustomerResponse>(response);
                LoggingClass.LogServiceInfo("service response", "like");
            }
            catch (Exception exe)
            {
                LoggingClass.LogError(exe.Message, screenid, exe.StackTrace.ToString());
            }
            return output;
        }

        public async Task<int> InsertUpdateToken1(TokenModel token)
        {
            try
            {
                LoggingClass.LogServiceInfo("service called", "tok");
                var uri = new Uri(ServiceURL + "UpdateDeviceToken1/" + token.User_id + "/token/" + token.DeviceToken.Replace(":", ",") + "/DeviceType/" + token.DeviceType);
                var content = JsonConvert.SerializeObject(token);
                var cont = new StringContent(content, System.Text.Encoding.UTF8, "application/json");
                var response = await client.PostAsync(uri, cont); // In debug mode it do not work, Else it works
                LoggingClass.LogServiceInfo("service responce", "tok");
                //var result = response.Content.ReadAsStringAsync().Result;
            }
            catch (Exception exe)
            {
                LoggingClass.LogError(exe.Message, screenid, exe.StackTrace.ToString());
            }
            return 1;
        }

        public async Task<ItemReviewResponse> GetItemReviewsByWineID(int WineID)
        {
            LoggingClass.LogServiceInfo("service called", "itemrev");
            var uri = new Uri(ServiceURL + "/GetItemReviewsWineID/" + WineID);
            var response = await client.GetStringAsync(uri).ConfigureAwait(false);
            var output = JsonConvert.DeserializeObject<ItemReviewResponse>(response);
            LoggingClass.LogServiceInfo("service responce", "itemrev");
            return output;
        }

        public async Task<ItemReviewResponse> GetItemReviewUID(int userId)
        {
            LoggingClass.LogServiceInfo("service called", "userrev");
            var uri = new Uri(ServiceURL + "GetItemReviewsUID/" + userId);
            var response = await client.GetStringAsync(uri).ConfigureAwait(false);
            var output = JsonConvert.DeserializeObject<ItemReviewResponse>(response);
            LoggingClass.LogServiceInfo("service responce", "userrev");
            return output;
        }

        public async Task<int> InsertUpdateReview(Review review)
        {
            try
            {
                LoggingClass.LogServiceInfo("service called", "updatrev");
                var uri = new Uri(ServiceURL + "InsertUpdateReview/");
                var content = JsonConvert.SerializeObject(review);
                var cont = new StringContent(content, System.Text.Encoding.UTF8, "application/json");
                var response = await client.PostAsync(uri, cont);
                LoggingClass.LogServiceInfo("service responce", "updatrev");
                // In debug mode it do not work, Else it works
                //var result = response.Content.ReadAsStringAsync().Result;
            }
            catch (Exception exe)
            {
                LoggingClass.LogError(exe.Message, screenid, exe.StackTrace.ToString());
            }
            return 1;
        }
        public async Task<int> DeleteReview(Review review)
        {
            try
            {
                LoggingClass.LogServiceInfo("service called", "deleterev");
                var uri = new Uri(ServiceURL + "DeleteReview/");
                var content = JsonConvert.SerializeObject(review);
                var cont = new StringContent(content, System.Text.Encoding.UTF8, "application/json");
                var response = await client.PostAsync(uri, cont); // In debug mode it do not work, Else it works
                LoggingClass.LogServiceInfo("service responce", "deleterev");
                //var result = response.Content.ReadAsStringAsync().Result;
            }
            catch (Exception exe)
            {
                LoggingClass.LogError(exe.Message, screenid, exe.StackTrace.ToString());
            }
            return 1;
        }
        public async Task<int> UpdateCustomer(Customer customer)
        {
            try
            {
                LoggingClass.LogServiceInfo("service called", "updatcs");
                var uri = new Uri(ServiceURL + "UpdateCustomer/");
                var content = JsonConvert.SerializeObject(customer);
                var cont = new StringContent(content, System.Text.Encoding.UTF8, "application/json");
                var response = await client.PostAsync(uri, cont); // In debug mode it do not work, Else it works
                LoggingClass.LogServiceInfo("service responce", "updatcs");
                //var result = response.Content.ReadAsStringAsync().Result;
            }
            catch (Exception exe)
            {
                LoggingClass.LogError(exe.Message, screenid, exe.StackTrace.ToString());
            }
            return 1;
        }
        public async Task<ItemListResponse> GetItemFavsUID(int userId)
        {
            LoggingClass.LogServiceInfo("service called", "csfavs");
            var uri = new Uri(ServiceURL + "GetItemFavsUID/" + userId);
            var response = await client.GetStringAsync(uri).ConfigureAwait(false);
            var output = JsonConvert.DeserializeObject<ItemListResponse>(response);
            LoggingClass.LogServiceInfo("service responce", "csfavs");
            return output;
        }
        public async Task<CustomerResponse> GetCustomerDetails(int userID)
        {
            LoggingClass.LogServiceInfo("service called", "getcsdet");
            var uri = new Uri(ServiceURL + "GetCustomerDetails/" + userID);
            var response = await client.GetStringAsync(uri).ConfigureAwait(false);
            var output = JsonConvert.DeserializeObject<CustomerResponse>(response);
            LoggingClass.LogServiceInfo("service responce", "getcsdet");
            return output;
        }
        public async Task<TastingListResponse> GetMyTastingsList(int customerid)
        {
            //customerid = 38691;
            LoggingClass.LogServiceInfo("service called", "cstast");
            var uri = new Uri(ServiceURL + "GetMyTastingsList/" + customerid);
            var response = await client.GetStringAsync(uri).ConfigureAwait(false);
            var output = JsonConvert.DeserializeObject<TastingListResponse>(response);
            LoggingClass.LogServiceInfo("service responce", "cstast");
            return output;
        }
    }
}