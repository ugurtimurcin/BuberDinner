using BuberDinner.Application.Common.Interfaces.Authentication;
using BuberDinner.Application.Common.Interfaces.Persistence;
using BuberDinner.Domain.Entities;

namespace BuberDinner.Application.Services.Authentication;

public class AuthenticationService : IAuthenticationService
{
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly IUserRepository _userRepository;
    public AuthenticationService(IJwtTokenGenerator jwtTokenGenerator,
                                 IUserRepository userRepository)
    {
        _jwtTokenGenerator = jwtTokenGenerator;
        _userRepository = userRepository;
    }

    public AuthenticationResult Login(string email, string password)
    {
        if(_userRepository.GetByEmail(email) is not User user)
        {
            //TODO: Change exception
            throw new Exception("User with given email does not exist.");
        }

        if(user.Password != password)
        {
            //TODO: Change exception
            throw new Exception("Invalid password");
        }

        string token = _jwtTokenGenerator.GenerateToken(user);

        return new AuthenticationResult(user, token);
    }

    public AuthenticationResult Register(string firstName, string lastName, string email, string password)
    {
        if(_userRepository.GetByEmail(email) is not null)
        {
            //TODO: Change exception
            throw new Exception("User with given email already exists.");
        }

        User user = new()
        {
            Id = Guid.NewGuid(),
            FirstName = firstName,
            LastName = lastName,
            Email = email,
            Password = password
        };

        _userRepository.Add(user);

        var token = _jwtTokenGenerator.GenerateToken(user);
        return new AuthenticationResult(user, token);
    }
}
