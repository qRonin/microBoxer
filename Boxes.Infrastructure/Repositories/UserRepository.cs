using Boxes.Domain.AggregatesModel.UserAggregate;
using IntegrationEventLogEF.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Boxes.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly BoxesContext _boxContext;
        public IUnitOfWork UnitOfWork => _boxContext;

        public Task<IEnumerable<User>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<bool> SaveUser(User user)
        {
            _boxContext.Set<User>().Add(user);
            return Task.FromResult(true);
        }

        public Task<bool> UserDeleted()
        {
            throw new NotImplementedException();
        }

        public async Task<bool> UserExists(Guid userId)
        {
            //_context.Database.UseTransaction(transaction.GetDbTransaction());
            return _boxContext.Set<User>().Any(u => u.Id == userId);
        }
    }
}
