using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CartografiasMusicais.Business.Context
{
    [Table("Cidades")]
    public class Cidade
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Imagem { get; set; }
        public string Descricao { get; set; }
        public string Slug { get; set; }

        public virtual ICollection<Musica> Musicas { get; set; }
        public virtual ICollection<Corpo> Corpos { get; set; }
        public virtual ICollection<Grupo> Grupos { get; set; }
        public virtual ICollection<Narrativa> Narrativas { get; set; }

    }
}
