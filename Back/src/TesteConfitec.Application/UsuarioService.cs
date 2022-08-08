using System;
using System.Threading.Tasks;
using AutoMapper;
using TesteConfitec.Application.Contratos;
using TesteConfitec.Application.Dtos;
using TesteConfitec.Domain;
using TesteConfitec.Persistence.Contratos;

namespace TesteConfitec.Application
{
    public class UsuarioService : IUsuarioService
    {
        private readonly IGeralPersist _geralPersist;
        private readonly IUsuarioPersist _usuarioPersist;
        private readonly IMapper _mapper;
        public UsuarioService(IGeralPersist geralPersist,
                             IUsuarioPersist usuarioPersist,
                             IMapper mapper)
        {
            _geralPersist = geralPersist;
            _usuarioPersist = usuarioPersist;
            _mapper = mapper;
        }
    public async Task<UsuarioDto> AddUsuarios(UsuarioDto model)
    {
        try
        {
            var usuario = _mapper.Map<Usuario>(model);

            _geralPersist.Add<Usuario>(usuario);

            if (await _geralPersist.SaveChangesAsync())
            {
                var usuarioRetorno = await _usuarioPersist.GetUsuarioByIdAsync(usuario.Id);

                return _mapper.Map<UsuarioDto>(usuarioRetorno);
            }
            return null;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public async Task<UsuarioDto> UpdateUsuario(int usuarioId, UsuarioDto model)
    {
        try
        {
            var usuario = await _usuarioPersist.GetUsuarioByIdAsync(usuarioId);
            if (usuario == null) return null;

            model.Id = usuario.Id;

            _mapper.Map(model, usuario);

            _geralPersist.Update<Usuario>(usuario);

            if (await _geralPersist.SaveChangesAsync())
            {
                var usuarioRetorno = await _usuarioPersist.GetUsuarioByIdAsync(usuario.Id);

                return _mapper.Map<UsuarioDto>(usuarioRetorno);
            }
            return null;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public async Task<bool> DeleteUsuario(int usuarioId)
    {
        try
        {
            var usuario = await _usuarioPersist.GetUsuarioByIdAsync(usuarioId);
            if (usuario == null) throw new Exception("Usuario para delete não encontrado.");

            _geralPersist.Delete<Usuario>(usuario);
            return await _geralPersist.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public async Task<UsuarioDto[]> GetAllUsuariosAsync()
    {
        try
        {
            var usuarios = await _usuarioPersist.GetAllUsuariosAsync();
            if (usuarios == null) return null;

            var resultado = _mapper.Map<UsuarioDto[]>(usuarios);

            return resultado;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public async Task<UsuarioDto[]> GetAllUsuariosByNomeAsync(string nome)
    {
        try
        {
            var usuarios = await _usuarioPersist.GetAllUsuariosByNomeAsync(nome);
            if (usuarios == null) return null;

            var resultado = _mapper.Map<UsuarioDto[]>(usuarios);

            return resultado;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public async Task<UsuarioDto> GetUsuarioByIdAsync(int usuarioId)
    {
        try
        {
            var usuario = await _usuarioPersist.GetUsuarioByIdAsync(usuarioId);
            if (usuario == null) return null;

            var resultado = _mapper.Map<UsuarioDto>(usuario);

            return resultado;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
}
}