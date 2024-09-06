namespace Boxes.API.Infrastructure.Services;

public interface IIdentityService
{
    Task<string> GetUserIdentity();

    string GetUserName();
    //string GetWholeUser();
}

