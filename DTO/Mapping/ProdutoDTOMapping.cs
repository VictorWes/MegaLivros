using AutoMapper;
using MegaLivros.Models;

namespace MegaLivros.DTO.Mapping;

public class ProdutoDTOMapping : Profile
{
    public ProdutoDTOMapping()
    {
        CreateMap<ProdutosModel, ProdutoDTO>().ReverseMap();
        CreateMap<ProdutosModel, ProdutoDTOUpdateRequest>().ReverseMap();
        CreateMap<ProdutosModel, ProdutoDTOUpdateResponse>().ReverseMap();
    }
}
