using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Npgsql;
using Pastures2019.Data;
using Pastures2019.Models;

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
        private readonly ApplicationDbContext _context;

        public MapsController(ApplicationDbContext context)
        {
            _context = context;
        }

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
            ViewBag.CATO = _context.CATO.OrderBy(c => c.Name).ToList();
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

        [HttpPost]
        public async Task<IActionResult> GetPastureInfo(
            string objectid)
        {
            string DefaultConnection = Microsoft
               .Extensions
               .Configuration
               .ConfigurationExtensions
               .GetConnectionString(Startup.Configuration, "DefaultConnection");
            pasturepol pasturepol = new pasturepol();
            using (var connection = new NpgsqlConnection(DefaultConnection))
            {
                connection.Open();
                var pasturepols = connection.Query<pasturepol>($"SELECT gid, objectid, class_id, otdely_id, " +
                    $"subtype_id, group_id, ur_v, ur_l, ur_o, ur_z, korm_v, korm_l, korm_o, korm_z, " +
                    $"recommend_, recom_catt, relief_id, zone_id, haying_id, shape_leng, shape_area " +
                    $"FROM public.pasturepol " +
                    $"WHERE objectid = {objectid};");
                pasturepol = pasturepols.FirstOrDefault();
            }
            pasturepol.otdel = _context.Otdel.FirstOrDefault(o => o.Code == pasturepol.otdely_id)?.Description;
            pasturepol.ptype = _context.PType.FirstOrDefault(p => p.Code == pasturepol.class_id)?.Description;
            pasturepol.group = _context.Soob.FirstOrDefault(s => s.Code == pasturepol.group_id)?.Description;
            pasturepol.group_lat = _context.Soob.FirstOrDefault(s => s.Code == pasturepol.group_id)?.DescriptionLat;
            pasturepol.recommend = _context.Recommend.FirstOrDefault(r => r.Code == pasturepol.recommend_)?.Description;
            pasturepol.recomcatt = _context.RecomCattle.FirstOrDefault(r => r.Code == pasturepol.recom_catt)?.Description;
            return Json(new
            {
                pasturepol
            });
        }
    }
}