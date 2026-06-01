using MediatR;

namespace Bombers_System.Application.UseCases.TestUseCases.Queries;

public record GetGreetingQuery(string Name) : IRequest<string>;

internal sealed class GetGreetingQueryHandler : IRequestHandler<GetGreetingQuery, string>
{
    public Task<string> Handle(GetGreetingQuery request, CancellationToken cancellationToken)
    {
        return Task.FromResult($"Hello {request.Name}");
    }
}