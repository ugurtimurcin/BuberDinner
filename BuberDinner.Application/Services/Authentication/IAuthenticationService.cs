namespace BuberDinner.Application.Services.Authentication;

public interface IAuthenticationService
{
    AuthenticationResult Login(string username, string password);
    AuthenticationResult Register(string firstName, string lastName, string email, string password);
}
