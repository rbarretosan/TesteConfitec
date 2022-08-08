using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TesteConfitec.Domain
{
    public class HistoricoEscolar
    {
        public int Id { get; set; }
        public string Formato { get; set; }
        public string Nome { get; set; }
        public string arquivoURL { get; set; }
        public int UsuarioId { get; set; }
        public Usuario Usuario { get; set; }
    }
}