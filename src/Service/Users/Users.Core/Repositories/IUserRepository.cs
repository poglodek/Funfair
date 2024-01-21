using Funfair.Shared.Core.Repository;
using Microsoft.Azure.Cosmos;
using Users.Core.ValueObjects;
using User = Users.Core.Entities.User;

namespace Users.Core.Repositories;

public interface IUserRepository 
{
    void AddUser(User user, TransactionalBatch? batch = null);
    Task<User?> GetUserByEmail(EmailAddress email);
    Task<User> SignIn(string requestMail, string requestPassword);
}