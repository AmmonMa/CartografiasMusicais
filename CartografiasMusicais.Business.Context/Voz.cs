﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CartografiasMusicais.Business.Context
{
    [Table("Vozes")]
    public class Voz
    {
        public int Id { get; set; }
        public string Descricao { get; set; }
        public string Imagem { get; set; }
        public int NarrativaId { get; set; }
        public virtual Narrativa Narrativa { get; set; }
        public string Slug { get; set; }
    }
}
