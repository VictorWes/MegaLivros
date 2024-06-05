namespace MegaLivros.Pagination;

public class ProdutosFiltroPreco : QueryStringParameters
{
    public decimal? Preco { get; set; }
    public string? PrecoCriteiro { get; set; }

}
