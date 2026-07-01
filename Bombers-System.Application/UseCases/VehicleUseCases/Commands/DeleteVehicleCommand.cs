using Bombers_System.Domain.Ports;
using MediatR;

namespace Bombers_System.Application.UseCases.VehicleUseCases.Commands;

public class DeleteVehicleCommand : IRequest<bool>
{
    public int VehicleId { get; set; }
    public DeleteVehicleCommand(int vehicleId) => VehicleId = vehicleId;
}

internal sealed class DeleteVehicleCommandHandler : IRequestHandler<DeleteVehicleCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeleteVehicleCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(DeleteVehicleCommand request, CancellationToken cancellationToken)
    {
        var vehicle = await _unitOfWork.Vehicles.GetByIdAsync(request.VehicleId, cancellationToken);
        if (vehicle is null) return false;

        _unitOfWork.Vehicles.Delete(vehicle);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return true;
    }
}