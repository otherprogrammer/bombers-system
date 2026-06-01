using Bombers_System.Domain.DTOs.Station;
using Bombers_System.Domain.Entities;
using Bombers_System.Domain.Interfaces;
using MediatR;

namespace Bombers_System.Application.UseCases.StationUseCases.Commands;

public class CreateStationCommand : IRequest<StationDto>
{
    public CreateStationDto Dto { get; set; } = null!;
    public CreateStationCommand(CreateStationDto dto) => Dto = dto;
}

internal sealed class CreateStationCommandHandler : IRequestHandler<CreateStationCommand, StationDto>
{
    private readonly IStationRepository _repository;

    public CreateStationCommandHandler(IStationRepository repository)
    {
        _repository = repository;
    }

    public async Task<StationDto> Handle(CreateStationCommand request, CancellationToken cancellationToken)
    {
        var station = new Station
        {
            StationId = request.Dto.StationId,
            StationNumber = request.Dto.StationNumber,
            Address = request.Dto.Address,
            VehicleCapacity = request.Dto.VehicleCapacity
        };

        await _repository.AddAsync(station, cancellationToken);
        await _repository.SaveChangesAsync(cancellationToken);

        return new StationDto
        {
            StationId = station.StationId,
            StationNumber = station.StationNumber,
            Address = station.Address,
            VehicleCapacity = station.VehicleCapacity
        };
    }
}