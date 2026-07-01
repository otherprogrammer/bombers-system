using Bombers_System.Application.DTOs.Incident;
using MediatR;

namespace Bombers_System.Application.UseCases.IncidentUseCases.Commands;

public class UpdateIncidentCommand : IRequest<bool>
{
    public UpdateIncidentDto? Dto { get; set; }
}