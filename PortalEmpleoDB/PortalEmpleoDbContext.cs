using Microsoft.EntityFrameworkCore;

namespace PortalEmpleoDB
{
    public class PortalEmpleoDbContext : DbContext
    {
        public PortalEmpleoDbContext(DbContextOptions<PortalEmpleoDbContext> options) : base(options) { }

        public DbSet<OfertaDeEmpleo> OfertasDeEmpleo { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Empresa> Empresas { get; set; }
    }
}
