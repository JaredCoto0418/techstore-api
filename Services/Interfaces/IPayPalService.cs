using ApiTienda.Dtos.Payments;

namespace ApiTienda.Services.Interfaces
{
    public interface IPayPalService
    {
        /// <summary>Crea una orden de pago en PayPal por el monto indicado.</summary>
        Task<PayPalCreateResult> CreateOrderAsync(decimal amount, string currency, string referenceId);

        /// <summary>Captura (cobra) una orden de PayPal previamente aprobada por el comprador.</summary>
        Task<PayPalCaptureResult> CaptureOrderAsync(string paypalOrderId);
    }
}
