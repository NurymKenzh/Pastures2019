using Newtonsoft.Json.Linq;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;

namespace Modis
{
    class Program
    {
        static DateTime ModisDateStart = new DateTime(2000, 02, 18);
        const int ModisPeriod = 16,
            ModisDataSetIndex = 1;
        static string[] ModisSpans = { "h21v03", "h21v04", "h22v03", "h22v04", "h23v03", "h23v04" };
        const string ModisSource = "MOLT",
            ModisProduct = "MOD13Q1.006",
            ModisDataSet = "NDVI",
            ModisProjection = "3857",
            GeoServerWorkspace = "MODIS";

        static string ModisUser = "",
            ModisPassword = "",
            CMDPath = "",
            GeoServerModisDataDir = "",
            GeoServerUser = "",
            GeoServerPassword = "",
            GeoServerURL = "",
            ClipShape = "";

        static void Main(string[] args)
        {
            DateTime dateTimeLast = new DateTime(2000, 1, 1);
            while (true)
            {
                if ((DateTime.Now - dateTimeLast).TotalDays < 1)
                {
                    continue;
                }
                bool Server = true;
                string settingsString = System.IO.File.ReadAllText(@"modis.json");
                var json = JObject.Parse(settingsString);
                foreach (JProperty property in json.Properties())
                {
                    if (property.Name == "Server")
                    {
                        Server = Convert.ToBoolean(property.Value);
                    }
                    if (property.Name == "ModisUser")
                    {
                        ModisUser = property.Value.ToString();
                    }
                    if (property.Name == "ModisPassword")
                    {
                        ModisPassword = property.Value.ToString();
                    }
                    if (property.Name == "ClipShape")
                    {
                        ClipShape = property.Value.ToString();
                    }
                }
                dynamic settingsJObject = null;
                if (Server)
                {
                    Log("Server");
                    foreach (JProperty property in json.Properties())
                    {
                        if (property.Name == "ServerSettings")
                        {
                            settingsJObject = property.Value;
                        }
                    }
                }
                else
                {
                    Log("Debug");
                    foreach (JProperty property in json.Properties())
                    {
                        if (property.Name == "DebugSettings")
                        {
                            settingsJObject = property.Value;
                        }
                    }
                }
                string DownloadDir = JObject.Parse(settingsJObject.ToString())["DownloadDir"];
                CMDPath = JObject.Parse(settingsJObject.ToString())["CMDPath"];
                GeoServerModisDataDir = JObject.Parse(settingsJObject.ToString())["GeoServerModisDataDir"];
                ClipShape = Path.Combine(GeoServerModisDataDir, ClipShape);
                GeoServerUser = JObject.Parse(settingsJObject.ToString())["GeoServerUser"];
                GeoServerPassword = JObject.Parse(settingsJObject.ToString())["GeoServerPassword"];
                GeoServerURL = JObject.Parse(settingsJObject.ToString())["GeoServerURL"];

                DateTime dateTimeStart = ModisDateStart,
                    dateTimeFinish = new DateTime(dateTimeStart.Year, dateTimeStart.Month, 1).AddMonths(1).AddDays(-1); // dateTimeStart.AddDays(ModisPeriod - 1);
                while (true)
                {
                    // delete folders which names start with "!"
                    foreach (string folder in Directory.EnumerateDirectories(DownloadDir, "!*"))
                    {
                        try
                        {
                            Directory.Delete(folder, true);
                        }
                        catch
                        {

                        }
                    }

                    // determine period (dateTimeStart, dateTimeFinish)
                    foreach (string folder in Directory.EnumerateDirectories(DownloadDir, "*"))
                    {
                        string downloadFolder = Directory.GetParent(Path.Combine(folder, "file.file")).Name;
                        if (downloadFolder.Contains("!"))
                        {
                            continue;
                        }
                        string dateTimeStartCurrentS = downloadFolder.Split("-")[0],
                            dateTimeFinishCurrentS = downloadFolder.Split("-")[1];
                        int yearStart = Convert.ToInt32(dateTimeStartCurrentS.Split(".")[0]),
                            monthStart = Convert.ToInt32(dateTimeStartCurrentS.Split(".")[1]),
                            dayStart = Convert.ToInt32(dateTimeStartCurrentS.Split(".")[2]),
                            yearFinish = Convert.ToInt32(dateTimeFinishCurrentS.Split(".")[0]),
                            monthFinish = Convert.ToInt32(dateTimeFinishCurrentS.Split(".")[1]),
                            dayFinish = Convert.ToInt32(dateTimeFinishCurrentS.Split(".")[2]);
                        DateTime dateTimeStartCurrent = new DateTime(yearStart, monthStart, dayStart),
                            dateTimeFinishCurrent = new DateTime(yearFinish, monthFinish, dayFinish);
                        if (dateTimeFinishCurrent > dateTimeStart)
                        {
                            dateTimeStart = dateTimeFinishCurrent.AddDays(1);
                            dateTimeFinish = dateTimeStart.AddMonths(1).AddDays(-1);
                            if (dateTimeFinish > DateTime.Today)
                            {
                                dateTimeFinish = DateTime.Today;
                            }
                        }
                    }
                    if (dateTimeStart == DateTime.Today)
                    {
                        break;
                    }
                    try
                    {
                        //// create subfolder
                        //string folderDownload = Path.Combine(DownloadDir, $"!{dateTimeStart.ToString("yyyy.MM.dd")}-{dateTimeFinish.ToString("yyyy.MM.dd")}");
                        //Directory.CreateDirectory(folderDownload);

                        //// download modis
                        //ModisDownload(dateTimeStart, dateTimeFinish, folderDownload);

                        //// mosaic
                        //ModisMosaic(folderDownload);

                        //// convert
                        //ModisConvert(folderDownload);

                        //// clip
                        //TifClip(folderDownload);

                        //// move to GeoServer
                        //// publish
                        //Publish(folderDownload);

                        //// rename folder (remove "!")
                        //string folderDownloadFinale = Path.Combine(DownloadDir, $"{dateTimeStart.ToString("yyyy.MM.dd")}-{dateTimeFinish.ToString("yyyy.MM.dd")}");
                        //Directory.Move(folderDownload, folderDownloadFinale);

                        foreach(string folder in Directory.EnumerateDirectories(DownloadDir, "*"))
                        {
                            // mosaic
                            ModisMosaic(folder);

                            // convert
                            ModisConvert(folder);

                            // clip
                            TifClip(folder);

                            // move to GeoServer
                            // publish
                            Publish(folder);
                        }
                    }
                    catch
                    {
                        // delete folders which names start with "!" 
                        foreach (string folder in Directory.EnumerateDirectories(DownloadDir, "!*"))
                        {
                            Directory.Delete(folder, true);
                        }
                    }
                }
                if (dateTimeFinish == DateTime.Today)
                {
                    // 1 hour
                    Thread.Sleep(60 * 60 * 60);
                }
            }
        }

