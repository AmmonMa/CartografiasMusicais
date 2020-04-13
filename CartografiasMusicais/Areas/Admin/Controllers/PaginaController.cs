using CartografiasMusicais.Business.Context;
using CartografiasMusicais.CrossCutting.ValidationModels.Pagina;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CartografiasMusicais.Areas.Admin.Controllers
{
    [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
    [Area("Admin")]
    [Authorize]
    public class PaginaController : Controller
    {
        private CoreContext Context;

        public PaginaController(CoreContext context)
        {
            Context = context;
        }

        public async Task<IActionResult> Index()
        {
            var model = await Context.Paginas.OrderBy(x => x.Nome).ToListAsync();
            return View(model);
        }

        public async Task<IActionResult> Edit(string nome)
        {
            var pagina = await Context.Paginas.FirstOrDefaultAsync(x => x.Nome.Equals(nome));
            var model = new MudaPaginaDTO()
            {
                Id = pagina.Id,
                Nome = pagina.Nome,
                Descricao = pagina.Descricao
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(MudaPaginaDTO model)
        {
            var pagina = await Context.Paginas.FirstOrDefaultAsync(x => x.Nome.Equals(model.Nome));
            if(ModelState.IsValid)
            {
                pagina.Descricao = model.Descricao;
                await Context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }
       
    }
}
