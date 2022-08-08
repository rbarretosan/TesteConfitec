namespace TesteConfitec.Application.Dtos
{
    public class HistoricoEscolarDto
    {
        public int Id { get; set; }
        public string Formato { get; set; }
        public string Nome { get; set; }
        public string arquivoURL { get; set; }
        public int UsuarioId { get; set; }
        // public UsuarioDto Usuario { get; set; }
    }
}