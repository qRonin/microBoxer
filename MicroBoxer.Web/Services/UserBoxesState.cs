using MicroBoxer.Web.Services.ViewModel;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;
using System.Xml.Linq;

namespace MicroBoxer.Web.Services;

public class UserBoxesState(
    BoxesService boxesService,
    AuthenticationStateProvider authenticationStateProvider
    ) : IUserBoxesState
{
    public async Task<IReadOnlyCollection<BoxRecord>> GetBoxesAsync()
    => (await GetUserAsync()).Identity?.IsAuthenticated == true
    ? await boxesService.GetBoxes()
    : [];

    public async Task<BoxRecord?> GetBoxAsync(Guid id)
    => (await GetUserAsync()).Identity?.IsAuthenticated == true
    ? await boxesService.GetBox(id)
    : null;

    public async Task<HttpResponseMessage> UpdateBoxAsync(Guid boxId, string boxName, IEnumerable<MicroBoxer.Web.Services.ViewModel.BoxContent> boxContents)
    {
        if ((await GetUserAsync()).Identity?.IsAuthenticated == false)
        {
            return null;
        }
        else
        {
            var request = new UpdateBoxRequest(boxId.ToString(), boxName, boxContents);
            var result = await boxesService.UpdateBox(request, Guid.NewGuid());
            Console.WriteLine(result);
            return result;
        }
        

    }
    public async Task<HttpResponseMessage> UpdateBoxContentAsync(Guid? boxId, string name, string description, Guid? id)
    {
        if ((await GetUserAsync()).Identity?.IsAuthenticated == false)
        {
            return null;
        }
        else
        {
            var request = new UpdateBoxContentRequest(boxId.ToString(), name, description, id.ToString());
            var result = await boxesService.UpdateBoxContent(request, Guid.NewGuid());
            Console.WriteLine(result);
            return result;
        }

    }


    private async Task<ClaimsPrincipal> GetUserAsync()
    => (await authenticationStateProvider.GetAuthenticationStateAsync()).User;
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