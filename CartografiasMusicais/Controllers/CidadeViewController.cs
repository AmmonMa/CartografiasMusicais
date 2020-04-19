using CartografiasMusicais.Business.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CartografiasMusicais.Controllers
{
    public class CidadeViewController : Controller
    {
        private CoreContext Context;
        public CidadeViewController(CoreContext context)
        {
            Context = context;
        }


        public async Task<IActionResult> Details(string slug)
        {
            var model = await Context.Cidades.FirstOrDefaultAsync(x => x.Slug == slug);
            return View(model);
        }

        [HttpGet]
        public IActionResult Video(string link)
        {
            ViewBag.Link = link;
            return View();
        }

        [HttpGet]
        public IActionResult Image(string imagem)
        {
            ViewBag.Imagem = imagem;
            return View();
        }
    }
}
