using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CartografiasMusicais.Business.Context
{
    [Table("narrativas")]
    public class Narrativa
    {
        public int Id { get; set; }
        public string Descricao { get; set; }
        public string Imagem { get; set; }
        public string Video { get; set; }
        public int? CidadeId { get; set; }
        public virtual Cidade Cidade { get; set; }
        public string Slug { get; set; }

        public virtual IList<Musico> Musicos { get; set; }
        public virtual IList<Frequentador> Frequentadores { get; set; }
        public virtual IList<Voz> Vozes { get; set; }

    }
}
