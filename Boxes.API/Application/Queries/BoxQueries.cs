using Boxes.Domain.AggregatesModel.BoxAggregate;
using Boxes.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Boxes.API.Application.Queries
{
    public class BoxQueries(BoxesContext context) : IBoxQueries
    {

        public async Task<BoxVM> GetBoxAsync(Guid id)
        {
            return await context.Boxes
                .Where(b => b.Id == id)
                .Select(b => new BoxVM
                {
                    BoxName = b.BoxName,
                    BoxContents = b.BoxContents
                    .Select(bc => new BoxContentVM
                    {
                        Id = bc.Id,
                        BoxId = bc.BoxId ?? bc.LastKnownBoxId,
                        Description = bc.Description,
                        Name = bc.Name
                    }),
                    Id = b.Id

                }).FirstOrDefaultAsync();               
        }
        public async Task<IEnumerable<BoxVM>> GetBoxesAsync()
        {
            return await context.Boxes
                .Select(b => new BoxVM
                {
                    BoxName = b.BoxName,
                    BoxContents = b.BoxContents                    
                    .Select(bc => new BoxContentVM
                    {
                        Id = bc.Id,
                        BoxId = bc.BoxId ?? bc.LastKnownBoxId,
                        Description = bc.Description,
                        Name = bc.Name
                    }),
                    Id = b.Id

                })
                .ToListAsync();
        }
    }
}
