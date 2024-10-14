using NatechAPI.Models.ViewModels;

namespace NatechAPI.Services
{
    public interface IExternalApiService
    {
        Task<ExternalApiResponseVM> GetDataFromExternalApiAsync();
    }
}
