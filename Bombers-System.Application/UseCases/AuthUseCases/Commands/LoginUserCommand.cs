using System.Security.Authentication;
using Bombers_System.Domain.Entities;
using Bombers_System.Domain.Ports;
using MediatR;

namespace Bombers_System.Application.UseCases.AuthUseCases.Commands;

public record LoginUserCommand(string Username, string Password) : IRequest<LoginUserResponse>;

public record LoginUserResponse(string AccessToken, string RefreshToken);

internal sealed class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, LoginUserResponse>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IJwtProvider _jwtProvider;
    private readonly IPasswordHasher _passwordHasher;

    public LoginUserCommandHandler(IUnitOfWork unitOfWork, IJwtProvider jwtProvider, IPasswordHasher passwordHasher)
    {
        _unitOfWork = unitOfWork;
        _jwtProvider = jwtProvider;
        _passwordHasher = passwordHasher;
    }

    public async Task<LoginUserResponse> Handle(LoginUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _unitOfWork.Users.GetByUsernameAsync(request.Username, cancellationToken);
        if (user == null) throw new InvalidCredentialException("Invalid username or password.");

        bool isPasswordValid = _passwordHasher.Verify(request.Password, user.PasswordHash);
        if (!isPasswordValid) throw new InvalidCredentialException("Invalid username or password.");
        
        var roles = await _unitOfWork.UserRoles.GetRoleNamesByUserIdAsync(user.UserId, cancellationToken);

        var accessToken = _jwtProvider.GenerateToken(
            user.UserId.ToString(),
            user.Username,
            roles);

        var refreshToken = _jwtProvider.GenerateRefreshToken();

        var userToken = new UserToken()
        {
            UserId = user.UserId,
            TokenValue = refreshToken,
            ExpiresAt = DateTime.UtcNow.AddDays(15),
            IsRevoked = false,
        };
        
        await _unitOfWork.UserTokens.AddAsync(userToken, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        return new LoginUserResponse(accessToken, refreshToken);
    }
}