using Bombers_System.Domain.Exceptions;
using Bombers_System.Domain.Ports;
using MediatR;

namespace Bombers_System.Application.UseCases.UserUseCases.Commands;

public record UpdateUsernameCommand(int UserId, string Username) : IRequest<UpdateUsernameCommandResponse>;

public record UpdateUsernameCommandResponse(int UserId, string Username, int? FirefighterId);

internal sealed class UpdateUsernameCommandHandler : IRequestHandler<UpdateUsernameCommand, UpdateUsernameCommandResponse>
{
    private readonly IUnitOfWork _unitOfWork;
    
    public UpdateUsernameCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<UpdateUsernameCommandResponse> Handle(UpdateUsernameCommand request, CancellationToken cancellationToken)
    {
        var user = await _unitOfWork.Users.GetByIdAsync(request.UserId, cancellationToken);

        if (user == null)
        {
            throw new NotFoundException("User does not exist.");
        }
        
        if (request.Username == user.Username)
        {
            return new UpdateUsernameCommandResponse(
                user.UserId, 
                user.Username, 
                user.FirefighterId);
        }
        
        if (await _unitOfWork.Users.ExistsByUsernameAsync(request.Username, cancellationToken))
        {
            throw new ConflictException("Username already exists.");
        }
        
        user.Username = request.Username;
        
        _unitOfWork.Users.Update(user);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new UpdateUsernameCommandResponse(
            user.UserId, 
            user.Username, 
            user.FirefighterId);
    }
}