using L01_2020VD601.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace L01_2020VD601.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class usuariosController : ControllerBase
    {
        private readonly blogContext _blogContexto;

        public usuariosController(blogContext blogContexto)
        {
            _blogContexto = blogContexto;
        }

        [HttpGet]
        [Route("GetAll")]
        public IActionResult Get()
        {
            List<usuarios> usuarioslst = (from e in _blogContexto.usuarios
                                          select e).ToList();
            if (usuarioslst.Count == 0)
            {
                return NotFound();
            }

            return Ok(usuarioslst);
        }

        [HttpGet]
        [Route("GetbyId/{id}")]
        public IActionResult Get(int id)
        {
            usuarios? comentario = (from e in _blogContexto.usuarios
                                          where e.usuarioId == id
                                          select e).FirstOrDefault();
            if (comentario == null)
            {
                return NotFound();
            }

            return Ok(comentario);
        }

        [HttpPost]
        [Route("Add")]
        public IActionResult GuardarUsuario([FromBody] usuarios usuario) {
            try
            {
                _blogContexto.usuarios.Add(usuario);
                _blogContexto.SaveChanges();
                return Ok(usuario);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("actualizar/{id}")]
        public IActionResult ActualizarUsuario(int id, [FromBody] usuarios usuario)
        {
            usuarios? usuarioActual = (from e in _blogContexto.usuarios
                                       where e.usuarioId == id
                                       select e).FirstOrDefault();

            if(usuarioActual == null) { return NotFound(); }

            usuarioActual.rolId = usuario.rolId;
            usuarioActual.nombreUsuario = usuario.nombreUsuario;
            usuarioActual.clave = usuario.clave;
            usuarioActual.nombre = usuario.nombre;
            usuarioActual.apellido = usuario.apellido;

            _blogContexto.Entry(usuarioActual).State = EntityState.Modified;
            _blogContexto.SaveChanges();
            return Ok(usuarioActual);  
        }

        [HttpDelete]
        [Route("eliminar/{id}")]

        public IActionResult BorrarUsuario(int id)
        {
            usuarios? usuario = (from e in _blogContexto.usuarios
                                       where e.usuarioId == id
                                       select e).FirstOrDefault();
            if (usuario == null) {
                return NotFound();
            }
            _blogContexto.Attach(usuario);
            _blogContexto.Remove(usuario);
            _blogContexto.SaveChanges();
            return Ok(usuario);
        }

        [HttpGet]
        [Route("findbyParameters/{nombre}&{apellido}&{rol}")]

        public IActionResult finder(string nombre, string apellido, int rol)
        {
            List<usuarios> usuarioslst = (from e in _blogContexto.usuarios
                                          where e.nombre.Contains(nombre) && e.apellido.Contains(apellido)
                                          && e.rolId == rol
                                          select e).ToList();
            if(usuarioslst == null)
            {
                return NotFound();
            }
            return Ok(usuarioslst);
        }
    }
}
