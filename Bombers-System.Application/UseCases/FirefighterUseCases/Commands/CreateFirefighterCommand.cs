using Bombers_System.Domain.DTOs.Firefighter;
using Bombers_System.Domain.Entities;
using Bombers_System.Domain.Ports;
using MediatR;

namespace Bombers_System.Application.UseCases.FirefighterUseCases.Commands;

public class CreateFirefighterCommand : IRequest<FirefighterDto>
{
    public CreateFirefighterDto Dto { get; set; } = null!;
    public CreateFirefighterCommand(CreateFirefighterDto dto) => Dto = dto;
}

internal sealed class CreateFirefighterCommandHandler : IRequestHandler<CreateFirefighterCommand, FirefighterDto>
{
    private readonly IUnitOfWork _unitOfWork;

    public CreateFirefighterCommandHandler(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

    public async Task<FirefighterDto> Handle(CreateFirefighterCommand request, CancellationToken cancellationToken)
    {
        var firefighter = new FirefighterPersonnel
        {
            FirefighterId = request.Dto.FirefighterId,
            StationId = request.Dto.StationId,
            FullName = request.Dto.FullName,
            MedicalCertification = request.Dto.MedicalCertification,
            HireDate = request.Dto.HireDate,
            Rank = request.Dto.Rank,
            CurrentStatus = request.Dto.CurrentStatus
        };

        await _unitOfWork.Firefighters.AddAsync(firefighter, cancellationToken);
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