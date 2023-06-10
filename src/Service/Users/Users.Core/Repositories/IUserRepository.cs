﻿using Users.Core.Entities;

namespace Users.Core.Repositories;

public interface IUserRepository
{
    Task AddUser(User user);
    Task<User> GetUserByEmail(string email);
    Task<User> GetUserByEmail(string requestMail, string requestPassword);
}