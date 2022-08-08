using System.Threading.Tasks;
using TesteConfitec.Domain;

namespace TesteConfitec.Persistence.Contratos
{
    public interface IUsuarioPersist
    {
        Task<Usuario[]> GetAllUsuariosByNomeAsync(string nome);
        Task<Usuario[]> GetAllUsuariosAsync();
        Task<Usuario> GetUsuarioByIdAsync(int nome);
    }
}