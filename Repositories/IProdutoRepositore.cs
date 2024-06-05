using MegaLivros.Models;
using MegaLivros.Pagination;
using X.PagedList;

namespace MegaLivros.Repositories;

public interface IProdutoRepositore : IRepositore<ProdutosModel>
{
    Task<IPagedList<ProdutosModel>> GetProdutosAsync(QueryStringParameters parameters);
    Task<IPagedList<ProdutosModel>> GetProdutosFiltrosPrecoAsync(ProdutosFiltroPreco ProdutosFiltroParams);
    Task<IEnumerable<ProdutosModel>> GetProdutosPorCategoriaAsync(int id);
}
