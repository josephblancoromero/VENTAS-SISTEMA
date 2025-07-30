using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Fecomvr1._2.Entidades
{
    public class Proveedor
    {
        public int ProveedorId { get; set; }

        [Required(ErrorMessage = "El campo Nombre es obligatorio.")]
        [StringLength(100, ErrorMessage = "El campo Nombre no puede tener más de 100 caracteres.")]
        [DisplayName("Nombre de la Proveedor")]
        public string Nombre { get; set; }

        [StringLength(255, ErrorMessage = "El campo Contacto no puede tener más de 255 caracteres.")]
        public string Contacto { get; set; }
    }
}
