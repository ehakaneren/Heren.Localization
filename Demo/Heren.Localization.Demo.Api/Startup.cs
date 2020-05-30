using Heren.Localization.Demo.Api.Providers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Heren.Localization.Demo.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddScoped<IUserProvider, UserProvider>();
            services.AddJsonLocalization(options =>
            {
                options.ResourcesPath = "Resources";
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            var localizationOptions = new RequestLocalizationOptions();
            localizationOptions.SetDefaultCulture("en-US");
            localizationOptions.AddSupportedCultures(new string[] { "en-US", "tr-TR" });
            localizationOptions.AddSupportedUICultures(new string[] { "en-US", "tr-TR" });
            app.UseRequestLocalization(localizationOptions);

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}