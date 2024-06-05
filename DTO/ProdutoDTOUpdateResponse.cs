using System.ComponentModel.DataAnnotations;

namespace MegaLivros.DTO;

public class ProdutoDTOUpdateResponse
{
    public int ProdutoId { get; set; }

    public string? NomeLivro { get; set; }
    public string? CategoriaLivro { get; set; }
    public int ValorLivro { get; set; }
    public int QteLivro { get; set; }
    public int CategoriaId { get; set; }
}
