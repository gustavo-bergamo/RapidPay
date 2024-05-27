using MediatR;

namespace RapidPay.Domain.Infrastructure.Services;

public class AggregateRoot
{
    private readonly List<IDomainEvent> _domainEvents = new();
    private readonly IMediator _mediator;

    protected AggregateRoot(IMediator mediator)
    {
        _mediator = mediator;
    }

    protected void RaiseDomainEvent(IDomainEvent domainEvent)
    {
        _domainEvents.Add(domainEvent);
        _mediator.Publish(domainEvent);
    }
}
