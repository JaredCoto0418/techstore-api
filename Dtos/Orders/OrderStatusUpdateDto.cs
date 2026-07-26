using System.ComponentModel.DataAnnotations;

namespace ApiTienda.Dtos.Orders
{
    public class OrderStatusUpdateDto
    {
        [Required(ErrorMessage = "El estado de la orden es requerido.")]
        [StringLength(50, ErrorMessage = "El estado no puede exceder los 50 caracteres.")]
        public string Status { get; set; } = string.Empty;
    }
} 