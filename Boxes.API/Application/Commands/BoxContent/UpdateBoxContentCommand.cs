using MediatR;
using System.Runtime.Serialization;

namespace Boxes.API.Application.Commands.BoxContent;

[DataContract]
public record UpdateBoxContentCommand : IRequest<bool>
{
    [DataMember]
    public Guid? BoxId { get; set; }
    public Guid? Id { get; set; }

    [DataMember]
    public string Name { get; set; }
    [DataMember]
    public string Description { get; set; }


    public UpdateBoxContentCommand(string name, string description, Guid? boxId, Guid id)
    {
        BoxId = boxId == null ? null : boxId;
        Id = id;
        Name = name;
        Description = description;
    }

}
