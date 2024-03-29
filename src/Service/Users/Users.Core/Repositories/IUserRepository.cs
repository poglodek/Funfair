﻿using Users.Core.ValueObjects;
using User = Users.Core.Entities.User;

namespace Users.Core.Repositories;

public interface IUserRepository
{
    Task AddUser(User user, CancellationToken cancellationToken);
    Task<User?> GetUserByEmail(EmailAddress email, CancellationToken cancellationToken);
    Task<User> SignIn(string requestMail, string requestPassword, CancellationToken cancellationToken);
}