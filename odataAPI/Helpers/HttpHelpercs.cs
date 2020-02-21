using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
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
    }
}
