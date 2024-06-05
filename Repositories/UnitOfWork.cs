using MegaLivros.Context;

namespace MegaLivros.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private IProdutoRepositore _produtoRepo;

    private ICategoriaRepositore _categoriaRepo;
    public MegaLivrosContext _context;

    public UnitOfWork(MegaLivrosContext context)
    {
        _context = context;
    }
    
    public IProdutoRepositore ProdutoRepository
    {
        get 
        {
            return _produtoRepo = _produtoRepo ?? new ProdutoRepositore(_context);
                
        }
    }

    public ICategoriaRepositore CategoriaRepository
    {
        get
        {
            return _categoriaRepo = _categoriaRepo ?? new CategoriaRepositore(_context);

        }
    }
    public async Task CommitAsync()
    {
        await _context.SaveChangesAsync();
    }
    public void Dispose()
    {
        _context?.Dispose();
    }

}
