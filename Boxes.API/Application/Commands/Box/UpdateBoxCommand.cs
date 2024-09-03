using Boxes.API.Application.Commands.BoxContent;
using MediatR;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using Boxes.API.Application.Models;

namespace Boxes.API.Application.Commands.Box;

[DataContract]
public record UpdateBoxCommand : IRequest<bool>
{
    [DataMember]
    public Guid Id { get; private set; }
    [DataMember]
    public string BoxName { get; private set; }
    [DataMember]
    public IEnumerable<BoxContentDTO>? BoxContents => _boxContents;
    [DataMember]
    private readonly List<BoxContentDTO> _boxContents;
    /*
    public UpdateBoxCommand(Guid id, string boxName)
    {
        this.BoxName = boxName;
        Id = id;
        BoxContents = new List<BoxContentDTO>();
    }*/

    public UpdateBoxCommand()
    {
        _boxContents = new List<BoxContentDTO>();
    }

    public UpdateBoxCommand(Guid id, string boxName, IEnumerable<Boxes.API.Application.Models.BoxContent> boxContents)
    {
        this.BoxName = boxName;
        Id = id;
        _boxContents = boxContents.ToBoxContentsDTO().ToList();
        //DataContractSerializer serializer = new DataContractSerializer(typeof(BoxContentDTO));
        //serializer.
    }



}




