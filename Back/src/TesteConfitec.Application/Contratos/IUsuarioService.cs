using System.Threading.Tasks;
using TesteConfitec.Application.Dtos;

namespace TesteConfitec.Application.Contratos
{
    public interface IUsuarioService
    {
        Task<UsuarioDto> AddUsuarios(UsuarioDto model);
        Task<UsuarioDto> UpdateUsuario(int usuarioId, UsuarioDto model);
        Task<bool> DeleteUsuario(int usuarioId);

        Task<UsuarioDto[]> GetAllUsuariosAsync();
        Task<UsuarioDto[]> GetAllUsuariosByNomeAsync(string nome);
        Task<UsuarioDto> GetUsuarioByIdAsync(int usuarioId);
    }
}