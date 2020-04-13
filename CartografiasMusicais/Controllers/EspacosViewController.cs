using CartografiasMusicais.Business.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CartografiasMusicais.Controllers
{
    public class EspacosViewController : Controller
    {
        private CoreContext Context;
        public EspacosViewController(CoreContext context)
        {
            Context = context;
        }

        public async Task<IActionResult> Index()
        {
            var model = await Context.Espacos.OrderByDescending(x => x.Id).ToListAsync();
            ViewBag.Imagens = await Context.Espacos.Where(x => string.IsNullOrEmpty(x.Video)).OrderByDescending(x => x.Id).ToListAsync();
            ViewBag.Videos = await Context.Espacos.Where(x => !string.IsNullOrEmpty(x.Video)).OrderByDescending(x => x.Id).ToListAsync();
            ViewBag.ImagemDefault = await Context.Espacos.Where(x => string.IsNullOrEmpty(x.Video)).OrderByDescending(x => x.Id).FirstAsync();
            return View(model);
        }

        public async Task<IActionResult> Details(string slug)
        {
            var model = await Context.Espacos.FirstOrDefaultAsync(x => x.Slug == slug);
            ViewBag.Imagens = await Context.Espacos.Where(x => string.IsNullOrEmpty(x.Video)).OrderByDescending(x => x.Id).ToListAsync();
            ViewBag.Videos = await Context.Espacos.Where(x => !string.IsNullOrEmpty(x.Video)).OrderByDescending(x => x.Id).ToListAsync();
            ViewBag.ImagemDefault = await Context.Espacos.Where(x => string.IsNullOrEmpty(x.Video)).OrderByDescending(x => x.Id).FirstAsync();

            return View(model);
        }

        [HttpGet]
        public IActionResult Video(string link)
        {
            ViewBag.Link = link;
            return View();
        }
    }
}
