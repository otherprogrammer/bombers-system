using Bombers_System.Domain.DTOs.Role;
using Bombers_System.Domain.Exceptions;
using Bombers_System.Domain.Ports;
using MediatR;

namespace Bombers_System.Application.UseCases.RoleUseCases.Commands;

public record UpdateRoleCommand(int RoleId, string RoleName) : IRequest<UpdateRoleResponse>;

public record UpdateRoleResponse(int RoleId, string RoleName);

internal sealed class UpdateRoleCommandHandler : IRequestHandler<UpdateRoleCommand, UpdateRoleResponse>
{
    private readonly IUnitOfWork _unitOfWork;
    
    public UpdateRoleCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<UpdateRoleResponse> Handle(UpdateRoleCommand request, CancellationToken cancellationToken)
    {
        var role = await _unitOfWork.Roles.GetByIdAsync(request.RoleId, cancellationToken);

        if (role == null)
        {
            throw new NotFoundException("Role does not exist.");
        }

        if (request.RoleName != role.RoleName)
        {
            if (await _unitOfWork.Roles.ExistsByRoleNameAsync(request.RoleName, cancellationToken))
            {
                throw new ConflictException("Role already exists.");
            }
        }
        
        role.RoleName = request.RoleName;

        _unitOfWork.Roles.Update(role);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new UpdateRoleResponse(
            role.RoleId, 
            role.RoleName);
    }
}