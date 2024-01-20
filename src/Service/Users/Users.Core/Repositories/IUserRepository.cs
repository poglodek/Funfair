using Funfair.Shared.Core.Repository;
using Users.Core.Entities;
using Users.Core.ValueObjects;

namespace Users.Core.Repositories;

public interface IUserRepository 
{
    Task<User> AddUser(User user);
    Task<User> GetUserByEmail(EmailAddress email);
    Task<User> SignIn(string requestMail, string requestPassword);
}