using Bombers_System.Domain.DTOs.Role;
using Bombers_System.Domain.Exceptions;
using Bombers_System.Domain.Ports;
using MediatR;

namespace Bombers_System.Application.UseCases.RoleUseCases.Queries;

public record GetRoleByIdQuery(int RoleId) : IRequest<RoleDto>;

internal sealed class GetRoleByIdQueryHandler : IRequestHandler<GetRoleByIdQuery, RoleDto>
{
    private readonly IUnitOfWork _unitOfWork;
    
    public GetRoleByIdQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<RoleDto> Handle(GetRoleByIdQuery request, CancellationToken cancellationToken)
    {
        var role = await _unitOfWork.Roles.GetByIdAsync(request.RoleId, cancellationToken);

        if (role == null)
        {
            throw new NotFoundException("Role does not exist.");
        }

        return new RoleDto()
        {
            RoleId = role.RoleId,
            RoleName = role.RoleName,
        };
    }
}