using Bombers_System.Domain.Ports;
using MediatR;

namespace Bombers_System.Application.UseCases.FirefighterUseCases.Commands;

public class DeleteFirefighterCommand : IRequest<bool>
{
    public int FirefighterId { get; set; }
    public DeleteFirefighterCommand(int id) => FirefighterId = id;
}

internal sealed class DeleteFirefighterCommandHandler : IRequestHandler<DeleteFirefighterCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeleteFirefighterCommandHandler(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

    public async Task<bool> Handle(DeleteFirefighterCommand request, CancellationToken cancellationToken)
    {
        var firefighter = await _unitOfWork.Firefighters.GetByIdAsync(request.FirefighterId, cancellationToken);
        if (firefighter is null) return false;

        await _unitOfWork.Firefighters.DeleteAsync(firefighter);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return true;
    }
}