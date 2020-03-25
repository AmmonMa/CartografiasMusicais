using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CartografiasMusicais.Business.Context
{
    [Table("espacos")]
    public class Espaco
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public string Imagem { get; set; }
        public string Slug { get; set; }
    }
}
