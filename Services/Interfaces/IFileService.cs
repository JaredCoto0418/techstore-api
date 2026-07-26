using Microsoft.AspNetCore.Http;

namespace ApiTienda.Services.Interfaces
{
    public interface IFileService
    {
        Task<string?> SaveProductImageAsync(IFormFile file, int productId);
        Task<bool> DeleteProductImageAsync(string imageUrl);
        bool IsValidImageFile(IFormFile file);
    }
} 