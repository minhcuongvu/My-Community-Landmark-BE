using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DataController:ControllerBase
    {
        private static readonly string[] Usernames = new[]
        {
        "Bob O'Brian", "Henry Clary", "Aston Martin", "Leo Dio", "Ben Shapiro", "Jordan P", "Keyle", "Mercury", "user124", "Anonymous"
        };

        private readonly ILogger<DataController> _logger;

        public DataController(ILogger<DataController> logger)
        {
            _logger = logger;
        }

        public class Data
        {
            public string Username { get; set; }
            public float[] Location { get; set; }
            public string Note { get; set; }
        }

        private static List<Data> DataArray = new List<Data> { };

        static bool init = false;
        private static List<Data> GetData()
        {
            string fileName = "Notes.json";
            if (!init)
            {
                // Sample data
                var test = new Data {
                    Username = Usernames[0],
                    Location = new float[2] { 51.5f,-1.09f },
                    Note = "Sugar Rush"
                };
                DataArray.Add(test);
                string jsonString = JsonSerializer.Serialize(DataArray);
                System.IO.File.WriteAllText(fileName,jsonString);
                init = true;
            }

            // Get data from a JSON file, nothing fancy
            string jsonString1 = System.IO.File.ReadAllText(fileName);
            List<Data> dataArray = JsonSerializer.Deserialize<List<Data>>(jsonString1)!;
            return dataArray;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        [Route("[controller]/Get")]
        public IEnumerable<Data> Get()
        {
            HttpContext.Response.Headers.Add("Access-Control-Allow-Origin","*");
            HttpContext.Response.Headers.Add("X-Custom-Header","Hee-Hee");
            HttpContext.Response.Headers.Add("Access-Control-Allow-Headers","Origin, X-Requested-With, Content-Type, Accept");

            return GetData();
        }

        [HttpPost(Name = "PostData")]
        [Route("[controller]/Post")]
        public IEnumerable<Data> Post(Data data)
        {
            DataArray.Add(data);
            string jsonString = JsonSerializer.Serialize(DataArray);
            string fileName = "Notes.json";
            System.IO.File.WriteAllText(fileName,jsonString);

            return DataArray;
        }
    }
}