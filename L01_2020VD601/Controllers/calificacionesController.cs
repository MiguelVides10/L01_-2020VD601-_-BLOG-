using L01_2020VD601.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace L01_2020VD601.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class calificacionesController : ControllerBase
    {
        private readonly blogContext _blogContexto;
        public calificacionesController(blogContext context) {
            _blogContexto = context;
        }

        [HttpGet]
        [Route("GetAllCali")]
        public IActionResult Get()
        {
            List<calificaciones> calificacioneslst = (from e in _blogContexto.calificaciones
                                          select e).ToList();
            if (calificacioneslst.Count == 0)
            {
                return NotFound();
            }

            return Ok(calificacioneslst);
        }

        [HttpPost]
        [Route("AddCali")]
        public IActionResult GuardarCalificacion([FromBody] calificaciones califiacion)
        {
            try
            {
                _blogContexto.calificaciones.Add(califiacion);
                _blogContexto.SaveChanges();
                return Ok(califiacion);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("actualizar/{id}")]
        public IActionResult ActualizarCalificacion(int id, [FromBody] calificaciones calificacion)
        {
            calificaciones? calificacionActual = (from e in _blogContexto.calificaciones
                                       where e.calificacionId == id
                                       select e).FirstOrDefault();

            if (calificacionActual == null) { return NotFound(); }
            calificacionActual.publicacionId = calificacion.publicacionId;
            calificacionActual.usuarioId = calificacion.usuarioId;
            calificacionActual.calificacion = calificacion.calificacion;

            _blogContexto.Entry(calificacionActual).State = EntityState.Modified;
            _blogContexto.SaveChanges();
            return Ok(calificacionActual);
        }

        [HttpDelete]
        [Route("eliminar/{id}")]

        public IActionResult BorrarCalificaion(int id)
        {
            calificaciones? calificacion = (from e in _blogContexto.calificaciones
                                 where e.calificacionId == id
                                 select e).FirstOrDefault();
            if (calificacion == null)
            {
                return NotFound();
            }
            _blogContexto.Attach(calificacion);
            _blogContexto.Remove(calificacion);
            _blogContexto.SaveChanges();
            return Ok(calificacion);
        }
    }
}
