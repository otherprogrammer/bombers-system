using Bombers_System.Domain.DTOs.Firefighter;
using Bombers_System.Domain.Ports;
using MediatR;

namespace Bombers_System.Application.UseCases.FirefighterUseCases.Commands;

public class UpdateFirefighterCommand : IRequest<FirefighterDto?>
{
    public int FirefighterId { get; set; }
    public UpdateFirefighterDto Dto { get; set; } = null!;
    public UpdateFirefighterCommand(int id, UpdateFirefighterDto dto) { FirefighterId = id; Dto = dto; }
}

internal sealed class UpdateFirefighterCommandHandler : IRequestHandler<UpdateFirefighterCommand, FirefighterDto?>
{
    private readonly IUnitOfWork _unitOfWork;

    public UpdateFirefighterCommandHandler(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

    public async Task<FirefighterDto?> Handle(UpdateFirefighterCommand request, CancellationToken cancellationToken)
    {
        var firefighter = await _unitOfWork.Firefighters.GetByIdAsync(request.FirefighterId, cancellationToken);
        if (firefighter is null) return null;

        firefighter.StationId = request.Dto.StationId;
        firefighter.FullName = request.Dto.FullName;
        firefighter.MedicalCertification = request.Dto.MedicalCertification;
        firefighter.Rank = request.Dto.Rank;
        firefighter.CurrentStatus = request.Dto.CurrentStatus;

        _unitOfWork.Firefighters.Update(firefighter);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new FirefighterDto
        {
            FirefighterId = firefighter.FirefighterId,
            StationId = firefighter.StationId,
            FullName = firefighter.FullName,
            MedicalCertification = firefighter.MedicalCertification,
            HireDate = firefighter.HireDate,
            Rank = firefighter.Rank,
            CurrentStatus = firefighter.CurrentStatus
        };
    }
}