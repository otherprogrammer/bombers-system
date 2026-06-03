using Bombers_System.Domain.DTOs.Firefighter;
using Bombers_System.Domain.Ports;
using MediatR;

namespace Bombers_System.Application.UseCases.FirefighterUseCases.Queries;

public class GetAllFirefightersQuery : IRequest<IEnumerable<FirefighterDto>>;

internal sealed class GetAllFirefightersQueryHandler : IRequestHandler<GetAllFirefightersQuery, IEnumerable<FirefighterDto>>
{
    private readonly IFirefighterRepository _repository;

    public GetAllFirefightersQueryHandler(IFirefighterRepository repository) => _repository = repository;

    public async Task<IEnumerable<FirefighterDto>> Handle(GetAllFirefightersQuery request, CancellationToken cancellationToken)
    {
        var firefighters = await _repository.GetAllAsync(cancellationToken);
        return firefighters.Select(f => new FirefighterDto
        {
            FirefighterId = f.FirefighterId,
            StationId = f.StationId,
            FullName = f.FullName,
            MedicalCertification = f.MedicalCertification,
            HireDate = f.HireDate,
            Rank = f.Rank,
            CurrentStatus = f.CurrentStatus
        });
    }
}