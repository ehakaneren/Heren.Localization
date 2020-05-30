using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Globalization;

namespace Heren.Localization
{
    public class JsonStringLocalizer<TResource> : LocalizerBase<TResource>, IStringLocalizer<TResource>
    {
        private readonly IMemoryCache _memoryCache;
        private readonly IOptions<LocalizationOptions> _options;
        private readonly IHostingEnvironment _hostingEnvironment;

        public JsonStringLocalizer(IMemoryCache memoryCache,
            IOptions<LocalizationOptions> options,
            IHostingEnvironment hostingEnvironment) : base(memoryCache, options, hostingEnvironment)
        {
            _memoryCache = memoryCache;
            _options = options;
            _hostingEnvironment = hostingEnvironment;
        }

        public LocalizedString this[string name]
        {
            get
            {
                return GetString(name);
            }
        }

        public LocalizedString this[string name, params object[] arguments]
        {
            get
            {
                return GetString(name, arguments);
            }
        }

        public IEnumerable<LocalizedString> GetAllStrings(bool includeParentCultures)
        {
            return GetAllStrings();
        }

        public IStringLocalizer WithCulture(CultureInfo culture)
        {
            return new JsonStringLocalizer(_memoryCache, _options, _hostingEnvironment);
        }
    }

    public class JsonStringLocalizer : JsonStringLocalizer<SharedResource>
    {
        public JsonStringLocalizer(IMemoryCache memoryCache, IOptions<LocalizationOptions> options, IHostingEnvironment hostingEnvironment) : base(memoryCache, options, hostingEnvironment)
        {
        }
    }
}