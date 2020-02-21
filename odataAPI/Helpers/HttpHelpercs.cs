using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace odataAPI.Helpers
{

    public static class HttpHelper
    {
        

        public static async Task<ObjectResult> AsyncCall(string url, string username, string tenantId)
        {
            HttpClientHandler clientHandler = new HttpClientHandler();
            clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };

            var client = new HttpClient(clientHandler);

            using (var requestMessage = new HttpRequestMessage(HttpMethod.Get, url))
            {
                requestMessage.Headers.Add("user", username);
                requestMessage.Headers.Add("x-tenant", tenantId);
                var result = await client.SendAsync(requestMessage);
                var response = await result.Content.ReadAsStringAsync();
                if (response == null) return null;
                dynamic jToken = JToken.Parse(response);
                return new ObjectResult(jToken);
            }
        }

        public static async Task PostAsync(string url, string tenantId, object postData)
        {
            HttpClientHandler clientHandler = new HttpClientHandler();
            clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };

            var client = new HttpClient(clientHandler);
            var json = JsonConvert.SerializeObject(postData);
            var data = new StringContent(json, Encoding.UTF8, "application/json");
            var content = new System.Net.Http.StringContent(json, Encoding.UTF8, "application/json");
            using (var requestMessage = new HttpRequestMessage(HttpMethod.Post, url))
            {
                requestMessage.Headers.Add("x-tenant", tenantId);
                requestMessage.Content = content;
                var result = await client.SendAsync(requestMessage);
            }

            
        }

     
    }
}
