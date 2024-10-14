using NatechAPI.Models.ViewModels;

namespace NatechAPI.Services
{
    public interface ICatService
    {
        Task<HashSet<string>> AddCatsToDb();
        Task<ReturnedCatsVM> GetCatById(string id);
        Task<GetCatsPegResponseVM> GetCatsWithPegination(int page, int pageSize);
        Task<GetCatsPegResponseVM> GetCatsWithPegination(string tag, int page, int pageSize);

    }
}
