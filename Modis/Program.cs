﻿using Newtonsoft.Json.Linq;
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

        static int AnomalyStartYear = 0,
            AnomalyFinishYear = 0;

        static void Main(string[] args)
        {
            DateTime dateTimeLast = new DateTime(2000, 1, 1);
            while (true)
            {
                if ((DateTime.Now - dateTimeLast).TotalDays < 1)
                {
                    continue;
                }

                // load parameters
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
                    if (property.Name == "AnomalyStartYear")
                    {
                        AnomalyStartYear = Convert.ToInt32(property.Value);
                    }
                    if (property.Name == "AnomalyFinishYear")
                    {
                        AnomalyFinishYear = Convert.ToInt32(property.Value);
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
                    string folderDownload = Path.Combine(DownloadDir, $"!{dateTimeStart.ToString("yyyy.MM.dd")}-{dateTimeFinish.ToString("yyyy.MM.dd")}"),
                        folderDownloadFinale = Path.Combine(DownloadDir, $"{dateTimeStart.ToString("yyyy.MM.dd")}-{dateTimeFinish.ToString("yyyy.MM.dd")}");

                    try
                    {
                        // create subfolder                        
                        Directory.CreateDirectory(folderDownload);

                        // download modis
                        ModisDownload(dateTimeStart, dateTimeFinish, folderDownload);

                        // mosaic
                        ModisMosaic(folderDownload);

                        // convert
                        ModisConvert(folderDownload);

                        // clip
                        TifClip(folderDownload);

                        // move to GeoServer
                        // publish
                        Publish(folderDownload);

                        // rename folder (remove "!")
                        Directory.Move(folderDownload, folderDownloadFinale);

                        // anomaly
                        foreach (string file in Directory.EnumerateFiles(GeoServerModisDataDir, "*.tif", SearchOption.TopDirectoryOnly))
                        {
                            if (file.Contains("Anomaly") || file.Contains("BASE"))
                            {
                                continue;
                            }
                            // calculate anomaly
                            Anomaly(GeoServerModisDataDir, file);
                        }

                        //// work with downloaded MODIS
                        //foreach(string folder in Directory.EnumerateDirectories(DownloadDir, "*"))
                        //{
                        //    // mosaic
                        //    ModisMosaic(folder);

                        //    // convert
                        //    ModisConvert(folder);

                        //    // clip
                        //    TifClip(folder);

                        //    // move to GeoServer
                        //    // publish
                        //    Publish(folder);
                        //}
                    }
                    catch
                    {
                        // delete folders which names start with "!" 
                        foreach (string folder in Directory.EnumerateDirectories(DownloadDir, "!*"))
                        {
                            Directory.Delete(folder, true);
                        }
                    }

                    // delete empty folder
                    if (Directory.EnumerateFiles(folderDownloadFinale, "*hdf*").Count() == 0)
                    {
                        Directory.Delete(folderDownloadFinale, true);
                    }

                    // rename last subfolder date finish if it is 30 days recent
                    if (dateTimeFinish.AddDays(30) > DateTime.Now)
                    {
                        // get last hdf date
                        DateTime dateTimeLastHDF = dateTimeFinish;
                        foreach (string file in Directory.EnumerateFiles(GeoServerModisDataDir, "*.hdf", SearchOption.TopDirectoryOnly))
                        {
                            string fileDate = Path.GetFileName(file).Split('.')[1].Remove(0, 1);
                            DateTime dateTimeHDF = new DateTime(Convert.ToInt32(fileDate.Substring(0, 4)), 1, 1).AddDays(Convert.ToInt32(fileDate.Substring(4, 3)));
                            if(dateTimeHDF >= dateTimeLastHDF)
                            {
                                dateTimeLastHDF = dateTimeHDF;
                            }
                        }
                        string folderDownloadRename = Path.Combine(DownloadDir, $"!{dateTimeStart.ToString("yyyy.MM.dd")}-{dateTimeLastHDF.ToString("yyyy.MM.dd")}");
                        Directory.Move(folderDownloadFinale, folderDownloadRename);
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
                $" \"{Folder}\"";
            GDALExecute(CMDPath, "modis_download.py", Folder, arguments);
        }

        private static void ModisMosaic(string Folder)
        {
            string modisListFile = Directory.EnumerateFiles(Folder, "*listfile*", SearchOption.TopDirectoryOnly).FirstOrDefault(),
                index = ModisDataSetIndex.ToString().PadLeft(2, '0');
            string arguments = $"-o {ModisSource}_{ModisProduct.Replace(".", "")}_B{index}_{ModisDataSet}.tif" +
                $" -s \"{ModisDataSetIndex.ToString()}\"" +
                $" \"{modisListFile}\"";
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
                    arguments = $"-v -s \"( 1 )\" -o {tifReprojected} -e {ModisProjection} \"{tif}\"";
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
                string tifToClip = Path.GetFileName(tif),
                    tifClipped = $"{Path.GetFileNameWithoutExtension(tif)}_KZ.tif";
                //// 1 (MODIS1) old clip
                //string arguments = $"-cutline {ClipShape} {tifToClip} {tifClipped}";
                // 0 (MODIS) with crop and compress
                string arguments = $"-overwrite -dstnodata -3000 -co COMPRESS=LZW -cutline \"{ClipShape}\" -crop_to_cutline {tifToClip} {tifClipped}";
                //// 2 (MODIS2) with crop without compress
                //string arguments = $"-overwrite -dstnodata -3000 -cutline {ClipShape} -crop_to_cutline {tifToClip} {tifClipped}";
                GDALExecute(
                    CMDPath,
                    "gdalwarp",
                    Folder,
                    arguments);
                File.Delete(tif);
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
                string index = ModisDataSetIndex.ToString().PadLeft(2, '0'),
                    style = $"{GeoServerWorkspace}:{ModisSource}_{ModisProduct.Replace(".", "")}_B{index}_{ModisDataSet}";
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

        private static void Anomaly(string Folder,
            // File - full path
            string FileCalc)
        {
            string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(FileCalc),
                anomalyFile = Path.Combine(GeoServerModisDataDir, Path.ChangeExtension(fileNameWithoutExtension + "_Anomaly", "tif")),
                baseFile = Path.Combine(GeoServerModisDataDir, Path.ChangeExtension($"ABASE{fileNameWithoutExtension.Substring(5)}", "tif")),
                letters = "ABCDEFGHIJKLMNOPQRSTUVWXY";

            // check if anomaly already exists
            if (File.Exists(anomalyFile))
            {
                return;
            }

            // check if base layer for anomaly calculation already exists, if no then try to create it
            string arguments = "--co COMPRESS=LZW";
            if (!File.Exists(baseFile))
            {
                // check if base layers for base calculation already exist
                bool baseExists = true;
                for (int year = AnomalyStartYear; year <= AnomalyFinishYear; year++)
                {
                    string baseYearFile = Path.Combine(GeoServerModisDataDir, Path.ChangeExtension(fileNameWithoutExtension.Remove(1, 4).Insert(1, year.ToString()), "tif"));
                    if (!File.Exists(baseYearFile))
                    {
                        baseExists = false;
                        break;
                    }
                }
                if (!baseExists)
                {
                    return;
                }
                // create base file to day
                for (int year = AnomalyStartYear; year <= AnomalyFinishYear; year++)
                {
                    int letterIndex = year - AnomalyStartYear;
                    string baseYearFile = Path.ChangeExtension(fileNameWithoutExtension.Remove(1, 4).Insert(1, year.ToString()), "tif");
                    arguments += $" -{letters[letterIndex]} {Path.GetFileName(baseYearFile)}";
                }
                arguments += $" --outfile={Path.GetFileName(baseFile)}";
                arguments += $" --calc=\"((";
                for (int year = AnomalyStartYear; year <= AnomalyFinishYear; year++)
                {
                    int letterIndex = year - AnomalyStartYear;
                    arguments += $"{letters[letterIndex]}+";
                }
                arguments = arguments.Remove(arguments.Length - 1);
                arguments += $")/{(AnomalyFinishYear - AnomalyStartYear + 1).ToString()})\"";
                GDALExecute(
                    CMDPath,
                    "gdal_calc.py",
                    Folder,
                    arguments);
            }

            // calculate
            arguments = "--co COMPRESS=LZW";
            arguments += $" -{letters[0]} {Path.GetFileName(baseFile)}";
            arguments += $" -{letters[1]} {Path.GetFileName(FileCalc)} ";
            arguments += $"--outfile={Path.GetFileName(anomalyFile)} ";
            arguments += $"--calc=\"(B-A)*0.01\"";
            GDALExecute(
                CMDPath,
                "gdal_calc.py",
                Folder,
                arguments);

            // publish
            string layerName = Path.GetFileNameWithoutExtension(anomalyFile);
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
            string index = ModisDataSetIndex.ToString().PadLeft(2, '0'),
                style = $"{GeoServerWorkspace}:{ModisSource}_{ModisProduct.Replace(".", "")}_B{index}_{ModisDataSet}_Anomaly";
            publishParameters = $" -v -u" +
                $" {GeoServerUser}:{GeoServerPassword}" +
                $" -X PUT -H \"Content-type: text/xml\"" +
                $" -d \"<layer><defaultStyle><name>{style}</name></defaultStyle></layer>\"" +
                $" {GeoServerURL}rest/layers/{GeoServerWorkspace}:{layerName}.xml";
            CurlExecute(
                CMDPath,
                publishParameters);
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
