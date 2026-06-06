using Bombers_System.Domain.DTOs.User;
using Bombers_System.Domain.Exceptions;
using Bombers_System.Domain.Ports;
using MediatR;

namespace Bombers_System.Application.UseCases.AuthUseCases.Queries;

public record GetCurrentUserQuery (int UserId) : IRequest<UserDto>;

internal sealed class GetCurrentUserQueryHandler : IRequestHandler<GetCurrentUserQuery, UserDto>
{
    private readonly IUnitOfWork _unitOfWork;
    
    public  GetCurrentUserQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<UserDto> Handle(GetCurrentUserQuery request, CancellationToken cancellationToken)
    {
        var user = await _unitOfWork.Users.GetByIdAsync(request.UserId);

        if (user == null)
        {
            throw new NotFoundException("Session is no longer valid.");
        }

        return new UserDto()
        {
            Username = user.Username,
            UserId = user.UserId,
            FirefighterId =  user.FirefighterId,
        };
    }
}