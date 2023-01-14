using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Swashbuckle.AspNetCore.Annotations;

namespace IpDetectorProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientInfoController : ControllerBase
    {
        [HttpGet]
        //[Authorize]
        [SwaggerOperation(
            Summary = "Get User IP and location",
            Description = "Get User IP and location by accessing an external API",
            OperationId = "GetUserIPAndLocation",
            Tags = new[] { "IP" }
        )]
        public ActionResult<UserLocation> Get()
        {
            // Kullanıcının IP adresini al
            string ip = GetUserIP();

            // IP adresini kullanarak kullanıcının konum bilgilerini al
            string location = GetLocation(ip);

            return Ok(new UserLocation { IP = ip, Location = location });
        }

        private string GetUserIP()
        {
            string ip = "";
            try
            {
                // IP adresi almak için api çağrısı yap
                string apiUrl = "http://api.ipstack.com/check?access_key=74c49023e5079bf5f88d025c2733974a\r\n";
                var json = new WebClient().DownloadString(apiUrl);
                var data = JObject.Parse(json);

                // IP adresini al
                ip = data["ip"].ToString();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return ip;
        }
        private string GetLocation(string ip)
        {
            string location = "";
            try
            {
                // IP adresi kullanarak konum bilgilerini almak için api çağrısı yap
                string apiUrl = $"http://api.ipstack.com/{ip}?access_key=74c49023e5079bf5f88d025c2733974a\r\n";
                var json = new WebClient().DownloadString(apiUrl);
                var data = JObject.Parse(json);

                // Şehir ve ülke bilgilerini al
                string city = data["city"].ToString();
                string country = data["country_name"].ToString();

                location = $"{city}, {country}";
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return location;
        }
    }

    public class UserLocation
    {
        public string IP { get; set; }
        public string Location { get; set; }
    }
}