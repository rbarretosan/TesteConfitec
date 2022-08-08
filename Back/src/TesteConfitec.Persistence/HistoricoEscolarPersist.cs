using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TesteConfitec.Domain;
using TesteConfitec.Persistence.Contextos;
using TesteConfitec.Persistence.Contratos;

namespace TesteConfitec.Persistence
{
    public class HistoricoEscolarPersist : IHistoricoEscolarPersist
    {
        private readonly TesteConfitecContext _context;
        public HistoricoEscolarPersist(TesteConfitecContext context)
        {
            _context = context;
        }

        public async Task<HistoricoEscolar> GetHistoricoEscolarByIdsAsync(int usuarioId, int id)
        {
            IQueryable<HistoricoEscolar> query = _context.HistoricosEscolares;

            query = query.AsNoTracking()
                         .Where(historicoEscolar => 
                                historicoEscolar.UsuarioId == usuarioId && 
                                historicoEscolar.Id == id);

            return await query.FirstOrDefaultAsync();
        }

        public async Task<HistoricoEscolar[]> GetHistoricosEscolaresByUsuarioIdAsync(int usuarioId)
        {
            IQueryable<HistoricoEscolar> query = _context.HistoricosEscolares;

            query = query.AsNoTracking()
                         .Where(historicoEscolar => historicoEscolar.UsuarioId == usuarioId);

            return await query.ToArrayAsync();
        }
    }
}