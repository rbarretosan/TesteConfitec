using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using TesteConfitec.Application.Contratos;
using TesteConfitec.Application.Dtos;
using TesteConfitec.Domain;
using TesteConfitec.Persistence.Contratos;

namespace TesteConfitec.Application
{
    public class HistoricoEscolarService : IHistoricoEscolarService
    {
        private readonly IHistoricoEscolarPersist _historicoEscolarPersist;
        private readonly IMapper _mapper;

        public HistoricoEscolarService(IHistoricoEscolarPersist historicoEscolarPersist,
                           IMapper mapper)
        {
            _historicoEscolarPersist = historicoEscolarPersist;
            _mapper = mapper;
        }

        public async Task AddHistoricoEscolar(int usuarioId, HistoricoEscolarDto model)
        {
            try
            {
                var historicoEscolar = _mapper.Map<HistoricoEscolar>(model);
                historicoEscolar.UsuarioId = usuarioId;

                _historicoEscolarPersist.Add<HistoricoEscolar>(historicoEscolar);

                await _historicoEscolarPersist.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<HistoricoEscolarDto[]> SaveHistoricosEscolares(int usuarioId, HistoricoEscolarDto[] models)
        {
            try
            {
                var historicoEscolars = await _historicoEscolarPersist.GetHistoricosEscolaresByUsuarioIdAsync(usuarioId);
                if (historicoEscolars == null) return null;

                foreach (var model in models)
                {
                    if (model.Id == 0)
                    {
                        await AddHistoricoEscolar(usuarioId, model);
                    }
                    else
                    {
                        var historicoEscolar = historicoEscolars.FirstOrDefault(historicoEscolar => historicoEscolar.Id == model.Id);
                        model.UsuarioId = usuarioId;

                        _mapper.Map(model, historicoEscolar);

                        _historicoEscolarPersist.Update<HistoricoEscolar>(historicoEscolar);

                        await _historicoEscolarPersist.SaveChangesAsync();
                    }
                }

                var historicoEscolarRetorno = await _historicoEscolarPersist.GetHistoricosEscolaresByUsuarioIdAsync(usuarioId);

                return _mapper.Map<HistoricoEscolarDto[]>(historicoEscolarRetorno);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> DeleteHistoricoEscolar(int usuarioId, int historicoEscolarId)
        {
            try
            {
                var historicoEscolar = await _historicoEscolarPersist.GetHistoricoEscolarByIdsAsync(usuarioId, historicoEscolarId);
                if (historicoEscolar == null) throw new Exception("HistoricoEscolar para delete não encontrado.");

                _historicoEscolarPersist.Delete<HistoricoEscolar>(historicoEscolar);
                return await _historicoEscolarPersist.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<HistoricoEscolarDto[]> GetHistoricosEscolaresByUsuarioIdAsync(int usuarioId)
        {
            try
            {
                var historicoEscolars = await _historicoEscolarPersist.GetHistoricosEscolaresByUsuarioIdAsync(usuarioId);
                if (historicoEscolars == null) return null;

                var resultado = _mapper.Map<HistoricoEscolarDto[]>(historicoEscolars);

                return resultado;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<HistoricoEscolarDto> GetHistoricoEscolarByIdsAsync(int usuarioId, int historicoEscolarId)
        {
            try
            {
                var historicoEscolar = await _historicoEscolarPersist.GetHistoricoEscolarByIdsAsync(usuarioId, historicoEscolarId);
                if (historicoEscolar == null) return null;

                var resultado = _mapper.Map<HistoricoEscolarDto>(historicoEscolar);

                return resultado;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<HistoricoEscolarDto> UpdateHistoricoEscolar(int usuarioId, int historicoEscolarId, HistoricoEscolarDto model)
        {

        try
        {
            var historicoEscolar = await _historicoEscolarPersist.GetHistoricoEscolarByIdsAsync(usuarioId, historicoEscolarId);
            if (historicoEscolar == null) return null;

            model.Id = historicoEscolar.Id;

            _mapper.Map(model, historicoEscolar);

            _historicoEscolarPersist.Update<HistoricoEscolar>(historicoEscolar);

            if (await _historicoEscolarPersist.SaveChangesAsync())
            {
                var historicoEscolarRetorno = await _historicoEscolarPersist.GetHistoricoEscolarByIdsAsync(usuarioId, historicoEscolarId);

                return _mapper.Map<HistoricoEscolarDto>(historicoEscolarRetorno);
            }
            return null;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }

        }
    }
}