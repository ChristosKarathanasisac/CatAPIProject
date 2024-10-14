
using Microsoft.Extensions.Options;
using NatechAPI.Models.Config;
using NatechAPI.Models.ViewModels;

namespace NatechAPI.Services
{
    public class ExternalApiService : IExternalApiService
    {
        private readonly ExternalApiSettings _externalApiSettings;

        public ExternalApiService(IOptions<ExternalApiSettings> externalApiSettings)
        {
            _externalApiSettings = externalApiSettings.Value;
        }

        public async Task<ExternalApiResponseVM> GetDataFromExternalApiAsync()
        {
            ExternalApiResponseVM resp = new ExternalApiResponseVM();
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    client.DefaultRequestHeaders.Add("x-api-key", _externalApiSettings.Token);
                    HttpResponseMessage response = await client.GetAsync(_externalApiSettings.BaseUrl);
                    response.EnsureSuccessStatusCode();
                    resp.body = await response.Content.ReadAsStringAsync();
                    resp.IsSuccess = true;
                    return resp;
                }
                catch (HttpRequestException e)
                {
                    resp.Error =  $"HttpRequestException. Message: {e.Message}";
                    resp.IsSuccess = false;
                    return resp;
                }
                catch (Exception e) 
                {
                    resp.Error = $"Exception. Message: {e.Message}";
                    resp.IsSuccess = false;
                    return resp;
                }
            };
        }
    }
}
