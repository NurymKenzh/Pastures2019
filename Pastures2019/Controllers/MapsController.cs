using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Pastures2019.Controllers
{
    public struct YearDay
    {
        public string year;
        public string day;
    }

    public class MapsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Modis()
        {
            ViewBag.GeoServerUrl = Startup.Configuration["GeoServerUrl"].ToString();
            ViewBag.ModisWorkspace = Startup.Configuration["ModisWorkspace"].ToString();
            ViewBag.ModisLayerTemplate1 = Startup.Configuration["ModisLayerTemplate1"].ToString();
            ViewBag.ModisLayerTemplate_MOLT_MOD13Q1006_B01_NDVI = Startup.Configuration["ModisLayerTemplate_MOLT_MOD13Q1006_B01_NDVI"].ToString();
            ViewBag.ModisLayerTemplate_MOLT_MOD13Q1006_B01_NDVI_Anomaly = Startup.Configuration["ModisLayerTemplate_MOLT_MOD13Q1006_B01_NDVI_Anomaly"].ToString();
            ViewBag.YearDays_MOLT_MOD13Q1006_B01_NDVI = GetModisYearDays_MOLT_MOD13Q1006_B01_NDVI();
            ViewBag.YearDays_MOLT_MOD13Q1006_B01_NDVI_Anomaly = GetModisYearDays_MOLT_MOD13Q1006_B01_NDVI_Anomaly();
            return View();
        }

        private YearDay[] GetModisYearDays_MOLT_MOD13Q1006_B01_NDVI()
        {
            List<YearDay> yearDays = new List<YearDay>();
            string[] layers = GetModisLayers();
            for (int i = 0; i < layers.Length; i++)
            {
                layers[i] = layers[i]
                    .Replace(Startup.Configuration["ModisLayerTemplate_MOLT_MOD13Q1006_B01_NDVI"].ToString(), "")
                    .Replace(Startup.Configuration["ModisLayerTemplate1"].ToString(), "");
                if(layers[i].Length == 7)
                {
                    yearDays.Add(new YearDay()
                    {
                        year = layers[i].Substring(0, 4),
                        day = layers[i].Substring(4, 3)
                    });
                }
            }
            return yearDays.ToArray();
        }

        private YearDay[] GetModisYearDays_MOLT_MOD13Q1006_B01_NDVI_Anomaly()
        {
            List<YearDay> yearDays = new List<YearDay>();
            string[] layers = GetModisLayers();
            for (int i = 0; i < layers.Length; i++)
            {
                layers[i] = layers[i]
                    .Replace(Startup.Configuration["ModisLayerTemplate_MOLT_MOD13Q1006_B01_NDVI_Anomaly"].ToString(), "")
                    .Replace(Startup.Configuration["ModisLayerTemplate1"].ToString(), "");
                if(layers[i].Length == 7)
                {
                    yearDays.Add(new YearDay()
                    {
                        year = layers[i].Substring(0, 4),
                        day = layers[i].Substring(4, 3)
                    });
                }
            }
            return yearDays.ToArray();
        }

        private string[] GetModisLayers()
        {
            List<string> layers = new List<string>();

            WebClient webClient = new WebClient();
            webClient.Credentials = new NetworkCredential(
                Startup.Configuration["GeoServerUser"].ToString(),
                Startup.Configuration["GeoServerPassword"].ToString());
            string GeoServerUrl = Startup.Configuration["GeoServerUrl"].ToString() + "rest/layers.json",
                modisWorkspace = Startup.Configuration["ModisWorkspace"].ToString(),
                response = webClient.DownloadString(GeoServerUrl);
            var responseData = JObject.Parse(response);
            foreach (JProperty property in responseData.Properties())
            {
                if (property.Name == "layers")
                {
                    dynamic layersObject = JsonConvert.DeserializeObject(property.Value.ToString()),
                        layerObjectArray = JsonConvert.DeserializeObject(layersObject.layer.ToString());
                    foreach (dynamic layer in layerObjectArray)
                    {
                        string layerName = layer.name;
                        if (layerName.Split(':')[0] == modisWorkspace)
                        {
                            layers.Add(layerName.Split(':')[1]);
                        }
                    }
                }
            }

            return layers.ToArray();
        }
    }
}