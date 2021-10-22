using Newtonsoft.Json;
using System;
using System.Net.Http;

namespace ClientMVC.Data
{
    public class CmsClient
    {
        public CmsClient()
        {}

        public static T Get<T>(string url)
        {
            using (HttpClient httpClient = new HttpClient())
            {
                using (HttpResponseMessage response = httpClient.GetAsync(url).Result)
                {
                    if (!response.IsSuccessStatusCode)
                    {
                        throw new Exception(response.Content.ReadAsStringAsync().Result);
                    }

                    var settings = new JsonSerializerSettings();
                    settings.NullValueHandling = NullValueHandling.Ignore;
                    settings.MissingMemberHandling = MissingMemberHandling.Ignore;
                    return JsonConvert.DeserializeObject<T>(response.Content.ReadAsStringAsync().Result, settings);
                }
            }
        }
    }
}
