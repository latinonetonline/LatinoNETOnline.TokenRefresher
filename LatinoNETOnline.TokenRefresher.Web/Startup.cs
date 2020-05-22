using LatinoNETOnline.TokenRefresher.Web.Authentication.Providers.Mixer;
using LatinoNETOnline.TokenRefresher.Web.Business;
using LatinoNETOnline.TokenRefresher.Web.Business.Interfaces;
using LatinoNETOnline.TokenRefresher.Web.Extensions;
using LatinoNETOnline.TokenRefresher.Web.Services;
using LatinoNETOnline.TokenRefresher.Web.Services.Interfaces;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace LatinoNETOnline.TokenRefresher.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddHttpClient();

            services.AddOptions(Configuration);
            services.AddRefreshTokenHandlers();

            services.AddAuthorization();

            services.AddAuthentication("Bearer")
                .AddIdentityServerAuthentication(options =>
                {
                    options.Authority = Configuration["IdentityServer:Host"];
                    options.ApiName = Configuration["IdentityServer:ApiResource"];
                    options.RequireHttpsMetadata = false;
                });

            services.AddAuthentication()
                .AddCookie()
                .AddMixerOAuth(Configuration);

            services.AddFluentMigrator(Configuration);

            services.AddHealthChecks(Configuration);

            services.AddApiVersioning(Configuration);

            services.AddSwagger();

            services.AddTransient<ITokenService, TokenService>();
            services.AddTransient<ITokenBusiness, TokenBusiness>();
            services.AddTransient<ITokenBusinessCreate, TokenBusinessCreate>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IApiVersionDescriptionProvider provider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHealthChecks();
            });

            app.UseSwagger(provider);

        }
    }
}
