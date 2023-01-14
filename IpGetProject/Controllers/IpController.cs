using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System;
using Newtonsoft.Json.Linq;

namespace IpGetProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IpController : ControllerBase
    {

    //    [HttpGet]
    //    public IActionResult Get()
    //    {
    //        // Kullanıcının IP adresini al
    //        string ip = GetUserIP();

    //        // IP adresini kullanarak kullanıcının konum bilgilerini al
    //        string location = GetLocation(ip);

    //        return Ok(new { IP = ip, Location = location });
    //    }

    //    public string GetUserIP()
    //    {
    //        string ip = "";
    //        try
    //        {
    //            // IP adresi almak için api çağrısı yap
    //            string apiUrl = "http://api.ipstack.com/check?access_key=YOUR_API_KEY";
    //            var json = new WebClient().DownloadString(apiUrl);
    //            var data = JObject.Parse(json);

    //            // IP adresini al
    //            ip = data["ip"].ToString();
    //        }
    //        catch (Exception ex)
    //        {
    //            Console.WriteLine(ex.Message);
    //        }

    //        return ip;
    //    }

    //    public string GetLocation(string ip)
    //    {
    //        string location = "";
    //        try
    //        {
    //            // IP adresi kullanarak konum bilgilerini almak için api çağrısı yap
    //            string apiUrl = $"http://api.ipstack.com/{ip}?access_key=4243c5a6ffad2313454d4fc5122162c8\r\n";
    //            var json = new WebClient().DownloadString(apiUrl);
    //            var data = JObject.Parse(json);

    //            // Şehir ve ülke bilgilerini al
    //            string city = data["city"].ToString();
    //            string country = data["country_name"].ToString();

    //            location = $"{city}, {country}";
    //        }
    //        catch (Exception ex)
    //        {
    //            Console.WriteLine(ex.Message);
    //        }

    //        return location;
    //    }
    //}
}