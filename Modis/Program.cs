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
            ModisProjection = "4326";
        static void Main(string[] args)
        {
            DateTime dateTimeLast = new DateTime(2000, 1, 1);
            while (true)
            {
                if((DateTime.Now - dateTimeLast).TotalDays < 1)
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
                string DownloadDir = JObject.Parse(settingsJObject.ToString())["DownloadDir"],
                   GeoServerDataDir = JObject.Parse(settingsJObject.ToString())["GeoServerDataDir"],
                   CMDPath = JObject.Parse(settingsJObject.ToString())["CMDPath"];

                // delete folders which names start with "!" 
                foreach(string folder in Directory.EnumerateDirectories(DownloadDir, "!*"))
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
                // rewrite => ...
                DateTime dateTimeStart = ModisDateStart,
                    dateTimeFinish = dateTimeStart.AddDays(ModisPeriod - 1);
                // ...
                try
                {
                    //// create subfolder
                    //string folderDownload = Path.Combine(DownloadDir, $"!{dateTimeStart.ToString("yyyy.MM.dd")}-{dateTimeFinish.ToString("yyyy.MM.dd")}");
                    //Directory.CreateDirectory(folderDownload);
                    string folderDownload = @"E:\Documents\New\Modis\Test";

                    //// download modis
                    //// modis_download.py -U caesarmod -P caesar023Earthdata -r -t h21v03,h21v04,h22v03,h22v04 -p MOD13Q1.006 -f 2018-04-01 -e 2018-05-03 D:\Documents\MOD13Q1
                    //// string arguments = $"-r -s {ModisSource} -p {ModisProduct} -t {string.Join(',', ModisSpans)} -f {dateTimeStart.Year}-{dateTimeStart.Month}-{dateTimeStart.Day} -e {DateFinish.Year}-{DateFinish.Month}-{DateFinish.Day} {folder}"
                    //string arguments = $"-U caesarmod -P caesar023Earthdata -r -t {string.Join(',', ModisSpans)} -p {ModisProduct}" +
                    //    $" -f {dateTimeStart.ToString("yyyy-MM-dd")} -e {dateTimeFinish.ToString("yyyy-MM-dd")}" +
                    //    $" {folderDownload}";
                    //ModisExecute(CMDPath, "modis_download.py", arguments);

                    // mosaic
                    string modisListFile = Directory.EnumerateFiles(folderDownload, "*listfile*", SearchOption.TopDirectoryOnly).FirstOrDefault(),
                        arguments = $"-o {ModisSource}_{ModisProduct.Replace('.', '_')}_{ModisDataSet}.tif" +
                        $" -s \"{ModisDataSetIndex.ToString()}\"" +
                        $" {modisListFile}";
                    ModisExecute(
                        CMDPath,
                        "modis_mosaic.py",
                        folderDownload,
                        arguments);

                    // convert
                    foreach(string tif in Directory.EnumerateFiles(folderDownload, "*tif", SearchOption.TopDirectoryOnly))
                    {
                        string tifReprojected = $"{Path.GetFileNameWithoutExtension(tif)}_{ModisProjection}";
                        arguments = $"-v -s \"( 1 )\" -o {tifReprojected} -e {ModisProjection} {tif}";
                        ModisExecute(
                            CMDPath,
                            "modis_convert.py",
                            folderDownload,
                            arguments);
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
                

                // 1 hour
                Thread.Sleep(60 * 60 * 60);
            }
        }

        public static void Log(string log)
        {
            foreach(string line in log.Split("\r\n"))
            {
                Console.WriteLine($"{DateTime.Now.ToString()} >> {line}");
            }
        }

        public static void ModisExecute(
            string CMDPath,
            string ModisFileName,
            string FolderToMove,
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
                if(!string.IsNullOrEmpty(FolderToMove))
                {
                    process.StandardInput.WriteLine($"{FolderToMove[0]}:");
                    process.StandardInput.WriteLine($"cd {FolderToMove}");
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
    }
}
