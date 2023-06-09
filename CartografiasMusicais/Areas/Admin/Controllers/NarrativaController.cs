﻿using CartografiasMusicais.Business.Context;
using CartografiasMusicais.CrossCutting.Utils;
using CartografiasMusicais.CrossCutting.ValidationModels.Narrativa;
using ImageMagick;
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
    [Authorize]
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
                var model = new Narrativa
                {
                    Descricao = obj.Descricao,
                    Video = obj.Video,
                    CidadeId = obj.CidadeId,
                    // Slug = SlugHelper.GenerateSlug(obj.Descricao).ToString(),
                    Musicos = new List<Musico>(),
                    Frequentadores = new List<Frequentador>(),
                    Vozes = new List<Voz>(),
                    Imagem = ((obj.Imagem != null) ? await FileService
                                    .UploadFileAsync(obj.Imagem,
                                                    HostingEnvironment.WebRootPath + "/imagens/content/",
                                                    $"{DateTimeOffset.UtcNow.ToUnixTimeSeconds()}_{Path.GetExtension(obj.Imagem.FileName)}") : null)
                };
                var musicos = obj.Musicos ?? new List<NarrativaItemValidationModel>();
                foreach (var m in musicos)
                {
                    var musico = new Musico
                    {
                        Nome = m.Nome,
                        Video = m.Video,
                        Descricao = m.Descricao,
                        // Slug = SlugHelper.GenerateSlug(m.Nome).ToString(),
                        Imagem = ((m.Imagem != null) ? await FileService
                                    .UploadFileAsync(m.Imagem,
                                                    HostingEnvironment.WebRootPath + "/imagens/content/",
                                                    $"{DateTimeOffset.UtcNow.ToUnixTimeSeconds()}_{Path.GetExtension(m.Imagem.FileName)}") : null)
                    };

                    if(musico.Imagem != null)
                    {
                        using (var image = new MagickImage(HostingEnvironment.WebRootPath + "/imagens/content/" + musico.Imagem))
                        {
                            var size = new MagickGeometry(150, 90);
                            size.IgnoreAspectRatio = false;
                            image.Quality = 100;
                            image.Resize(size);
                            image.Write(HostingEnvironment.WebRootPath + "/imagens/content/thumbs/" + musico.Imagem);
                        }
                    }

                    model.Musicos.Add(musico);
                }

                var frequentadores = obj.Frequentadores ?? new List<NarrativaItemValidationModel>();
                foreach (var f in frequentadores)
                {
                    var frequentador = new Frequentador
                    {
                        Nome = f.Nome,
                        Video = f.Video,
                        Descricao = f.Descricao,
                        // Slug = SlugHelper.GenerateSlug(f.Nome).ToString(),
                        Imagem = ((f.Imagem != null) ? await FileService
                                    .UploadFileAsync(f.Imagem,
                                                    HostingEnvironment.WebRootPath + "/imagens/content/",
                                                    $"{DateTimeOffset.UtcNow.ToUnixTimeSeconds()}_{Path.GetExtension(f.Imagem.FileName)}") : null)
                    };

                    if(frequentador.Imagem != null)
                    {
                        using (var image = new MagickImage(HostingEnvironment.WebRootPath + "/imagens/content/" + frequentador.Imagem))
                        {
                            var size = new MagickGeometry(150, 90);
                            size.IgnoreAspectRatio = false;
                            image.Quality = 100;
                            image.Resize(size);
                            image.Write(HostingEnvironment.WebRootPath + "/imagens/content/thumbs/" + frequentador.Imagem);
                        }
                    }

                    model.Frequentadores.Add(frequentador);
                }

                var vozes = obj.Vozes ?? new List<NarrativaItemValidationModel>();
                foreach (var v in vozes)
                {
                    var voz = new Voz
                    {
                        Nome = v.Nome,
                        Video = v.Video,
                        Descricao = v.Descricao,
                        // Slug = SlugHelper.GenerateSlug(v.Nome).ToString(),
                        Imagem = ((v.Imagem != null) ? await FileService
                                    .UploadFileAsync(v.Imagem,
                                                    HostingEnvironment.WebRootPath + "/imagens/content/",
                                                    $"{DateTimeOffset.UtcNow.ToUnixTimeSeconds()}_{Path.GetExtension(v.Imagem.FileName)}") : null)
                    };

                    if(voz.Imagem != null)
                    {
                        using (var image = new MagickImage(HostingEnvironment.WebRootPath + "/imagens/content/" + voz.Imagem))
                        {
                            var size = new MagickGeometry(150, 90);
                            size.IgnoreAspectRatio = false;
                            image.Quality = 100;
                            image.Resize(size);
                            image.Write(HostingEnvironment.WebRootPath + "/imagens/content/thumbs/" + voz.Imagem);
                        }
                    }

                    model.Vozes.Add(voz);
                }

                await Context.Narrativas.AddAsync(model);
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
               // Slug = narrativa.Slug,
                CaminhoImagem = narrativa.Imagem,
                Musicos = new List<NarrativaItemValidationModel>(),
                Frequentadores = new List<NarrativaItemValidationModel>(),
                Vozes = new List<NarrativaItemValidationModel>()
            };

            var musicos = narrativa.Musicos ?? new List<Musico>();
            foreach (var m in musicos)
            {
                model.Musicos.Add(new NarrativaItemValidationModel()
                {
                    Id = m.Id,
                    Nome = m.Nome,
                    Video = m.Video,
                    Descricao = m.Descricao,
                    CaminhoImagem = m.Imagem,
                    NarrativaId = m.NarrativaId
                });
            }

            var frequentadores = narrativa.Frequentadores ?? new List<Frequentador>();
            foreach (var f in frequentadores)
            {
                model.Frequentadores.Add(new NarrativaItemValidationModel()
                {
                    Id = f.Id,
                    Descricao = f.Descricao,
                    Nome = f.Nome,
                    Video = f.Video,
                    CaminhoImagem = f.Imagem,
                    NarrativaId = f.NarrativaId
                });
            }

            var vozes = narrativa.Vozes ?? new List<Voz>();
            foreach (var v in vozes)
            {
                model.Vozes.Add(new NarrativaItemValidationModel()
                {
                    Id = v.Id,
                    Descricao = v.Descricao,
                    Nome = v.Nome,
                    Video = v.Video,
                    CaminhoImagem = v.Imagem,
                    NarrativaId = v.NarrativaId
                });
            }

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

                // narrativa.Slug = SlugHelper.GenerateSlug(obj.Descricao).ToString();
                if (obj.Imagem != null)
                {
                    narrativa.Imagem = await FileService
                                        .UploadFileAsync(obj.Imagem,
                                                        HostingEnvironment.WebRootPath + "/imagens/content/",
                                                        $"{DateTimeOffset.UtcNow.ToUnixTimeSeconds()}_{Path.GetExtension(obj.Imagem.FileName)}");

                    using (var image = new MagickImage(HostingEnvironment.WebRootPath + "/imagens/content/" + narrativa.Imagem))
                    {
                        var size = new MagickGeometry(150, 90);
                        size.IgnoreAspectRatio = false;
                        image.Quality = 100;
                        image.Resize(size);
                        image.Write(HostingEnvironment.WebRootPath + "/imagens/content/thumbs/" + narrativa.Imagem);
                    }
                }

                narrativa.Musicos = new List<Musico>();
                var musicos = obj.Musicos ?? new List<NarrativaItemValidationModel>();
                foreach (var m in musicos)
                {
                    
                    var n = narrativa.Musicos.Where(x => x.Id == m.Id).SingleOrDefault();
                    if(n != null)
                    {
                        if(m.Video != null)
                        {
                            narrativa.Musicos.Remove(n);
                            n.Nome = m.Nome;
                            n.Video = m.Video;
                            n.Descricao = m.Descricao;
                           // n.Slug = SlugHelper.GenerateSlug(m.Nome).ToString();
                            n.Imagem = ((m.Imagem != null) ? await FileService
                                        .UploadFileAsync(m.Imagem,
                                                        HostingEnvironment.WebRootPath + "/imagens/content/",
                                                        $"{DateTimeOffset.UtcNow.ToUnixTimeSeconds()}_{Path.GetExtension(m.Imagem.FileName)}") : n.Imagem);

                            if (n.Imagem != null)
                            {
                                using (var image = new MagickImage(HostingEnvironment.WebRootPath + "/imagens/content/" + n.Imagem))
                                {
                                    var size = new MagickGeometry(150, 90);
                                    size.IgnoreAspectRatio = false;
                                    image.Quality = 100;
                                    image.Resize(size);
                                    image.Write(HostingEnvironment.WebRootPath + "/imagens/content/thumbs/" + n.Imagem);
                                }
                            }

                            narrativa.Musicos.Add(n);
                        }
                        else
                        {
                            Context.Musicos.Remove(n);
                        }
                    }
                    else
                    {
                        var musico = new Musico
                        {
                            Nome = m.Nome,
                            Video = m.Video,
                            Descricao = m.Descricao,
                            //    Slug = SlugHelper.GenerateSlug(m.Nome).ToString(),
                            Imagem = ((m.Imagem != null) ? await FileService
                                        .UploadFileAsync(m.Imagem,
                                                        HostingEnvironment.WebRootPath + "/imagens/content/",
                                                        $"{DateTimeOffset.UtcNow.ToUnixTimeSeconds()}_{Path.GetExtension(m.Imagem.FileName)}") : null)
                        };

                        if (musico.Imagem != null)
                        {
                            using (var image = new MagickImage(HostingEnvironment.WebRootPath + "/imagens/content/" + musico.Imagem))
                            {
                                var size = new MagickGeometry(150, 90);
                                size.IgnoreAspectRatio = false;
                                image.Quality = 100;
                                image.Resize(size);
                                image.Write(HostingEnvironment.WebRootPath + "/imagens/content/thumbs/" + musico.Imagem);
                            }
                        }

                        narrativa.Musicos.Add(musico);
                    }
                }

                narrativa.Frequentadores = new List<Frequentador>();
                var frequentadores = obj.Frequentadores ?? new List<NarrativaItemValidationModel>();
                foreach (var f in frequentadores)
                {
                    var n = narrativa.Frequentadores.Where(x => x.Id == f.Id).SingleOrDefault();
                    if (n != null)
                    {
                        if(f.Video != null)
                        {
                            narrativa.Frequentadores.Remove(n);
                            n.Nome = f.Nome;
                            n.Video = f.Video;
                            n.Descricao = f.Descricao;
                         //   n.Slug = SlugHelper.GenerateSlug(f.Nome).ToString();
                            n.Imagem = ((f.Imagem != null) ? await FileService
                                        .UploadFileAsync(f.Imagem,
                                                        HostingEnvironment.WebRootPath + "/imagens/content/",
                                                        $"{DateTimeOffset.UtcNow.ToUnixTimeSeconds()}_{Path.GetExtension(f.Imagem.FileName)}") : n.Imagem);
                            if (n.Imagem != null)
                            {
                                using (var image = new MagickImage(HostingEnvironment.WebRootPath + "/imagens/content/" + n.Imagem))
                                {
                                    var size = new MagickGeometry(150, 90);
                                    size.IgnoreAspectRatio = false;
                                    image.Quality = 100;
                                    image.Resize(size);
                                    image.Write(HostingEnvironment.WebRootPath + "/imagens/content/thumbs/" + n.Imagem);
                                }
                            }

                            narrativa.Frequentadores.Add(n);
                        }
                        else
                        {
                            Context.Frequentadores.Remove(n);
                        }
                    }
                    else
                    {
                        var frequentador = new Frequentador
                        {
                            Nome = f.Nome,
                            Video = f.Video,
                            Descricao = f.Descricao,
                            //  Slug = SlugHelper.GenerateSlug(f.Nome).ToString(),
                            Imagem = ((f.Imagem != null) ? await FileService
                                        .UploadFileAsync(f.Imagem,
                                                        HostingEnvironment.WebRootPath + "/imagens/content/",
                                                        $"{DateTimeOffset.UtcNow.ToUnixTimeSeconds()}_{Path.GetExtension(f.Imagem.FileName)}") : null)
                        };
                        if (frequentador.Imagem != null)
                        {
                            using (var image = new MagickImage(HostingEnvironment.WebRootPath + "/imagens/content/" + frequentador.Imagem))
                            {
                                var size = new MagickGeometry(150, 90);
                                size.IgnoreAspectRatio = false;
                                image.Quality = 100;
                                image.Resize(size);
                                image.Write(HostingEnvironment.WebRootPath + "/imagens/content/thumbs/" + frequentador.Imagem);
                            }
                        }
                        narrativa.Frequentadores.Add(frequentador);
                    }
                }

                narrativa.Vozes = new List<Voz>();
                var vozes = obj.Vozes ?? new List<NarrativaItemValidationModel>();
                foreach (var v in vozes)
                {
                    var n = narrativa.Vozes.Where(x => x.Id == v.Id).SingleOrDefault();
                    if (n != null)
                    {
                        if(v.Video != null)
                        {
                            narrativa.Vozes.Remove(n);
                            n.Nome = v.Nome;
                            n.Video = v.Video;
                            n.Descricao = v.Descricao;
                           // n.Slug = SlugHelper.GenerateSlug(v.Nome).ToString();
                            n.Imagem = ((v.Imagem != null) ? await FileService
                                        .UploadFileAsync(v.Imagem,
                                                        HostingEnvironment.WebRootPath + "/imagens/content/",
                                                        $"{DateTimeOffset.UtcNow.ToUnixTimeSeconds()}_{Path.GetExtension(v.Imagem.FileName)}") : n.Imagem);
                            if (n.Imagem != null)
                            {
                                using (var image = new MagickImage(HostingEnvironment.WebRootPath + "/imagens/content/" + n.Imagem))
                                {
                                    var size = new MagickGeometry(150, 90);
                                    size.IgnoreAspectRatio = false;
                                    image.Quality = 100;
                                    image.Resize(size);
                                    image.Write(HostingEnvironment.WebRootPath + "/imagens/content/thumbs/" + n.Imagem);
                                }
                            }

                            narrativa.Vozes.Add(n);
                        }
                        else
                        {
                            Context.Vozes.Remove(n);
                        }
                    }
                    else
                    {
                        var voz = new Voz
                        {
                            Nome = v.Nome,
                            Video = v.Video,
                            Descricao = v.Descricao,
                            // Slug = SlugHelper.GenerateSlug(v.Nome).ToString(),
                            Imagem = ((v.Imagem != null) ? await FileService
                                        .UploadFileAsync(v.Imagem,
                                                        HostingEnvironment.WebRootPath + "/imagens/content/",
                                                        $"{DateTimeOffset.UtcNow.ToUnixTimeSeconds()}_{Path.GetExtension(v.Imagem.FileName)}") : null)
                        };

                        if (voz.Imagem != null)
                        {
                            using (var image = new MagickImage(HostingEnvironment.WebRootPath + "/imagens/content/" + n.Imagem))
                            {
                                var size = new MagickGeometry(150, 90);
                                size.IgnoreAspectRatio = false;
                                image.Quality = 100;
                                image.Resize(size);
                                image.Write(HostingEnvironment.WebRootPath + "/imagens/content/thumbs/" + n.Imagem);
                            }
                        }

                        narrativa.Vozes.Add(voz);
                    }
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
            Context.Musicos.RemoveRange(narrativa.Musicos);
            Context.Frequentadores.RemoveRange(narrativa.Frequentadores);
            Context.Vozes.RemoveRange(narrativa.Vozes);
            Context.Narrativas.Remove(narrativa);
            await Context.SaveChangesAsync();
            return Ok();
        }

        public ActionResult AddMusico()
        {
            return PartialView("MusicosItemForm", new NarrativaItemValidationModel());
        }

        public ActionResult AddFrequentador()
        {
            return PartialView("FrequentadorItemForm", new NarrativaItemValidationModel());
        }

        public ActionResult AddVoz()
        {
            return PartialView("VozItemForm", new NarrativaItemValidationModel());
        }
    }
}
