using System.Threading.Tasks;

namespace Heren.Localization.Demo.Api.Providers
{
    public interface IUserProvider
    {
        Task<LoginResult> Login(string email, string password);
    }
}