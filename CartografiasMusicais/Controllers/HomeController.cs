using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using CartografiasMusicais.Models;

namespace CartografiasMusicais.Controllers
{
    public class HomeController : Controller
    {

        public HomeController()
        {
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Apresentacao()
        {
            return View();
        }

        public IActionResult Equipe()
        {
            return View();
        }
        public IActionResult Contato()
        {
            return View();
        }

    }
}
