using Boxes.Domain.AggregatesModel.BoxAggregate;
using MediatR;
using System.Runtime.Serialization;
using System.Xml.Linq;

namespace Boxes.API.Application.Commands.BoxContent;

[DataContract]
public record CreateBoxContentCommand : IRequest<BoxContentDTO>
{
    [DataMember]
    public Guid? BoxId { get; set; }
    public Guid? Id { get; set; }
    public Guid UserId { get; set; }
    [DataMember]
    public string Name { get; set; }
    [DataMember]
    public string Description { get; set; }


    public CreateBoxContentCommand(string name, string description, Guid? boxId, Guid userId)
    {
        BoxId = boxId == null ? null : boxId;
        Id = new Guid();
        Name = name;
        Description = description;
        UserId = userId;
    }

}



