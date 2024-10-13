
using NatechAPI.Models.ViewModels;

namespace NatechAPI.Services
{
    public class ExternalApiService 
    {
        private readonly ConfigureServices configureServices;

        public ExternalApiService(ConfigureServices configureServices)
        {
            this.configureServices = configureServices;
        }

        public async Task<ExternalApiResponseVM> GetDataFromExternalApiAsync()
        {
            ExternalApiResponseVM resp = new ExternalApiResponseVM();
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    client.DefaultRequestHeaders.Add("x-api-key", configureServices.Token);
                    HttpResponseMessage response = await client.GetAsync(this.configureServices.Url);
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
