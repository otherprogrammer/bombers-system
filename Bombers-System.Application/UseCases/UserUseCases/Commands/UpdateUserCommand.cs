using Bombers_System.Domain.Exceptions;
using Bombers_System.Domain.Ports;
using MediatR;

namespace Bombers_System.Application.UseCases.UserUseCases.Commands;

public record UpdateUserCommand(int UserId, string Username, int? FirefighterId) : IRequest<UpdateUserCommandResponse>;

public record UpdateUserCommandResponse(int UserId, string Username, int? FirefighterId);

internal sealed class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, UpdateUserCommandResponse>
{
    private readonly IUnitOfWork _unitOfWork;

    public UpdateUserCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<UpdateUserCommandResponse> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _unitOfWork.Users.GetByIdAsync(request.UserId,cancellationToken);

        if (user == null)
        {
            throw new NotFoundException("User does not exist");
        }

        if (request.Username != user.Username)
        {
            if (await _unitOfWork.Users.ExistsByUsernameAsync(request.Username, cancellationToken))
            {
                throw new ConflictException("Username already exists.");
            }
        }

        if (request.FirefighterId != null && user.FirefighterId != request.FirefighterId)
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
        
        user.Username = request.Username;
        user.FirefighterId = request.FirefighterId;
        
        _unitOfWork.Users.Update(user);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new UpdateUserCommandResponse(
            user.UserId, 
            user.Username, 
            user.FirefighterId);
    }
}