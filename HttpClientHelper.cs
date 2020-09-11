using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Rbac.Auth.RemoteApi
{
    internal class HttpClientHelper
    {
        private HttpClient _httpClient;
        private HttpClientHelper()
        {
            var handler = new HttpClientHandler() { AutomaticDecompression = DecompressionMethods.None };
            _httpClient = new HttpClient(handler);
            _httpClient.DefaultRequestHeaders.Add("User-Agent", @"Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/79.0.3945.88 Safari/537.36");
        }

        private static HttpClientHelper _httpClientHelper;
        public static HttpClientHelper Instance
        {
            get
            {
                if (_httpClientHelper == null)
                {
                    _httpClientHelper = new HttpClientHelper();
                }
                return _httpClientHelper;
            }
        }

        /// <summary>
        /// Get请求
        /// </summary>
        /// <param name="url"></param>
        /// <param name="accessToken"></param>
        /// <param name="paramArray"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<string> HttpGetResponseAsync(string url, string accessToken, List<KeyValuePair<string, string>> paramArray, CancellationToken cancellationToken)
        {
            url = url + "?" + BuildParam(paramArray);

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            var response = await _httpClient.GetAsync(url, cancellationToken);

            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsStringAsync();
            return result;
        }

        /// <summary>
        /// Post 请求
        /// </summary>
        /// <param name="url"></param>
        /// <param name="accessToken"></param>
        /// <param name="paramArray"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<string> HttpPostRequestAsync(string url, string accessToken, ICollection<KeyValuePair<string, string>> paramArray, CancellationToken cancellationToken)
        {
            var myContent = BuildParam(paramArray);
            var content = new StringContent(myContent, Encoding.UTF8, "application/x-www-form-urlencoded");

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            HttpResponseMessage response = await _httpClient.PostAsync(url, content, cancellationToken);

            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsStringAsync();
            return result;
        }

        private string BuildParam(ICollection<KeyValuePair<string, string>> paramArray, Encoding encode = null)
        {
            string url = "";

            if (encode == null) encode = Encoding.UTF8;

            if (paramArray != null && paramArray.Count > 0)
            {
                var parms = "";
                foreach (var item in paramArray)
                {
                    parms += string.Format("{0}={1}&", Encode(item.Key, encode), Encode(item.Value, encode));
                }
                if (parms != "")
                {
                    parms = parms.TrimEnd('&');
                }
                url += parms;
            }
            return url;
        }

        private string Encode(string content, Encoding encode = null)
        {
            if (encode == null) return content;

            return System.Web.HttpUtility.UrlEncode(content, Encoding.UTF8);

        }
    }
}
