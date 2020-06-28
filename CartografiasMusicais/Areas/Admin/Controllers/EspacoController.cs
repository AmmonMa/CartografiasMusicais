using CartografiasMusicais.Business.Context;
using CartografiasMusicais.CrossCutting.Utils;
using CartografiasMusicais.CrossCutting.ValidationModels.Espaco;
using ImageMagick;
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
    [Authorize]
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
            foreach (var item in model)
            {
                if (item.Imagem != null)
                {
                    using (var image = new MagickImage(HostingEnvironment.WebRootPath + "/imagens/content/" + item.Imagem))
                    {
                        var size = new MagickGeometry(150, 90);
                        size.IgnoreAspectRatio = false;
                        image.Quality = 100;
                        image.Resize(size);
                        image.Write(HostingEnvironment.WebRootPath + "/imagens/content/thumbs/" + item.Imagem);
                    }

                }
            }
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
                var espaco = new Espaco
                {
                    Nome = obj.Nome,
                    Descricao = obj.Descricao,
                    Video = obj.Video,
                    Slug = SlugHelper.GenerateSlug(obj.Nome).ToString(),
                    Imagem = ((obj.Imagem != null) ? await FileService
                                    .UploadFileAsync(obj.Imagem,
                                                    HostingEnvironment.WebRootPath + "/imagens/content/",
                                                    $"{DateTimeOffset.UtcNow.ToUnixTimeSeconds()}_{Path.GetExtension(obj.Imagem.FileName)}") : null)
                };
                if (espaco.Imagem != null)
                {
                    using (var image = new MagickImage(HostingEnvironment.WebRootPath + "/imagens/content/" + espaco.Imagem))
                    {
                        var size = new MagickGeometry(150, 90);
                        size.IgnoreAspectRatio = false;
                        image.Quality = 100;
                        image.Resize(size);
                        image.Write(HostingEnvironment.WebRootPath + "/imagens/content/thumbs/" + espaco.Imagem);
                    }

                }

                await Context.Espacos.AddAsync(espaco);
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
                Video = espaco.Video,
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
                espaco.Video = obj.Video;
                espaco.Slug = SlugHelper.GenerateSlug(obj.Nome).ToString();
                if (obj.Imagem != null)
                {
                    espaco.Imagem = await FileService
                                        .UploadFileAsync(obj.Imagem,
                                                        HostingEnvironment.WebRootPath + "/imagens/content/",
                                                        $"{DateTimeOffset.UtcNow.ToUnixTimeSeconds()}_{Path.GetExtension(obj.Imagem.FileName)}");

                    using (var image = new MagickImage(HostingEnvironment.WebRootPath + "/imagens/content/" + espaco.Imagem))
                    {
                        var size = new MagickGeometry(150, 90);
                        size.IgnoreAspectRatio = false;
                        image.Quality = 100;
                        image.Resize(size);
                        image.Write(HostingEnvironment.WebRootPath + "/imagens/content/thumbs/" + espaco.Imagem);
                    }
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
