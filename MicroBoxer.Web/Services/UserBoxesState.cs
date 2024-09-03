using MicroBoxer.Web.Services.ViewModel;

namespace MicroBoxer.Web.Services;

public class UserBoxesState
{
}







public record CreateBoxRequest(
    string Id,
    string BoxName,
    IEnumerable<BoxContentRecord> BoxContents
    );
public record UpdateBoxRequest(
    string id,
    string boxName,
    IEnumerable<BoxContent> boxContents
    //IEnumerable<BoxContentRecord> boxContents
    );
public record DeleteBoxRequest(
    string Id
    ,bool withContent
    );

public record CreateBoxContentRequest(
    string? BoxId,
    string Name,
    string Description
    );
public record UpdateBoxContentRequest(
    string? BoxId,
    string Name,
    string Description,
    string Id
    );
public record DeleteBoxContentRequest(
    string Id
    );