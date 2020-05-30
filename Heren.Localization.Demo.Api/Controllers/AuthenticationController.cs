using Heren.Localization.Demo.Api.Providers;
using Heren.Localization.Demo.Api.ViewModels.Request;
using Heren.Localization.Demo.Api.ViewModels.Response;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using System;
using System.Threading.Tasks;

namespace Heren.Localization.Demo.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthenticationController : ControllerBase
    {
        private readonly IUserProvider _userProvider;
        private readonly IStringLocalizer<AuthenticationController> _localizer;

        public AuthenticationController(IUserProvider userProvider,
            IStringLocalizer<AuthenticationController> localizer)
        {
            _userProvider = userProvider;
            _localizer = localizer;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            var loginResult = await _userProvider.Login(request.Email, request.Password);

            if (loginResult.Status != LoginStatus.Succeeded)
            {
                return
                    loginResult.Status == LoginStatus.EmailNotFound ? Unauthorized(new ErrorViewModel(_localizer["EmailNotFound"]))
                    : loginResult.Status == LoginStatus.WrongPassword ? Unauthorized(new ErrorViewModel(_localizer["WrongPassword"]))
                    : loginResult.Status == LoginStatus.LockedOut ? new ObjectResult(new ErrorViewModel(_localizer["LockedOut", DateTime.Now.AddMinutes(5).ToString()])) { StatusCode = StatusCodes.Status403Forbidden }
                    : loginResult.Status == LoginStatus.RequiresTwoFactor ? new ObjectResult(new ErrorViewModel(_localizer["RequiresTwoFactor"])) { StatusCode = StatusCodes.Status403Forbidden }
                    : loginResult.Status == LoginStatus.RequiresEmailConfirmation ? new ObjectResult(new ErrorViewModel(_localizer["RequiresEmailConfirmation"])) { StatusCode = StatusCodes.Status403Forbidden }
                    : loginResult.Status == LoginStatus.RequiresPhoneConfirmation ? new ObjectResult(new ErrorViewModel(_localizer["RequiresPhoneConfirmation"])) { StatusCode = StatusCodes.Status403Forbidden }
                    : Unauthorized(new ErrorViewModel(_localizer["EmailNotFound"], LoginStatus.EmailNotFound.ToString()));
            }

            var mappedUser = new UserViewModel
            {
                Id = loginResult.User.Id,
                Email = loginResult.User.Email,
                PhoneNumber = loginResult.User.PhoneNumber,
                FirstName = loginResult.User.FirstName,
                LastName = loginResult.User.LastName
            };

            return Ok(mappedUser);
        }
    }
}