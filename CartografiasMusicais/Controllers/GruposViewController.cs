using CartografiasMusicais.Business.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CartografiasMusicais.Controllers
{
    public class GruposViewController : Controller
    {
        private CoreContext Context;
        public GruposViewController(CoreContext context)
        {
            Context = context;
        }

        public async Task<IActionResult> Index()
        {
            var model = await Context.Grupos.OrderByDescending(x => x.Id).ToListAsync();
            ViewBag.Imagens = await Context.Grupos.Where(x => string.IsNullOrEmpty(x.Video)).OrderByDescending(x => x.Id).ToListAsync();
            ViewBag.Videos = await Context.Grupos.Where(x => !string.IsNullOrEmpty(x.Video)).OrderByDescending(x => x.Id).ToListAsync();
            return View(model);
        }

        public async Task<IActionResult> Details(string slug)
        {
            var model = await Context.Grupos.FirstOrDefaultAsync(x => x.Slug == slug);

            ViewBag.Imagens = await Context.Grupos.Where(x => string.IsNullOrEmpty(x.Video)).OrderByDescending(x => x.Id).ToListAsync();
            ViewBag.Videos = await Context.Grupos.Where(x => !string.IsNullOrEmpty(x.Video)).OrderByDescending(x => x.Id).ToListAsync();

            return View(model);
        }
    }
}
