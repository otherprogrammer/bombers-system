using Bombers_System.Domain.Interfaces;
using MediatR;

namespace Bombers_System.Application.UseCases.StationUseCases.Commands;

public class DeleteStationCommand : IRequest<bool>
{
    public int StationId { get; set; }
    public DeleteStationCommand(int stationId) => StationId = stationId;
}

internal sealed class DeleteStationCommandHandler : IRequestHandler<DeleteStationCommand, bool>
{
    private readonly IStationRepository _repository;

    public DeleteStationCommandHandler(IStationRepository repository)
    {
        _repository = repository;
    }

    public async Task<bool> Handle(DeleteStationCommand request, CancellationToken cancellationToken)
    {
        var station = await _repository.GetByIdAsync(request.StationId, cancellationToken);
        if (station is null) return false;

        _repository.Delete(station);
        await _repository.SaveChangesAsync(cancellationToken);
        return true;
    }
}
