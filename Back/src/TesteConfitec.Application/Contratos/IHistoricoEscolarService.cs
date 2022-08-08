using System.Threading.Tasks;
using TesteConfitec.Application.Dtos;

namespace TesteConfitec.Application.Contratos
{
    public interface IHistoricoEscolarService
    {
        Task<HistoricoEscolarDto[]> SaveHistoricosEscolares(int usuarioId, HistoricoEscolarDto[] models);
        Task<HistoricoEscolarDto> UpdateHistoricoEscolar(int usuarioId, int historicoEscolarId, HistoricoEscolarDto model);
        Task<bool> DeleteHistoricoEscolar(int usuarioId, int historicoEscolarId);
        Task<HistoricoEscolarDto[]> GetHistoricosEscolaresByUsuarioIdAsync(int usuarioId);
        Task<HistoricoEscolarDto> GetHistoricoEscolarByIdsAsync(int usuarioId, int historicoEscolarId);
    }
}