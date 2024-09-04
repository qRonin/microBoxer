namespace ConsoleBenchmarks.BoxesApiCalls.Model;

public class Box
{
    public Guid Id { get; set; }
    public string BoxName { get; set; }

    public List<BoxContent> BoxContents { get; set; }
    public Box()
    {
        BoxName = "";
        BoxContents = new List<BoxContent>();
        Id = Guid.Parse("00000000-0000-0000-0000-000000000000");
    }
}

public class BoxContent
{
    public string Name { get; set; }
    public string Description { get; set; }
    public Guid BoxId { get; set; }
    public Guid Id { get; set; }

    public BoxContent()
    {
        Name = "";
        Description = "";
        Id = Guid.Parse("00000000-0000-0000-0000-000000000000");
        BoxId = Guid.Parse("00000000-0000-0000-0000-000000000000");
    }

}


public static class BoxModelExtensions
{

    public static async Task<Box> FromRecord(this BoxRecord boxRecord)
    {
        if (boxRecord != null)
        {
            var box = new Box()
            {
                Id = Guid.Parse(boxRecord.Id),
                BoxName = boxRecord.BoxName,
                BoxContents = new List<BoxContent>()
            };
            foreach (var content in boxRecord.BoxContents)
            {
                box.BoxContents.Add(await FromRecord(content));
            }
            return box;
        }
        else return new Box();

    }
    public static async Task<BoxContent> FromRecord(this BoxContentRecord boxContentRecord)
    {
        if (boxContentRecord != null)
        {
            var boxContent = new BoxContent()
            {
                Id = Guid.Parse(boxContentRecord.Id),
                Name = boxContentRecord.Name,
                Description = boxContentRecord.Description,
                BoxId = Guid.Parse(boxContentRecord.BoxId)
            };
            return boxContent;
        }
        else return new BoxContent();
    }

}
