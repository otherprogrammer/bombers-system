using Bombers_System.Domain.DTOs.Role;
using Bombers_System.Domain.Exceptions;
using Bombers_System.Domain.Ports;
using MediatR;

namespace Bombers_System.Application.UseCases.RoleUseCases.Commands;

public record UpdateRoleCommand(int RoleId, string RoleName) : IRequest<RoleDto>;

internal sealed class UpdateRoleCommandHandler : IRequestHandler<UpdateRoleCommand, RoleDto>
{
    private readonly IUnitOfWork _unitOfWork;
    
    public UpdateRoleCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<RoleDto> Handle(UpdateRoleCommand request, CancellationToken cancellationToken)
    {
        var role = await _unitOfWork.Roles.GetByIdAsync(request.RoleId, cancellationToken);

        if (role == null)
        {
            throw new NotFoundException($"Role with ID {request.RoleId} does not exist.");
        }
        
        role.RoleName = request.RoleName;

        _unitOfWork.Roles.Update(role);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new RoleDto()
        {
            RoleId = role.RoleId,
            RoleName = role.RoleName
        };
    }
}