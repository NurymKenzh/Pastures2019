using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Pastures2019.Controllers
{
    public class MODISController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}