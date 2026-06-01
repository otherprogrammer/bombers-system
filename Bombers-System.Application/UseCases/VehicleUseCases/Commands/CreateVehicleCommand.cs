using Bombers_System.Domain.DTOs.Vehicle;
using Bombers_System.Domain.Entities;
using Bombers_System.Domain.Ports;
using MediatR;

namespace Bombers_System.Application.UseCases.VehicleUseCases.Commands;

public class CreateVehicleCommand : IRequest<VehicleDto>
{
    public CreateVehicleDto Dto { get; set; } = null!;
    public CreateVehicleCommand(CreateVehicleDto dto) => Dto = dto;
}

internal sealed class CreateVehicleCommandHandler : IRequestHandler<CreateVehicleCommand, VehicleDto>
{
    private readonly IUnitOfWork _unitOfWork;

    public CreateVehicleCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
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

        await _unitOfWork.Vehicles.AddAsync(vehicle, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

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