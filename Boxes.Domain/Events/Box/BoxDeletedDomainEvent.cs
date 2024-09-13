using MediatR;

namespace Boxes.Domain.Events.Box;
public record BoxDeletedDomainEvent(
            Guid id, Guid userId
    ) : INotification;


