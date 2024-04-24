using Microsoft.AspNetCore.Mvc;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;
using Microsoft.EntityFrameworkCore;
using PortalEmpleoDB;

namespace PortalEmpleoBackend.Controllers
{
    public class UsuarioResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public Usuario Usuario { get; set; }
    }

    [Route("/[Controller]")]
    [ApiController]
    public class UsuariosController : Controller
    {
        private PortalEmpleoDbContext _context;

        public UsuariosController(PortalEmpleoDbContext context)
        {
            _context = context;
        }

        //GET: /Usuarios
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Usuario>>> GetUsuarios()
        {
            return await _context.Usuarios.ToListAsync();
        }

        //GET: /Usuarios/id
        [HttpGet("{id}")]
        public async Task<ActionResult<Usuario>> GetUsuario(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);

            if(usuario == null)
            {
                return NotFound();
            }

            return Ok(usuario);
        }

        //POST: /Usuarios
        [HttpPost]
        public async Task<ActionResult<UsuarioResponse>> CreateUsuario([Bind("Nombre,Correo,Telefono,Contraseña,Tipo")] Usuario usuario)
        {
            if (!ModelState.IsValid)
            {
                return Ok(new UsuarioResponse { Success = false, Message = "No se pudo registrar el usuario" });
            }

            // Verificar si ya existe un usuario con el mismo correo electrónico
            var existingUsuario = await _context.Usuarios.FirstOrDefaultAsync(e => e.Correo == usuario.Correo);
            if (existingUsuario != null)
            {
                return Ok(new UsuarioResponse { Success = false, Message = "Ya existe un usuario con ese correo electrónico" });
            }

            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();

            return Ok(new UsuarioResponse { Success = true, Message = "Registro Exitoso", Usuario = usuario });
        }

        //PATCH: /Usuarios/id
        [HttpPatch("{id}")]
        public async Task<ActionResult<Usuario>> UpdateUsuario(int id, [FromBody] Dictionary<string, string> usuarioUpdates)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var existingUsuario = await _context.Usuarios.FindAsync(id);
                if (existingUsuario == null)
                {
                    return NotFound();
                }

                _context.Attach(existingUsuario);

                foreach(var update in usuarioUpdates)
                {
                    switch(update.Key.ToLower())
                    {
                        case "nombre":
                            existingUsuario.Nombre = update.Value;
                            break;
                        case "correo":
                            existingUsuario.Correo = update.Value;
                            break;
                        case "telefono":
                            existingUsuario.Telefono = update.Value;
                            break;
                        case "contraseña":
                                existingUsuario.Contraseña = update.Value;
                            break;
                        default:
                            break;
                    }
                }

                await _context.SaveChangesAsync();

            } catch (Exception ex)
            {
                return StatusCode(500, $"Ocurrió un error al actualizar el usuario: {ex.Message}");
            }

            return Ok(new { message = "Los datos del usuario han sido actualizados correctamente" });
        }

        //DELETE: /Usuarios/id
        [HttpDelete("{id}")]
        public async Task<ActionResult<Usuario>> DeleteUsuario(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null)
            {
                return NotFound();
            }

            _context.Usuarios.Remove(usuario);
            await _context.SaveChangesAsync();

            return Ok(new { message = $"El usuario {usuario.Nombre} a sido eliminada correctamente" });
        }
    }
}
