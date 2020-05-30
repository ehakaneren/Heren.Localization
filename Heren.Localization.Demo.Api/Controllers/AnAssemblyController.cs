using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using System.Collections.Generic;

namespace Heren.Localization.Demo.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AnAssemblyController : ControllerBase
    {
        private readonly IStringLocalizer<Heren.Localization.Demo.AnAssembly.AClass> _localizer;

        public AnAssemblyController(IStringLocalizer<Heren.Localization.Demo.AnAssembly.AClass> localizer)
        {
            _localizer = localizer;
        }

        [HttpGet]
        public IEnumerable<LocalizedString> Get()
        {
            return _localizer.GetAllStrings();
        }

        [HttpGet("{id}")]
        public string Get(string id)
        {
            return _localizer[id].Value;
        }
    }
}