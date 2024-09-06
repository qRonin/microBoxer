using Boxes.API.Infrastructure.Services.IdentityResolver;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Boxes.API.Infrastructure.Services;

public class IdentityService(IHttpContextAccessor context
    ,IIdentityResolver resolver
    ) : IIdentityService
{
    public async Task<string> GetUserIdentity()
    {

         var messageUserId =  context.HttpContext?.User.FindFirst("sub")?.Value;
        if (!messageUserId.IsNullOrEmpty())
        {
           var result = await RequestUserExists(messageUserId);
            if (result)
            {
                return messageUserId;
            }
            else await SaveUserToLocal(messageUserId);
        }

        return messageUserId;
    }

    public string GetUserName()
    {
        var messageUserName = context.HttpContext?.User.Identity?.Name;
        return messageUserName;
    }

    //WIP
    
    public async Task<bool> RequestUserExists(string userId)
    {
        var result = await resolver.FindUser(userId);
        return result;
    }
    public async Task<bool> SaveUserToLocal(string userId)
    {
        CancellationToken cancellationToken = new CancellationToken();
        var result = await resolver.SaveUserToLocal(userId, cancellationToken);
        cancellationToken.ThrowIfCancellationRequested();
        return true;
    }
    


}
