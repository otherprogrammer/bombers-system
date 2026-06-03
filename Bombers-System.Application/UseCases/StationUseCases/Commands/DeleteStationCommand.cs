using Bombers_System.Domain.Ports;
using MediatR;

namespace Bombers_System.Application.UseCases.StationUseCases.Commands;

public class DeleteStationCommand : IRequest<bool>
{
    public int StationId { get; set; }
    public DeleteStationCommand(int stationId) => StationId = stationId;
}

internal sealed class DeleteStationCommandHandler : IRequestHandler<DeleteStationCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeleteStationCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(DeleteStationCommand request, CancellationToken cancellationToken)
    {
        var station = await _unitOfWork.Stations.GetByIdAsync(request.StationId, cancellationToken);
        if (station is null) return false;

        await _unitOfWork.Stations.DeleteAsync(station);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return true;
    }
}
