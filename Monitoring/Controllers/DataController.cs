using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Authentication;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;
using Newtonsoft.Json.Linq;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Monitoring.Controllers
{
    public class Sensors
    {
        public double Temperature_Inside { get; set; }
        public double Humidity_Inside { get; set; }
        public double Power_Voltage { get; set; }
        public string Power_Status { get; set; }
    }
    [Route("api/[controller]")]
    public class DataController : Controller
    {

        public DataController()
        {
        }

        // GET: api/values
        [HttpGet]
        public Array GetData()
        {
            return GetData("Temperature_Inside", DateTime.Today, DateTime.Today.AddDays(1), 10000);
        }

        // GET: api/values
        [HttpGet("sensor/{sensor}")]
        public Array GetData(string sensor)
        {
            return GetData(sensor, DateTime.Today, DateTime.Today.AddDays(1), 10000);
        }

        // GET: api/values
        [HttpGet("latest")]
        public Sensors GetLatestState()
        {

            return MongoAccessor.GetState();
        }

        //[HttpGet]
        //public JsonResult GetData()
        //{

        //    return JArray.Parse() GetData(DataTypes.Sin, DateTime.Today, DateTime.Today.AddDays(1), 100);
        //}

        [HttpGet("{type}/{from}/{to}/{pnts}")]
        public Array GetData(string sensor, DateTime from, DateTime to, int maxPoints)
        {
            return MongoAccessor.LoadTimeSeries(sensor);
            //return Sin(from, to, maxPoints);
        }

        private static Array Sin(DateTime from, DateTime to, int maxPoints)
        {
            var result = new ArrayList();

            for (DateTime t = from; t <= to; t = t + TimeSpan.FromTicks((to - from).Ticks / maxPoints))
            {
                long y = Convert.ToInt32(1000 * Math.Sin(2 * Math.PI * t.Ticks / TimeSpan.TicksPerDay));
                result.Add(new long[] { t.ToJavaScriptMilliseconds(), y });
            }
                    
            return result.ToArray();
        }

    }
}
