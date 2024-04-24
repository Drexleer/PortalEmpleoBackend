using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortalEmpleoDB
{
    public class Empresa
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int EmpresaId { get; set; }
        public string? Nombre { get; set; }
        public string? Descripcion { get; set; }
        public string? Tamaño { get; set; }
        public string? Sector { get; set; }
        public string? Correo { get; set; }
        public string? Contraseña { get; set; }
        public int? Tipo { get; set; }

        public virtual ICollection<OfertaDeEmpleo>? OfertasDeEmpleo { get; set; }
    }
}