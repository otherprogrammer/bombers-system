using System.Security.Authentication;
using Bombers_System.Domain.Ports;
using MediatR;

namespace Bombers_System.Application.UseCases.AuthUseCases.Commands;

public record LogoutUserCommand(string RefreshToken) : IRequest;

internal sealed class LogoutUserCommandHandler : IRequestHandler<LogoutUserCommand>
{
    public readonly IUnitOfWork _unitOfWork;

    public LogoutUserCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(LogoutUserCommand request, CancellationToken cancellationToken)
    {
        var storedToken = await _unitOfWork.UserTokens.GetByValueAsync(request.RefreshToken);

        if (storedToken == null)
        {
            throw new InvalidCredentialException("Invalid refresh token.");
        }

        if (storedToken.IsRevoked)
        {
            throw new InvalidCredentialException("Refresh token has already been revoked.");
        }

        if (storedToken.ExpiresAt < DateTime.UtcNow)
        {
            throw new InvalidCredentialException("Refresh token has expired.");
        }
        
        storedToken.IsRevoked = true;
        
        _unitOfWork.UserTokens.Update(storedToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}