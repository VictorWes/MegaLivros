using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace MegaLivros.Models;

public class ProdutosModel
{
    [Key]
    [JsonIgnore]
    public int ProdutoId { get; set; }

    public string? NomeLivro { get; set; }
    public string? CategoriaLivro { get; set; }
    public int ValorLivro { get; set; }
    public int QteLivro { get; set; }

    public int CategoriaId { get; set; }


}
