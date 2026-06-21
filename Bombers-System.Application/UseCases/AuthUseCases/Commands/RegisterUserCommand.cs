using Bombers_System.Domain.Entities;
using Bombers_System.Domain.Exceptions;
using Bombers_System.Domain.Ports;
using MediatR;

namespace Bombers_System.Application.UseCases.AuthUseCases.Commands;

public record RegisterUserCommand(string Username, string Password, int? FirefighterId) : IRequest<RegisterUserResponse>;

public record RegisterUserResponse(int UserId, string Username, int? FirefighterId);

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
            throw new ConflictException("Username already exists.");
        }

        if (request.FirefighterId != null)
        {
            if (!await _unitOfWork.Firefighters.ExistsByIdAsync(request.FirefighterId.Value!, cancellationToken))
            {
                throw new NotFoundException("Firefighter does not exist.");
            }

            if (await _unitOfWork.Users.ExistsByFirefighterIdAsync(request.FirefighterId.Value, cancellationToken))
            {
                throw new ConflictException("Firefighter is already linked to another user.");
            }
        }
        
        string passwordHash = _passwordHasher.Hash(request.Password);
        
        var newUser = new User()
        {
            Username = request.Username,
            PasswordHash = passwordHash,
            FirefighterId = request.FirefighterId
        };

        var userRole = new UserRole()
        {
            User = newUser,
            RoleId = 3,
            AssignedAt = DateTime.UtcNow
        };
        
        await _unitOfWork.Users.AddAsync(newUser, cancellationToken);
        await _unitOfWork.UserRoles.AddAsync(userRole, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new RegisterUserResponse(newUser.UserId, newUser.Username,  newUser.FirefighterId);
    }
}