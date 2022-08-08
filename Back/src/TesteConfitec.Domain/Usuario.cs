using System;
using System.Collections.Generic;
using TesteConfitec.Domain.Enum;

namespace TesteConfitec.Domain
{
    public class Usuario
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Sobrenome { get; set; }
        public string Email { get; set; }
        public DateTime DataNascimento { get; set; }
        public Escolaridade EscolaridadeId { get; set; }
        public IEnumerable<HistoricoEscolar> HistoricosEscolares { get; set; }
    }
}
