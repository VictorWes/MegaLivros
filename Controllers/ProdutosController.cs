using AutoMapper;
using MegaLivros.Context;
using MegaLivros.DTO;
using MegaLivros.Models;
using MegaLivros.Pagination;
using MegaLivros.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using X.PagedList;
using Microsoft.AspNetCore.Http;

namespace MegaLivros.Controllers;

[Route("[controller]")]
[ApiController]
[EnableRateLimiting("fixedwindow")]
[ApiConventionType(typeof(DefaultApiConventions))]
public class ProdutosController : ControllerBase
{

    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    public ProdutosController(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }
    ///<summary>
    ///Obtem uma lista de produtos pesquisando por categorias
    /// </summary>
    /// <returns>Uma lista de Produtos</returns>
    [HttpGet("produtos/{id}")]

    public async Task<ActionResult<IEnumerable<ProdutoDTO>>> GetProdutoCategorias(int id)
    {
        var produtos = await _unitOfWork.ProdutoRepository.GetAsync(i => i.ProdutoId == id);
        if (produtos is null)
        {
            return NotFound("Produtos não localizados tente outro ID de categoria");
        }

        var produtoDTO = _mapper.Map<ProdutoDTO>(produtos);

        return Ok(produtos);
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    [ProducesDefaultResponseType]
    public async Task<ActionResult<IEnumerable<ProdutoDTO>>> Get()
    {

       var TodosProdutos = await _unitOfWork.ProdutoRepository.GetAllAsync();
        if (TodosProdutos is null)
        {
            return NotFound("Produtos não localizados");
        }

        var produtoDTO = _mapper.Map<IEnumerable<ProdutoDTO>>(TodosProdutos);
        return Ok(produtoDTO);


    }

    [HttpGet("{id:int}", Name = "ObterProduto")]
    

    public async Task<ActionResult<ProdutoDTO>> Get(int id)
    {
      var ProdutoId = await _unitOfWork.ProdutoRepository.GetAsync(i=> i.ProdutoId==id);
        if (ProdutoId is null)
        {
            return NotFound("Produto não localizado");
        }

        var produtoDTO = _mapper.Map<ProdutoDTO>(ProdutoId);
        return Ok(produtoDTO);

    
    }
    [HttpGet("pagination")]
    public async Task<ActionResult<IEnumerable<ProdutoDTO>>> Get([FromQuery] QueryStringParameters parameters)
    {
        var produtos = await _unitOfWork.ProdutoRepository.GetProdutosAsync(parameters);

   return ObterProdutos(produtos);
    }   

    [HttpGet("filter/preco/pagination")]
    public async Task<ActionResult<IEnumerable<ProdutoDTO>>> GetProdutosFilterPreco([FromQuery] ProdutosFiltroPreco ProdutosFiltroParams)
    {
        var produtos = await _unitOfWork.ProdutoRepository.GetProdutosFiltrosPrecoAsync(ProdutosFiltroParams);

        return ObterProdutos(produtos);
    }

    private ActionResult<IEnumerable<ProdutoDTO>> ObterProdutos(IPagedList<ProdutosModel> produtos)
    {
        var metadados = new
        {
            produtos.Count,
            produtos.PageSize,
            produtos.PageCount,
            produtos.TotalItemCount,
            produtos.HasNextPage,
            produtos.HasPreviousPage
        };
        Response.Headers.Append("X-Pagination", JsonConvert.SerializeObject(metadados));

        var produtosDto = _mapper.Map<IEnumerable<ProdutoDTO>>(produtos);

        return Ok(produtosDto);
    }

    [HttpPost]
    [Authorize(Policy = "AdminOnly")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    [ProducesDefaultResponseType]
    public async Task<ActionResult<CategoriaDTO>> Post(ProdutoDTO CategoriaDtoproduto)

    {
        if (CategoriaDtoproduto is null)
        {
            return BadRequest();
        }

        var atualizarProdutoDto = _mapper.Map<ProdutosModel>(CategoriaDtoproduto);
        var CriarProduto = _unitOfWork.ProdutoRepository.Create(atualizarProdutoDto);
       await _unitOfWork.CommitAsync();
       
        var responseDTO = _mapper.Map<ProdutoDTO>(CriarProduto);
       
        return Ok(responseDTO);

    }

    [HttpPut("{id:int}")]
    [Authorize(Policy = "AdminOnly")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    [ProducesDefaultResponseType]
    public async Task<ActionResult<ProdutoDTO>> Put(int id, ProdutoDTO produtoDto)
    {
        
        if (id != produtoDto.ProdutoId)
        {
            return BadRequest();
        }
        var requestDtoPut = _mapper.Map<ProdutosModel>(produtoDto);

        var produtoDtoAtualizado = _unitOfWork.ProdutoRepository.Update(requestDtoPut);

        await _unitOfWork.CommitAsync();

        var responsePutDto = _mapper.Map<ProdutoDTO>(produtoDtoAtualizado);
        return Ok(responsePutDto);

    }
    [HttpDelete("{id:int}")]
    [Authorize(Policy = "AdminOnly")]
    public async Task<ActionResult<ProdutoDTO>> Delete(int id)
    {
        var FindId = await _unitOfWork.ProdutoRepository.GetAsync(i => i.ProdutoId == id);
        if (FindId is null)
        {
            return BadRequest("Id não localizado");
        }
        var Delete = _unitOfWork.ProdutoRepository.Delete(FindId);
       await  _unitOfWork.CommitAsync();

        var responseDto = _mapper.Map<ProdutoDTO>(Delete);

        return Ok(responseDto);
    }
    [HttpPatch("{id}/UpdatePartial")]
    [Authorize(Policy = "AdminOnly")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    [ProducesDefaultResponseType]
    public async Task<ActionResult<ProdutoDTOUpdateResponse>> Patch(int id, JsonPatchDocument<ProdutoDTOUpdateRequest> patchProdutoDTO)
    {
        if (patchProdutoDTO is null || id <= 0)
            return BadRequest("Errad");
      
        var produto = await _unitOfWork.ProdutoRepository.GetAsync(c=> c.ProdutoId==id);

        if (produto is null)
            return NotFound("errox");
        
        var produtoUpdateRequest = _mapper.Map<ProdutoDTOUpdateRequest>(produto);

        patchProdutoDTO.ApplyTo(produtoUpdateRequest,ModelState);

        if (!ModelState.IsValid || !TryValidateModel(produtoUpdateRequest))
        {
            return BadRequest(ModelState);
        }   


        _mapper.Map(produtoUpdateRequest, produto);

        _unitOfWork.ProdutoRepository.Update(produto);
        await _unitOfWork.CommitAsync();

        return Ok(_mapper.Map<ProdutoDTOUpdateResponse>(produto));
    }
}

 
