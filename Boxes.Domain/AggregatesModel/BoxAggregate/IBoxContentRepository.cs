using Boxes.Domain.SeedWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Boxes.Domain.AggregatesModel.BoxAggregate
{
    public interface IBoxContentRepository : IRepository<BoxContent>
    {
        BoxContent Add(BoxContent content);
        Task<BoxContent> GetAsync(Guid id);
        Task<IEnumerable<BoxContent>> GetAsync();
        bool Delete(Guid id);
        BoxContent Update(BoxContent content);
    }
}
