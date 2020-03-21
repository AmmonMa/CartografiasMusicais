using CartografiasMusicais.Business.Context;
using CartografiasMusicais.CrossCutting.Utils;
using CartografiasMusicais.CrossCutting.ValidationModels.Narrativa;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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
    public class NarrativaController : Controller
    {
        private CoreContext Context;
        private readonly ISlugHelper SlugHelper;
        private readonly IWebHostEnvironment HostingEnvironment;

        public NarrativaController(CoreContext context, ISlugHelper slugHelper, IWebHostEnvironment hostingEnvironment)
        {
            Context = context;
            SlugHelper = slugHelper;
            HostingEnvironment = hostingEnvironment;
        }
        public async Task<IActionResult> Index()
        {
            var model = await Context.Narrativas.OrderByDescending(x => x.Id).ToListAsync();
            return View(model);
        }

        public IActionResult Create()
        {
            ViewBag.Cidades = new SelectList(Context.Cidades.OrderByDescending(x => x.Id).ToList(), "Id", "Nome");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(NovaNarrativaDTO obj)
        {
            if (ModelState.IsValid)
            {

                await Context.Narrativas.AddAsync(new Narrativa
                {
                    Descricao = obj.Descricao,
                    Video = obj.Video,
                    CidadeId = obj.CidadeId,
                    Slug = SlugHelper.GenerateSlug(obj.Descricao).ToString(),
                    Imagem = ((obj.Imagem != null) ? await FileService
                                    .UploadFileAsync(obj.Imagem,
                                                    HostingEnvironment.WebRootPath + "/imagens/",
                                                    $"{obj.Descricao}_{DateTimeOffset.UtcNow.ToUnixTimeSeconds()}_{Path.GetExtension(obj.Imagem.FileName)}") : null)
                });
                await Context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Cidades = new SelectList(Context.Cidades.OrderByDescending(x => x.Id).ToList(), "Id", "Nome");
            return View(obj);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var narrativa = await Context.Narrativas.FirstOrDefaultAsync(x => x.Id == id);
            var model = new MudaNarrativaDTO
            {
                Id = narrativa.Id,
                Descricao = narrativa.Descricao,
                Video = narrativa.Video,
                CidadeId = narrativa.CidadeId,
                Slug = narrativa.Slug,
                CaminhoImagem = narrativa.Imagem
            };
            ViewBag.Cidades = new SelectList(Context.Cidades.OrderByDescending(x => x.Id).ToList(), "Id", "Nome");
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(MudaNarrativaDTO obj)
        {
            var narrativa = await Context.Narrativas.FirstOrDefaultAsync(x => x.Id == obj.Id);
            if (ModelState.IsValid && narrativa != null)
            {
                narrativa.Descricao = obj.Descricao;
                narrativa.Video = obj.Video;
                narrativa.CidadeId = obj.CidadeId;

                narrativa.Slug = SlugHelper.GenerateSlug(obj.Descricao).ToString();
                if (obj.Imagem != null)
                {
                    narrativa.Imagem = await FileService
                                        .UploadFileAsync(obj.Imagem,
                                                        HostingEnvironment.WebRootPath + "/imagens/",
                                                        $"{obj.Descricao}_{DateTimeOffset.UtcNow.ToUnixTimeSeconds()}_{Path.GetExtension(obj.Imagem.FileName)}");
                }
                await Context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Cidades = new SelectList(Context.Cidades.OrderByDescending(x => x.Id).ToList(), "Id", "Nome");
            return View(obj);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var narrativa = await Context.Narrativas.FirstOrDefaultAsync(x => x.Id == id);

            Context.Narrativas.Remove(narrativa);
            await Context.SaveChangesAsync();
            return Ok();
        }
    }
}
