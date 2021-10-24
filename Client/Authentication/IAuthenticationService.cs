using SMSGateway.Client.Models;
using System.Threading.Tasks;

namespace SMSGateway.Client.Authentication
{
    public interface IAuthenticationService
    {
        Task<AuthenticatedUserModel> Login(AuthenticationUserModel model);
        Task Logout();
    }
}