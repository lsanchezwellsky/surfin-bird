using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace MDW.openreferralsApi.helpers
{

    public static class HttpHelper
    {
        private static readonly HttpClient Client = new HttpClient();


        public static async Task<ObjectResult> AsyncCall(string url, string username, string tenantId)
        {
            using (var requestMessage = new HttpRequestMessage(HttpMethod.Get, url))
            {
                requestMessage.Headers.Add("user", username);
                requestMessage.Headers.Add("tenantId", tenantId);
                var result = await Client.SendAsync(requestMessage);
                if (result == null) return null;
                var text = await result.Content.ReadAsStringAsync();
                dynamic jToken = JToken.Parse(text);
                return new ObjectResult(jToken);
            }
        }
    }
}
