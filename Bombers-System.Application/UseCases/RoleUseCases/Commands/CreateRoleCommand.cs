using Bombers_System.Domain.DTOs.Role;
using Bombers_System.Domain.Entities;
using Bombers_System.Domain.Ports;
using MediatR;

namespace Bombers_System.Application.UseCases.RoleUseCases.Commands;

public record CreateRoleCommand(string RoleName) : IRequest<RoleDto>;

internal sealed class CreateRoleCommandHandler : IRequestHandler<CreateRoleCommand, RoleDto>
{
    private readonly IUnitOfWork _unitOfWork;
    
    public  CreateRoleCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<RoleDto> Handle(CreateRoleCommand request, CancellationToken cancellationToken)
    {
        var role = new Role()
        {
            RoleName = request.RoleName
        };
        
        await _unitOfWork.Roles.AddAsync(role,cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new RoleDto()
        {
            RoleId = role.RoleId,
            RoleName = role.RoleName
        };
    }
}