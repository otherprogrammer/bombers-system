using Bombers_System.Domain.DTOs.Station;
using Bombers_System.Domain.Ports;
using MediatR;

namespace Bombers_System.Application.UseCases.StationUseCases.Commands;

public class UpdateStationCommand : IRequest<StationDto?>
{
    public int StationId { get; set; }
    public UpdateStationDto Dto { get; set; } = null!;
    public UpdateStationCommand(int stationId, UpdateStationDto dto) { StationId = stationId; Dto = dto; }
}

internal sealed class UpdateStationCommandHandler : IRequestHandler<UpdateStationCommand, StationDto?>
{
    private readonly IUnitOfWork _unitOfWork;

    public UpdateStationCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<StationDto?> Handle(UpdateStationCommand request, CancellationToken cancellationToken)
    {
        var station = await _unitOfWork.Stations.GetByIdAsync(request.StationId, cancellationToken);
        if (station is null) return null;

        station.StationNumber = request.Dto.StationNumber;
        station.Address = request.Dto.Address;
        station.VehicleCapacity = request.Dto.VehicleCapacity;

        _unitOfWork.Stations.Update(station);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new StationDto
        {
            StationId = station.StationId,
            StationNumber = station.StationNumber,
            Address = station.Address,
            VehicleCapacity = station.VehicleCapacity
        };
    }
}