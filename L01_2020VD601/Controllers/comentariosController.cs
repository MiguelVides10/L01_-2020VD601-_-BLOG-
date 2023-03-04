using L01_2020VD601.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace L01_2020VD601.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class comentariosController : ControllerBase
    {
        public readonly blogContext _blogContext;

        public comentariosController(blogContext blogContext)
        {
            _blogContext = blogContext;
        }

        [HttpGet]
        [Route("GetAll")]
        public IActionResult Get()
        {
            List<comentarios> comentarioslst = (from e in _blogContext.comentarios
                                                      select e).ToList();
            if (comentarioslst.Count == 0)
            {
                return NotFound();
            }

            return Ok(comentarioslst);
        }

        [HttpGet]
        [Route("GetbyId/{id}")]
        public IActionResult Get(int id)
        {
            comentarios? comentario = (from e in _blogContext.comentarios
                                      where e.cometarioId== id select e).FirstOrDefault();
            if (comentario== null)
            {
                return NotFound();
            }

            return Ok(comentario);
        }

        [HttpPost]
        [Route("Add")]
        public IActionResult GuardarComentario([FromBody] comentarios comentario)
        {
            try
            {
                _blogContext.comentarios.Add(comentario);
                _blogContext.SaveChanges();
                return Ok(comentario);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("actualizar/{id}")]
        public IActionResult ActualizarCalificacion(int id, [FromBody] comentarios comentario)
        {
            comentarios? comentarioActual = (from e in _blogContext.comentarios
                                                  where e.cometarioId == id
                                                  select e).FirstOrDefault();

            if (comentarioActual == null) { return NotFound(); }
            comentarioActual.publicacionId = comentario.publicacionId;
            comentarioActual.comentario = comentario.comentario;
            comentarioActual.usuarioId = comentario.usuarioId;

            _blogContext.Entry(comentarioActual).State = EntityState.Modified;
            _blogContext.SaveChanges();
            return Ok(comentarioActual);
        }

        [HttpDelete]
        [Route("eliminar/{id}")]
        public IActionResult BorrarComentario(int id)
        {
            comentarios? comentario = (from e in _blogContext.comentarios
                                            where e.cometarioId == id
                                            select e).FirstOrDefault();
            if (comentario == null)
            {
                return NotFound();
            }
            _blogContext.Attach(comentario);
            _blogContext.Remove(comentario);
            _blogContext.SaveChanges();
            return Ok(comentario);
        }

        [HttpGet]
        [Route("filterbyUser/{usuarioId}")]

        public IActionResult filterByUserId(int usuarioId)
        {
            List<comentarios> comentarioslst = (from e in _blogContext.comentarios
                                                      where e.usuarioId == usuarioId
                                                      select e).ToList();

            if (comentarioslst == null)
            {
                return NotFound();
            }
            return Ok(comentarioslst);
        }
    }
}
