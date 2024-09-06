using Boxes.Domain.AggregatesModel.BoxAggregate;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Boxes.Infrastructure.Repositories
{
    public class BoxContentRepository : IBoxContentRepository
    {
        private readonly BoxesContext _boxContext;
        public IUnitOfWork UnitOfWork => _boxContext;


        public BoxContentRepository(BoxesContext context)
        {
            _boxContext = context ?? throw new ArgumentNullException(nameof(context));
        }

        public BoxContent Add(BoxContent content)
        {
            var result = _boxContext.BoxContents.Add(content).Entity;
            return result;
        }

        public async Task<BoxContent> GetAsync(Guid id)
        {
            return await _boxContext.BoxContents.Where(bc => bc.Id == id)
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<BoxContent>> GetAsync()
        {
            return _boxContext.BoxContents.AsEnumerable();
        }

        public BoxContent Update(BoxContent content)
        {
            var result = _boxContext.BoxContents.Update(content).Entity;
            return result;
        }

        public bool Delete(Guid id)
        {
            var content = _boxContext.BoxContents.Where(bc => bc.Id == id).FirstOrDefault();
            _boxContext.BoxContents.Remove(content);
            return true;
        }

        public Task<IEnumerable<BoxContent>> GetByUserAsync(Guid userId)
        {
            throw new NotImplementedException();
        }
    }
}
