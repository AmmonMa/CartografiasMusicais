using CartografiasMusicais.Business.Context;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CartografiasMusicais.Business.Services
{
    public interface ICidadeService
    {
        Task<IList<Cidade>> List();
    }
}
