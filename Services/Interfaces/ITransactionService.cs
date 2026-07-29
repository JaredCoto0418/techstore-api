using ApiTienda.Dtos.Common;
using ApiTienda.Dtos.Transactions;

namespace ApiTienda.Services.Interfaces
{
    public interface ITransactionService
    {
        Task<ResponseDto<List<TransactionDto>>> GetMyTransactionsAsync(string userId);
        Task<ResponseDto<List<TransactionDto>>> GetListAsync();
        Task<ResponseDto<TransactionDto>> CreateAsync(TransactionCreateDto dto);
    }
}
