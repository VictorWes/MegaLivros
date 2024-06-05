using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MegaLivros.Migrations
{
    /// <inheritdoc />
    public partial class MigrationInicial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder mb)
        {
            mb.Sql("Insert into Categorias(NomeCategoria,Valor) Values('Acao',0)");
            mb.Sql("Insert into Categorias(NomeCategoria,Valor) Values('Ficcao',0)");
            mb.Sql("Insert into Categorias(NomeCategoria,Valor) Values('Romance',0)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder mb)
        {
            mb.Sql("Detele from Categorias");
        }
    }
}
