namespace Boxes.API.Application.Queries;


public record Box
{
    public string BoxName { get; set; }
    public Guid Id { get; set; }
    public IEnumerable<BoxContent> BoxContents { get; set; }

}
