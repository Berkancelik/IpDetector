using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Net;
using System.Web.Http;
using System.Linq;
using Newtonsoft.Json;
using HttpGetAttribute = System.Web.Http.HttpGetAttribute;
using RouteAttribute = System.Web.Http.RouteAttribute;

namespace test.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ClientInfoController : ControllerBase
    {
        [HttpGet]
        public HttpResponseMessage GetClientInfo(HttpRequestMessage request)
        {
            var clientIp = "";
            if (request.Headers.Contains("X-Real-IP"))
            {
                clientIp = request.Headers.GetValues("X-Real-IP").First();
            }
            else if (request.Headers.Contains("X-Forwarded-For"))
            {
                clientIp = request.Headers.GetValues("X-Forwarded-For").First();
            }
            var location = GetLocationInfo(clientIp);
            var isp = GetISPInfo(clientIp);
            var clientInfo = new { IP = clientIp, Location = location, ISP = isp };
            return Request.CreateResponse(HttpStatusCode.OK, clientInfo);
        }

        private string GetLocationInfo(string clientIp)
        {
            using (var client = new WebClient())
            {
                var json = client.DownloadString("http://ip-api.com/json/" + clientIp);
                dynamic data = JsonConvert.DeserializeObject(json);
                return data.city + ", " + data.regionName + ", " + data.country;
            }
        }

        private string GetISPInfo(string clientIp)
        {
            using (var client = new WebClient())
            {
                var json = client.DownloadString("http://ip-api.com/json/" + clientIp);
                dynamic data = JsonConvert.DeserializeObject(json);
                return data.isp;
            }
        }
    }
}