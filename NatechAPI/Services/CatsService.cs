using Microsoft.AspNetCore.Cors.Infrastructure;
using NatechAPI.Data;

namespace NatechAPI.Services
{
    public class CatsService 
    {
        private readonly ApplicationDbContext dbContext;
        private readonly ExternalApiService externalApiService;
        public CatsService(ApplicationDbContext context, ExternalApiService externalApiService)
        {
            this.dbContext = context;
            this.externalApiService = externalApiService;
        }

        public bool AddCatsToDb() 
        {
            return true;
        }
    }
}
