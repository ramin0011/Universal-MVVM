using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using UniMvvm.DataServices.Interfaces;

namespace UniMvvm.DataServices.Base
{
    public class RequestProvider : IRequestProvider
    {
        private readonly JsonSerializerSettings _serializerSettings;
        public RequestProvider()
        {
            _serializerSettings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                DateTimeZoneHandling = DateTimeZoneHandling.Utc,
                NullValueHandling = NullValueHandling.Ignore
            };
            _serializerSettings.Converters.Add(new StringEnumConverter());
        }

     

        private static HttpClient _httpClient;
        private HttpClient CreateHttpClient
        {
            get
            {
                if (_httpClient == null)
                {
                    _httpClient = new HttpClient {Timeout = TimeSpan.FromMinutes(2)};
                    _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                  //  _httpClient.DefaultRequestHeaders.AcceptEncoding.Add(new StringWithQualityHeaderValue("gzip"));
                }
                return _httpClient;
            }
        }

        public HttpClient AddHeaders(HttpClient client,Dictionary<string, string> headers)
        {
            if (headers == null)
                return client;
            else
                foreach (var header in headers)
                {
                    if(header.Key.Equals("Authorization",StringComparison.InvariantCultureIgnoreCase))
                        client.DefaultRequestHeaders.Authorization=new AuthenticationHeaderValue("Bearer", header.Value);
                    else
                    client.DefaultRequestHeaders.Add(header.Key, new[] { header.Value });
                }
            return client;
        }

        public async Task<TResult> GetAsync<TResult>(string uri, Dictionary<string, string> headers=null)
        {
            var response = await AddHeaders(CreateHttpClient,headers).GetAsync(uri);
            await HandleResponse(response);
            TResult result = JsonConvert.DeserializeObject<TResult>(await response.Content.ReadAsStringAsync(), _serializerSettings);
            return result;
        }

        public Task<TResult> PostAsync<TResult>(string uri, TResult data, Dictionary<string, string> headers=null) => PostAsync<TResult, TResult>(uri, data,headers);

        public async Task<TResult> PostAsync<TRequest, TResult>(string uri, TRequest data, Dictionary<string, string> headers=null)
        {
            var content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");
            var res = await AddHeaders(CreateHttpClient, headers).PostAsync(uri, content);
            return JsonConvert.DeserializeObject<TResult>(await res.Content.ReadAsStringAsync(), _serializerSettings);         
        }

        public Task<TResult> PutAsync<TResult>(string uri, TResult data)
        {
            return PutAsync<TResult, TResult>(uri, data);
        }

        public async Task<TResult> PutAsync<TRequest, TResult>(string uri, TRequest data)
        {
            string serialized = await Task.Run(() => JsonConvert.SerializeObject(data, _serializerSettings));
            var response = await CreateHttpClient.PutAsync(uri, new StringContent(serialized, Encoding.UTF8, "application/json"));

            await HandleResponse(response);

            string responseData = await response.Content.ReadAsStringAsync();

            return await Task.Run(() => JsonConvert.DeserializeObject<TResult>(responseData, _serializerSettings));
        }

   

        private async Task HandleResponse(HttpResponseMessage response)
        {
            if (!response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                if (response.StatusCode == HttpStatusCode.Forbidden || response.StatusCode == HttpStatusCode.Unauthorized)
                    throw new Exception(content);
                throw new HttpRequestException(content);
            }
        }
    }
}
