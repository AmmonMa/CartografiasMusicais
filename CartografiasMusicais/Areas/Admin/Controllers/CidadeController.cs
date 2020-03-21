﻿using CartografiasMusicais.Business.Context;
using CartografiasMusicais.CrossCutting.Utils;
using CartografiasMusicais.CrossCutting.ValidationModels.Cidade;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting.Internal;
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
    public class CidadeController : Controller
    {
        private CoreContext Context;
        private readonly ISlugHelper SlugHelper;
        private readonly IWebHostEnvironment HostingEnvironment;

        public CidadeController(CoreContext context, ISlugHelper slugHelper, IWebHostEnvironment hostingEnvironment)
        {
            Context = context;
            SlugHelper = slugHelper;
            HostingEnvironment = hostingEnvironment;
        }
        public async Task<IActionResult> Index()
        {
            var model = await Context.Cidades.OrderByDescending(x => x.Id).ToListAsync();
            return View(model);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(NovaCidadeDTO obj)
        {
            if (ModelState.IsValid)
            {

                await Context.Cidades.AddAsync(new Cidade
                {
                    Nome = obj.Nome,
                    Descricao = obj.Descricao,
                    Slug = SlugHelper.GenerateSlug(obj.Nome).ToString(),
                    Imagem = ((obj.Imagem != null) ? await FileService
                                    .UploadFileAsync(obj.Imagem,
                                                    HostingEnvironment.WebRootPath + "/imagens/",
                                                    $"{obj.Nome}_{DateTimeOffset.UtcNow.ToUnixTimeSeconds()}_{Path.GetExtension(obj.Imagem.FileName)}") : null)
                });
                await Context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(obj);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var cidade = await Context.Cidades.FirstOrDefaultAsync(x => x.Id == id);
            var model = new MudaCidadeDTO
            {
                Id = cidade.Id,
                Nome = cidade.Nome,
                Descricao = cidade.Descricao,
                CaminhoImagem = cidade.Imagem
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(MudaCidadeDTO obj)
        {
            var cidade = await Context.Cidades.FirstOrDefaultAsync(x => x.Id == obj.Id);
            if (ModelState.IsValid && cidade != null)
            {
                cidade.Nome = obj.Nome;
                cidade.Descricao = obj.Descricao;
                cidade.Slug = SlugHelper.GenerateSlug(obj.Nome).ToString();
                if (obj.Imagem != null)
                {
                    cidade.Imagem = await FileService
                                        .UploadFileAsync(obj.Imagem,
                                                        HostingEnvironment.WebRootPath + "/imagens/",
                                                        $"{obj.Nome}_{DateTimeOffset.UtcNow.ToUnixTimeSeconds()}_{Path.GetExtension(obj.Imagem.FileName)}");
                }
                await Context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(obj);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var cidade = await Context.Cidades.FirstOrDefaultAsync(x => x.Id == id);

            Context.Cidades.Remove(cidade);
            await Context.SaveChangesAsync();
            return Ok();
        }
    }
}