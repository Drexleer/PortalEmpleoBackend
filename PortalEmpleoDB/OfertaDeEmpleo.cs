using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortalEmpleoDB
{
    public class OfertaDeEmpleo
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int OfertaId { get; set; }
        public string? Nombre { get; set; }
        public string? Descripcion { get; set; }
        public decimal Salario { get; set; }
        public DateTime FechaDePublicacion { get; set; } = DateTime.Now;
        public int EmpresaId { get; set; }
        public virtual Empresa? Empresa { get; set; }

        public virtual ICollection<Usuario>? UsuariosPostulados { get; set; }
    }
}
