using CartografiasMusicais.CrossCutting.Utils;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CartografiasMusicais.CrossCutting.ValidationModels.Cidade
{
    public class NovaCidadeDTO
    {
        [Display(Name = "Nome do Cidade")]
        [Required(ErrorMessage = "Nome da Cidade é obrigatório")]
        public string Nome { get; set; }
        [Display(Name = "Descrição da Cidade")]
        [Required(ErrorMessage = "Descrição da Cidade é obrigatório")]
        public string Descricao { get; set; }
        public string Slug { get; set; }
        [Display(Name = "Imagem")]
        [ValidExtension(".jpg,.jpeg,.png,.JPG,.JPEG,.PNG,.pdf,.PDF")]
        public IFormFile Imagem { get; set; }
        public string CaminhoImagem { get; set; }
    }
}
