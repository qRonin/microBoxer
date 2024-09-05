using Boxes.Domain.AggregatesModel.BoxAggregate;

namespace Boxes.API.Application.Queries;

public record BoxContentVM
{
    public string Name { get; set; }
    public string Description { get; set; }
    public Guid Id { get; set; }
    public Guid? BoxId { get; set; }


}
