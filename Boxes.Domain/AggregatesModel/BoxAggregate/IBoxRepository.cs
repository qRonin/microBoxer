using Boxes.Domain.SeedWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Boxes.Domain.AggregatesModel.BoxAggregate
{
    public interface IBoxRepository : IRepository<Box>
    {
        Box Add(Box box);
        Task<Box> GetAsync(Guid id);
        Task<IEnumerable<Box>> GetAsync();
        bool Delete(Guid id);
        Box Update(Box box);


    }
}
