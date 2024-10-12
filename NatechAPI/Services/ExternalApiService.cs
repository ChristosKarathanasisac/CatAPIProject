
using NatechAPI.Models.ViewModels;

namespace NatechAPI.Services
{
    public class ExternalApiService 
    {
        public async Task<ResponseVM> GetDataFromExternalApiAsync(string url)
        {
            ResponseVM resp = new ResponseVM();
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    HttpResponseMessage response = await client.GetAsync(url);
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
