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
        public string monthday;
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
            ViewBag.ModisLayerTemplate_MOLA_MYD13Q1006_B01_NDVI = Startup.Configuration["ModisLayerTemplate_MOLA_MYD13Q1006_B01_NDVI"].ToString();
            ViewBag.ModisLayerTemplate_MOLA_MYD13Q1006_B01_NDVI_Anomaly = Startup.Configuration["ModisLayerTemplate_MOLA_MYD13Q1006_B01_NDVI_Anomaly"].ToString();
            ViewBag.YearDays_MOLT_MOD13Q1006 = GetModisYearDays_MOLT_MOD13Q1006();
            ViewBag.YearDays_MOLA_MYD13Q1006 = GetModisYearDays_MOLA_MYD13Q1006();
            return View();
        }

        public ActionResult FodderResources()
        {
            ViewBag.GeoServerUrl = Startup.Configuration["GeoServerUrl"].ToString();
            return View();
        }

        private YearDay[] GetModisYearDays_MOLT_MOD13Q1006()
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
                        day = layers[i].Substring(4, 3),
                        monthday = $"{layers[i].Substring(4, 3)}: " +
                            $"{(new DateTime(Convert.ToInt32(layers[i].Substring(0, 4)), 1, 1).AddDays(Convert.ToInt32(layers[i].Substring(4, 3)))).ToString("dd/MM")}".Replace('.', '/') +
                            $" - " +
                            $"{(new DateTime(Convert.ToInt32(layers[i].Substring(0, 4)), 1, 1).AddDays(Convert.ToInt32(layers[i].Substring(4, 3)) + 16)).ToString("dd/MM")}".Replace('.', '/')
                    });
                }
            }
            return yearDays.ToArray();
        }

        private YearDay[] GetModisYearDays_MOLA_MYD13Q1006()
        {
            List<YearDay> yearDays = new List<YearDay>();
            string[] layers = GetModisLayers();
            for (int i = 0; i < layers.Length; i++)
            {
                layers[i] = layers[i]
                    .Replace(Startup.Configuration["ModisLayerTemplate_MOLA_MYD13Q1006_B01_NDVI"].ToString(), "")
                    .Replace(Startup.Configuration["ModisLayerTemplate1"].ToString(), "");
                if(layers[i].Length == 7)
                {
                    yearDays.Add(new YearDay()
                    {
                        year = layers[i].Substring(0, 4),
                        day = layers[i].Substring(4, 3),
                        monthday = $"{layers[i].Substring(4, 3)}: " +
                            $"{(new DateTime(Convert.ToInt32(layers[i].Substring(0, 4)), 1, 1).AddDays(Convert.ToInt32(layers[i].Substring(4, 3)))).ToString("dd/MM")}".Replace('.', '/') +
                            $" - " +
                            $"{(new DateTime(Convert.ToInt32(layers[i].Substring(0, 4)), 1, 1).AddDays(Convert.ToInt32(layers[i].Substring(4, 3)) + 16)).ToString("dd/MM")}".Replace('.', '/')
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