using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using CartografiasMusicais.Models;
using CartografiasMusicais.Business.Context;
using Microsoft.EntityFrameworkCore;

namespace CartografiasMusicais.Controllers
{
    public class HomeController : Controller
    {
        private CoreContext Context;

        public HomeController(CoreContext context)
        {
            Context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Apresentacao()
        {
            var model = Context.Paginas.FirstOrDefault(x => x.Nome.Equals("Apresentacao"));
            return View(model);
        }

        public IActionResult Equipe()
        {
            var model = Context.Paginas.FirstOrDefault(x => x.Nome.Equals("Equipe"));
            return View(model);
        }
        public IActionResult Contato()
        {
            var model = Context.Paginas.FirstOrDefault(x => x.Nome.Equals("Contato"));
            return View(model);
        }

    }
}
