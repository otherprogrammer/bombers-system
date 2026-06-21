using Bombers_System.Domain.DTOs.Role;
using Bombers_System.Domain.Exceptions;
using Bombers_System.Domain.Ports;
using MediatR;

namespace Bombers_System.Application.UseCases.UserRoleUseCases.Queries;

public record GetRolesByUserIdQuery(int UserId) : IRequest<IEnumerable<RoleDto>>;

internal sealed class GetRolesByUserIdQueryHandler : IRequestHandler<GetRolesByUserIdQuery, IEnumerable<RoleDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    
    public GetRolesByUserIdQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<RoleDto>> Handle(GetRolesByUserIdQuery request, CancellationToken cancellationToken)
    {
        if (!await _unitOfWork.Users.ExistsByIdAsync(request.UserId, cancellationToken))
        {
            throw new NotFoundException("User does not exist.");
        }
        
        var roles = await _unitOfWork.UserRoles.GetRolesByUserIdAsync(request.UserId, cancellationToken);

        return roles.Select(r => new RoleDto()
        {
            RoleId = r.RoleId,
            RoleName = r.RoleName
        });
    }
}