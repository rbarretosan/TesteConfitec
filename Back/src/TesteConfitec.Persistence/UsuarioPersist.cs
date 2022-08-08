using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TesteConfitec.Domain;
using TesteConfitec.Persistence.Contextos;
using TesteConfitec.Persistence.Contratos;

namespace TesteConfitec.Persistence
{
    public class UsuarioPersist : IUsuarioPersist
    {
        private readonly TesteConfitecContext _context;
        public UsuarioPersist(TesteConfitecContext context)
        {
            _context = context;
        }

        public async Task<Usuario[]> GetAllUsuariosAsync()
        {
            IQueryable<Usuario> query = _context.Usuarios
                .Include(h => h.HistoricosEscolares);

            query = query.AsNoTracking().OrderBy(h => h.Id);

            return await query.ToArrayAsync();
        }

        public async Task<Usuario[]> GetAllUsuariosByNomeAsync(string nome)
        {
            IQueryable<Usuario> query = _context.Usuarios
                .Include(h => h.HistoricosEscolares);

            query = query.AsNoTracking().OrderBy(h => h.Id)
                         .Where(h => h.Nome.ToLower().Contains(nome.ToLower()));

            return await query.ToArrayAsync();
        }

        public async Task<Usuario> GetUsuarioByIdAsync(int usuarioId)
        {
            IQueryable<Usuario> query = _context.Usuarios
                .Include(h => h.HistoricosEscolares);

            query = query.AsNoTracking().OrderBy(h => h.Id)
                         .Where(h => h.Id == usuarioId);

            return await query.FirstOrDefaultAsync();
        }
    }
}