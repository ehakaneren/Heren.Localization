using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Localization;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using System;

namespace Heren.Localization
{
    public class JsonHtmlLocalizerFactory : IHtmlLocalizerFactory
    {
        private readonly IMemoryCache _memoryCache;
        private readonly IOptions<LocalizationOptions> _options;
        private readonly IHostingEnvironment _hostingEnvironment;

        public JsonHtmlLocalizerFactory(IMemoryCache memoryCache,
            IOptions<LocalizationOptions> options,
            IHostingEnvironment hostingEnvironment)
        {
            _memoryCache = memoryCache;
            _options = options;
            _hostingEnvironment = hostingEnvironment;
        }

        public IHtmlLocalizer Create(Type resourceSource)
        {
            var type = typeof(JsonHtmlLocalizer<>).MakeGenericType(resourceSource);
            var constructorParameters = new object[] { _memoryCache, _options, _hostingEnvironment };

            var instance = (IHtmlLocalizer)Activator.CreateInstance(type, constructorParameters);

            return instance;
        }

        public IHtmlLocalizer Create(string baseName, string location)
        {
            var type = Type.GetType(location);
            var constructorParameters = new object[] { _memoryCache, _options, _hostingEnvironment };

            var instance = (IHtmlLocalizer)Activator.CreateInstance(type, constructorParameters);
            return instance;
        }
    }
}