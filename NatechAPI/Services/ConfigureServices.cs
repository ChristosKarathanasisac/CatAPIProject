namespace NatechAPI.Services
{
    public class ConfigureServices
    {
        private readonly IConfiguration _configuration;
        private readonly string url;
        private readonly string token;

        public ConfigureServices(IConfiguration configuration)
        {
            //this.url = _configuration["ExternalApi:BaseUrl"].ToString();
            //this.token = _configuration["ExternalApi:token"].ToString();
            this.url = "https://api.thecatapi.com/v1/images/search?limit=25&has_breeds=1";
            this.token = "live_kv6YKakZbGsA6zuVUzl8Vb5DkBYKze4Nn0kiATsCYoKxZkFVwno6B1fvnD6H6IZ7";
        }

        public string Url => url;

        public string Token => token;
    }
}
