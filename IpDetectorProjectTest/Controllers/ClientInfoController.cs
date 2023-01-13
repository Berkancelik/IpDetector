using System;
using System.Linq;
using System.Net;
using System.Net.Http;
  using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace IpDetectorProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientInfoController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetClientInfo()
        {
            var clientIp = HttpContext.Connection.RemoteIpAddress.ToString();
            var location = GetLocationInfo(clientIp);
            var isp = GetISPInfo(clientIp);
            var clientInfo = new { IP = clientIp, Location = location, ISP = isp };
            return Ok(clientInfo);
        }

        private string GetLocationInfo(string clientIp)
        {
            //Api key buraya eklenmeli
            var apiKey = "<Your_API_Key>";
            using (var client = new HttpClient())
            {
                var json = client.GetStringAsync("https://api.ipinfodb.com/v3/ip-city/?key=" + apiKey + "&ip=" + clientIp + "&format=json").Result;
                dynamic data = JsonConvert.DeserializeObject(json);
                return data.cityName + ", " + data.regionName + ", " + data.countryName;
            }
        }

        private string GetISPInfo(string clientIp)
        {
            //Api key buraya eklenmeli
            var apiKey = "<Your_API_Key>";
            using (var client = new HttpClient())
            {
                var json = client.GetStringAsync("https://api.ipinfodb.com/v3/ip-isp/?key=" + apiKey + "&ip=" + clientIp + "&format=json").Result;
                dynamic data = JsonConvert.DeserializeObject(json);
                return data.isp;
            }
        }
    }
}