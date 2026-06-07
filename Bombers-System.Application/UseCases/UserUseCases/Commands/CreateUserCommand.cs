using Bombers_System.Domain.Entities;
using Bombers_System.Domain.Exceptions;
using Bombers_System.Domain.Ports;
using MediatR;

namespace Bombers_System.Application.UseCases.UserUseCases.Commands;

public record CreateUserCommand(string Username, string Password, int? FirefighterId) : IRequest<CreateUserCommandResponse>;

public record CreateUserCommandResponse(int UserId, string Username, int? FirefighterId);

internal sealed class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, CreateUserCommandResponse>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPasswordHasher _passwordHasher;
    
    public CreateUserCommandHandler(IUnitOfWork unitOfWork,  IPasswordHasher passwordHasher)
    {
        _unitOfWork = unitOfWork;
        _passwordHasher = passwordHasher;
    }

    public async Task<CreateUserCommandResponse> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        if (await _unitOfWork.Users.ExistsByUsernameAsync(request.Username, cancellationToken))
        {
            throw new ConflictException("Username already exists.");
        }

        if (request.FirefighterId != null)
        {
            if (!await _unitOfWork.Firefighters.ExistsByIdAsync(request.FirefighterId.Value, cancellationToken))
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
        
        await _unitOfWork.Users.AddAsync(newUser, cancellationToken);
        await _unitOfWork.Users.AssignRoleAsync(newUser, 3);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new CreateUserCommandResponse(
            newUser.UserId, 
            newUser.Username, 
            newUser.FirefighterId);
    }
}