using MegaLivros.Context;
using MegaLivros.Migrations;
using MegaLivros.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;

namespace MegaLivros.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [EnableRateLimiting("fixedwindow")]
    public class UsuarioController : ControllerBase
    {
      private readonly MegaLivrosContext _context;

        public UsuarioController(MegaLivrosContext context)
        {
            _context = context;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        public ActionResult Post(UsuarioModel usuario)
        {
            if (usuario == null)
            {
                return BadRequest("Preenchimento invalido");
            }
            _context.Usuarios.Add(usuario);
            _context.SaveChanges();

            return Ok(usuario);
        }
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public ActionResult<IEnumerable<UsuarioModel>> Get()
        {
            var resultado = _context.Usuarios.ToList();
            if (resultado is null)
            {
                return NotFound("Não existe usuarios no banco de dados");
            }
            return Ok(resultado);
        }
        [HttpGet("{id}:int")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public ActionResult<UsuarioModel> Get(int id)
        {
            var usuario = _context.Usuarios.FirstOrDefault(p => p.IdUsuario == id);

            if (usuario is null)
            {
                return NotFound("Usuario não localizado, por favor me informe outro");
            }
            return Ok(usuario);
        }
        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public ActionResult Delete(int id)
        {
            var usuario = _context.Usuarios.FirstOrDefault(p => p.IdUsuario == id);

            if (usuario is null)
            {
                return NotFound("Usuario Não identificado");
            }
            _context.Usuarios.Remove(usuario);
            _context.SaveChanges();
            return Ok("Usuario deletado");
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        public ActionResult Put(int id, UsuarioModel usuario)
        {
            if (id != usuario.IdUsuario)
            {
                return BadRequest("Id invalido");
            }
            _context.Entry(usuario).State = EntityState.Modified;
            _context.SaveChanges();
            return Ok(usuario);
        }

    }

   
}
