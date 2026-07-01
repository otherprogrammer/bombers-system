using Bombers_System.Domain.DTOs.User;
using Bombers_System.Domain.Exceptions;
using Bombers_System.Domain.Ports;
using MediatR;

namespace Bombers_System.Application.UseCases.UserRoleUseCases.Queries;

public record GetUsersByRoleIdQuery(int RoleId) : IRequest<IEnumerable<UserDto>>;

internal sealed class GetUsersByRoleIdQueryHandler : IRequestHandler<GetUsersByRoleIdQuery, IEnumerable<UserDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    
    public GetUsersByRoleIdQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<UserDto>> Handle(GetUsersByRoleIdQuery request, CancellationToken cancellationToken)
    {
        if (!await _unitOfWork.Roles.ExistsByIdAsync(request.RoleId, cancellationToken))
        {
            throw new NotFoundException("Role does not exist.");
        }

        var users = await _unitOfWork.UserRoles.GetUsersByRoleIdAsync(request.RoleId, cancellationToken);

        return users.Select(u => new UserDto()
        {
            UserId = u.UserId,
            Username = u.Username,
            FirefighterId = u.FirefighterId
        });
    }
}