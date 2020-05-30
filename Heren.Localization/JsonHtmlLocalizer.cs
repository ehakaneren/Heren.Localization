using Microsoft.AspNetCore.Mvc.Localization;
using Microsoft.Extensions.Localization;

namespace Heren.Localization
{
    public class JsonHtmlLocalizer<TResource> : HtmlLocalizer
    {
        private readonly IStringLocalizer<TResource> _stringLocalizer;

        public JsonHtmlLocalizer(IStringLocalizer<TResource> stringLocalizer) : base(stringLocalizer)
        {
            _stringLocalizer = stringLocalizer;
        }

        public override LocalizedHtmlString this[string name]
        {
            get
            {
                return ToHtmlString(_stringLocalizer[name]);
            }
        }

        public override LocalizedHtmlString this[string name, params object[] arguments]
        {
            get
            {
                return ToHtmlString(_stringLocalizer[name], arguments);
            }
        }
    }

    public class JsonHtmlLocalizer : JsonHtmlLocalizer<SharedResource>
    {
        public JsonHtmlLocalizer(IStringLocalizer<SharedResource> stringLocalizer) : base(stringLocalizer)
        {
        }
    }
}