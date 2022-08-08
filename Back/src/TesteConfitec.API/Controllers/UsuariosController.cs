using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TesteConfitec.Application.Contratos;
using Microsoft.AspNetCore.Http;
using TesteConfitec.Application.Dtos;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using System.Linq;

namespace TesteConfitec.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsuariosController : ControllerBase
    {
        private readonly IUsuarioService _usuarioService;
        private readonly IWebHostEnvironment _hostEnvironment;

        public UsuariosController(IUsuarioService usuarioService, IWebHostEnvironment hostEnvironment)
        {
            _hostEnvironment = hostEnvironment;
            _usuarioService = usuarioService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var usuarios = await _usuarioService.GetAllUsuariosAsync();
                if (usuarios == null) return NoContent();

                return Ok(usuarios);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                    $"Erro ao tentar recuperar usuarios. Erro: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var usuario = await _usuarioService.GetUsuarioByIdAsync(id);
                if (usuario == null) return NoContent();

                return Ok(usuario);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                    $"Erro ao tentar recuperar usuarios. Erro: {ex.Message}");
            }
        }

        [HttpGet("{nome}/nome")]
        public async Task<IActionResult> GetByNome(string nome)
        {
            try
            {
                var usuario = await _usuarioService.GetAllUsuariosByNomeAsync(nome);
                if (usuario == null) return NoContent();

                return Ok(usuario);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                    $"Erro ao tentar recuperar usuarios. Erro: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post(UsuarioDto model)
        {
            try
            {
                var usuario = await _usuarioService.AddUsuarios(model);
                if (usuario == null) return NoContent();

                return Ok(usuario);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                    $"Erro ao tentar adicionar usuarios. Erro: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, UsuarioDto model)
        {
            try
            {
                var usuario = await _usuarioService.UpdateUsuario(id, model);
                if (usuario == null) return NoContent();

                return Ok(usuario);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                    $"Erro ao tentar atualizar usuarios. Erro: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var usuario = await _usuarioService.GetUsuarioByIdAsync(id);
                if (usuario == null) return NoContent();

                if (await _usuarioService.DeleteUsuario(id))
                {
                    // DeleteImage(usuario.ImagemURL);
                    return Ok(new { message = "Deletado" });
                }
                else
                {
                    throw new Exception("Ocorreu um problem não específico ao tentar deletar Usuario.");
                }
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                    $"Erro ao tentar deletar usuarios. Erro: {ex.Message}");
            }
        }
    }
}
