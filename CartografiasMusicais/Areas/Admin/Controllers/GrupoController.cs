using CartografiasMusicais.Business.Context;
using CartografiasMusicais.CrossCutting.Utils;
using CartografiasMusicais.CrossCutting.ValidationModels.Grupo;
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
    public class GrupoController : Controller
    {
        private CoreContext Context;
        private readonly ISlugHelper SlugHelper;
        private readonly IWebHostEnvironment HostingEnvironment;

        public GrupoController(CoreContext context, ISlugHelper slugHelper, IWebHostEnvironment hostingEnvironment)
        {
            Context = context;
            SlugHelper = slugHelper;
            HostingEnvironment = hostingEnvironment;
        }
        public async Task<IActionResult> Index()
        {
            var model = await Context.Grupos.OrderByDescending(x => x.Id).ToListAsync();
            return View(model);
        }

        public IActionResult Create()
        {
            ViewBag.Cidades = new SelectList(Context.Cidades.OrderByDescending(x => x.Id).ToList(), "Id", "Nome");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(NovoGrupoDTO obj)
        {
            if (ModelState.IsValid)
            {

                await Context.Grupos.AddAsync(new Grupo
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
            var grupo = await Context.Grupos.FirstOrDefaultAsync(x => x.Id == id);
            var model = new MudaGrupoDTO
            {
                Id = grupo.Id,
                Nome = grupo.Nome,
                Descricao = grupo.Descricao,
                Video = grupo.Video,
                CidadeId = grupo.CidadeId,
                Slug = grupo.Slug,
                CaminhoImagem = grupo.Imagem
            };
            ViewBag.Cidades = new SelectList(Context.Cidades.OrderByDescending(x => x.Id).ToList(), "Id", "Nome");
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(MudaGrupoDTO obj)
        {
            var grupo = await Context.Grupos.FirstOrDefaultAsync(x => x.Id == obj.Id);
            if (ModelState.IsValid && grupo != null)
            {
                grupo.Id = obj.Id;
                grupo.Nome = obj.Nome;
                grupo.Descricao = obj.Descricao;
                grupo.Video = obj.Video;
                grupo.CidadeId = obj.CidadeId;

                grupo.Slug = SlugHelper.GenerateSlug(obj.Descricao).ToString();
                if (obj.Imagem != null)
                {
                    grupo.Imagem = await FileService
                                        .UploadFileAsync(obj.Imagem,
                                                        HostingEnvironment.WebRootPath + "/imagens/",
                                                        $"{DateTimeOffset.UtcNow.ToUnixTimeSeconds()}_{Path.GetExtension(obj.Imagem.FileName)}");
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
            var grupo = await Context.Grupos.FirstOrDefaultAsync(x => x.Id == id);

            Context.Grupos.Remove(grupo);
            await Context.SaveChangesAsync();
            return Ok();
        }
    }
}
