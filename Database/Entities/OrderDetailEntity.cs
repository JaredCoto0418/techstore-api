using ApiTienda.Database.Entities.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiTienda.Database.Entities
{
    public class OrderDetailEntity : EntidadBase
    {
        [Required]
        public int OrderId { get; set; }
        public OrderEntity Order { get; set; } = null!;

        public int? ProductId { get; set; }
        public ProductEntity? Product { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal UnitPrice { get; set; }
    }
} 