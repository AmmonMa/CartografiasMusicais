using CartografiasMusicais.Business.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CartografiasMusicais.Controllers
{
    public class NarrativaViewController : Controller
    {
        private CoreContext Context;
        public NarrativaViewController(CoreContext context)
        {
            Context = context;
        }

        public async Task<IActionResult> Index()
        {
            var model = await Context.Narrativas.FirstOrDefaultAsync(x => x.CidadeId == 0);

            return View(model);
        }

        public async Task<IActionResult> Musico(string slug)
        {
            var model = await Context.Musicos.FirstOrDefaultAsync(x => x.Slug == slug);
            ViewBag.Narrativas = await Context.Narrativas.FirstOrDefaultAsync(x => x.CidadeId == 0);
            return View(model);
        }


        public async Task<IActionResult> Voz(string slug)
        {
            var model = await Context.Vozes.FirstOrDefaultAsync(x => x.Slug == slug);
            ViewBag.Narrativas = await Context.Narrativas.FirstOrDefaultAsync(x => x.CidadeId == 0);
            return View(model);
        }


        public async Task<IActionResult> Frequentador(string slug)
        {
            var model = await Context.Frequentadores.FirstOrDefaultAsync(x => x.Slug == slug);
            ViewBag.Narrativas = await Context.Narrativas.FirstOrDefaultAsync(x => x.CidadeId == 0);
            return View(model);
        }

        public async Task<IActionResult> Specific([FromRoute] string permalink, [FromRoute] string slug)
        {
            var cidade = await Context.Cidades.FirstOrDefaultAsync(x => x.Slug == permalink);


            return View(cidade.Narrativas.FirstOrDefault(x => x.Slug == slug));

        }

        [HttpGet]
        public IActionResult Video(string link)
        {
            ViewBag.Link = link;
            return View();
        }
    }
}
