using Bombers_System.Domain.Ports;
using MediatR;

namespace Bombers_System.Application.UseCases.AuthUseCases.Command;

public record LoginUserCommand(string Username, string Password) : IRequest<string?>;

internal sealed class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, string?>
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

    public async Task<string?> Handle(LoginUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _unitOfWork.Users.GetByUsernameAsync(request.Username, cancellationToken);
        if (user == null) return null;

        bool isPasswordValid = _passwordHasher.Verify(request.Password, user.PasswordHash);
        if (!isPasswordValid) return null;
        
        var roles = user.UserRoles.Select(ur => ur.Role.RoleName);

        return _jwtProvider.GenerateToken(
            user.UserId.ToString(),
            user.Username,
            roles
        );
    }
}