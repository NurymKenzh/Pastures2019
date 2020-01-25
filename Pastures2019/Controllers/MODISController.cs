﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Pastures2019.Data;
using Pastures2019.Models;

namespace Pastures2019.Controllers
{
    public class MODISController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MODISController(ApplicationDbContext context)
        {
            _context = context;
        }

        [Authorize(Roles = "Administrator, Moderator")]
        public IActionResult Index()
        {
            return View();
        }

        [Authorize(Roles = "Administrator, Moderator")]
        public IActionResult Download()
        {
            ViewData["MODISDataSetId"] = new SelectList(_context.MODISDataSet.OrderBy(m => m.Name), "Id", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<IActionResult> Download(
            DateTime DateTimeStart,
            DateTime DateTimeFinish,
            int MODISDataSetId,
            string User,
            string Password)
        {
            // folder template: createdate_MOLT_MOD13Q1.006_B01_NDVI_20000218_20001213
            // delete old (> 7 days) folder
            string DownloadDir = Startup.Configuration["ModisDownloadDirectory"].ToString();
            foreach (string folderOld in Directory.EnumerateDirectories(DownloadDir, "!*"))
            {
                string dateS = folderOld.Substring(0, 6);
                int year = Convert.ToInt32(dateS.Substring(0, 4)),
                    month = Convert.ToInt32(dateS.Substring(4, 2)),
                    day = Convert.ToInt32(dateS.Substring(6, 2));
                DateTime dateTimeFolder = new DateTime(year, month, day);
                if (dateTimeFolder > DateTime.Today.AddDays(-7))
                {
                    try
                    {
                        Directory.Delete(folderOld, true);
                    }
                    catch { }
                }
            }

            // create subfolder
            MODISDataSet mODISDataSet = _context.MODISDataSet
                .Include(m => m.MODISProduct)
                .Include(m => m.MODISProduct.MODISSource)
                .FirstOrDefault(m => m.Id == MODISDataSetId);
            string index = mODISDataSet.Index.ToString().PadLeft(2, '0'),
                folder = $"{DateTime.Today.ToString("yyyyMMdd")}_" +
                    $"{mODISDataSet.MODISProduct.MODISSource.Name}_" +
                    $"{mODISDataSet.MODISProduct.Name}_" +
                    $"B{index}_" +
                    $"{mODISDataSet.Name}_" +
                    $"{DateTimeStart.ToString("yyyyMMdd")}_" +
                    $"{DateTimeFinish.ToString("yyyyMMdd")}";
            folder = Path.Combine(DownloadDir, folder);
            if (Directory.Exists(folder))
            {
                try
                {
                    Directory.Delete(folder, true);
                }
                catch { }
            }
            Directory.CreateDirectory(folder);

            string CMDPath = Startup.Configuration["CMDPath"].ToString();
            Thread ModisDownloadThread = new Thread(() => ModisDownload(
                DateTimeStart,
                DateTimeFinish,
                folder,
                User,
                Password,
                mODISDataSet.MODISProduct.Name,
                CMDPath));
            ModisDownloadThread.Start();

            ViewData["MODISDataSetId"] = new SelectList(_context.MODISDataSet.OrderBy(m => m.Name), "Id", "Name");
            return View();
        }

        public void ModisDownload(DateTime DateTimeStart,
            DateTime DateTimeFinish,
            string Folder,
            string ModisUser,
            string ModisPassword,
            string ModisProduct,
            string CMDPath)
        {
            string[] ModisSpans = { "h21v03", "h21v04", "h22v03", "h22v04", "h23v03", "h23v04" };
            string arguments = $"-U {ModisUser} -P {ModisPassword} -r -t {string.Join(',', ModisSpans)} -p {ModisProduct}" +
                $" -f {DateTimeStart.ToString("yyyy-MM-dd")} -e {DateTimeFinish.ToString("yyyy-MM-dd")}" +
                $" \"{Folder}\"";
            GDALExecute(CMDPath, "modis_download.py", Folder, arguments);
        }

        private void GDALExecute(
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
                if (!string.IsNullOrEmpty(FolderToNavigate))
                {
                    process.StandardInput.WriteLine($"{FolderToNavigate[0]}:");
                    process.StandardInput.WriteLine($"cd {FolderToNavigate}");
                }

                process.StandardInput.WriteLine(ModisFileName + " " + string.Join(" ", Parameters));
                process.StandardInput.WriteLine("exit");
                string output = process.StandardOutput.ReadToEnd();
                string error = process.StandardError.ReadToEnd();
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