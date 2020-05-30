namespace Heren.Localization.Demo.Api.Providers
{
    public enum LoginStatus
    {
        None = 0,
        Succeeded = 1,
        EmailNotFound = 2,
        WrongPassword = 3,
        LockedOut = 4,
        RequiresEmailConfirmation = 5,
        RequiresPhoneConfirmation = 6,
        RequiresTwoFactor = 7
    }
}