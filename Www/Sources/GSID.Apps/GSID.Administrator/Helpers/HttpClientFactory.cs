using GSID.Model.ExtraEntities;
using GSID.Service.MongoRepositories.Service;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace GSID.Admin.Helpers
{
    public static class HttpResponseMessageExtension
    {
        public static async Task<ExceptionResponse> ExceptionResponse(this HttpResponseMessage httpResponseMessage)
        {
            string responseContent = await httpResponseMessage.Content.ReadAsStringAsync();
            ExceptionResponse exceptionResponse = JsonConvert.DeserializeObject<ExceptionResponse>(responseContent);
            return exceptionResponse;
        }
    }

    public class ExceptionResponse
    {
        public string Message { get; set; }
        public string ExceptionMessage { get; set; }
        public string ExceptionType { get; set; }
        public string StackTrace { get; set; }
        public ExceptionResponse InnerException { get; set; }
    }

    public static class HttpClientFactory
    {
        public static HttpClient GetClient()
        {
            var paraService = DependencyResolver.Current.GetService<IParameterService>();
            var obj = paraService.GetByCode(new BFOAPIConfig().Code);
            HttpClient client = new HttpClient();
            if (obj != null)
            {
                var model = JsonConvert.DeserializeObject<BFOAPIConfig>(obj.Content.ToString());
                client.BaseAddress = new Uri(model.Url);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/json"));
            }

            return client;
        }
    }

    public static class JsonContentFactory
    {
        public static StringContent CreateJsonContent(object obj)
        {
            var json = JsonConvert.SerializeObject(obj);
            var content = new StringContent(json, UnicodeEncoding.UTF8, "application/json");
            return content;
        }
    }
}