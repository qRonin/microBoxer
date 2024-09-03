using MediatR;

namespace Boxes.Domain.Events.Box;
public record BoxUpdatedDomainEvent(
            Guid id
    ) : INotification;


