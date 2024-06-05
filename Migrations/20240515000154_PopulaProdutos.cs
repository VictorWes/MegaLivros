using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MegaLivros.Migrations
{
    /// <inheritdoc />
    public partial class PopulaProdutos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder mb)
        {
            mb.Sql("Insert into Produtos(NomeLivro,CategoriaLivro,ValorLivro,QteLivro,CategoriaId)" +
             "Values('Baladeira','Ficcao',30,50,2)");
            mb.Sql("Insert into Produtos(NomeLivro,CategoriaLivro,ValorLivro,QteLivro,CategoriaId)" +
               "Values('A Farsa','Ficcao',30,50,2)");
            mb.Sql("Insert into Produtos(NomeLivro,CategoriaLivro,ValorLivro,QteLivro,CategoriaId)" +
               "Values('The Sert','Romance',30,50,3)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder mb)
        {
            mb.Sql("Detele from Produtos");
        }
    }
}
