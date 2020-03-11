using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CartografiasMusicais.Business.Context
{
    public class CoreContext : IdentityDbContext<User>
    {
        public CoreContext()
        {
        }

        public CoreContext(DbContextOptions<CoreContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
 
            base.OnModelCreating(modelBuilder);
        }


    }
}
