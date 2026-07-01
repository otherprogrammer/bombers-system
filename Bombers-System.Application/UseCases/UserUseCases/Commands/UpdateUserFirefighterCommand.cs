using Bombers_System.Domain.Exceptions;
using Bombers_System.Domain.Ports;
using MediatR;

namespace Bombers_System.Application.UseCases.UserUseCases.Commands;

public record UpdateUserFirefighterCommand(int UserId, int? FirefighterId) : IRequest<UpdateUserFirefighterCommandResponse>;

public record UpdateUserFirefighterCommandResponse(int UserId, string Username, int? FirefighterId);

internal sealed class UpdateUserFirefighterCommandHandler : IRequestHandler<UpdateUserFirefighterCommand, UpdateUserFirefighterCommandResponse>
{
    private readonly IUnitOfWork _unitOfWork;
    
    public UpdateUserFirefighterCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<UpdateUserFirefighterCommandResponse> Handle(UpdateUserFirefighterCommand request, CancellationToken cancellationToken)
    {
        var user = await _unitOfWork.Users.GetByIdAsync(request.UserId, cancellationToken);

        if (user == null)
        {
            throw new NotFoundException("User does not exist.");
        }
        
        if (request.FirefighterId == user.FirefighterId)
        {
            return new UpdateUserFirefighterCommandResponse(
                user.UserId, 
                user.Username, 
                user.FirefighterId);
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
        
        user.FirefighterId = request.FirefighterId;
        
        _unitOfWork.Users.Update(user);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new UpdateUserFirefighterCommandResponse(
            user.UserId, 
            user.Username, 
            user.FirefighterId);
    }
}