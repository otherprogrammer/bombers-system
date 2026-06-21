using Bombers_System.Domain.Entities;
using Bombers_System.Domain.Exceptions;
using Bombers_System.Domain.Ports;
using MediatR;

namespace Bombers_System.Application.UseCases.AuthUseCases.Commands;

public record RefreshTokenCommand(string RefreshToken) : IRequest<RefreshTokenResponse>;

public record RefreshTokenResponse(string AccessToken, string RefreshToken);

internal sealed class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, RefreshTokenResponse>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IJwtProvider _jwtProvider;
    
    public RefreshTokenCommandHandler(IUnitOfWork unitOfWork, IJwtProvider jwtProvider)
    {
        _unitOfWork = unitOfWork;
        _jwtProvider = jwtProvider;
    }

    public async Task<RefreshTokenResponse> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
    {
        var storedToken = await _unitOfWork.UserTokens.GetByValueAsync(request.RefreshToken, cancellationToken);

        if (storedToken == null)
        {
            throw new UnauthorizedException("Invalid refresh token.");
        }

        if (storedToken.ExpiresAt < DateTime.UtcNow)
        {
            throw new UnauthorizedException("Refresh token has expired.");
        }

        if (storedToken.IsRevoked)
        {
            throw new UnauthorizedException("Refresh token has been revoked.");
        }

        storedToken.IsRevoked = true;
        _unitOfWork.UserTokens.Update(storedToken);
        
        var user = await _unitOfWork.Users.GetByIdAsync(storedToken.UserId, cancellationToken);

        var roles = await _unitOfWork.UserRoles.GetRoleNamesByUserIdAsync(user.UserId, cancellationToken);
        var newAccessToken = _jwtProvider.GenerateToken(
            user.UserId.ToString(),
            user.Username,
            roles
            );
        var newRefreshToken = _jwtProvider.GenerateRefreshToken();

        var newUserToken = new UserToken()
        {
            UserId = user.UserId,
            TokenValue = newRefreshToken,
            ExpiresAt = DateTime.UtcNow.AddDays(15),
            IsRevoked = false,
        };

        await _unitOfWork.UserTokens.AddAsync(newUserToken, cancellationToken);
        await _unitOfWork.SaveChangesAsync();
        
        return new RefreshTokenResponse(newAccessToken, newRefreshToken);
    }
}