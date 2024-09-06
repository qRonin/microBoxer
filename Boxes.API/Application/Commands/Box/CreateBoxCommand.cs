using Boxes.API.Application.Commands.BoxContent;
using Boxes.API.Extensions;
using Boxes.Domain.AggregatesModel.BoxAggregate;
using MediatR;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Boxes.API.Application.Commands.Box;

[DataContract]
public record CreateBoxCommand : IRequest<BoxDTO>
{
    [DataMember]
    public Guid Id { get; private set; }
    public Guid UserId { get; set; }
    [DataMember]
    public string BoxName { get; private set; }
    public List<BoxContentDTO> BoxContents { get; private set; }


    public CreateBoxCommand(string BoxName, Guid userId)
    {
        this.BoxName = BoxName;
        Id = new Guid();
        BoxContents = new List<BoxContentDTO>();
        UserId = userId;
    }




}
