using ApiTienda.Database.Entities.Common;
using System.ComponentModel.DataAnnotations;

namespace ApiTienda.Database.Entities
{
    public class CategoryEntity : EntidadBase
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [StringLength(20)]
        public string Type { get; set; } = string.Empty; // "Product" o "Service"

        public ICollection<ProductEntity> Products { get; set; } = new List<ProductEntity>();
    }
} 