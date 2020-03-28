using CartografiasMusicais.Business.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CartografiasMusicais.Controllers
{
    public class MusicaViewController : Controller
    {
        private CoreContext Context;
        public MusicaViewController(CoreContext context)
        {
            Context = context;
        }

        public async Task<IActionResult> Index()
        {
            var model = await Context.Musicas.OrderByDescending(x => x.Id).ToListAsync();
            ViewBag.Imagens = await Context.Musicas.Where(x => string.IsNullOrEmpty(x.Video)).OrderByDescending(x => x.Id).ToListAsync();
            ViewBag.Videos = await Context.Musicas.Where(x => !string.IsNullOrEmpty(x.Video)).OrderByDescending(x => x.Id).ToListAsync();
            ViewBag.VideoDefault = await Context.Musicas.Where(x => !string.IsNullOrEmpty(x.Video)).OrderByDescending(x => x.Id).FirstAsync();
            return View(model);
        }

        public async Task<IActionResult> Details(string slug)
        {
            var model = await Context.Musicas.FirstOrDefaultAsync(x => x.Slug == slug);

            ViewBag.Imagens = await Context.Musicas.Where(x => string.IsNullOrEmpty(x.Video)).OrderByDescending(x => x.Id).ToListAsync();
            ViewBag.Videos = await Context.Musicas.Where(x => !string.IsNullOrEmpty(x.Video)).OrderByDescending(x => x.Id).ToListAsync();
            
            return View(model);
        }

        public async Task<IActionResult> Specific([FromRoute] string permalink, [FromRoute] string slug)
        {
            var cidade = await Context.Cidades.FirstOrDefaultAsync(x => x.Slug == permalink);

            ViewBag.Imagens = await Context.Musicas.Where(x => string.IsNullOrEmpty(x.Video) && x.CidadeId == cidade.Id).OrderByDescending(x => x.Id).ToListAsync();
            ViewBag.Videos = await Context.Musicas.Where(x => !string.IsNullOrEmpty(x.Video) && x.CidadeId == cidade.Id).OrderByDescending(x => x.Id).ToListAsync();

            return View(cidade.Musicas.FirstOrDefault(x => x.Slug == slug));

        }

        [HttpGet]
        public IActionResult Video(string link)
        {
            ViewBag.Link = link;
            return View();
        }
    }
}
