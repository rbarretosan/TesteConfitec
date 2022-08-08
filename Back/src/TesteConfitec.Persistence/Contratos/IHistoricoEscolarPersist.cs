using System.Threading.Tasks;
using TesteConfitec.Domain;

namespace TesteConfitec.Persistence.Contratos
{
    public interface IHistoricoEscolarPersist: IGeralPersist
    {
        Task<HistoricoEscolar[]> GetHistoricosEscolaresByUsuarioIdAsync(int usuarioId);
        Task<HistoricoEscolar> GetHistoricoEscolarByIdsAsync(int usuarioId, int id);
    }
}