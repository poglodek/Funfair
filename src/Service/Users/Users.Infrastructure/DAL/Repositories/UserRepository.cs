using Microsoft.AspNetCore.Identity;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Linq;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Users.Core.Repositories;
using Users.Core.ValueObjects;
using Users.Infrastructure.DAL.Container;
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
    
    public Task AddUser(User user, CancellationToken cancellationToken)
    {
        user.SetPassword(_hasher, user.Password.Value);
        return _container.CreateItemAsync(user, cancellationToken: cancellationToken);
    }
    

    public async Task<User?> GetUserByEmail(EmailAddress email, CancellationToken cancellationToken)
    {
        var iterator = _container.GetItemLinqQueryable<User>()
            .Where(x=>x.Email.Value == email.Value)
            .ToFeedIterator();

        if (iterator.HasMoreResults)
        {
            foreach (var user in await iterator.ReadNextAsync(cancellationToken))
            {
                return user;
            }
        }
        
        return null;
    }

    public async Task<User> SignIn(string requestMail, string requestPassword, CancellationToken cancellationToken)
    {
        var user = await GetUserByEmail(requestMail,cancellationToken);

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