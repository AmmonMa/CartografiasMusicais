using CartografiasMusicais.Business.Context;
using CartografiasMusicais.CrossCutting.Utils;
using CartografiasMusicais.CrossCutting.ValidationModels.Corpo;
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
    public class CorpoController : Controller
    {
        private CoreContext Context;
        private readonly ISlugHelper SlugHelper;
        private readonly IWebHostEnvironment HostingEnvironment;

        public CorpoController(CoreContext context, ISlugHelper slugHelper, IWebHostEnvironment hostingEnvironment)
        {
            Context = context;
            SlugHelper = slugHelper;
            HostingEnvironment = hostingEnvironment;
        }
        public async Task<IActionResult> Index()
        {
            var model = await Context.Corpos.OrderByDescending(x => x.Id).ToListAsync();
            return View(model);
        }

        public IActionResult Create()
        {
            ViewBag.Cidades = new SelectList(Context.Cidades.OrderByDescending(x => x.Id).ToList(), "Id", "Nome");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(NovoCorpoDTO obj)
        {
            if (ModelState.IsValid)
            {

                await Context.Corpos.AddAsync(new Corpo
                {
                    Nome = obj.Nome,
                    Descricao = obj.Descricao,
                    Video = obj.Video,
                    CidadeId = obj.CidadeId,
                    Slug = SlugHelper.GenerateSlug(obj.Descricao).ToString(),
                    Imagem = ((obj.Imagem != null) ? await FileService
                                    .UploadFileAsync(obj.Imagem,
                                                    HostingEnvironment.WebRootPath + "/imagens/",
                                                    $"{DateTimeOffset.UtcNow.ToUnixTimeSeconds()}_{Path.GetExtension(obj.Imagem.FileName)}") : null)
                });
                await Context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Cidades = new SelectList(Context.Cidades.OrderByDescending(x => x.Id).ToList(), "Id", "Nome");
            return View(obj);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var corpo = await Context.Corpos.FirstOrDefaultAsync(x => x.Id == id);
            var model = new MudaCorpoDTO
            {
                Id = corpo.Id,
                Descricao = corpo.Descricao,
                Nome = corpo.Nome,
                Video = corpo.Video,
                CidadeId = corpo.CidadeId,
                Slug = corpo.Slug,
                CaminhoImagem = corpo.Imagem
            };
            ViewBag.Cidades = new SelectList(Context.Cidades.OrderByDescending(x => x.Id).ToList(), "Id", "Nome");
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(MudaCorpoDTO obj)
        {
            var corpo = await Context.Corpos.FirstOrDefaultAsync(x => x.Id == obj.Id);
            if (ModelState.IsValid && corpo != null)
            {
                corpo.Id = obj.Id;
                corpo.Descricao = obj.Descricao;
                corpo.Nome = obj.Nome;
                corpo.Video = obj.Video;
                corpo.CidadeId = obj.CidadeId;

                corpo.Slug = SlugHelper.GenerateSlug(obj.Descricao).ToString();
                if(obj.Imagem != null)
                {
                    corpo.Imagem = await FileService
                                        .UploadFileAsync(obj.Imagem,
                                                        HostingEnvironment.WebRootPath + "/imagens/",
                                                        $"{DateTimeOffset.UtcNow.ToUnixTimeSeconds()}_{Path.GetExtension(obj.Imagem.FileName)}");
                }

                await Context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            Console.WriteLine(ModelState);
            ViewBag.Cidades = new SelectList(Context.Cidades.OrderByDescending(x => x.Id).ToList(), "Id", "Nome");
            return View(obj);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var corpo = await Context.Corpos.FirstOrDefaultAsync(x => x.Id == id);

            Context.Corpos.Remove(corpo);
            await Context.SaveChangesAsync();
            return Ok();
        }
    }
}
