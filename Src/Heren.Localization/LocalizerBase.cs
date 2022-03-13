using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Reflection;

namespace Heren.Localization
{
    public abstract class LocalizerBase<TResource>
    {
        private readonly IMemoryCache _memoryCache;
        private readonly IOptions<LocalizationOptions> _options;
        private readonly IHostingEnvironment _hostingEnvironment;

        public LocalizerBase(IMemoryCache memoryCache,
            IOptions<LocalizationOptions> options,
            IHostingEnvironment hostingEnvironment)
        {
            _memoryCache = memoryCache;
            _options = options;
            _hostingEnvironment = hostingEnvironment;
        }

        protected virtual LocalizedString GetString(string name, params object[] arguments)
        {
            var resourceContainer = GetResourceContainerFromCache();
            var @default = new LocalizedString(name, name, true, resourceContainer.ResourcePath);

            if (!resourceContainer.Resources.TryGetValue(name, out var resource))
                return @default;

            if (string.IsNullOrWhiteSpace(resource))
                return @default;

            return new LocalizedString(name, string.Format(resource, arguments), false, resourceContainer.ResourcePath);
        }

        protected virtual IEnumerable<LocalizedString> GetAllStrings()
        {
            var resourceContainer = GetResourceContainerFromCache();

            foreach (var resourceItem in resourceContainer.Resources)
                yield return new LocalizedString(resourceItem.Key, resourceItem.Value, false, resourceContainer.ResourcePath);
        }

        private ResourceContainer GetResourceContainerAndConfigureCache(ICacheEntry cacheEntry)
        {
            var resourceName = GetResourceName();
            var cultureResourceName = $"{resourceName}.{CultureInfo.CurrentUICulture.Name}";

            var baseResourcePath = Path.Combine(_hostingEnvironment.ContentRootPath, _options.Value.ResourcesPath, $"{resourceName}.json");
            var cultureResourcePath = Path.Combine(_hostingEnvironment.ContentRootPath, _options.Value.ResourcesPath, $"{cultureResourceName}.json");

            cacheEntry.AddExpirationToken(_hostingEnvironment.ContentRootFileProvider.Watch($"{_options.Value.ResourcesPath}/{resourceName}.json"));
            cacheEntry.AddExpirationToken(_hostingEnvironment.ContentRootFileProvider.Watch($"{_options.Value.ResourcesPath}/{cultureResourceName}.json"));

            Dictionary<string, string> baseResource = null;
            if (File.Exists(baseResourcePath))
            {
                var baseContent = File.ReadAllText(baseResourcePath);
                baseResource = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, string>>(baseContent);
            }

            Dictionary<string, string> cultureResource = null;
            if (File.Exists(cultureResourcePath))
            {
                var cultureContent = File.ReadAllText(cultureResourcePath);
                cultureResource = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, string>>(cultureContent);
            }

            var resources = MergeResources(baseResource, cultureResource);

            return new ResourceContainer
            {
                ResourceName = resourceName,
                ResourcePath = baseResourcePath,
                Culture = CultureInfo.CurrentUICulture,
                Resources = resources
            };
        }

        private ResourceContainer GetResourceContainerFromCache()
        {
            var cacheKey = GetResourceName() + "." + CultureInfo.CurrentUICulture.Name;
            return _memoryCache.GetOrCreate(cacheKey, GetResourceContainerAndConfigureCache);
        }

        private Dictionary<string, string> MergeResources(Dictionary<string, string> baseResource, Dictionary<string, string> cultureResource)
        {
            var resource = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase);

            if (baseResource != null)
                foreach (var baseResourceItem in baseResource)
                    resource.Add(baseResourceItem.Key, baseResourceItem.Value);

            if (cultureResource != null)
                foreach (var cultureResourceItem in cultureResource)
                    if (resource.ContainsKey(cultureResourceItem.Key)) resource[cultureResourceItem.Key] = cultureResourceItem.Value;
                    else resource.Add(cultureResourceItem.Key, cultureResourceItem.Value);

            return resource;
        }

        private string GetResourceName()
        {
            if (typeof(TResource) == typeof(SharedResource))
                return nameof(SharedResource);

            var resourceAssemblyName = typeof(TResource).Assembly.GetName().Name;
            var entryAssemblyName = Assembly.GetEntryAssembly().GetName().Name;

            if (resourceAssemblyName.Equals(entryAssemblyName, StringComparison.InvariantCultureIgnoreCase))
                return typeof(TResource).FullName.Substring(resourceAssemblyName.Length).TrimStart('.');

            return typeof(TResource).FullName;
        }
    }
}