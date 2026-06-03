using Bombers_System.Domain.DTOs.Station;
using Bombers_System.Domain.Ports;
using MediatR;

namespace Bombers_System.Application.UseCases.StationUseCases.Queries;

public class GetAllStationsQuery : IRequest<IEnumerable<StationDto>>;

internal sealed class GetAllStationsQueryHandler : IRequestHandler<GetAllStationsQuery, IEnumerable<StationDto>>
{
    private readonly IStationRepository _repository;

    public GetAllStationsQueryHandler(IStationRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<StationDto>> Handle(GetAllStationsQuery request, CancellationToken cancellationToken)
    {
        var stations = await _repository.GetAllAsync(cancellationToken);
        return stations.Select(s => new StationDto
        {
            StationId = s.StationId,
            StationNumber = s.StationNumber,
            Address = s.Address,
            VehicleCapacity = s.VehicleCapacity
        });
    }
}
