using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Users.Core.Entities;
using Users.Core.Repositories;
using Users.Infrastructure.DAL.DbContext;

namespace Users.Infrastructure.DAL.Repositories;

public class UserRepository : IUserRepository
{
    private readonly UserDbContext _dbContext;
    private readonly IPasswordHasher<User> _hasher;
    private readonly ILogger<UserRepository> _logger;

    public UserRepository(UserDbContext dbContext,IPasswordHasher<User> hasher, ILogger<UserRepository> logger)
    {
        _dbContext = dbContext;
        _hasher = hasher;
        _logger = logger;
    }
    public Task AddUser(User user)
    {
        user.SetPassword(_hasher,user.Password.Value);
        
        _dbContext.User.Add(user);
        
        return _dbContext.SaveChangesAsync();
    }

    public Task<User> GetUserByEmail(string email)
    {
        return _dbContext.User.AsNoTracking().FirstOrDefaultAsync(u => u.Email == email);
    }

    public async Task<User> GetUserByEmail(string requestMail, string requestPassword)
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