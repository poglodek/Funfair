using System.Text.Json;
using Microsoft.AspNetCore.Identity;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Linq;
using Microsoft.Extensions.Logging;
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
//TODO: fix this
    public Task AddUser(User user)
    {
       // user.SetPassword(_hasher, user.Password.Value);
       
       var json = JsonSerializer.Serialize(user);
        
        return _container.CreateItemAsync(json,new PartitionKey(user.PartitionKey));
    }

    public async Task<User?> GetUserByEmail(EmailAddress email)
    {
        var iterator = _container.GetItemLinqQueryable<User>()
            .ToFeedIterator();

        if (iterator.HasMoreResults)
        {
            foreach (var user in await iterator.ReadNextAsync())
            {
                return user;
            }
        }
        
        return null;
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