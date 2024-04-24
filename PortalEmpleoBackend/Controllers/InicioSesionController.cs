using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PortalEmpleoDB;

namespace PortalEmpleoBackend.Controllers
{
    public class LoginInputModel
    {
        public string Correo { get; set; }
        public string Contraseña { get; set; }
        public int Tipo { get; set; }
    }
    public class LoginResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public Usuario Usuario { get; set; }
        public Empresa Empresa { get; set; }
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
            if (model.Tipo == 0)
            {
                var empresaLog = await _context.Empresas.FirstOrDefaultAsync(e => e.Correo == model.Correo && e.Contraseña == model.Contraseña);

                if (empresaLog == null)
                {
                    return Ok(new LoginResponse { Success = false, Message = "Inicio de sesión fallido" });
                }

                // Devolver la información de la empresa en la respuesta
                return Ok(new LoginResponse { Success = true, Message = "Inicio de sesión exitoso", Empresa = empresaLog });
            }
            else if (model.Tipo == 1)
            {
                var usuarioLog = await _context.Usuarios.FirstOrDefaultAsync(u => u.Correo == model.Correo && u.Contraseña == model.Contraseña);

                if (usuarioLog == null)
                {
                    return Ok(new LoginResponse { Success = false, Message = "Inicio de sesión fallido" });
                }

                // Devolver la información del usuario en la respuesta
                return Ok(new LoginResponse { Success = true, Message = "Inicio de sesión exitoso", Usuario = usuarioLog });
            }
            else
            {
                return BadRequest("Tipo de usuario no válido");
            }
        }


    }
}
