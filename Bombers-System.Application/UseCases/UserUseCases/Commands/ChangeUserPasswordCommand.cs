using Bombers_System.Domain.Exceptions;
using Bombers_System.Domain.Ports;
using MediatR;

namespace Bombers_System.Application.UseCases.UserUseCases.Commands;

public record ChangeUserPasswordCommand(int UserId, string Password) : IRequest<ChangeUserPasswordCommandResponse>;

public record ChangeUserPasswordCommandResponse(int UserId, string Username, int? FirefighterId);

internal sealed class ChangeUserPasswordCommandHandler : IRequestHandler<ChangeUserPasswordCommand, ChangeUserPasswordCommandResponse>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPasswordHasher _passwordHasher;

    public ChangeUserPasswordCommandHandler(IUnitOfWork unitOfWork, IPasswordHasher passwordHasher)
    {
        _unitOfWork = unitOfWork;
        _passwordHasher = passwordHasher;
    }

    public async Task<ChangeUserPasswordCommandResponse> Handle(ChangeUserPasswordCommand request, CancellationToken cancellationToken)
    {
        var user = await _unitOfWork.Users.GetByIdAsync(request.UserId, cancellationToken);

        if (user == null)
        {
            throw new NotFoundException("User does not exist.");
        }
        
        user.PasswordHash = _passwordHasher.Hash(request.Password);
        
        _unitOfWork.Users.Update(user);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        return new ChangeUserPasswordCommandResponse(
            user.UserId, 
            user.Username, 
            user.FirefighterId);
    }
}