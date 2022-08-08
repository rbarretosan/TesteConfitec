using System;
using System.Collections.Generic;
using TesteConfitec.Application.Dtos.Enum;

namespace TesteConfitec.Application.Dtos
{
    public class UsuarioDto
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Sobrenome { get; set; }
        public string Email { get; set; }
        public DateTime DataNascimento { get; set; }
        public EscolaridadeDto EscolaridadeId { get; set; }
        public IEnumerable<HistoricoEscolarDto> HistoricosEscolares { get; set; }
    }
}