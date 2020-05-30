using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using System;

namespace Heren.Localization
{
    public class JsonStringLocalizerFactory : IStringLocalizerFactory
    {
        private readonly IMemoryCache _memoryCache;
        private readonly IOptions<LocalizationOptions> _options;
        private readonly IHostingEnvironment _hostingEnvironment;

        public JsonStringLocalizerFactory(IMemoryCache memoryCache,
            IOptions<LocalizationOptions> options,
            IHostingEnvironment hostingEnvironment)
        {
            _memoryCache = memoryCache;
            _options = options;
            _hostingEnvironment = hostingEnvironment;
        }

        public IStringLocalizer Create(Type resourceSource)
        {
            var type = typeof(JsonStringLocalizer<>).MakeGenericType(resourceSource);
            var constructorParameters = new object[] { _memoryCache, _options, _hostingEnvironment };

            var instance = (IStringLocalizer)Activator.CreateInstance(type, constructorParameters);
            return instance;
        }

        public IStringLocalizer Create(string baseName, string location)
        {
            var type = Type.GetType(location);
            var constructorParameters = new object[] { _memoryCache, _options, _hostingEnvironment };

            var instance = (IStringLocalizer)Activator.CreateInstance(type, constructorParameters);
            return instance;
        }
    }
}