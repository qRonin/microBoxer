using Boxes.Domain.AggregatesModel.BoxAggregate;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Boxes.Infrastructure.Repositories
{
    public class BoxRepository : IBoxRepository
    {
        private readonly BoxesContext _boxContext;
        public IUnitOfWork UnitOfWork => _boxContext;

        public BoxRepository(BoxesContext context)
        {
                _boxContext = context ?? throw new ArgumentNullException(nameof(context));
        }
        public Box Add(Box box)
        {
           var result = _boxContext.Boxes.Add(box).Entity;
            return result;
        }

        public async Task<Box> GetAsync(Guid id)
        {
            return await _boxContext.Boxes.Where(b => b.Id == id)
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Box>> GetAsync()
        {
            return _boxContext.Boxes.AsEnumerable();
        }

        public Box Update(Box box)
        {
            return _boxContext.Boxes.Update(box).Entity;           
        }

        public bool Delete(Guid id)
        {
            var box = _boxContext.Boxes.Where(b => b.Id == id).FirstOrDefault();
            _boxContext.Boxes.Remove(box);
            return true;
        }
    }
}
