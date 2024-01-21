
using Users.Core.ValueObjects;
using User = Users.Core.Entities.User;

namespace Users.Core.Repositories;

public interface IUserRepository
{
    Task AddUser(User user);
    Task<User?> GetUserByEmail(EmailAddress email);
    Task<User> SignIn(string requestMail, string requestPassword);
}