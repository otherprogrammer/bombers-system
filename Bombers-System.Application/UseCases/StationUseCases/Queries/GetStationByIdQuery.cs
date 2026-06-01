using Bombers_System.Domain.DTOs.Station;
using Bombers_System.Domain.Interfaces;
using MediatR;

namespace Bombers_System.Application.UseCases.StationUseCases.Queries;

public class GetStationByIdQuery : IRequest<StationDto?>
{
    public int StationId { get; set; }
    public GetStationByIdQuery(int stationId) => StationId = stationId;
}

internal sealed class GetStationByIdQueryHandler : IRequestHandler<GetStationByIdQuery, StationDto?>
{
    private readonly IStationRepository _repository;

    public GetStationByIdQueryHandler(IStationRepository repository)
    {
        _repository = repository;
    }

    public async Task<StationDto?> Handle(GetStationByIdQuery request, CancellationToken cancellationToken)
    {
        var station = await _repository.GetByIdAsync(request.StationId, cancellationToken);
        if (station is null) return null;
        return new StationDto
        {
            StationId = station.StationId,
            StationNumber = station.StationNumber,
            Address = station.Address,
            VehicleCapacity = station.VehicleCapacity
        };
    }
}