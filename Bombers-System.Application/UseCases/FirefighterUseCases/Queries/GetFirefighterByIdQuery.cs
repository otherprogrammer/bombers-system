using Bombers_System.Domain.DTOs.Firefighter;
using Bombers_System.Domain.Ports;
using MediatR;

namespace Bombers_System.Application.UseCases.FirefighterUseCases.Queries;

public class GetFirefighterByIdQuery : IRequest<FirefighterDto?>
{
    public int FirefighterId { get; set; }
    public GetFirefighterByIdQuery(int id) => FirefighterId = id;
}

internal sealed class GetFirefighterByIdQueryHandler : IRequestHandler<GetFirefighterByIdQuery, FirefighterDto?>
{
    private readonly IFirefighterRepository _repository;

    public GetFirefighterByIdQueryHandler(IFirefighterRepository repository) => _repository = repository;

    public async Task<FirefighterDto?> Handle(GetFirefighterByIdQuery request, CancellationToken cancellationToken)
    {
        var f = await _repository.GetByIdAsync(request.FirefighterId, cancellationToken);
        if (f is null) return null;

        return new FirefighterDto
        {
            FirefighterId = f.FirefighterId,
            StationId = f.StationId,
            FullName = f.FullName,
            MedicalCertification = f.MedicalCertification,
            HireDate = f.HireDate,
            Rank = f.Rank,
            CurrentStatus = f.CurrentStatus
        };
    }
}