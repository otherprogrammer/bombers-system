using Bombers_System.Domain.Entities;
using Bombers_System.Domain.Ports;
using MediatR;

namespace Bombers_System.Application.UseCases.AuthUseCases.Command;

public record RegisterUserCommand(string Username, string Password) : IRequest<RegisterUserResponse>;

public record RegisterUserResponse(int UserId, string Username);

internal sealed class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, RegisterUserResponse>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPasswordHasher _passwordHasher;
    
    public RegisterUserCommandHandler(IUnitOfWork unitOfWork, IPasswordHasher passwordHasher)
    {
        _unitOfWork = unitOfWork;
        _passwordHasher = passwordHasher;
    }

    public async Task<RegisterUserResponse> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        if (await _unitOfWork.Users.ExistsByUsernameAsync(request.Username, cancellationToken))
        {
            throw new InvalidOperationException("Username already exists");
        }
        
        string passwordHash = _passwordHasher.Hash(request.Password);
        
        var newUser = new User()
        {
            Username = request.Username,
            PasswordHash = passwordHash,
        };
        
        await _unitOfWork.Users.AddAsync(newUser, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new RegisterUserResponse(newUser.UserId, newUser.Username);
    }
}