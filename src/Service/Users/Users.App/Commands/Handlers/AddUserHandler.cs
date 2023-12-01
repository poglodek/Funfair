using MediatR;
using Users.App.Exceptions;
using Users.Core.Entities;
using Users.Core.Repositories;

namespace Users.App.Commands.Handlers;

public class AddUserHandler : IRequestHandler<AddUserCommand,Unit>
{
    private readonly IUserRepository _userRepository;

    public AddUserHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }
    
    public async Task<Unit> Handle(AddUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetUserByEmail(request.Email);
        
        if (user is not null)
        {
            throw new UserExistsException(request.Email);
        }
        
        var newUser = User.CreateInstance(request.FirstName, request.LastName, request.DateOfBirth, DateTime.Now, request.Email, request.Password,new Role());
        
        _userRepository.AddUser(newUser);

        return Unit.Value;
    }
}