# Heren.Localization
Json Localization library for .NetCore projects.

# Project
This library allows to use JSON files instead of RESX in an ASP.NET application. The code tries to be most compliant with Microsoft guidelines.

# Configuration
An extension method is available for IServiceCollection. It has similar configuration and usage with Microsoft guidlines. There is no extra configuration options.

```csharp
using Heren.Localization;

public void ConfigureServices(IServiceCollection services)
{
    services.AddJsonLocalization(options => { options.ResourcesPath = "Resources"; });
}
```

You can use localization middleware as described [on microsoft document][MicrosoftLocalization]. You can see a short configuration example below and also you can use all explained information on microsoft document.

```csharp
public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
{
    var localizationOptions = new RequestLocalizationOptions();
    localizationOptions.SetDefaultCulture("en-US");
    localizationOptions.AddSupportedCultures(new string[] { "en-US", "tr-TR" });
    localizationOptions.AddSupportedUICultures(new string[] { "en-US", "tr-TR" });
    app.UseRequestLocalization(localizationOptions);
}
```

# Samples

First of all usage is same with native usage. The only difference is the use of json files instead of resx files. For example, you should create json files like this;

```bash
.
├── Resources
│   ├── Controllers.AuthenticationController.json
│   ├────── Controllers.AuthenticationController.tr-TR.json
│   ├── Heren.Localizer.Demo.AnAssembly.AClass.json
│   ├────── Heren.Localizer.Demo.AnAssembly.AClass.tr-TR.json
│   ├── SharedResource.json
│   └────── SharedResource.tr-TR.json
│
```

# Example 1: Shared Resource Usage

```csharp
namespace MyProject.Controllers
{
    public class ResourcesController : ControllerBase
    {
        private readonly IStringLocalizer _localizer;

        public ResourcesController(IStringLocalizer localizer)
        {
            _localizer = localizer;
        }
    }
}
```
If you use like IStringLocalizer (without generic type), then it reads from Resource/SharedResource.json file.

# Example 2: Generic Type Usage With Same Assembly Model

```csharp
namespace MyProject.Controllers
{
    public class AuthenticationController : ControllerBase
    {
        private readonly IStringLocalizer<AuthenticationController> localizer _localizer;

        public AuthenticationController(IStringLocalizer<AuthenticationController> localizer)
        {
            _localizer = localizer;
        }
    }
}
```
If you use like IStringLocalizer\<AuthenticationController\>, then it reads from Resource/Controllers.AuthenticationController.json file.

# Example 3: Generic Type Usage With Different Assembly Model

```csharp
namespace MyProject.Controllers
{
    public class AuthenticationController : ControllerBase
    {
        private readonly IStringLocalizer<DifferentAssembly.AClass> localizer _localizer;

        public AuthenticationController(IStringLocalizer<DifferentAssembly.AClass> localizer)
        {
            _localizer = localizer;
        }
    }
}
```
If you use like IStringLocalizer\<DifferentAssembly.AClass\>, then it reads resource from Resource/DifferentAssembly.AClass.json file. If the generic type model is not in executed assembly then, it reads resource from "Resource/{AssemblyName}.{NameSpace}.{ClassName}.json".

You can see the demo project for all different samples from [here][DemoProject].

# Sample Resource Files

{AssemblyName}.{NameSpace}.{ClassName}.en-US.json
```json
{
  "EmailNotFound": "The e-mail address you tried to login is not registered in our system.",
  "WrongPassword": "The password you tried to login is incorrect.",
  "LockedOut": "Your account has been locked due to failed login attempts. You can log in again after {0}.",
  "RequiresEmailConfirmation": "You must confirm your e-mail address before login.",
  "RequiresPhoneConfirmation": "You must confirm your phone number before login."
}
```
{AssemblyName}.{NameSpace}.{ClassName}.tr-TR.json
```json
{
  "EmailNotFound": "Giriş yapmaya çalıştığınız e-posta adresi sistemimizde kayıtlı değildir.",
  "WrongPassword": "Giriş yapmaya çalıştığınız parola hatalıdır.",
  "LockedOut": "Hesabınız başarısız giriş denemeleri nedeniyle kilitlenmiştir. {0} sonra tekrar giriş yapabilirsiniz.",
  "RequiresEmailConfirmation": "Giriş yapmak için önce e-posta adresinizi onaylamanız gerekmektedir.",
  "RequiresPhoneConfirmation": "Giriş yapmak için önce telefon numaranızı onaylamanız gerekmektedir."
}
```

# Demo Project

You can find the demo project and sample usages [here][DemoProject]. You can run api project and try this endpoints to see outputs;

* \/api\/resources
* \/api\/resources?culture=tr-TR

* \/api\/resources\/apple
* \/api\/resources\/apple?culture=tr-TR

* \/api\/resources\/anassembly
* \/api\/resources\/anassembly?culture=tr-TR

* \/api\/anassembly\/message1
* \/api\/anassembly\/message1?culture=tr-TR

# Good to Know
* The resource key searchs in the {culture}.json file. If the culture file is not exists or key not exists in the culture file, then resource key searchs in base file.

* The resource file is not reading every time. Files caches by culture in memory until the base file or the culture file is modified. So the library is applies memory cache and it expiration depends to files modifications.

[MicrosoftLocalization]: <https://docs.microsoft.com/en-us/aspnet/core/fundamentals/localization?view=aspnetcore-3.1>
[DemoProject]: <https://github.com/ehakaneren/Heren.Localizer/tree/master/Demo/Heren.Localization.Demo.Api>
