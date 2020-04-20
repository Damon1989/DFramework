using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using WebApiClient;

namespace MyMvcTest.Controllers
{
    using System.Net.Http;
    using System.Threading.Tasks;
    using System.Web.Mvc;

    using MyMvcTest.Helper;

    using Newtonsoft.Json;

    using WebApiClient.Attributes;

    public class WebApiClientController : Controller
    {
        // GET: WebApiClient
        public async Task<ActionResult> Index()
        {
            var api = HttpApi.Resolve<IUserbApi>();
            var userinfoList = await api.GetAsync();
            ViewBag.UserInfoList = userinfoList;
            return View();
        }

        public async Task<ActionResult> Abount()
        {
            var api = HttpApi.Create<IMyWebApi>();
            var response=await api.GetUserAsync();
            if (response.IsSuccessStatusCode)
            {
                ViewBag.UserInfoList =
                    JsonConvert.DeserializeObject<List<UserInfo>>(await response.Content.ReadAsStringAsync());
            }
            return this.View();
        }
    }



    [Obsolete("host无法配置,global全局注册 动态配置")]
    public interface IMyWebApi : IHttpApi
    {
        [WebApiClient.Attributes.HttpGet("http://jsonplaceholder.typicode.com/users")]
        ITask<HttpResponseMessage> GetUserAsync();
    }

    public interface IUserbApi : IHttpApi
    {
        [WebApiClient.Attributes.HttpGet("users")]
        ITask<List<UserInfo>> GetAsync();
    }

    public class UserInfo
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public Address Address { get; set; }

        public string Phone { get; set; }

        public string Website { get; set; }

        public Company Company { get; set; }
    }

    public class Address
    {
        public string Street { get; set; }

        public string Suite { get; set; }

        public string City { get; set; }

        public string ZipCode { get; set; }

        public Geo Geo { get; set; }
    }

    public class Geo
    {
        public decimal Lat { get; set; }

        public string Lng { get; set; }
    }

    public class Company
    {
        public string Name { get; set; }

        public string CatchPhrase { get; set; }

        public string Bs { get; set; }
    }
}