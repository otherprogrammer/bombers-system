using Bombers_System.Domain.DTOs.Vehicle;
using Bombers_System.Domain.Entities;
using Bombers_System.Domain.Interfaces;
using MediatR;

namespace Bombers_System.Application.UseCases.VehicleUseCases.Commands;

public class CreateVehicleCommand : IRequest<VehicleDto>
{
    public CreateVehicleDto Dto { get; set; } = null!;
    public CreateVehicleCommand(CreateVehicleDto dto) => Dto = dto;
}

internal sealed class CreateVehicleCommandHandler : IRequestHandler<CreateVehicleCommand, VehicleDto>
{
    private readonly IVehicleRepository _repository;

    public CreateVehicleCommandHandler(IVehicleRepository repository)
    {
        _repository = repository;
    }

    public async Task<VehicleDto> Handle(CreateVehicleCommand request, CancellationToken cancellationToken)
    {
        var vehicle = new Vehicle
        {
            VehicleId = request.Dto.VehicleId,
            StationId = request.Dto.StationId,
            WaterLevelGallons = request.Dto.WaterLevelGallons,
            LastMaintenanceDate = request.Dto.LastMaintenanceDate,
            RadioCode = request.Dto.RadioCode,
            VehicleType = request.Dto.VehicleType,
            OperationalStatus = request.Dto.OperationalStatus
        };

        await _repository.AddAsync(vehicle, cancellationToken);
        await _repository.SaveChangesAsync(cancellationToken);

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