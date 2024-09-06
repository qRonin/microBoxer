using Boxes.Domain.SeedWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Boxes.Domain.AggregatesModel.UserAggregate;

public interface IUserRepository : IRepository<User>
{
    Task<IEnumerable<User>> GetAll();
    Task<bool> UserExists(Guid userId);
    Task<bool> UserDeleted();
    Task<bool> SaveUser(User user);

}
