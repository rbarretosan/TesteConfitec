using Microsoft.EntityFrameworkCore;
using TesteConfitec.Domain;

namespace TesteConfitec.Persistence.Contextos
{
    public class TesteConfitecContext : DbContext
    {
        public TesteConfitecContext(DbContextOptions<TesteConfitecContext> options) : base(options) { }
        
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<HistoricoEscolar> HistoricosEscolares { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        }
    }
}