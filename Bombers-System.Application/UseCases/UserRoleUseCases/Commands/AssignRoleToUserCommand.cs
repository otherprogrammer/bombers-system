using Bombers_System.Domain.Entities;
using Bombers_System.Domain.Exceptions;
using Bombers_System.Domain.Ports;
using MediatR;

namespace Bombers_System.Application.UseCases.UserRoleUseCases.Commands;

public record AssignRoleToUserCommand(int UserId, int RoleId) : IRequest<AssignRoleToUserCommandResponse>;

public record AssignRoleToUserCommandResponse(int RoleId, int UserId, DateTime AssignedAt);

internal sealed class AssignRoleToUserCommandHandler : IRequestHandler<AssignRoleToUserCommand, AssignRoleToUserCommandResponse> 
{
    private readonly IUnitOfWork _unitOfWork;

    public AssignRoleToUserCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    
    public async Task<AssignRoleToUserCommandResponse> Handle(AssignRoleToUserCommand request, CancellationToken cancellationToken)
    {
        if (!await _unitOfWork.Users.ExistsByIdAsync(request.UserId, cancellationToken))
        {
            throw new NotFoundException("User does not exist.");
        }

        if (!await _unitOfWork.Roles.ExistsByIdAsync(request.RoleId, cancellationToken))
        {
            throw new NotFoundException("Role does not exist.");
        }

        if (await _unitOfWork.UserRoles.ExistsByIdsAsync(request.UserId, request.RoleId, cancellationToken))
        {
            throw new ConflictException("Role already assigned to user.");
        }

        var newUserRole = new UserRole()
        {
            RoleId = request.RoleId,
            UserId = request.UserId,
            AssignedAt = DateTime.UtcNow
        };

        await _unitOfWork.UserRoles.AddAsync(newUserRole, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new AssignRoleToUserCommandResponse(
            RoleId: newUserRole.RoleId,
            UserId: newUserRole.UserId,
            AssignedAt: newUserRole.AssignedAt);
    }
}