        private static void ModisDownload(
            DateTime DateTimeStart,
            DateTime DateTimeFinish,
            string Folder)
        {
            string arguments = $"-U {ModisUser} -P {ModisPassword} -r -t {string.Join(',', ModisSpans)} -p {ModisProduct}" +
                $" -f {DateTimeStart.ToString("yyyy-MM-dd")} -e {DateTimeFinish.ToString("yyyy-MM-dd")}" +
                $" {Folder}";
            GDALExecute(CMDPath, "modis_download.py", Folder, arguments);
        }

        private static void ModisMosaic(string Folder)
        {
            string modisListFile = Directory.EnumerateFiles(Folder, "*listfile*", SearchOption.TopDirectoryOnly).FirstOrDefault(),
                            index = ModisDataSetIndex.ToString().PadLeft(2, '0');
            string arguments = $"-o {ModisSource}_{ModisProduct.Replace(".", "")}_B{index}_{ModisDataSet}.tif" +
                $" -s \"{ModisDataSetIndex.ToString()}\"" +
                $" {modisListFile}";
            GDALExecute(
                CMDPath,
                "modis_mosaic.py",
                Folder,
                arguments);
        }

        private static void ModisConvert(string Folder)
        {
            foreach (string tif in Directory.EnumerateFiles(Folder, "*tif", SearchOption.TopDirectoryOnly))
            {
                string xml = tif + ".xml",
                    tifReprojected = $"{Path.GetFileNameWithoutExtension(tif)}_{ModisProjection}",
                    arguments = $"-v -s \"( 1 )\" -o {tifReprojected} -e {ModisProjection} {tif}";
                GDALExecute(
                    CMDPath,
                    "modis_convert.py",
                    Folder,
                    arguments);
                File.Delete(tif);
                File.Delete(xml);
            }
        }

        private static void TifClip(string Folder)
        {
            foreach (string tif in Directory.EnumerateFiles(Folder, "*tif", SearchOption.TopDirectoryOnly))
            {
                string xml = tif + ".xml",
                    tifTiClip = Path.GetFileName(tif),
                    tifClipped = $"{Path.GetFileNameWithoutExtension(tif)}_KZ.tif",
                    arguments = $"-cutline {ClipShape} {tif} {tifClipped}";
                GDALExecute(
                    CMDPath,
                    "gdalwarp",
                    Folder,
                    arguments);
                File.Delete(tif);
                File.Delete(xml);
            }
        }

