using Bombers_System.Domain.DTOs.Vehicle;
using Bombers_System.Domain.Ports;
using MediatR;

namespace Bombers_System.Application.UseCases.VehicleUseCases.Queries;

public class GetVehiclesFilteredQuery : IRequest<IEnumerable<VehicleDto>>
{
    public string? Type { get; set; }
    public string? Status { get; set; }

    public GetVehiclesFilteredQuery(string? type, string? status)
    {
        Type = type;
        Status = status;
    }
}

internal sealed class GetVehiclesFilteredQueryHandler : IRequestHandler<GetVehiclesFilteredQuery, IEnumerable<VehicleDto>>
{
    private readonly IVehicleRepository _repository;

    public GetVehiclesFilteredQueryHandler(IVehicleRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<VehicleDto>> Handle(GetVehiclesFilteredQuery request, CancellationToken cancellationToken)
    {
        var vehicles = await _repository.GetAllAsync(cancellationToken);

        if (!string.IsNullOrWhiteSpace(request.Type))
            vehicles = vehicles.Where(v => v.VehicleType.Equals(request.Type, StringComparison.OrdinalIgnoreCase));

        if (!string.IsNullOrWhiteSpace(request.Status))
            vehicles = vehicles.Where(v => v.OperationalStatus.Equals(request.Status, StringComparison.OrdinalIgnoreCase));

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
