namespace Boxes.API.Infrastructure.Services.IdentityResolver
{
    public interface IIdentityResolver
    {
        Task<bool> FindUser(string requestUserId);
        Task<bool> SaveUserToLocal(string requestUserId, CancellationToken cancellationToken);

    }
}
