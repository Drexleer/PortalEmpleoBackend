using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using PortalEmpleoDB;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace PortalEmpleoBackend.Controllers
{
    [Route("/Ofertas")]
    [ApiController]
    public class OfertasController : Controller
    {
        private PortalEmpleoDbContext _context;

        public OfertasController(PortalEmpleoDbContext context)
        {
            _context = context;
        }

        // GET: /Ofertas
        [HttpGet]
        public async Task<ActionResult<IEnumerable<object>>> GetOfertasDeEmpleo()
        {
            var ofertasDeEmpleo = await _context.OfertasDeEmpleo
                .Include(o => o.Empresa)
                .Select(o => new
                {
                    o.OfertaId,
                    o.Nombre,
                    o.Descripcion,
                    o.Salario,
                    o.FechaDePublicacion,
                    EmpresaId = o.Empresa.EmpresaId,
                    EmpresaNombre = o.Empresa.Nombre
                })
                .ToListAsync();

            return Ok(ofertasDeEmpleo);
        }

        //GET: /Ofertas/id
        [HttpGet("{id}")]
        public async Task<ActionResult<object>> GetOferta(int id)
        {
            var ofertaDeEmpleo = await _context.OfertasDeEmpleo
                .Include(o => o.Empresa) // Incluir la información de la empresa
                .FirstOrDefaultAsync(o => o.OfertaId == id);

            if (ofertaDeEmpleo == null)
            {
                return NotFound();
            }

            // Proyectar los datos deseados en un nuevo objeto
            var result = new
            {
                ofertaDeEmpleo.OfertaId,
                ofertaDeEmpleo.Nombre,
                ofertaDeEmpleo.Descripcion,
                ofertaDeEmpleo.Salario,
                ofertaDeEmpleo.FechaDePublicacion,
                EmpresaId = ofertaDeEmpleo.Empresa.EmpresaId,
                EmpresaNombre = ofertaDeEmpleo.Empresa.Nombre
            };

            return Ok(result);
        }

        // POST: /Ofertas
        [HttpPost]
        public async Task<ActionResult<OfertaDeEmpleo>> PostOfertaDeEmpleo(OfertaDeEmpleo ofertaDeEmpleo)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Verificar si la empresa existe
            var empresa = await _context.Empresas.FindAsync(ofertaDeEmpleo.EmpresaId);
            if (empresa == null)
            {
                return BadRequest("La empresa especificada no existe.");
            }

            _context.OfertasDeEmpleo.Add(ofertaDeEmpleo);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Oferta de empleo creada correctamente." });
        }

        //PATCH: /Ofertas/id
        [HttpPatch("{id}")]
        public async Task<ActionResult<OfertaDeEmpleo>> UpdateOfertaDeEmpleo(int id, [FromBody] Dictionary<string, string> ofertasUpdates)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var existingOferta = await _context.OfertasDeEmpleo.FindAsync(id);
                if(existingOferta == null)
                {
                    return NotFound();
                }

                _context.Attach(existingOferta);

                foreach (var update in ofertasUpdates)
                {
                    switch(update.Key.ToLower())
                    {
                        case "nombre":
                            existingOferta.Nombre = update.Value;
                            break;
                        case "descripcion":
                            existingOferta.Descripcion = update.Value;
                            break;
                        case "salario":
                            if (decimal.TryParse(update.Value, out decimal salario))
                            {
                                existingOferta.Salario = salario;
                            }
                            else
                            {
                                return BadRequest("El valor de salario no es válido");
                            }
                            break;
                        default:
                            break;
                    }
                }

                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Ocurrió un error al actualizar la oferta: {ex.Message}");
            }

            return Ok(new { message = "Los datos de la oferta han sido actualizados correctamente" });
        }

        //DELETE: /Ofertas/id
        [HttpDelete("{id}")]
        public async Task<ActionResult<OfertaDeEmpleo>> DeleteOferta(int id)
        {
            var oferta = await _context.OfertasDeEmpleo.FindAsync(id);
            if(oferta == null )
            {
                return NotFound();
            }

            _context.OfertasDeEmpleo.Remove(oferta);
            await _context.SaveChangesAsync();

            return Ok(new { message = $"La oferta {oferta.Nombre} a sido eliminada correctamente" });
        }
    }
}