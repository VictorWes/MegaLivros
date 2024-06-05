using MegaLivros.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace MegaLivros.Context;

public class MegaLivrosContext : IdentityDbContext<AplicationUser>
{
    public MegaLivrosContext(DbContextOptions<MegaLivrosContext> options) : base(options)
    {

    }

    public DbSet<CategoriaModel> Categorias { get; set; }
    public DbSet<ProdutosModel> Produtos { get; set; }
    public DbSet<UsuarioModel> Usuarios { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
    }

}
