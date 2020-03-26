using CartografiasMusicais.Business.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CartografiasMusicais.Business.Services
{
    public class CidadeService : ICidadeService
    {
        private CoreContext Context;

        public CidadeService(CoreContext context)
        {
            Context = context;
        }

        public async Task<IList<Cidade>> List()
        {
            return await Context.Cidades.OrderBy(x => x.Id).ToListAsync();
        }
    }
}
