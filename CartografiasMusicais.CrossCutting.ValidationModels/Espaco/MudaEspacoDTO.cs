using CartografiasMusicais.CrossCutting.Utils;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CartografiasMusicais.CrossCutting.ValidationModels.Espaco
{
    public class MudaEspacoDTO
    {
        public int Id { get; set; }
        [Display(Name = "Nome")]
        [Required(ErrorMessage = "Nome é obrigatório")]
        public string Nome { get; set; }
        [Display(Name = "Descrição")]
        [Required(ErrorMessage = "Descrição é obrigatório")]
        public string Descricao { get; set; }
        public string Slug { get; set; }
        [Display(Name = "Imagem")]
        [ValidExtension(".jpg,.jpeg,.png,.JPG,.JPEG,.PNG")]
        public IFormFile Imagem { get; set; }
        [Display(Name = "Codigo do Video")]
        public string Video { get; set; }
        public string CaminhoImagem { get; set; }
    }
}
