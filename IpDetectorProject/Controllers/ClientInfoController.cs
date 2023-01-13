using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using NWebsec.Core.Web;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace IpDetectorProject.Controllers
{
    [Route("api/[controller]")]
    public class ClientInfoController : ApiController
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