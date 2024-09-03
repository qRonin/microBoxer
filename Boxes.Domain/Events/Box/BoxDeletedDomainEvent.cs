using MediatR;

namespace Boxes.Domain.Events.Box;
public record BoxDeletedDomainEvent(
            Guid id
    ) : INotification;


