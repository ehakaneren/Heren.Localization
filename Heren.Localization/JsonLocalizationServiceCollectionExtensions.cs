using Microsoft.AspNetCore.Mvc.Localization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using System;

namespace Heren.Localization
{
    public static class JsonLocalizationServiceCollectionExtensions
    {
        private static IServiceCollection AddServices(this IServiceCollection services)
        {
            return services
                .AddMemoryCache()
                .AddSingleton<IStringLocalizerFactory, JsonStringLocalizerFactory>()
                .AddTransient(typeof(IStringLocalizer), typeof(JsonStringLocalizer))
                .AddTransient(typeof(IStringLocalizer<>), typeof(JsonStringLocalizer<>))

                .AddSingleton<IHtmlLocalizerFactory, JsonHtmlLocalizerFactory>()
                .AddTransient(typeof(IHtmlLocalizer), typeof(JsonHtmlLocalizer))
                .AddTransient(typeof(IHtmlLocalizer<>), typeof(JsonHtmlLocalizer<>));
        }

        public static IServiceCollection AddJsonLocalization(this IServiceCollection services)
        {
            return
                AddServices(services);
        }

        public static IServiceCollection AddJsonLocalization(this IServiceCollection services, Action<LocalizationOptions> setupAction)
        {
            return
                AddServices(services).Configure(setupAction);
        }
    }
}