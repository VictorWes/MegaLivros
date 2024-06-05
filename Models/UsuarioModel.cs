using System.ComponentModel.DataAnnotations;

namespace MegaLivros.Models
{
    public class UsuarioModel
    {
        [Key]
        public int IdUsuario { get; set; }
        public string? Nome { get; set; }
        public int Idade { get; set; }
        public string? Endereco { get; set; }
        public string? Senha { get; set; }
        public string? Email { get; set; }

        
       
    }
}
