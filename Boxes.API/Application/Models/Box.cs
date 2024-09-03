using Boxes.API.Application.Commands.BoxContent;
using MediatR.NotificationPublishers;
using System.Runtime.CompilerServices;

namespace Boxes.API.Application.Models;

public class Box
{
    public Guid Id { get; set; }
    public string BoxName { get; set; }

    public List<BoxContent> BoxContents { get; set; }
}

public class BoxContent
{
    public string Name { get; set; }
    public string Description { get; set; }
    public Guid BoxId { get; set; }
    public Guid Id { get; set; }

}



public static class BoxExtensions
{

    public static IEnumerable<BoxContentDTO> ToBoxContentsDTO(this IEnumerable<BoxContent> boxContents )
    {
        foreach(var content in boxContents)
        {
            yield return content.ToBoxContentDTO();
        }

    }
    public static BoxContentDTO ToBoxContentDTO(this BoxContent content)
    {
        return new BoxContentDTO()
        {
            Name = content.Name,
            Description = content.Description,
            BoxId = content.BoxId,
            Id = content.Id
        };
    }
}