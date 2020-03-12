using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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

    public class analytic
    {
        public string raster;
        public decimal min;
        public decimal max;
        public decimal median;
        public decimal majority;
        public decimal mean;
        public decimal objectid;
        public DateTime date;
        public int day;
    }

    public class year_dataset
    {
        public int year;
        public List<decimal?> data;
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
            ViewBag.CATO = _context.CATO.OrderBy(c => c.Name).ToList();
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

            ViewBag.Otdel = new SelectList(_context.Otdel.ToList().OrderBy(o => o.Description), "Code", "Description");
            ViewBag.PType = new SelectList(_context.PType.ToList().OrderBy(p => p.Description), "Code", "Description");
            ViewBag.Soob = new SelectList(_context.Soob.ToList().OrderBy(s => s.Description), "Code", "Description");
            ViewBag.Recommend = new SelectList(_context.Recommend.ToList().OrderBy(s => s.Description), "Code", "Description");
            ViewBag.RecomCattle = new SelectList(_context.RecomCattle.ToList().OrderBy(s => s.Description), "Code", "Description");
            return View();
        }

        public ActionResult LandSupply()
        {
            ViewBag.CATO = _context.CATO.OrderBy(c => c.Name).ToList();
            ViewBag.GeoServerUrl = Startup.Configuration["GeoServerUrl"].ToString();
            ViewBag.SType = new SelectList(_context.SType.ToList().OrderBy(s => s.Description), "Code", "Description");
            ViewBag.DominantType = new SelectList(_context.DominantType.ToList().OrderBy(s => s.Description), "Code", "Description");
            ViewBag.SupplyRecommend = new SelectList(_context.SupplyRecommend.ToList().OrderBy(s => s.Description), "Code", "Description");
            return View();
        }

        public ActionResult Species()
        {
            ViewBag.CATO = _context.CATO.OrderBy(c => c.Name).ToList();
            ViewBag.GeoServerUrl = Startup.Configuration["GeoServerUrl"].ToString();
            return View();
        }

        public ActionResult PasturesBurden()
        {
            ViewBag.CATO = _context.CATO.OrderBy(c => c.Name).ToList();
            ViewBag.GeoServerUrl = Startup.Configuration["GeoServerUrl"].ToString();

            ViewBag.BurOtdel = new SelectList(_context.BurOtdel.ToList().OrderBy(o => o.Description), "Code", "Description");
            ViewBag.BType = new SelectList(_context.BType.ToList().OrderBy(o => o.Description), "Code", "Description");
            ViewBag.BurSubOtdel = new SelectList(_context.BurSubOtdel.ToList().OrderBy(o => o.Description), "Code", "Description");
            ViewBag.BClass = new SelectList(_context.BClass.ToList().OrderBy(o => o.Description), "Code", "Description");
            ViewBag.BGroup = new SelectList(_context.BGroup.ToList().OrderBy(o => o.Description), "Code", "Description");
            return View();
        }

        public ActionResult Wells()
        {
            ViewBag.CATO = _context.CATO.OrderBy(c => c.Name).ToList();
            ViewBag.GeoServerUrl = Startup.Configuration["GeoServerUrl"].ToString();

            ViewBag.WType = new SelectList(_context.WType.ToList().OrderBy(o => o.Description), "Code", "Description");
            ViewBag.WSubType = new SelectList(_context.WSubType.ToList().OrderBy(o => o.Description), "Code", "Description");
            ViewBag.ChemicalComp = new SelectList(_context.ChemicalComp.ToList().OrderBy(o => o.Description), "Code", "Description");
            ViewBag.WClass = new SelectList(_context.WClass.ToList().OrderBy(o => o.Description), "Code", "Description");
            return View();
        }

        public ActionResult Charts(decimal objectid)
        {
            ViewBag.objectid = objectid;
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
                pasturepol.otdel = _context.Otdel.FirstOrDefault(o => o.Code == pasturepol.otdely_id)?.Description;
                pasturepol.ptype = _context.PType.FirstOrDefault(p => p.Code == pasturepol.class_id)?.Description;
                pasturepol.group = _context.Soob.FirstOrDefault(s => s.Code == pasturepol.group_id)?.Description;
                pasturepol.group_lat = _context.Soob.FirstOrDefault(s => s.Code == pasturepol.group_id)?.DescriptionLat;
                pasturepol.recommend = _context.Recommend.FirstOrDefault(r => r.Code == pasturepol.recommend_)?.Description;
                pasturepol.recomcatt = _context.RecomCattle.FirstOrDefault(r => r.Code == pasturepol.recom_catt)?.Description;
            }
            ViewBag.pasturepol = pasturepol;
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
                if (layers[i].Length == 7)
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
                if (layers[i].Length == 7)
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

        [HttpPost]
        public ActionResult GetCATOPastureInfo(
            string catote)
        {
            string DefaultConnection = Microsoft
               .Extensions
               .Configuration
               .ConfigurationExtensions
               .GetConnectionString(Startup.Configuration, "DefaultConnection");
            string ab = catote?.Substring(0, 2),
                cd = catote?.Substring(2, 2),
                ef = catote?.Substring(4, 2),
                te = ab;
            if (cd != "00")
            {
                te += cd;
            }
            if (ef != "00")
            {
                te += ef;
            }
            List<pasturestat> pasturestats = new List<pasturestat>();

            List<pasturestat> pasturepols_otdels = new List<pasturestat>();
            using (var connection = new NpgsqlConnection(DefaultConnection))
            {
                connection.Open();
                string query = $"SELECT otdely_id, SUM(shape_area) as shape_area FROM public.pasturestat WHERE kato_te LIKE '{te}%' GROUP BY otdely_id;";
                var pasturepolsDB = connection.Query<pasturestat>(query);
                pasturepols_otdels = pasturepolsDB.ToList();
            }

            List<pasturestat> pasturepols_classes = new List<pasturestat>();
            using (var connection = new NpgsqlConnection(DefaultConnection))
            {
                connection.Open();
                string query = $"SELECT class_id, SUM(shape_area) as shape_area FROM public.pasturestat WHERE kato_te LIKE '{te}%' GROUP BY class_id;";
                var pasturepolsDB = connection.Query<pasturestat>(query);
                pasturepols_classes = pasturepolsDB.ToList();
                decimal sum = pasturepols_classes.Sum(p => p.shape_area);
                for (int i = 0; i < pasturepols_classes.Count(); i++)
                {
                    pasturepols_classes[i].percent = pasturepols_classes[i].shape_area / sum * 100;
                }
            }

            return Json(new
            {
                pasturepols_otdels,
                pasturepols_classes
            });
        }

        [HttpPost]
        public async Task<IActionResult> GetZemfondInfo(
            string objectid)
        {
            string DefaultConnection = Microsoft
               .Extensions
               .Configuration
               .ConfigurationExtensions
               .GetConnectionString(Startup.Configuration, "DefaultConnection");
            zemfondpol zemfondpol = new zemfondpol();
            using (var connection = new NpgsqlConnection(DefaultConnection))
            {
                connection.Open();
                var zemfondpols = connection.Query<zemfondpol>($"SELECT gid, objectid, type_k, ur_avgyear, " +
                    $"dominant_t, korm_avgye, area, s_recomend, subtype_k, kato_te_1, shape_leng, shape_area, geom " +
                    $"FROM public.zemfondpol " +
                    $"WHERE objectid = {objectid};");
                zemfondpol = zemfondpols.FirstOrDefault();
            }
            zemfondpol.stype = _context.SType.FirstOrDefault(s => s.Code == zemfondpol.type_k)?.Description;
            zemfondpol.dominanttype = _context.DominantType.FirstOrDefault(d => d.Code == zemfondpol.dominant_t)?.Description;
            zemfondpol.supplyrecommend = _context.SupplyRecommend.FirstOrDefault(s => s.Code == zemfondpol.s_recomend)?.Description;
            return Json(new
            {
                zemfondpol
            });
        }

        [HttpPost]
        public ActionResult GetCATOZemfondInfo(
            string catote)
        {
            string DefaultConnection = Microsoft
               .Extensions
               .Configuration
               .ConfigurationExtensions
               .GetConnectionString(Startup.Configuration, "DefaultConnection");
            string ab = catote?.Substring(0, 2),
                cd = catote?.Substring(2, 2),
                ef = catote?.Substring(4, 2),
                te = ab;
            if (cd != "00")
            {
                te += cd;
            }
            if (ef != "00")
            {
                te += ef;
            }

            List<zemfondpol> zemfondpols = new List<zemfondpol>();
            zemfondpol zemfondpol = new zemfondpol();
            using (var connection = new NpgsqlConnection(DefaultConnection))
            {
                connection.Open();
                string query = $"SELECT ur_avgyear, korm_avgye, area FROM public.zemfondpol WHERE kato_te_1 LIKE '{te}%';";
                var zemfondpolsDB = connection.Query<zemfondpol>(query);
                zemfondpols = zemfondpolsDB.ToList();
                zemfondpol.area = zemfondpols.Where(z => z.ur_avgyear > 0).Sum(z => z.area);
                zemfondpol.ur_avgyear = zemfondpols.Sum(z => z.area * z.ur_avgyear) / zemfondpol.area;
                zemfondpol.korm_avgye = zemfondpols.Sum(z => z.korm_avgye);
            }

            List<zemfondpol> zemfondpols_stypes = new List<zemfondpol>();
            using (var connection = new NpgsqlConnection(DefaultConnection))
            {
                connection.Open();
                string query = $"SELECT type_k, SUM(area) as shape_area FROM public.zemfondpol WHERE kato_te_1 LIKE '{te}%' GROUP BY type_k;";
                var zemfondpolsDB = connection.Query<zemfondpol>(query);
                zemfondpols_stypes = zemfondpolsDB.ToList();
            }

            List<zemfondpol> zemfondpols_supplyrecommends = new List<zemfondpol>();
            using (var connection = new NpgsqlConnection(DefaultConnection))
            {
                connection.Open();
                string query = $"SELECT s_recomend, SUM(area) as shape_area FROM public.zemfondpol WHERE kato_te_1 LIKE '{te}%' GROUP BY s_recomend;";
                var zemfondpolsDB = connection.Query<zemfondpol>(query);
                zemfondpols_supplyrecommends = zemfondpolsDB.ToList();
            }

            return Json(new
            {
                zemfondpol,
                zemfondpols_stypes,
                zemfondpols_supplyrecommends
            });
        }

        [HttpPost]
        public ActionResult GetCATOSpeciesInfo(
            string catote)
        {
            List<CATOSpecies> cATOSpecies = new List<CATOSpecies>();
            string cato = "",
                type_k = "";
            if (!string.IsNullOrEmpty(catote))
            {
                // area
                if (catote.Substring(2, 2) == "00")
                {
                    cATOSpecies = _context.CATOSpecies.Where(c => c.CATOTE.Substring(0, 2) == catote.Substring(0, 2)).ToList();
                    cato = _context.CATO.FirstOrDefault(c => c.TE == catote)?.Name;
                }
                // district
                else if (catote.Substring(4, 2) == "00")
                {
                    cATOSpecies = _context.CATOSpecies.Where(c => c.CATOTE.Substring(0, 4) == catote.Substring(0, 4)).ToList();
                    string te_area = catote.Substring(0, 2) + "0000000";
                    cato = _context.CATO.FirstOrDefault(c => c.TE == te_area)?.Name;
                    string cato_district = _context.CATO.FirstOrDefault(c => c.TE == catote)?.Name;
                    if (!string.IsNullOrEmpty(cato_district))
                    {
                        cato += ", " + cato_district;
                    }
                }
                // rural district
                else if (catote.Substring(6, 3) == "000")
                {
                    string te_area = catote.Substring(0, 2) + "0000000";
                    cato = _context.CATO.FirstOrDefault(c => c.TE == te_area)?.Name;
                    string te_district = catote.Substring(0, 4) + "00000";
                    string cato_district = _context.CATO.FirstOrDefault(c => c.TE == te_district)?.Name;
                    if (!string.IsNullOrEmpty(cato_district))
                    {
                        cato += ", " + cato_district;
                    }
                    string cato_rural = _context.CATO.FirstOrDefault(c => c.TE == catote)?.Name;
                    if (!string.IsNullOrEmpty(cato_rural))
                    {
                        cato += ", " + cato_rural;
                    }
                    string DefaultConnection = Microsoft
                      .Extensions
                      .Configuration
                      .ConfigurationExtensions
                      .GetConnectionString(Startup.Configuration, "DefaultConnection");
                    species3 species3 = new species3();
                    using (var connection = new NpgsqlConnection(DefaultConnection))
                    {
                        connection.Open();
                        var species3DB = connection.Query<species3>($"SELECT gid, name_adm1, name_adm2, kato_te, type_k, name_adm3," +
                            $" totalgoals, cattle, horses, smallcattl, camels, conditiona, date, source, population, pastures," +
                            $" poultry, shape_leng, shape_area, objectid" +
                            $" FROM public.species3" +
                            $" WHERE kato_te = '{catote}';");
                        species3 = species3DB.FirstOrDefault();
                    }
                    type_k = _context.ZType.FirstOrDefault(z => z.Code == species3.type_k).Description;
                }
            }
            List<Camel> camels = _context.Camel.Where(c => cATOSpecies.Select(cs => cs.Code).Contains(c.Code)).Distinct().ToList();
            List<Cattle> cattle = _context.Cattle.Where(c => cATOSpecies.Select(cs => cs.Code).Contains(c.Code)).Distinct().ToList();
            List<Horse> horses = _context.Horse.Where(h => cATOSpecies.Select(cs => cs.Code).Contains(h.Code)).Distinct().ToList();
            List<SmallCattle> smallcattle = _context.SmallCattle.Where(s => cATOSpecies.Select(cs => cs.Code).Contains(s.Code)).ToList();
            return Json(new
            {
                cato,
                type_k,
                camels,
                cattle,
                horses,
                smallcattle
            });
        }

        [HttpPost]
        public ActionResult GetBreedInfo(
            string Code)
        {
            int code = Convert.ToInt32(Code);
            if (code > 400)
            {
                SmallCattle smallcattle = _context.SmallCattle.FirstOrDefault(s => s.Code == code);
                smallcattle.Img = String.Format("data:image/gif;base64,{0}", Convert.ToBase64String(smallcattle.Photo));
                return Json(new
                {
                    smallcattle
                });
            }
            else if (code > 300)
            {
                Horse horse = _context.Horse.FirstOrDefault(h => h.Code == code);
                horse.Img = String.Format("data:image/gif;base64,{0}", Convert.ToBase64String(horse.Photo));
                return Json(new
                {
                    horse
                });
            }
            else if (code > 200)
            {
                Cattle cattle = _context.Cattle.FirstOrDefault(c => c.Code == code);
                cattle.Img = String.Format("data:image/gif;base64,{0}", Convert.ToBase64String(cattle.Photo));
                return Json(new
                {
                    cattle
                });
            }
            else
            {
                Camel camel = _context.Camel.FirstOrDefault(c => c.Code == code);
                camel.Img = String.Format("data:image/gif;base64,{0}", Convert.ToBase64String(camel.Photo));
                return Json(new
                {
                    camel
                });
            }
        }

        [HttpPost]
        public async Task<IActionResult> GetPastureBurdenInfo(
            string objectid)
        {
            string DefaultConnection = Microsoft
               .Extensions
               .Configuration
               .ConfigurationExtensions
               .GetConnectionString(Startup.Configuration, "DefaultConnection");
            burden_pasture burden_pasture = new burden_pasture();
            using (var connection = new NpgsqlConnection(DefaultConnection))
            {
                connection.Open();
                var burden_pastures = connection.Query<burden_pasture>($"SELECT gid, objectid, num_vydel, rule_grazi, bur_otdel, " +
                    $"bur_type_i, bur_subotd, bur_class_, bur_group_, average_yi, burden_gro, burden_deg, type_txt, shape_leng, shape_area " +
                    $"FROM public.burden_pasture " +
                    $"WHERE objectid = {objectid};");
                burden_pasture = burden_pastures.FirstOrDefault();
            }
            burden_pasture.burotdel = _context.BurOtdel.FirstOrDefault(b => b.Code == burden_pasture.bur_otdel)?.Description;
            burden_pasture.btype = _context.BType.FirstOrDefault(b => b.Code == burden_pasture.bur_type_i)?.Description;
            burden_pasture.bursubotdel = _context.BurSubOtdel.FirstOrDefault(b => b.Code == burden_pasture.bur_subotd)?.Description;
            burden_pasture.bclass = _context.BClass.FirstOrDefault(b => b.Code == burden_pasture.bur_class_)?.Description;
            burden_pasture.bgroup = _context.BGroup.FirstOrDefault(b => b.Code == burden_pasture.bur_group_)?.Description;
            return Json(new
            {
                burden_pasture
            });
        }

        [HttpPost]
        public async Task<IActionResult> GetWellPntInfo(
            string objectid)
        {
            string DefaultConnection = Microsoft
               .Extensions
               .Configuration
               .ConfigurationExtensions
               .GetConnectionString(Startup.Configuration, "DefaultConnection");
            wellspnt wellspnt = new wellspnt();
            using (var connection = new NpgsqlConnection(DefaultConnection))
            {
                connection.Open();
                string query = $"SELECT objectid, id, num, usl, indeks, debit, " +
                    $"decrease, depth, minerali, chemical_c, kato, wat_seepag, sost " +
                    $"FROM public.wellspnt " +
                    $"WHERE objectid = {objectid};";
                var wellspnts = connection.Query<wellspnt>(query);
                wellspnt = wellspnts.FirstOrDefault();
            }
            wellspnt.wtype = _context.WType.FirstOrDefault(b => b.Code == wellspnt.usl)?.Description;
            wellspnt.wsubtype = _context.WSubType.FirstOrDefault(b => b.Code == wellspnt.wat_seepag)?.Description;
            wellspnt.chemicalcomp = _context.ChemicalComp.FirstOrDefault(b => b.Code == wellspnt.chemical_c)?.Description;
            return Json(new
            {
                wellspnt
            });
        }

        [HttpPost]
        public async Task<IActionResult> GetChart1Data(
            decimal objectid,
            string sourceproduct,
            string dataset,
            int yearstart,
            int yearfinish)
        {
            string raster = sourceproduct + "_B01_NDVI_3857_KZ";
            if (dataset == "Anomaly")
            {
                raster += "_Anomaly";
            }
            raster += ".tif";
            string DefaultConnection = Microsoft
               .Extensions
               .Configuration
               .ConfigurationExtensions
               .GetConnectionString(Startup.Configuration, "DefaultConnection");
            List<analytic> analytics = new List<analytic>();
            using (var connection = new NpgsqlConnection(DefaultConnection))
            {
                connection.Open();
                string query = $"SELECT raster, min, max, median, majority, mean, objectid" +
                    $" FROM public.analytics" +
                    $" WHERE objectid = {objectid.ToString()}" +
                    $" AND raster LIKE '%{raster}%';";
                var analyticsDB = connection.Query<analytic>(query);
                analytics = analyticsDB.Where(a => !a.raster.Contains("BASE")).ToList();
            }
            for (int i = 0; i < analytics.Count(); i++)
            {
                analytics[i].day = Convert.ToInt32(analytics[i].raster.Substring(5, 3));
                analytics[i].date = new DateTime(Convert.ToInt32(analytics[i].raster.Substring(1, 4)), 1, 1);
                analytics[i].date = analytics[i].date.AddDays(analytics[i].day - 1);
            }
            analytics = analytics.Where(a => a.date.Year >= yearstart && a.date.Year <= yearfinish).OrderBy(a => a.date).ToList();

            // years
            List<string> labels = analytics.Select(a => a.date.ToString("yyyy.MM.dd")).ToList();
            List<decimal?> years_min = analytics.Select(a => (decimal?)a.min).ToList(),
                years_max = analytics.Select(a => (decimal?)a.max).ToList(),
                years_median = analytics.Select(a => (decimal?)a.median).ToList();

            return Json(new
            {
                labels,
                years_min,
                years_max,
                years_median
            });
        }

        [HttpPost]
        public async Task<IActionResult> GetChart2Data(
            decimal objectid,
            string sourceproduct,
            string dataset,
            int monthstart,
            int monthscount,
            int[] years)
        {
            string raster = sourceproduct + "_B01_NDVI_3857_KZ";
            if (dataset == "Anomaly")
            {
                raster += "_Anomaly";
            }
            raster += ".tif";
            string DefaultConnection = Microsoft
               .Extensions
               .Configuration
               .ConfigurationExtensions
               .GetConnectionString(Startup.Configuration, "DefaultConnection");
            List<analytic> analytics = new List<analytic>();
            using (var connection = new NpgsqlConnection(DefaultConnection))
            {
                connection.Open();
                string query = $"SELECT raster, min, max, median, majority, mean, objectid" +
                    $" FROM public.analytics" +
                    $" WHERE objectid = {objectid.ToString()}" +
                    $" AND raster LIKE '%{raster}%';";
                var analyticsDB = connection.Query<analytic>(query);
                analytics = analyticsDB.Where(a => !a.raster.Contains("BASE")).ToList();
            }
            for (int i = 0; i < analytics.Count(); i++)
            {
                analytics[i].day = Convert.ToInt32(analytics[i].raster.Substring(5, 3));
                analytics[i].date = new DateTime(Convert.ToInt32(analytics[i].raster.Substring(1, 4)), 1, 1);
                analytics[i].date = analytics[i].date.AddDays(analytics[i].day - 1);
            }
            analytics = analytics.OrderBy(a => a.date).ToList();

            // years average
            List<string> labels_ = new List<string>();
            List<decimal?> years_min_ = new List<decimal?>(),
                years_max_ = new List<decimal?>(),
                years_median_ = new List<decimal?>();
            foreach (int d in analytics.Select(a => a.day).Distinct().OrderBy(v => v))
            {
                labels_.Add((new DateTime(2001, 1, 1).AddDays(d - 1).ToString("MM.dd")));
                years_min_.Add(analytics.Where(a => a.day == d).Min(a => a.min));
                years_max_.Add(analytics.Where(a => a.day == d).Max(a => a.max));
                years_median_.Add(analytics.Where(a => a.day == d).Average(a => a.median));
            }

            // years
            List<int> days = analytics.Select(a => a.day).Distinct().OrderBy(v => v).ToList();
            List<year_dataset> year_min_datasets_ = new List<year_dataset>(),
                year_max_datasets_ = new List<year_dataset>(),
                year_median_datasets_ = new List<year_dataset>();
            foreach (int y in years)
            {
                year_dataset year_min_dataset_new = new year_dataset()
                {
                    year = y,
                    data = new List<decimal?>()
                },
                year_max_dataset_new = new year_dataset()
                {
                    year = y,
                    data = new List<decimal?>()
                },
                year_median_dataset_new = new year_dataset()
                {
                    year = y,
                    data = new List<decimal?>()
                };
                foreach (int d in days)
                {
                    year_min_dataset_new.data.Add(analytics.FirstOrDefault(a => a.day == d && a.date.Year == y)?.min);
                    year_max_dataset_new.data.Add(analytics.FirstOrDefault(a => a.day == d && a.date.Year == y)?.max);
                    year_median_dataset_new.data.Add(analytics.FirstOrDefault(a => a.day == d && a.date.Year == y)?.median);
                }
                year_min_datasets_.Add(year_min_dataset_new);
                year_max_datasets_.Add(year_max_dataset_new);
                year_median_datasets_.Add(year_median_dataset_new);
            }

            // shift by months
            List<string> labels = new List<string>();
            List<decimal?> years_min = new List<decimal?>(),
                years_max = new List<decimal?>(),
                years_median = new List<decimal?>();
            List<year_dataset> year_min_datasets = new List<year_dataset>(),
                year_max_datasets = new List<year_dataset>(),
                year_median_datasets = new List<year_dataset>();
            foreach (int y in years)
            {
                year_dataset year_min_dataset_new = new year_dataset()
                {
                    year = y,
                    data = new List<decimal?>()
                },
                year_max_dataset_new = new year_dataset()
                {
                    year = y,
                    data = new List<decimal?>()
                },
                year_median_dataset_new = new year_dataset()
                {
                    year = y,
                    data = new List<decimal?>()
                };
                year_min_datasets.Add(year_min_dataset_new);
                year_max_datasets.Add(year_max_dataset_new);
                year_median_datasets.Add(year_median_dataset_new);
            }
            for (int m = monthstart; m < monthstart + monthscount; m++)
            {
                int month = m;
                if (month > 12)
                {
                    month -= 12;
                }
                for (int d = 0; d < days.Count(); d++)
                {
                    if ((new DateTime(2001, 1, 1).AddDays(days[d] - 1)).Month == month)
                    {
                        labels.Add(labels_[d]);
                        years_min.Add(years_min_[d]);
                        years_max.Add(years_max_[d]);
                        years_median.Add(years_median_[d]);
                        for (int y = 0; y < years.Count(); y++)
                        {
                            year_min_datasets[y].data.Add(year_min_datasets_[y].data[d]);
                            year_max_datasets[y].data.Add(year_max_datasets_[y].data[d]);
                            year_median_datasets[y].data.Add(year_median_datasets_[y].data[d]);
                        }
                    }
                }
            }

            return Json(new
            {
                labels,
                years_min,
                years_max,
                years_median,
                year_min_datasets,
                year_max_datasets,
                year_median_datasets
            });
        }
    }
}