using Bombers_System.Domain.DTOs.Vehicle;
using Bombers_System.Domain.Ports;
using MediatR;

namespace Bombers_System.Application.UseCases.VehicleUseCases.Commands;

public class UpdateVehicleCommand : IRequest<VehicleDto?>
{
    public int VehicleId { get; set; }
    public UpdateVehicleDto Dto { get; set; } = null!;
    public UpdateVehicleCommand(int vehicleId, UpdateVehicleDto dto) { VehicleId = vehicleId; Dto = dto; }
}

internal sealed class UpdateVehicleCommandHandler : IRequestHandler<UpdateVehicleCommand, VehicleDto?>
{
    private readonly IUnitOfWork _unitOfWork;

    public UpdateVehicleCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<VehicleDto?> Handle(UpdateVehicleCommand request, CancellationToken cancellationToken)
    {
        var vehicle = await _unitOfWork.Vehicles.GetByIdAsync(request.VehicleId, cancellationToken);
        if (vehicle is null) return null;

        vehicle.StationId = request.Dto.StationId;
        vehicle.WaterLevelGallons = request.Dto.WaterLevelGallons;
        vehicle.LastMaintenanceDate = request.Dto.LastMaintenanceDate;
        vehicle.RadioCode = request.Dto.RadioCode;
        vehicle.VehicleType = request.Dto.VehicleType;
        vehicle.OperationalStatus = request.Dto.OperationalStatus;

        await _unitOfWork.Vehicles.UpdateAsync(vehicle);
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
