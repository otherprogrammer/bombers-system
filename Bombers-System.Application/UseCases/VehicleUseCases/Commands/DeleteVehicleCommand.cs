using Bombers_System.Domain.Interfaces;
using MediatR;

namespace Bombers_System.Application.UseCases.VehicleUseCases.Commands;

public class DeleteVehicleCommand : IRequest<bool>
{
    public int VehicleId { get; set; }
    public DeleteVehicleCommand(int vehicleId) => VehicleId = vehicleId;
}

internal sealed class DeleteVehicleCommandHandler : IRequestHandler<DeleteVehicleCommand, bool>
{
    private readonly IVehicleRepository _repository;

    public DeleteVehicleCommandHandler(IVehicleRepository repository)
    {
        _repository = repository;
    }

    public async Task<bool> Handle(DeleteVehicleCommand request, CancellationToken cancellationToken)
    {
        var vehicle = await _repository.GetByIdAsync(request.VehicleId, cancellationToken);
        if (vehicle is null) return false;

        _repository.Delete(vehicle);
        await _repository.SaveChangesAsync(cancellationToken);
        return true;
    }
}