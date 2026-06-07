using Bombers_System.Domain.Exceptions;
using Bombers_System.Domain.Ports;
using MediatR;

namespace Bombers_System.Application.UseCases.RoleUseCases.Commands;

public record DeleteRoleCommand(int RoleId) : IRequest;

internal sealed class DeleteRoleCommandHandler : IRequestHandler<DeleteRoleCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    
    public DeleteRoleCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    
    public async Task Handle(DeleteRoleCommand request, CancellationToken cancellationToken)
    {
        var role = await _unitOfWork.Roles.GetByIdAsync(request.RoleId, cancellationToken);

        if (role == null)
        {
            throw new NotFoundException("Role does not exist.");
        }

        _unitOfWork.Roles.Delete(role);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}