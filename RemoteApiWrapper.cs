using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Rbac.Auth.RemoteApi
{
    public class RemoteApiWrapper
    {
        private readonly string _domain;

        public RemoteApiWrapper(string domain)
        {
            _domain = domain;
        }

        protected async Task<string> PostRequestAsync(HttpRequest request, string baseUrl, List<KeyValuePair<string, string>> paramArray)
        {
            var url = $"{_domain}{baseUrl}";
            var token = request.Headers["Authorization"];
            var accessToken = token.ToString()?.Substring(7);
            var json = await HttpClientHelper.Instance.HttpPostRequestAsync(url, accessToken, paramArray, request.HttpContext.RequestAborted);
            var d2ret = JsonConvert.DeserializeObject<D2JsonResultModel>(json);
            if (d2ret.code == 0)
            {
                return d2ret.data.ToString();
            }
            else
            {
                throw new Exception(d2ret.msg);
            }
        }

        protected async Task<string> GetResponseAsync(HttpRequest request, string baseUrl, List<KeyValuePair<string, string>> paramArray)
        {
            var url = $"{_domain}{baseUrl}";
            var token = request.Headers["Authorization"];
            var accessToken = token.ToString()?.Substring(7);
            var json = await HttpClientHelper.Instance.HttpGetResponseAsync(url, accessToken, paramArray, request.HttpContext.RequestAborted);
            var d2ret = JsonConvert.DeserializeObject<D2JsonResultModel>(json);
            if (d2ret.code == 0)
            {
                return d2ret.data.ToString();
            }
            else
            {
                throw new Exception(d2ret.msg);
            }
        }
    }
}
