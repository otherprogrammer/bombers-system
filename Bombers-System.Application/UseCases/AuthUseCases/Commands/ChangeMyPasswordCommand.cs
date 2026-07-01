using Bombers_System.Domain.Exceptions;
using Bombers_System.Domain.Ports;
using MediatR;

namespace Bombers_System.Application.UseCases.AuthUseCases.Commands;

public record ChangeMyPasswordCommand(int UserId, string OldPassword, string NewPassword) : IRequest;

internal sealed class ChangeMyPasswordCommandHandler : IRequestHandler<ChangeMyPasswordCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPasswordHasher _passwordHasher;

    public ChangeMyPasswordCommandHandler(IUnitOfWork unitOfWork, IPasswordHasher passwordHasher)
    {
        _unitOfWork = unitOfWork;
        _passwordHasher = passwordHasher;
    }

    public async Task Handle(ChangeMyPasswordCommand request, CancellationToken cancellationToken)
    {
        var user = await _unitOfWork.Users.GetByIdAsync(request.UserId, cancellationToken);

        if (user == null)
        {
            throw new NotFoundException("User does not exist.");
        }
        
        if (!_passwordHasher.Verify(user.PasswordHash, request.OldPassword))
        {
            throw new UnauthorizedException("Current password is incorrect.");
        }
        
        if (_passwordHasher.Verify(user.PasswordHash, request.NewPassword))
        {
            throw new ConflictException("New password must be different from current password.");
        }
        
        user.PasswordHash = _passwordHasher.Hash(request.NewPassword);
        
        _unitOfWork.Users.Update(user);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}