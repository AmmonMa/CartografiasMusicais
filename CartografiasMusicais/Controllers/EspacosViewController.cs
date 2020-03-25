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

            return View(model);
        }

        public async Task<IActionResult> Details(string slug)
        {
            var model = await Context.Espacos.FirstOrDefaultAsync(x => x.Slug == slug);
            ViewBag.Espacos = await Context.Espacos.OrderByDescending(x => x.Id).ToListAsync();
            return View(model);
        }
    }
}
