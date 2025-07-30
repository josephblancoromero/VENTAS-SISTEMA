using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Fecomvr1._2.Entidades
{
    public class Factura
    {
        public int FacturaId { get; set; }

        [Required(ErrorMessage = "El campo Fecha es obligatorio.")]
        public DateTime Fecha { get; set; }

        [Required(ErrorMessage = "El campo Cliente es obligatorio.")]
        public string Cliente { get; set; }

        public List<Producto> Productos { get; set; } = new List<Producto>();

        public decimal IGV18 { get; set; }

        public decimal TotalIGV18 { get; set; }
    }
}

