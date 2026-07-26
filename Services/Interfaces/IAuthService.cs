using ApiTienda.Dtos.Common;
using ApiTienda.Dtos.Security.Auth;

namespace ApiTienda.Services.Interfaces
{
    public interface IAuthService
    {
        Task<ResponseDto<LoginResponseDto>> LoginAsync(LoginDto dto);
    }
} 