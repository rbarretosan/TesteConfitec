using System;
using AutoMapper;
using TesteConfitec.Application.Dtos;
using TesteConfitec.Domain;

namespace TesteConfitec.API.Helpers
{
    public class TesteConfitecProfile : Profile
    {
        public TesteConfitecProfile()
        {
            CreateMap<Usuario, UsuarioDto>().ReverseMap();
            CreateMap<HistoricoEscolar, HistoricoEscolarDto>().ReverseMap();			
        }
    }
}