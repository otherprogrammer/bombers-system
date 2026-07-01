using MediatR;
using Bombers_System.Application.DTOs.Incident;

public class CreateIncidentCommand : IRequest<int>
{
    public CreateIncidentDto Dto { get; set; }
}