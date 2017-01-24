using JNUE_ADAPI.Helper;
using JNUE_ADAPI.Models;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace JNUE_ADAPI.AD
{
    /// Azure AD
    public static class AzureAD
    {
        public static async Task<string> getUser(string userid)
        {
            var token = await oAuth.getSessionToken();
            var res = "";

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var request = string.Format(Properties.AzGraphApi + "users/{0}?api-version=1.6", userid + "@" + Properties.AzDomainUrl);

                using (var response = await client.GetAsync(request))
                {
                    if (response.Content != null)
                    {
                        res = await response.Content.ReadAsStringAsync();
                    }
                };
            };
            return res;
        }
        
        public static async Task<string> setUsageLocation(string userid)
        {
            var token = await oAuth.getSessionToken();
            var obj = "{\"usageLocation\": \"KR\" }";
            var res = "";
            var json = new StringContent(obj, Encoding.UTF8, "application/json");

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var request = string.Format(Properties.AzGraphApi + "users/{0}?api-version=1.6", userid);

                using (var response = await PatchAsync(client, request, json))
                {
                    if (response.Content != null)
                    {
                        res = await response.Content.ReadAsStringAsync(); 
                    }
                };
            };
            return res;
        }
        
        public static async Task<string> setAssignLicense(string userid, string skuid, string disableplans)
        {
            var token = await oAuth.getSessionToken();
            var res = "";
            var obj = "";

            //if (disableplans.Length > 0)
            if (disableplans.Equals("NONE") != true)
            {
                obj = "{\"addLicenses\": [{\"disabledPlans\": [\"" + disableplans + "\"], \"skuId\": \"" + skuid + "\"} ], \"removeLicenses\": [] }";
            }
            else
            {
                obj = "{\"addLicenses\": [{\"disabledPlans\": [], \"skuId\": \"" + skuid + "\"} ], \"removeLicenses\": [] }";
            }

            var json = new StringContent(obj, Encoding.UTF8, "application/json");

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var request = string.Format(Properties.AzGraphApi + "users/{0}/assignLicense?api-version=1.6", userid);

                using (var response = await client.PostAsync(request, json))
                {
                    if (response.Content != null)
                    {
                        res = await response.Content.ReadAsStringAsync();
                    }
                };
            };
            return obj;
        }
        
        public static async Task<string> delAssignLicense(string userid, string skuid)
        {
            var token = await oAuth.getSessionToken();
            var res = "";
            var obj = "{ \"addLicenses\":[], \"removeLicenses\":[\"" + skuid + "\"] }";

            var json = new StringContent(obj, Encoding.UTF8, "application/json");

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var request = string.Format(Properties.AzGraphApi + "users/{0}/assignLicense?api-version=1.6", userid);

                using (var response = await client.PostAsync(request, json))
                {
                    if (response.Content != null)
                    {
                        res = await response.Content.ReadAsStringAsync();
                    }
                };
            };
            return res;
        }
        
        public static Task<HttpResponseMessage> PatchAsync(this HttpClient client, string requestUri, HttpContent content)
        {
            HttpRequestMessage request = new HttpRequestMessage
            {
                Method = new HttpMethod("PATCH"),
                RequestUri = new Uri(client.BaseAddress + requestUri),
                Content = content,
            };
            return client.SendAsync(request);
        }
    }
}