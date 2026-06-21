using Bombers_System.Domain.DTOs.Role;
using Bombers_System.Domain.Ports;
using MediatR;

namespace Bombers_System.Application.UseCases.RoleUseCases.Queries;

public record GetRolesQuery : IRequest<IEnumerable<RoleDto>>;

internal sealed class GetRolesQueryHandler : IRequestHandler<GetRolesQuery, IEnumerable<RoleDto>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetRolesQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<RoleDto>> Handle(GetRolesQuery request, CancellationToken cancellationToken)
    {
        var roles = await _unitOfWork.Roles.GetAllAsync(cancellationToken);
        
        return roles.Select(r => new RoleDto()
        {
            RoleId = r.RoleId,
            RoleName = r.RoleName
        });
    }
}