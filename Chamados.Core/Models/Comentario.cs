namespace Chamados.Core.Models
{
    public class Comentario
    {
        public int Id { get; set; }
        public int ChamadoId { get; set; }
        public int UsuarioId { get; set; }
        public string? Texto { get; set; }
        public DateTime DataCriacao { get; set; }
        public string? NomeUsuario { get; set; } // Para exibição
    }
}
