using Bombers_System.Domain.DTOs.Vehicle;
using Bombers_System.Domain.Ports;
using MediatR;

namespace Bombers_System.Application.UseCases.VehicleUseCases.Queries;

public class GetVehicleByIdQuery : IRequest<VehicleDto?>
{
    public int VehicleId { get; set; }
    public GetVehicleByIdQuery(int vehicleId) => VehicleId = vehicleId;
}

internal sealed class GetVehicleByIdQueryHandler : IRequestHandler<GetVehicleByIdQuery, VehicleDto?>
{
    private readonly IVehicleRepository _repository;

    public GetVehicleByIdQueryHandler(IVehicleRepository repository)
    {
        _repository = repository;
    }

    public async Task<VehicleDto?> Handle(GetVehicleByIdQuery request, CancellationToken cancellationToken)
    {
        var vehicle = await _repository.GetByIdAsync(request.VehicleId, cancellationToken);
        if (vehicle is null) return null;
        return new VehicleDto
        {
            VehicleId = vehicle.VehicleId,
            StationId = vehicle.StationId,
            WaterLevelGallons = vehicle.WaterLevelGallons,
            LastMaintenanceDate = vehicle.LastMaintenanceDate,
            RadioCode = vehicle.RadioCode,
            VehicleType = vehicle.VehicleType,
            OperationalStatus = vehicle.OperationalStatus
        };
    }
}
