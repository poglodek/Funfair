using Users.Core.Entities;
using Users.Core.ValueObjects;

namespace Users.Core.Repositories;

public interface IUserRepository
{
    Task AddUser(User user);
    Task<User> GetUserByEmail(EmailAddress email);
    Task<User> GetUserByEmail(string requestMail, string requestPassword);
}