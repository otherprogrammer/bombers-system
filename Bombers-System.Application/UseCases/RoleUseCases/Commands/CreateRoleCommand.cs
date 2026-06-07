using Bombers_System.Domain.Entities;
using Bombers_System.Domain.Exceptions;
using Bombers_System.Domain.Ports;
using MediatR;

namespace Bombers_System.Application.UseCases.RoleUseCases.Commands;

public record CreateRoleCommand(string RoleName) : IRequest<CreateRoleResponse>;

public record CreateRoleResponse(int RoleId, string RoleName);

internal sealed class CreateRoleCommandHandler : IRequestHandler<CreateRoleCommand, CreateRoleResponse>
{
    private readonly IUnitOfWork _unitOfWork;
    
    public  CreateRoleCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<CreateRoleResponse> Handle(CreateRoleCommand request, CancellationToken cancellationToken)
    {
        if (await _unitOfWork.Roles.ExistsByRoleNameAsync(request.RoleName, cancellationToken))
        {
            throw new ConflictException("Role already exists.");
        }
        
        var role = new Role()
        {
            RoleName = request.RoleName
        };
        
        await _unitOfWork.Roles.AddAsync(role,cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new CreateRoleResponse(
            role.RoleId, 
            role.RoleName);
    }
}