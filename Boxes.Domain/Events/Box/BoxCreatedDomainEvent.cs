using Boxes.Domain.AggregatesModel.BoxAggregate;
using MediatR;

namespace Boxes.Domain.Events.Box;
public record BoxCreatedDomainEvent(
        Boxes.Domain.AggregatesModel.BoxAggregate.Box box
    ) : INotification;


