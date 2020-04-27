using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;

namespace DCommon
{
    public class HttpClientHelper
    {
        public HttpClientHelper(string host, Dictionary<string, string> headers = null, int timeout = 3600)
        {
            var handler = new HttpClientHandler { AutomaticDecompression = DecompressionMethods.GZip };
            HttpClient = new HttpClient(handler)
            {
                Timeout = new TimeSpan(0, 0, timeout),
                BaseAddress = new Uri(host)
            };
            headers?.ToList().ForEach(item => HttpClient.DefaultRequestHeaders.Add(item.Key, item.Value));
        }

        private HttpClient HttpClient { get; }

        public T Get<T>(string url)
        {
            var response = HttpClient.GetAsync(url).Result;
            response.EnsureSuccessStatusCode();

            var resultTask = response.Content.ReadAsStringAsync().Result;
            var result = JsonConvert.DeserializeObject<T>(resultTask);
            return result;
        }

        public T Post<T>(string url, Dictionary<string, string> args, HttpContent content = null)
        {
            if (content == null) content = new FormUrlEncodedContent(args);

            var response = HttpClient.PostAsync(url, content).Result;
            response.EnsureSuccessStatusCode();

            var resultTask = response.Content.ReadAsStringAsync().Result;
            var result = JsonConvert.DeserializeObject<T>(resultTask);
            return result;
        }

        public void Post(string url, Dictionary<string, string> args, HttpContent content = null)
        {
            if (content == null) content = new FormUrlEncodedContent(args);
            var response = HttpClient.PostAsync(url, content).Result;
            response.EnsureSuccessStatusCode();

            var resultTask = response.Content.ReadAsStringAsync().Result;
        }

        public TTo PostJsonData<TFrom, TTo>(string url, TFrom data, HttpContent content = null)
        {
            var sendData = JsonConvert.SerializeObject(data);
            if (content == null)
            {
                content = new StringContent(sendData);
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            }

            var response = HttpClient.PostAsync(url, content).Result;
            response.EnsureSuccessStatusCode();

            var resultTask = response.Content.ReadAsStringAsync().Result;
            var result = JsonConvert.DeserializeObject<TTo>(resultTask);
            return result;
        }

        public TTo PostFormUrlEncodedContent<TFrom, TTo>(string url, TFrom data, HttpContent content = null)
        {
            var dictionary = new Dictionary<string, string>();
            var properties = data.GetType().GetProperties();
            properties.ToList().ForEach(propertyinfo =>
            {
                var value = propertyinfo.GetValue(data);
                dictionary.Add(propertyinfo.Name.ToLower(), value.ToString());
            });

            if (content == null) content = new FormUrlEncodedContent(dictionary);

            //LoggerHelper.WriteLine(JsonConvert.SerializeObject(dictionary));

            var response = HttpClient.PostAsync(url, content).Result;
            response.EnsureSuccessStatusCode();

            var resultTask = response.Content.ReadAsStringAsync().Result;
            //LoggerHelper.WriteLine(resultTask);
            var result = JsonConvert.DeserializeObject<TTo>(resultTask);
            return result;
        }
    }
}