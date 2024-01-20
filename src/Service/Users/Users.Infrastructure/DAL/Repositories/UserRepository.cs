using Microsoft.AspNetCore.Identity;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Logging;
using Users.Core.Repositories;
using Users.Core.ValueObjects;
using Users.Infrastructure.DAL.Container;
using Users.Infrastructure.Exceptions;
using User = Users.Core.Entities.User;

namespace Users.Infrastructure.DAL.Repositories;

internal class UserRepository : IUserRepository
{
    private readonly Microsoft.Azure.Cosmos.Container _container;
    private readonly IPasswordHasher<User> _hasher;
    private readonly ILogger<UserRepository> _logger;

    public UserRepository(UserContainer container, IPasswordHasher<User> hasher, ILogger<UserRepository> logger)
    {
        _container = container.Container;
        _hasher = hasher;
        _logger = logger;
    }

    public async Task<User> AddUser(User user)
    {
        user.SetPassword(_hasher, user.Password.Value);
        
        var response = await _container.CreateItemAsync(user, new PartitionKey(user.Email.Value));
        
        CannotAddUserException.ThrowIfStatusCodeNotCreated(response.StatusCode);
        
        return response.Resource;
    }

    public async Task<User> GetUserByEmail(EmailAddress email)
    {
        var result = await _container.ReadItemAsync<User>(email.Value, new PartitionKey(email.Value));

        return result.Resource;
    }

    public async Task<User> SignIn(string requestMail, string requestPassword)
    {
        var user = await GetUserByEmail(requestMail);

        if (user is null)
        {
            return null;
        }

        var result = _hasher.VerifyHashedPassword(user, user.Password.Value, requestPassword);

        if (result == PasswordVerificationResult.Success)
        {
            return user;
        }

        _logger.LogWarning($"Invalid password for user {requestMail}!");

        return null;
    }
}