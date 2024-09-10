namespace Boxes.API.Application.Queries;


public record BoxVM
{
    public string BoxName { get; set; }
    public Guid Id { get; set; }
    public IEnumerable<BoxContentVM> BoxContents { get; set; }

    public Guid UserId { get; set; }

}
