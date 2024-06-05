using MegaLivros.Context;
using MegaLivros.Models;
using MegaLivros.Pagination;
using Microsoft.EntityFrameworkCore;
using X.PagedList;

namespace MegaLivros.Repositories;

public class ProdutoRepositore : Repositore<ProdutosModel>, IProdutoRepositore

{
    private readonly MegaLivrosContext _context;

    public ProdutoRepositore(MegaLivrosContext context) : base(context)
    {

    }

    public async Task<IEnumerable<ProdutosModel>> GetProdutosPorCategoriaAsync(int id)
    {
        var produtos = await GetAllAsync();

       var produtosCategorias = produtos.Where(c => c.CategoriaId == id);
        return produtosCategorias;
           
    }

    public async Task<IPagedList<ProdutosModel>> GetProdutosAsync(QueryStringParameters parameters)
    {
        var produtos = await GetAllAsync();
        
        var produtosOrdenados = produtos.OrderBy(p => p.ProdutoId).AsQueryable();

        var resultado = await produtosOrdenados.ToPagedListAsync(parameters.PageNumber, parameters.PageSize);
        return resultado;
    }

    public async Task<IPagedList<ProdutosModel>> GetProdutosFiltrosPrecoAsync(ProdutosFiltroPreco ProdutosFiltroParams)
    {
        var produtos = await GetAllAsync();

        if (ProdutosFiltroParams.Preco.HasValue && !string.IsNullOrEmpty(ProdutosFiltroParams.PrecoCriteiro))
        {
            if (ProdutosFiltroParams.PrecoCriteiro.Equals("maior", StringComparison.OrdinalIgnoreCase))
            {
                produtos = produtos.Where(p => p.ValorLivro > ProdutosFiltroParams.Preco.Value).OrderBy(p => p.ValorLivro);
            }
            else if (ProdutosFiltroParams.PrecoCriteiro.Equals("menor", StringComparison.OrdinalIgnoreCase))
            {
                produtos = produtos.Where(p => p.ValorLivro < ProdutosFiltroParams.Preco.Value).OrderBy(p => p.ValorLivro);
            }
            else if(ProdutosFiltroParams.PrecoCriteiro.Equals("igual", StringComparison.OrdinalIgnoreCase))
            {
                produtos = produtos.Where(p => p.ValorLivro == ProdutosFiltroParams.Preco.Value).OrderBy(p => p.ValorLivro);
            }
        }
        var produtosFiltrados = await produtos.ToPagedListAsync(ProdutosFiltroParams.PageNumber, ProdutosFiltroParams.PageSize);

        return produtosFiltrados;
    }
}
