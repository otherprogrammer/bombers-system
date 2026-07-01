using Bombers_System.Domain.DTOs.User;
using Bombers_System.Domain.Exceptions;
using Bombers_System.Domain.Ports;
using MediatR;

namespace Bombers_System.Application.UseCases.UserUseCases.Queries;

public record GetUserByIdQuery(int UserId) : IRequest<UserDto>;

internal sealed class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, UserDto>
{
    private readonly IUnitOfWork _unitOfWork;
    
    public GetUserByIdQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<UserDto> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        var user = await _unitOfWork.Users.GetByIdAsync(request.UserId, cancellationToken);

        if (user == null)
        {
            throw new NotFoundException("User does not exist.");
        }

        return new UserDto()
        {
            UserId = user.UserId,
            Username =  user.Username,
            FirefighterId =  user.FirefighterId,
        };
    }
}