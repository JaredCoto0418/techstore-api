using ApiTienda.Constants;
using ApiTienda.Dtos.Common;
using ApiTienda.Dtos.Transactions;
using ApiTienda.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ApiTienda.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TransactionsController : ControllerBase
    {
        private readonly ITransactionService _transactionService;

        public TransactionsController(ITransactionService transactionService)
        {
            _transactionService = transactionService;
        }

        /// <summary>
        /// Obtiene el historial de transacciones del usuario autenticado (cliente).
        /// </summary>
        [HttpGet("my-transactions")]
        [Authorize(Roles = "CLIENTE")]
        public async Task<ActionResult<ResponseDto<List<TransactionDto>>>> GetMyTransactions()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized(new ResponseDto<List<TransactionDto>>
                {
                    Status = false,
                    StatusCode = CodigosDeEstadoHttp.NO_AUTORIZADO,
                    Message = "Usuario no autenticado."
                });
            }

            var response = await _transactionService.GetMyTransactionsAsync(userId);
            return StatusCode(response.StatusCode, response);
        }

        /// <summary>
        /// Obtiene todas las transacciones (solo administradores).
        /// </summary>
        [HttpGet]
        [Authorize(Roles = "ADMINISTRADOR")]
        public async Task<ActionResult<ResponseDto<List<TransactionDto>>>> GetAll()
        {
            var response = await _transactionService.GetListAsync();
            return StatusCode(response.StatusCode, response);
        }
    }
}
