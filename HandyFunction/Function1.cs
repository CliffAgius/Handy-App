using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace HandyFunction
{
    public static class Function1
    {
        [FunctionName("Function1")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string sensorList = req.Query["sensorJSON"];

            List<SensorData> result = JsonConvert.DeserializeObject<List<SensorData>>(sensorList);

            return sensorList != null
                ? (ActionResult)new OkObjectResult($"Data Recieved... {result.Count}")
                : new BadRequestObjectResult("Please pass Sensor data in the query string or in the request body");
        }
    }

    public class SensorData
    {
        public int OpenSensorReading { get; set; }
        public int CloseSensorReading { get; set; }
    }
}
