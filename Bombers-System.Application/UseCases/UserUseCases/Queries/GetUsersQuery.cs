using Bombers_System.Domain.DTOs.User;
using Bombers_System.Domain.Ports;
using MediatR;

namespace Bombers_System.Application.UseCases.UserUseCases.Queries;

public record GetUsersQuery : IRequest<IEnumerable<UserDto>>;

internal sealed class GetUsersQueryHandler : IRequestHandler<GetUsersQuery, IEnumerable<UserDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    
    public GetUsersQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<UserDto>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
    {
        var users = await _unitOfWork.Users.GetAllAsync(cancellationToken);

        return users.Select(u => new UserDto()
        {
            UserId = u.UserId,
            Username = u.Username,
            FirefighterId = u.FirefighterId,
        });
    }
}