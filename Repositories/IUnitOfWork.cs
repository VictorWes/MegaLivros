namespace MegaLivros.Repositories;

public interface IUnitOfWork
{
    IProdutoRepositore ProdutoRepository { get; }
    ICategoriaRepositore CategoriaRepository { get; }

   Task CommitAsync();
}
