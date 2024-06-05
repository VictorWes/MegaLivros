using AutoMapper;
using MegaLivros.Context;
using MegaLivros.DTO;
using MegaLivros.Models;
using MegaLivros.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MegaLivros.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [ApiConventionType(typeof(DefaultApiConventions))]
    public class CategoriasController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;


        public CategoriasController(IUnitOfWork unitOfWork, IMapper mapper)
        {

            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpPost]
        [Authorize(Policy = "AdminOnly")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<CategoriaDTO>> Post(CategoriaDTO categoriaDto)
        {
            if (categoriaDto is null)
            {
                return BadRequest("Por favor preencha corretamente.");
            }

            var categoriaRequest = _mapper.Map<CategoriaModel>(categoriaDto);

            var CreateCategoria = _unitOfWork.CategoriaRepository.Create(categoriaRequest);
            await _unitOfWork.CommitAsync();

            var categoriaRepost = _mapper.Map<CategoriaDTO>(CreateCategoria);
         

            return  Ok(categoriaRepost);
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<IEnumerable<CategoriaDTO>>> Get()
        {
           
           var categorias =  await _unitOfWork.CategoriaRepository.GetAllAsync();
            if (categorias is null)
                return NotFound("Não localizado");
            var respostDto = _mapper.Map<IEnumerable<CategoriaDTO>>(categorias);


            return Ok(respostDto);

           
        }

        [HttpGet("{id:int}", Name = "ObterCategoria")]

        public async Task<ActionResult<CategoriaDTO>> GetIdAsync(int id)
        {
            var categoria = await _unitOfWork.CategoriaRepository.GetAsync(c => c.CategoriaId == id);

            if (categoria is null)
            {
                return NotFound("Id não localizado");
            }

            var destinoId = _mapper.Map<CategoriaDTO>(categoria);
            return Ok(destinoId);
        }

        [HttpDelete("{id:int}")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<ActionResult<CategoriaDTO>> Delete(int id)
        {
            var FindId = await _unitOfWork.CategoriaRepository.GetAsync(c=> c.CategoriaId ==id);
            if (FindId is null)
            {
                return BadRequest("Id não localizado");
            }
            var Delete = _unitOfWork.CategoriaRepository.Delete(FindId);
           await _unitOfWork.CommitAsync();

            var requestDeleteDTO = _mapper.Map<CategoriaDTO>(Delete);

            return Ok(requestDeleteDTO);
        }
        [HttpPut]
        [Authorize(Policy = "AdminOnly")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<CategoriaDTO>> Put(int id, CategoriaDTO categoriaDto) 
        { 
        
            if(id != categoriaDto.CategoriaId)
            {
                return BadRequest("Produto não localizado");
            }
            var requestDtoPut = _mapper.Map<CategoriaModel>(categoriaDto);
          
            var produtoDtoAtualizado = _unitOfWork.CategoriaRepository.Update(requestDtoPut);

           await _unitOfWork.CommitAsync();

            var responsePutDto = _mapper.Map<CategoriaDTO>(produtoDtoAtualizado);
            return Ok(responsePutDto);

        }

    }
    }
