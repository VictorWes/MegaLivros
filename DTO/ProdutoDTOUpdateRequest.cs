using System.ComponentModel.DataAnnotations;

namespace MegaLivros.DTO
{
    public class ProdutoDTOUpdateRequest
    {
        [Range(1,999, ErrorMessage = "Estoque deve estar entre 1 e 999")]
        public int QteLivro { get; set; }
    }
}
