using BANCO.Core;
using BANCO.Data.Configuration;
using Microsoft.EntityFrameworkCore;

namespace BANCO.Data
{
    public class Contexto : DbContext
    {
        public Contexto(DbContextOptions<Contexto> options)
             : base(options)
        {

        }

        public DbSet<Conta> Contas { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new ContaConfiguration());
        }
    }
}
