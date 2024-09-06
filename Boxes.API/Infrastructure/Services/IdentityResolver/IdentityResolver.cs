using Boxes.Domain.AggregatesModel.UserAggregate;
using System.Threading;

namespace Boxes.API.Infrastructure.Services.IdentityResolver
{
    public class IdentityResolver : IIdentityResolver
    {
        private readonly IUserRepository _userRepository;
        public IdentityResolver(IUserRepository userRepository)
        {
                _userRepository = userRepository;
        }

        public async Task<bool> FindUser(string requestUserId)
        {
            return await _userRepository.UserExists(Guid.Parse(requestUserId));

        }
        public async Task<bool> SaveUserToLocal(string requestUserId, CancellationToken cancellationToken)
        {
            User user = new User(Guid.Parse(requestUserId));
            await _userRepository.SaveUser(user);
            await _userRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
            return true;
        }

    }
}
