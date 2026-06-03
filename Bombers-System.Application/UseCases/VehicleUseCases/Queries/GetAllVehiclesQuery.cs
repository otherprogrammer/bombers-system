using Bombers_System.Domain.DTOs.Vehicle;
using Bombers_System.Domain.Ports;
using MediatR;

namespace Bombers_System.Application.UseCases.VehicleUseCases.Queries;

public class GetAllVehiclesQuery : IRequest<IEnumerable<VehicleDto>>;

internal sealed class GetAllVehiclesQueryHandler : IRequestHandler<GetAllVehiclesQuery, IEnumerable<VehicleDto>>
{
    private readonly IVehicleRepository _repository;

    public GetAllVehiclesQueryHandler(IVehicleRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<VehicleDto>> Handle(GetAllVehiclesQuery request, CancellationToken cancellationToken)
    {
        var vehicles = await _repository.GetAllAsync(cancellationToken);
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
