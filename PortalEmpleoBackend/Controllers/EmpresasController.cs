using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using PortalEmpleoDB;
using System.Data;

namespace PortalEmpleoBackend.Controllers
{
    public class EmpresasResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
    }

    [Route("/[controller]")]
    [ApiController]
    public class EmpresasController : Controller

    {
        private PortalEmpleoDbContext _context;

        public EmpresasController(PortalEmpleoDbContext context)
        {
            _context = context;
        }

        // GET: api/Empresas
        [HttpGet]
        public async Task<ActionResult<IEnumerable<object>>> GetEmpresas()
        {
            var empresas = await _context.Empresas
                .Include(e => e.OfertasDeEmpleo)
                .Select(e => new
                {
                    e.EmpresaId,
                    e.Nombre,
                    e.Descripcion,
                    e.Tamaño,
                    e.Sector,
                    OfertasDeEmpleo = e.OfertasDeEmpleo.Select(o => new
                    {
                        o.OfertaId,
                        o.Nombre,
                        o.Descripcion,
                        o.Salario,
                        o.FechaDePublicacion
                    }).ToList()
                })
                .ToListAsync();

            return Ok(empresas);
        }

        // GET: /Empresas/id
        [HttpGet("{id}")]
        public async Task<ActionResult<object>> GetEmpresa(int id)
        {
            var empresa = await _context.Empresas
                .Include(e => e.OfertasDeEmpleo) // Incluir las ofertas de empleo de la empresa
                .FirstOrDefaultAsync(e => e.EmpresaId == id);

            if (empresa == null)
            {
                return NotFound();
            }

            // Proyectar los datos deseados en un nuevo objeto anónimo
            var result = new
            {
                empresa.EmpresaId,
                empresa.Nombre,
                empresa.Descripcion,
                empresa.Tamaño,
                empresa.Sector,
                OfertasDeEmpleo = empresa.OfertasDeEmpleo.Select(o => new
                {
                    o.OfertaId,
                    o.Nombre,
                    o.Descripcion,
                    o.Salario,
                    o.FechaDePublicacion
                })
            };

            return Ok(result);
        }

        // POST: /Empresas
        [HttpPost]
        public async Task<ActionResult<Empresa>> CreateEmpresa([Bind("Nombre, Descripcion, Tamaño, Sector, Tipo, Correo")] Empresa empresa)
        {
            if (!ModelState.IsValid)
            {
                return Ok(new EmpresasResponse { Success = true, Message = "No se pudo registrar la Empresa" });
            }

            // Verificar si ya existe una empresa con el mismo correo electrónico
            var existingEmpresa = await _context.Empresas.FirstOrDefaultAsync(e => e.Correo == empresa.Correo);
            if (existingEmpresa != null)
            {
                return Ok(new EmpresasResponse { Success = false, Message = "Ya existe una empresa con ese correo electrónico" });
            }

            _context.Empresas.Add(empresa);
            await _context.SaveChangesAsync();

            return Ok(new EmpresasResponse { Success = true, Message = "Registro Exitoso" });
        }

        //PATCH: /Empresas/id
        [HttpPatch("{id}")]
        public async Task<ActionResult<Empresa>> UpdateEmpresa(int id, [FromBody] Dictionary<string, string> empresaUpdates)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var existingEmpresa = await _context.Empresas.FindAsync(id);
                if (existingEmpresa == null)
                {
                    return NotFound();
                }

                _context.Attach(existingEmpresa);

                foreach (var update in empresaUpdates)
                {
                    switch (update.Key.ToLower())
                    {
                        case "nombre":
                            existingEmpresa.Nombre = update.Value;
                            break;
                        case "descripcion":
                            existingEmpresa.Descripcion = update.Value;
                            break;
                        case "tamaño":
                            existingEmpresa.Tamaño = update.Value;
                            break;
                        case "sector":
                            existingEmpresa.Sector = update.Value;
                            break;
                        default:
                            break;
                    }
                }

                await _context.SaveChangesAsync();

            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Ocurrió un error al actualizar la empresa: {ex.Message}");
            }

            return Ok(new { message = "Los datos de la empresa han sido actualizados correctamente" });
        }

        //DELETE: /Empresas/id
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmpresa(int id)
        {
            var empresa = await _context.Empresas.FindAsync(id);
            if(empresa == null)
            {
                return NotFound();
            }

            _context.Empresas.Remove(empresa);
            await _context.SaveChangesAsync();

            return Ok(new { message = $"La empresa {empresa.Nombre} a sido eliminada correctamente" });
        }

    }
}
