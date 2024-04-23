using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PortalEmpleoDB;

namespace PortalEmpleoBackend.Controllers
{
    public class LoginInputModel
    {
        public string Correo { get; set; }
        public string Contraseña { get; set; }
        public string Tipo { get; set; }
    }

    [Route("/iniciarSesion")]
    [ApiController]
    public class InicioSesionController : Controller
    {
        private PortalEmpleoDbContext _context;

        public InicioSesionController(PortalEmpleoDbContext context)
        {
            _context = context;
        }
        
        //POST: /registro
        [HttpPost]
        public async Task<ActionResult> Login([FromBody] LoginInputModel model)
        {
            if (model.Tipo == "Empresa")
            {
                var empresaLog = await _context.Empresas.FirstOrDefaultAsync(e => e.Correo == model.Correo && e.Contraseña == model.Contraseña);

                if (empresaLog == null)
                {
                    return Unauthorized();
                }

                return Ok(new {message = "Inicio de sesion exitoso"});
            }
            else if (model.Tipo == "Usuario")
            {
                var usuarioLog = await _context.Usuarios.FirstOrDefaultAsync(u => u.Correo == model.Correo && u.Contraseña == model.Contraseña);

                if (usuarioLog == null)
                {
                    return Unauthorized();
                }

                return Ok(new { message = "Inicio de sesion exitoso" });
            }
            else
            {
                return BadRequest("Tipo de usuario no válido");
            }
        }

    }
}
