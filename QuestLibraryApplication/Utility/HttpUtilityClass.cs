using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Net.Http.Headers;
using Microsoft.Ajax.Utilities;
using System.Text;
using QuestLibraryApplication.Abstraction;

namespace QuestLibraryApplication.Utility
{
    public class HttpUtilityClass: IHttpUtility
    {
        private readonly string url;
        HttpClient client = new HttpClient();
        public HttpUtilityClass()
        {
            
        }
        public HttpUtilityClass(string url)
        {
            this.url = url;
            client.BaseAddress = new Uri(url);
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }
        public async Task<string> GetAsyncMethod()
        { 
            HttpResponseMessage res = await client.GetAsync(url);
            if (res.IsSuccessStatusCode)
            {
                var response = res.Content.ReadAsStringAsync().Result;
                return response;
            }
            else
            {
                return null;
            }
        }
        public async Task<string> EditAsyncMethod(string EditResponse)
        {
            HttpResponseMessage res = await client.PutAsync(url, new StringContent(EditResponse, Encoding.UTF8, "application/json"));
            if (res.IsSuccessStatusCode)
            {
                var response = res.Content.ReadAsStringAsync().Result;
                return response;
            }
            else
            {
                return null;
            }
            
        }
        public async Task<string> CreateAsyncMethod(string NewResponse)
        {
            HttpResponseMessage res = await client.PostAsync(url, new StringContent(NewResponse, Encoding.UTF8, "application/json"));
            if (res.IsSuccessStatusCode)
            {
                var response = res.Content.ReadAsStringAsync().Result;
                return response;
            }
            else
            {
                return null;
            }
        }
        public async Task<string> DeleteAsyncMethod()
        {
            HttpResponseMessage res = await client.DeleteAsync(url);
            if (res.IsSuccessStatusCode)
            {
                var response = res.Content.ReadAsStringAsync().Result;
                return response;
            }
            else
            {
                return null;
            }  
        }
    }
}