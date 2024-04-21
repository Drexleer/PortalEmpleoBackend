using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortalEmpleoDB
{
    public class Postulacion
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int OfertaDeEmpleoID { get; set; }
        public OfertaDeEmpleo OfertaDeEmpleo { get; set; }
        public int UsuarioId { get; set; }
        public Usuario Usuario { get; set; }
        public string ArchivoCV { get; set; }
    }
}
