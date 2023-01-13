using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Net;

namespace GetIpDetector.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientInfoController : ControllerBase
    {
        [HttpGet]
        public HttpResponseMessage GetClientInfo(HttpRequestMessage request)
        {
            var clientIp = ((HttpContextWrapper)request.Properties["MS_HttpContext"]).Request.UserHostAddress;
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
