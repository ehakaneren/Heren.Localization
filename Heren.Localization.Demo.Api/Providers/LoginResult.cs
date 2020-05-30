namespace Heren.Localization.Demo.Api.Providers
{
    public class LoginResult
    {
        public LoginStatus Status { get; set; }
        public User User { get; set; }

        public LoginResult(LoginStatus status) : this(status, null)
        {
        }

        public LoginResult(User user) : this(LoginStatus.Succeeded, user)
        {
        }

        public LoginResult(LoginStatus status, User user)
        {
            Status = status;
            User = user;
        }
    }
}