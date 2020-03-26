using CartografiasMusicais.Business.Context;
using CartografiasMusicais.CrossCutting.Utils;
using CartografiasMusicais.CrossCutting.ValidationModels.Espaco;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Slugify;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CartografiasMusicais.Areas.Admin.Controllers
{
    [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
    [Area("Admin")]
    [AllowAnonymous]
    public class EspacoController : Controller
    {
        private CoreContext Context;
        private readonly ISlugHelper SlugHelper;
        private readonly IWebHostEnvironment HostingEnvironment;

        public EspacoController(CoreContext context, ISlugHelper slugHelper, IWebHostEnvironment hostingEnvironment)
        {
            Context = context;
            SlugHelper = slugHelper;
            HostingEnvironment = hostingEnvironment;
        }
        public async Task<IActionResult> Index()
        {
            var model = await Context.Espacos.OrderByDescending(x => x.Id).ToListAsync();
            return View(model);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(NovoEspacoDTO obj)
        {
            if (ModelState.IsValid)
            {

                await Context.Espacos.AddAsync(new Espaco
                {
                    Nome = obj.Nome,
                    Descricao = obj.Descricao,
                    Slug = SlugHelper.GenerateSlug(obj.Nome).ToString(),
                    Imagem = ((obj.Imagem != null) ? await FileService
                                    .UploadFileAsync(obj.Imagem,
                                                    HostingEnvironment.WebRootPath + "/imagens/",
                                                    $"{DateTimeOffset.UtcNow.ToUnixTimeSeconds()}_{Path.GetExtension(obj.Imagem.FileName)}") : null)
                });
                await Context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(obj);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var espaco = await Context.Espacos.FirstOrDefaultAsync(x => x.Id == id);
            var model = new MudaEspacoDTO
            {
                Id = espaco.Id,
                Nome = espaco.Nome,
                Descricao = espaco.Descricao,
                CaminhoImagem = espaco.Imagem
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(MudaEspacoDTO obj)
        {
            var espaco = await Context.Espacos.FirstOrDefaultAsync(x => x.Id == obj.Id);
            if (ModelState.IsValid && espaco != null)
            {
                espaco.Nome = obj.Nome;
                espaco.Descricao = obj.Descricao;
                espaco.Slug = SlugHelper.GenerateSlug(obj.Nome).ToString();
                if (obj.Imagem != null)
                {
                    espaco.Imagem = await FileService
                                        .UploadFileAsync(obj.Imagem,
                                                        HostingEnvironment.WebRootPath + "/imagens/",
                                                        $"{DateTimeOffset.UtcNow.ToUnixTimeSeconds()}_{Path.GetExtension(obj.Imagem.FileName)}");
                }
                await Context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(obj);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var espaco = await Context.Espacos.FirstOrDefaultAsync(x => x.Id == id);

            Context.Espacos.Remove(espaco);
            await Context.SaveChangesAsync();
            return Ok();
        }
    }
}
