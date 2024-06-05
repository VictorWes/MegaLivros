using MegaLivros.Context;
using MegaLivros.Models;
using Microsoft.EntityFrameworkCore;

namespace MegaLivros.Repositories;
public class CategoriaRepositore : Repositore<CategoriaModel>, ICategoriaRepositore
{
    public CategoriaRepositore(MegaLivrosContext context) : base(context)   
    {
        
    }
}

