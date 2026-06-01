using Bombers_System.Domain.DTOs.Vehicle;
using Bombers_System.Domain.Ports;
using MediatR;

namespace Bombers_System.Application.UseCases.VehicleUseCases.Queries;

public class GetVehiclesByStationQuery : IRequest<IEnumerable<VehicleDto>>
{
    public int StationId { get; set; }
    public GetVehiclesByStationQuery(int stationId) => StationId = stationId;
}

internal sealed class GetVehiclesByStationQueryHandler : IRequestHandler<GetVehiclesByStationQuery, IEnumerable<VehicleDto>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetVehiclesByStationQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<VehicleDto>> Handle(GetVehiclesByStationQuery request, CancellationToken cancellationToken)
    {
        var vehicles = await _unitOfWork.Vehicles.GetByStationIdAsync(request.StationId, cancellationToken);
        return vehicles.Select(v => new VehicleDto
        {
            VehicleId = v.VehicleId,
            StationId = v.StationId,
            WaterLevelGallons = v.WaterLevelGallons,
            LastMaintenanceDate = v.LastMaintenanceDate,
            RadioCode = v.RadioCode,
            VehicleType = v.VehicleType,
            OperationalStatus = v.OperationalStatus
        });
    }
}