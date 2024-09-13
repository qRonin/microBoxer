using MediatR;

namespace Boxes.Domain.Events.Box;
public record BoxUpdatedDomainEvent(
            Guid id, Guid userId
    ) : INotification;


