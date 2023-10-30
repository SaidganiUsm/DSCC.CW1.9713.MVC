namespace DSCC.CW1._9713.MVC.Services
{
    public class UrlInjector
    {
        private readonly IConfiguration _configuration;

        public UrlInjector(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GetUrl()
        {
            return _configuration["BaseUrl"];
        }
    }
}
