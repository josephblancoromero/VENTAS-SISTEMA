using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Fecomvr1._2.Entidades
{
    public class Categoria
    {
        public int CategoriaId { get; set; }

        [Required(ErrorMessage = "El campo Nombre es obligatorio.")]
        [StringLength(100, ErrorMessage = "El campo Nombre no puede tener más de 100 caracteres.")]
        [DisplayName("Nombre de la Categoría")]
        public string Nombre { get; set; }

        [StringLength(255, ErrorMessage = "La Descripción no puede tener más de 255 caracteres.")]
        public string? Descripcion { get; set; }
    }
}
