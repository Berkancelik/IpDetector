using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RabbitMQ.Client;
using Swashbuckle.AspNetCore.Annotations;

namespace IpDetectorProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientInfoController : ControllerBase
    {
        private readonly IConnection _connection;
        public ClientInfoController(IConnection connection)
        {
            _connection = connection;
        }

        [HttpGet]
        public ActionResult<UserLocation> Get()
        {
            // Kullanıcının IP adresini al
            string ip = GetUserIP();

            // IP adresini kullanarak kullanıcının konum bilgilerini al
            string location = GetLocation(ip);

            // RabbitMQ publisher kullanarak verileri gönder
            var channel = _connection.CreateModel();
            channel.QueueDeclare(queue: "IPLocationQueue", durable: false, exclusive: false, autoDelete: false, arguments: null);
            var message = $"IP: {ip}, Location: {location}";
            var body = System.Text.Encoding.UTF8.GetBytes(message);
            channel.BasicPublish(exchange: "", routingKey: "IPLocationQueue", basicProperties: null, body: body);

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