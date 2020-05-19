
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace LatinoNETOnline.TokenRefresher.Web.Extensions
{
    public static class ApiVersioningExtensions
    {
        public static IServiceCollection AddApiVersioning(this IServiceCollection services, IConfiguration configuration)
        {
            return services.AddApiVersioning(
                 options =>
                 {
                     // reporting api versions will return the headers "api-supported-versions" and "api-deprecated-versions"
                     options.ReportApiVersions = true;
                     options.AssumeDefaultVersionWhenUnspecified = true;
                     options.DefaultApiVersion = new ApiVersion(1, 0);
                     options.ApiVersionReader = new MediaTypeApiVersionReader("v");
                 })
            .AddVersionedApiExplorer(
                options =>
                {
                    // add the versioned api explorer, which also adds IApiVersionDescriptionProvider service
                    // note: the specified format code will format the version as "'v'major[.minor][-status]"
                    options.GroupNameFormat = "'v'VVV";

                    // note: this option is only necessary when versioning by url segment. the SubstitutionFormat
                    // can also be used to control the format of the API version in route templates
                    options.SubstituteApiVersionInUrl = true;
                });
        }
    }
}
