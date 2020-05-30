using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Heren.Localization.Demo.Api.Providers
{
    public class UserProvider : IUserProvider
    {
        private readonly List<User> _users;

        public UserProvider()
        {
            _users = new List<User>();
            _users.Add(new User { Id = "1", IsLocked = true, IsEmailConfirmed = true, IsPhoneConfirmed = true, RequiresTwoFactor = false, FirstName = "Jon", LastName = "Snow", PhoneNumber = "+90 555 555 55 55", Email = "jon@gmail.com", Password = "123456" });
            _users.Add(new User { Id = "2", IsLocked = false, IsEmailConfirmed = false, IsPhoneConfirmed = true, RequiresTwoFactor = false, FirstName = "Arya", LastName = "Stark", PhoneNumber = "+90 444 444 44 44", Email = "arya@gmail.com", Password = "123456" });
            _users.Add(new User { Id = "3", IsLocked = false, IsEmailConfirmed = true, IsPhoneConfirmed = false, RequiresTwoFactor = false, FirstName = "Eddard", LastName = "Stark", PhoneNumber = "+90 333 333 33 33", Email = "eddard@gmail.com", Password = "123456" });
            _users.Add(new User { Id = "4", IsLocked = false, IsEmailConfirmed = true, IsPhoneConfirmed = true, RequiresTwoFactor = true, FirstName = "Joffrey", LastName = "Baratheon", PhoneNumber = "+90 222 222 22 22", Email = "joffrey@gmail.com", Password = "123456" });
            _users.Add(new User { Id = "5", IsLocked = false, IsEmailConfirmed = true, IsPhoneConfirmed = true, RequiresTwoFactor = false, FirstName = "Theon", LastName = "Greyjoy", PhoneNumber = "+90 111 111 11 11", Email = "theon@gmail.com", Password = "123456" });
        }

        public Task<LoginResult> Login(string email, string password)
        {
            var user = _users.FirstOrDefault(p => p.Email.Equals(email, System.StringComparison.InvariantCultureIgnoreCase));
            if (user == null)
                return Task.FromResult(new LoginResult(LoginStatus.EmailNotFound));

            if (!user.Password.Equals(password))
                return Task.FromResult(new LoginResult(LoginStatus.WrongPassword));

            if (user.IsLocked)
                return Task.FromResult(new LoginResult(LoginStatus.LockedOut));

            if (!user.IsEmailConfirmed)
                return Task.FromResult(new LoginResult(LoginStatus.RequiresEmailConfirmation));

            if (!user.IsPhoneConfirmed)
                return Task.FromResult(new LoginResult(LoginStatus.RequiresPhoneConfirmation));

            if (!user.RequiresTwoFactor)
                return Task.FromResult(new LoginResult(LoginStatus.RequiresTwoFactor));

            return Task.FromResult(new LoginResult(user));
        }
    }
}