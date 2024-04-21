using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PortalEmpleoDB;

namespace PortalEmpleoBackend.Controllers
{
    [Route("/Postulaciones")]
    [ApiController]
    public class PostulacionesController : Controller
    {
        private PortalEmpleoDbContext _context;

        public PostulacionesController(PortalEmpleoDbContext context)
        {
            _context = context;
        }

        //GET: /Postulaciones
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Postulacion>>> GetPostulaciones()
        {
            return await _context.Postulaciones.ToListAsync();
        }

        //GET: /Postulaciones/id
        [HttpGet("{id}")]
        public async Task<ActionResult<Postulacion>> GetPostulacion(int id)
        {
            var postulacion = await _context.Postulaciones.FindAsync(id);

            if(postulacion == null)
            {
                return NotFound();
            }

            return Ok(postulacion);
        }

        //POST: /Postulaciones
        [HttpPost]
        public async Task<ActionResult<Postulacion>> CreatePostulacion([FromForm] PostulacionInputModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Guardar el archivo PDF en el sistema de archivos
            var archivoCV = Guid.NewGuid().ToString() + Path.GetExtension(model.ArchivoCV.FileName);
            var filePath = Path.Combine("C:\\Users\\DrexleerJ\\source\\repos\\PortalEmpleoBackend\\CVS", archivoCV);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await model.ArchivoCV.CopyToAsync(stream);
            }

            // Crear la postulación y guardarla en la base de datos
            var postulacion = new Postulacion
            {
                OfertaDeEmpleoID = model.OfertaDeEmpleoID,
                UsuarioId = model.UsuarioId,
                ArchivoCV = archivoCV
            };

            _context.Postulaciones.Add(postulacion);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetPostulacion), new { id = postulacion.Id }, postulacion);
        }

        public class PostulacionInputModel
        {
            public int OfertaDeEmpleoID { get; set; }
            public int UsuarioId { get; set; }
            public IFormFile ArchivoCV { get; set; }
        }
    }
}
