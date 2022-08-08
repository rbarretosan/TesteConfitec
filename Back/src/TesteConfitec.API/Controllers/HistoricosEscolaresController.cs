using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TesteConfitec.Application.Contratos;
using Microsoft.AspNetCore.Http;
using TesteConfitec.Application.Dtos;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using System.Linq;
using System.Collections.Generic;

namespace TesteConfitec.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HistoricosEscolaresController : ControllerBase
    {
        private readonly IHistoricoEscolarService _historicoEscolarService;
        IWebHostEnvironment _hostEnvironment;

        public HistoricosEscolaresController(IHistoricoEscolarService HistoricoEscolarService, IWebHostEnvironment hostEnvironment)
        {
            _historicoEscolarService = HistoricoEscolarService;
            _hostEnvironment = hostEnvironment;
        }

        [HttpGet("{usuarioId}")]
        public async Task<IActionResult> Get(int usuarioId)
        {
            try
            {
                var historicoEscolars = await _historicoEscolarService.GetHistoricosEscolaresByUsuarioIdAsync(usuarioId);
                if (historicoEscolars == null) return NoContent();

                return Ok(historicoEscolars);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                    $"Erro ao tentar recuperar historicoEscolars. Erro: {ex.Message}");
            }
        }

        [HttpPut("{usuarioId}")]
        public async Task<IActionResult> SaveHistoricoEscolars(int usuarioId, HistoricoEscolarDto[] models)
        {            
            try
            {
                var historicoEscolars = await _historicoEscolarService.SaveHistoricosEscolares(usuarioId, models);
                if (historicoEscolars == null) return NoContent();

                return Ok(historicoEscolars);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                    $"Erro ao tentar salvar historicoEscolars. Erro: {ex.Message}");
            }
        }

        [HttpDelete("{usuarioId}/{historicoEscolarId}")]
        public async Task<IActionResult> Delete(int usuarioId, int historicoEscolarId)
        {
            try
            {
                var historicoEscolar = await _historicoEscolarService.GetHistoricoEscolarByIdsAsync(usuarioId, historicoEscolarId);
                if (historicoEscolar == null) return NoContent();

                var retornoDeleteHistoricoEscolar = await _historicoEscolarService.DeleteHistoricoEscolar(historicoEscolar.UsuarioId, historicoEscolar.Id);

                if (retornoDeleteHistoricoEscolar){
                    if (historicoEscolar.arquivoURL != null)
                        DeleteFile(historicoEscolar.arquivoURL);
                    return Ok(new { message = "HistoricoEscolar Deletado" }); 
                }else
                    throw new Exception("Ocorreu um problem não específico ao tentar deletar HistoricoEscolar.");
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                    $"Erro ao tentar deletar historicoEscolars. Erro: {ex.Message}");
            }
        }

        [HttpPost("upload-file")]
        public async Task<IActionResult> UploadFile(int usuarioId, int historicoEscolarId, IFormFile file)
        {
            try
            {
                var historicoEscolar = await _historicoEscolarService.GetHistoricoEscolarByIdsAsync(usuarioId, historicoEscolarId);
                if (historicoEscolar == null) return NoContent();

                if (file.Length > 0){
                    if (historicoEscolar.arquivoURL != null)
                        DeleteFile(historicoEscolar.arquivoURL);
                    historicoEscolar.arquivoURL = await SaveFile(file);
                    historicoEscolar.Formato = Path.GetExtension(file.FileName);
                }

                var historicoEscolarRetorno = await _historicoEscolarService.UpdateHistoricoEscolar(usuarioId, historicoEscolarId, historicoEscolar);

                return Ok(historicoEscolarRetorno);

            }catch(Exception ex){
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                $"Erro ao tentar fazer upload do arquivo. Erro {ex.Message}");
            }
        }

        [NonAction]
        public async Task<string> SaveFile(IFormFile file){
            string fileName = new String(Path.GetFileNameWithoutExtension(file.FileName)
                                             .Take(20)
                                             .ToArray()
                                             ).Replace(" ", "-");
            
            fileName = $"{fileName}{DateTime.UtcNow.ToString("yymmssfff")}{Path.GetExtension(file.FileName)}";

            var filePath = Path.Combine(_hostEnvironment.ContentRootPath, @"Resources\\files", fileName);

            using(var fileStream = new FileStream(filePath, FileMode.Create)){
                await file.CopyToAsync(fileStream);
            }

            return fileName;
        }

        [NonAction]
        public void DeleteFile(string fileName){
            var filePath = Path.Combine(_hostEnvironment.ContentRootPath, @"Resources/files", fileName);
            if (System.IO.File.Exists(filePath)){
                System.IO.File.Delete(filePath);
            }
        }
    }
}
