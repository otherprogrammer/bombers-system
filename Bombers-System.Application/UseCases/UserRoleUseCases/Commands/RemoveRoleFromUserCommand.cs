using Bombers_System.Domain.Exceptions;
using Bombers_System.Domain.Ports;
using MediatR;

namespace Bombers_System.Application.UseCases.UserRoleUseCases.Commands;

public record RemoveRoleFromUserCommand(int UserId, int RoleId) : IRequest;

internal sealed class RemoveRoleFromUserCommandHandler : IRequestHandler<RemoveRoleFromUserCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    
    public RemoveRoleFromUserCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(RemoveRoleFromUserCommand request, CancellationToken cancellationToken)
    {
        if (!await _unitOfWork.Roles.ExistsByIdAsync(request.RoleId, cancellationToken))
        {
            throw new NotFoundException("Role does not exist.");
        }

        if (!await _unitOfWork.Users.ExistsByIdAsync(request.UserId, cancellationToken))
        {
            throw new NotFoundException("User does not exist.");
        }
        
        var userRole = await _unitOfWork.UserRoles.GetByIdsAsync(request.UserId, request.RoleId, cancellationToken);
        
        if (userRole == null)
        {
            throw new NotFoundException("Role is not assigned to this user.");
        }
        
        _unitOfWork.UserRoles.Delete(userRole);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}