namespace ApiTienda.Services.Interfaces
{
    public interface IDataInitializationService
    {
        Task<InitializationResult> InitializeDataAsync();
    }
} 