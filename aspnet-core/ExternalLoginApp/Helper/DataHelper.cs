using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace ExternalLoginApp.Helper
{
    public static class DataHelper<T> where T : class, new()
    {
        public async static Task<ResponseVM<T>> Execute(string baseUrl, string route, string tenantId, OperationType type, object payload = null)
        {
            ResponseVM<T> response = new ResponseVM<T>();
            try
            {
                HttpClient client = new HttpClient()
                {
                    BaseAddress = new Uri(baseUrl)
                };
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Add("crossDomain", "true");
                client.DefaultRequestHeaders.Add("Abp.TenantId", tenantId);

                HttpResponseMessage httpResponse = null;
                if (type == OperationType.GET)
                {
                    httpResponse = await client.GetAsync(route);
                }
                else if (type == OperationType.POST)
                {
                    var data = JsonConvert.SerializeObject(payload);
                    var stringContent = new StringContent(data, Encoding.UTF8, "application/json");
                    httpResponse = await client.PostAsync(route, stringContent);
                }
                var result = await httpResponse.Content.ReadAsStringAsync();
                if (httpResponse.IsSuccessStatusCode)
                {
                    response.Result = JsonConvert.DeserializeObject<Result<T>>(result, new IsoDateTimeConverter());
                    //response.Message = "";
                }
                else
                {
                    response.Result = JsonConvert.DeserializeObject<Result<T>>(result, new IsoDateTimeConverter());
                }
                return response;
            }
            catch (Exception ex)
            {
                //response.Message = "Error occured!!";
            }
            return response;
        }

        public async static Task<ResponseVM<T>> ExecuteWithToken(string baseUrl, string route, string tenantId, OperationType type, string token, object payload = null)
        {
            ResponseVM<T> response = new ResponseVM<T>();
            try
            {
                HttpClient client = new HttpClient()
                {
                    BaseAddress = new Uri(baseUrl)
                };
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Add("crossDomain", "true");
                client.DefaultRequestHeaders.Add("Abp.TenantId", tenantId);
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);

                HttpResponseMessage httpResponse = null;
                if (type == OperationType.GET)
                {
                    httpResponse = await client.GetAsync(route);
                }
                else if (type == OperationType.POST)
                {
                    var data = JsonConvert.SerializeObject(payload);
                    var stringContent = new StringContent(data, Encoding.UTF8, "application/json");
                    httpResponse = await client.PostAsync(route, stringContent);
                }
                var result = await httpResponse.Content.ReadAsStringAsync();
                if (httpResponse.IsSuccessStatusCode)
                {
                    response.Result = JsonConvert.DeserializeObject<Result<T>>(result, new IsoDateTimeConverter());
                }
                else
                {
                    response.Result = JsonConvert.DeserializeObject<Result<T>>(result, new IsoDateTimeConverter());
                }
                return response;
            }
            catch (Exception ex)
            {
            }
            return response;
        }
    }

    public enum OperationType
    {
        GET,
        POST,
        PUT,
        DELETE
    }
    public class Response<T> where T : class
    {
        public string Message { get; set; }
        public T Data { get; set; }
    }

    public class ResponseVM<T> where T : class
    {
        public Result<T> Result { get; set; }
    }
    public class Result<T> where T : class
    {
        [JsonProperty("result")]
        public T Data { get; set; }
        public string Message { get; set; }
        // public T Data { get; set; }

        [JsonProperty("targetUrl")]
        public object TargetUrl { get; set; }

        [JsonProperty("success")]
        public bool Success { get; set; }

        [JsonProperty("error")]
        public Root Error { get; set; }

        [JsonProperty("unAuthorizedRequest")]
        public bool UnAuthorizeRequest { get; set; }

        [JsonProperty("__abp")]
        public bool __Abp { get; set; }
    }
    public class Root
    {
        public int code { get; set; }
        public string message { get; set; }
        public string details { get; set; }
        public object validationErrors { get; set; }
    }
}
