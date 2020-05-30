using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using System.Collections.Generic;

namespace Heren.Localization.Demo.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ResourcesController : ControllerBase
    {
        private readonly IStringLocalizer _localizer;

        public ResourcesController(IStringLocalizer localizer)
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