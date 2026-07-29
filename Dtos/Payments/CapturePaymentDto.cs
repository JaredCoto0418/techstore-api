using System.ComponentModel.DataAnnotations;

namespace ApiTienda.Dtos.Payments
{
    /// <summary>
    /// Solicitud del frontend para capturar (confirmar) un pago aprobado en PayPal.
    /// </summary>
    public class CapturePaymentDto
    {
        [Required]
        public int OrderId { get; set; }

        [Required]
        public string PaypalOrderId { get; set; } = string.Empty;
    }
}
