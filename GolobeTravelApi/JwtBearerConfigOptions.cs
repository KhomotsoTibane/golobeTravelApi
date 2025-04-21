using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace GolobeTravelApi
{
    public class JwtBearerConfigOptions(IConfiguration configuration ): IConfigureNamedOptions<JwtBearerOptions>
    {
        private const string ConfigurationSectionName = "JwtBearer";
   

        public void Configure(JwtBearerOptions options)
        {
            configuration.GetSection(ConfigurationSectionName).Bind(options);
        }

        public void Configure(string? name, JwtBearerOptions options)
        {
            Configure(options);
        }
    }
}
