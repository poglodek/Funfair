using Shouldly;
using Users.Core.Entities;
using Users.Core.Events;
using Users.Core.Exceptions;

namespace Funfair.Core.Tests;

public class UserTest
{
    [Fact]
    public void CreateUser_AllValid_ShouldReturnUser()
    {
        var user = User.CreateInstance("Jack", "Sparrow",
            new DateTime(1980, 1, 1),
            new DateTime(2021, 1, 1), "jack@google.com","SuperPassword12",new Role());
        
        user.FirstName.Value.ShouldBe("Jack");
        user.LastName.Value.ShouldBe("Sparrow");
        user.DateOfBirth.Value.ShouldBe(new DateTime(1980, 1, 1));
        user.CreatedAt.Value.ShouldBe(new DateTime(2021, 1, 1));
        user.Email.Value.ShouldBe("jack@google.com");
        user.Password.Value.ShouldBe("SuperPassword12");
        user.Role.Name.ShouldBe(new Role().Name);
    }
    
    [Fact]
    public void CreateUser_DomainEvents_ShouldReturnUser()
    {
        var user = User.CreateInstance("Jack", "Sparrow",
            new DateTime(1980, 1, 1),
            new DateTime(2021, 1, 1), "jack@google.com","SuperPassword12",new Role());
        
        user.DomainEvents.Count.ShouldBe(1);

        var @event =  user.DomainEvents.FirstOrDefault() as SignedUp;

        @event.ShouldNotBeNull();
        
        @event.Id.ShouldBe(user.Id.Value);
        @event.Email.ShouldBe(user.Email.Value);
        @event.FirstName.ShouldBe(user.FirstName.Value);
        @event.LastName.ShouldBe(user.LastName.Value);

    }
    
    [Fact]
    public void CreateUser_FirstNameInValid_ShouldThrowAnException()
    {
        Should.Throw<InvalidNameException>(() => User.CreateInstance(string.Empty, "Sparrow",
            new DateTime(1980, 1, 1),
            new DateTime(2021, 1, 1), "jack@google.com","SuperPassword12",new Role()));
        
    }
    
    [Fact]
    public void CreateUser_LastNameInValid_ShouldThrowAnException()
    {
        Should.Throw<InvalidNameException>(() => User.CreateInstance("Jack", string.Empty,
            new DateTime(1980, 1, 1),
            new DateTime(2021, 1, 1), "jack@google.com","SuperPassword12",new Role()));
        
    }
    
    [Fact]
    public void CreateUser_DateOfBirthInValidMax_ShouldThrowAnException()
    {
        Should.Throw<InvalidDateException>(() => User.CreateInstance("Jack", "Sparrow",
            DateTime.MaxValue,
            new DateTime(2021, 1, 1), "jack@google.com","SuperPassword12",new Role()));
        
    }
    
    [Fact]
    public void CreateUser_DateOfBirthInValidMin_ShouldThrowAnException()
    {
        Should.Throw<InvalidDateException>(() => User.CreateInstance("Jack", "Sparrow",
            DateTime.MinValue,
            new DateTime(2021, 1, 1), "jack@google.com","SuperPassword12",new Role()));
        
    }
    
    [Fact]
    public void CreateUser_InvalidEmail_ShouldThrowAnException()
    {
        Should.Throw<InvalidEmailAddressException>(() => User.CreateInstance("Jack", "Sparrow",
            new DateTime(1980, 1, 1),
            new DateTime(2021, 1, 1), "jackgoogle.com","SuperPassword12",new Role()));
        
    }
    
    [Fact]
    public void CreateUser_InvalidEmailEmpty_ShouldThrowAnException()
    {
        Should.Throw<InvalidEmailAddressException>(() => User.CreateInstance("Jack", "Sparrow",
            new DateTime(1980, 1, 1),
            new DateTime(2021, 1, 1), "","SuperPassword12",new Role()));
        
    }
    
    [Fact]
    public void CreateUser_InvalidEmailOnlyAt_ShouldThrowAnException()
    {
        Should.Throw<InvalidEmailAddressException>(() => User.CreateInstance("Jack", "Sparrow",
            new DateTime(1980, 1, 1),
            new DateTime(2021, 1, 1), "jack@","SuperPassword12",new Role()));
        
    }
    [Fact]
    public void CreateUser_InvalidPasswordShort_ShouldThrowAnException()
    {
        Should.Throw<InvalidPasswordException>(() => User.CreateInstance("Jack", "Sparrow",
            new DateTime(1980, 1, 1),
            new DateTime(2021, 1, 1), "jack@google.com","1",new Role()));
        
    }
    
    [Fact]
    public void CreateUser_InvalidPasswordEmpty_ShouldThrowAnException()
    {
        Should.Throw<InvalidPasswordException>(() => User.CreateInstance("Jack", "Sparrow",
            new DateTime(1980, 1, 1),
            new DateTime(2021, 1, 1), "jack@google.com","",new Role()));
        
    }
}