        private static void Publish(string Folder)
        {
            foreach (string file in Directory.EnumerateFiles(Folder, $"*{ModisProjection}*.tif", SearchOption.TopDirectoryOnly))
            {
                // move to GeoServer
                string fileGeoServer = Path.Combine(GeoServerModisDataDir, Path.GetFileName(file));
                File.Move(
                    file,
                    fileGeoServer
                    );
                // publish
                string layerName = Path.GetFileNameWithoutExtension(fileGeoServer);
                // store
                string publishParameters = $" -v -u" +
                    $" {GeoServerUser}:{GeoServerPassword}" +
                    $" -POST -H \"Content-type: text/xml\"" +
                    $" -d \"<coverageStore><name>{layerName}</name><type>GeoTIFF</type><enabled>true</enabled><workspace>{GeoServerWorkspace}</workspace><url>" +
                    $"/data/{GeoServerWorkspace}/{layerName}.tif</url></coverageStore>\"" +
                    $" {GeoServerURL}rest/workspaces/{GeoServerWorkspace}/coveragestores?configure=all";
                CurlExecute(
                    CMDPath,
                    publishParameters);
                // layer
                publishParameters = $" -v -u" +
                    $" {GeoServerUser}:{GeoServerPassword}" +
                    $" -PUT -H \"Content-type: text/xml\"" +
                    $" -d \"<coverage><name>{layerName}</name><title>{layerName}</title><defaultInterpolationMethod><name>nearest neighbor</name></defaultInterpolationMethod></coverage>\"" +
                    $" \"{GeoServerURL}rest/workspaces/{GeoServerWorkspace}/coveragestores/{layerName}/coverages?recalculate=nativebbox\"";
                CurlExecute(
                    CMDPath,
                    publishParameters);
                // style
                string style = $"{GeoServerWorkspace}:{ModisSource}_{ModisProduct.Replace(".", "")}_{ModisDataSet}";
                publishParameters = $" -v -u" +
                    $" {GeoServerUser}:{GeoServerPassword}" +
                    $" -X PUT -H \"Content-type: text/xml\"" +
                    $" -d \"<layer><defaultStyle><name>{style}</name></defaultStyle></layer>\"" +
                    $" {GeoServerURL}rest/layers/{GeoServerWorkspace}:{layerName}.xml";
                CurlExecute(
                    CMDPath,
                    publishParameters);
            }
        }

        private static void Log(string log)
        {
            foreach(string line in log.Split("\r\n"))
            {
                Console.WriteLine($"{DateTime.Now.ToString()} >> {line}");
            }
        }

        private static void GDALExecute(
            string CMDPath,
            string ModisFileName,
            string FolderToNavigate,
            params string[] Parameters)
        {
            Process process = new Process();
            try
            {
                // run cmd.exe
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.RedirectStandardInput = true;
                process.StartInfo.RedirectStandardError = true;
                process.StartInfo.FileName = CMDPath;
                process.Start();

                // move to folder
                if(!string.IsNullOrEmpty(FolderToNavigate))
                {
                    process.StandardInput.WriteLine($"{FolderToNavigate[0]}:");
                    process.StandardInput.WriteLine($"cd {FolderToNavigate}");
                }

                process.StandardInput.WriteLine(ModisFileName + " " + string.Join(" ", Parameters));
                process.StandardInput.WriteLine("exit");
                string output = process.StandardOutput.ReadToEnd();
                Log(output);
                string error = process.StandardError.ReadToEnd();
                Log(error);
                process.WaitForExit();
                if (!string.IsNullOrEmpty(error))
                {
                    throw new Exception(error);
                }
            }
            catch (Exception exception)
            {
                throw new Exception(exception.ToString(), exception?.InnerException);
            }
        }

        private static void CurlExecute(
            string CMDPath,
            string Parameters)
        {
            Process process = new Process();
            try
            {
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.RedirectStandardInput = true;
                process.StartInfo.RedirectStandardError = true;
                process.StartInfo.FileName = CMDPath;
                process.Start();

                process.StandardInput.WriteLine($"curl {Parameters}");
                process.StandardInput.WriteLine("exit");

                string output = process.StandardOutput.ReadToEnd();
                Log(output);
                string error = process.StandardError.ReadToEnd();
                Log(error);
                process.WaitForExit();
                if (error.ToLower().Contains("error"))
                {
                    throw new Exception(error);
                }
            }
            catch (Exception exception)
            {
                throw new Exception(exception.ToString(), exception?.InnerException);
            }
        }
    }
}
