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
            // kullanıcı Ip adresini alıyoruz 
            var clientIp = HttpContext.Connection.RemoteIpAddress.ToString();

            // eğer ip adresi ::1 ise , kullanıcının pc'sinin Ip adresi dns sınıfı olarak kullanılıyor
            if (clientIp == "::1")
            {
                clientIp = Dns.GetHostEntry(Dns.GetHostName()).AddressList[2].ToString();
            }

            // kullanıcının konum bilgisi alınıyor 
            var location = GetLocationInfo(clientIp);
            // kullanınının Ip adresi ve konum bilgileri object olarak dönderiyoruz 
             var clientInfo = new { IP = clientIp, Location = location };
            return Ok(clientInfo);
        }


        // kullanınının Ip adresi ve konum bilgileri object olarak dönderiyor

        private string GetLocationInfo(string clientIp)
        {
            // Api Key 
            var apiKey = "4243c5a6ffad2313454d4fc5122162c8\r\n";
            using (var client = new HttpClient())
            {
                // yapılan istekte kullanıcının Ip adresi ile birliklte gönderiyoruz
                var json = client.GetStringAsync("http://api.ipstack.com/" + clientIp + "?access_key=" + apiKey).Result;
                dynamic data = JsonConvert.DeserializeObject(json);

                // Kulanıcının şehir, bölge ve ülke bilgileri döndürülür 
                return data.city + ", " + data.region_name + ", " + data.country_name;
            }
        }

     }
}