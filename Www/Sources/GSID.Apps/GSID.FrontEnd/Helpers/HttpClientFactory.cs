using GSID.Model.ExtraEntities;
using GSID.Service.MongoRepositories;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Mvc;
using GSID.Service.MongoRepositories.Service;

namespace GSID.FrontEnd.Helpers
{
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

}