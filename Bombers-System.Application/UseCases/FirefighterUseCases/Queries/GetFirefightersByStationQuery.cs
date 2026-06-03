using Bombers_System.Domain.DTOs.Firefighter;
using Bombers_System.Domain.Ports;
using MediatR;

namespace Bombers_System.Application.UseCases.FirefighterUseCases.Queries;

public class GetFirefightersByStationQuery : IRequest<IEnumerable<FirefighterDto>>
{
    public int StationId { get; set; }
    public GetFirefightersByStationQuery(int stationId) => StationId = stationId;
}

internal sealed class GetFirefightersByStationQueryHandler : IRequestHandler<GetFirefightersByStationQuery, IEnumerable<FirefighterDto>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetFirefightersByStationQueryHandler(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

    public async Task<IEnumerable<FirefighterDto>> Handle(GetFirefightersByStationQuery request, CancellationToken cancellationToken)
    {
        var firefighters = await _unitOfWork.Firefighters.GetByStationIdAsync(request.StationId, cancellationToken);
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