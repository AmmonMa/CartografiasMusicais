﻿using CartografiasMusicais.CrossCutting.Utils;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CartografiasMusicais.CrossCutting.ValidationModels.Narrativa
{
    public class NarrativaItemValidationModel
    {
        public int? Id { get; set; }
        [Display(Name = "Descrição")]
        [Required(ErrorMessage = "Descrição é obrigatório")]
        public string Descricao { get; set; }
        [Display(Name = "Imagem")]
        [ValidExtension(".jpg,.jpeg,.png,.JPG,.JPEG,.PNG")]
        public IFormFile Imagem { get; set; }
        public string CaminhoImagem { get; set; }
        public int NarrativaId { get; set; }
    }
}
