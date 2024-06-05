using AutoMapper;
using MegaLivros.Models;
using System.Security.Cryptography.X509Certificates;

namespace MegaLivros.DTO.Mapping;

public class CategoriaDTOMapping : Profile
{
    public CategoriaDTOMapping()
    {
        CreateMap<CategoriaModel, CategoriaDTO>().ReverseMap();
    }
}
