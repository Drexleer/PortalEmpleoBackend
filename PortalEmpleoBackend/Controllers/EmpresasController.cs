﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using PortalEmpleoDB;
using System.Data;

namespace PortalEmpleoBackend.Controllers
{
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
        public async Task<ActionResult<IEnumerable<Empresa>>> GetEmpresas()
        {
            return await _context.Empresas.ToListAsync();
        }

        // GET: api/Empresas/id
        [HttpGet("{id}")]
        public async Task<ActionResult<Empresa>> GetEmpresa(int id)
        {
            var empresa = await _context.Empresas.FindAsync(id);

            if (empresa == null)
            {
                return NotFound();
            }
            return empresa;
        }

        // POST: api/Empresas
        [HttpPost]
        public async Task<ActionResult<Empresa>> CreateEmpresa([Bind("Nombre, Descripcion, Tamaño, Sector")] Empresa empresa)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Add(empresa);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Empresa creada satisfactoriamente" });
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Empresa>> UpdateEmpresa(int id, [FromBody] Empresa empresa)
        {
            if (id != empresa.EmpresaId)
            {
                empresa.EmpresaId = id; // Establecer el ID desde la URL en el objeto Empresa
            }

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

                _context.Entry(existingEmpresa).CurrentValues.SetValues(empresa);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Ocurrió un error al actualizar la empresa: {ex.Message}");
            }

            return Ok(new { message = "Los datos de la empresa han sido actualizados correctamente" });
        }

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