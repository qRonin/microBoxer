using Boxes.Domain.AggregatesModel.BoxAggregate;
using Boxes.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Boxes.API.Application.Queries
{
    public class BoxQueries(BoxesContext context) : IBoxQueries
    {

        public async Task<Box> GetBoxAsync(Guid id)
        {
            return await context.Boxes
                .Where(b => b.Id == id)
                .Select(b => new Box
                {
                    BoxName = b.BoxName,
                    BoxContents = b.BoxContents
                    .Select(bc => new BoxContent
                    {
                        Id = bc.Id,
                        BoxId = bc.BoxId ?? bc.LastKnownBoxId,
                        Description = bc.Description,
                        Name = bc.Name
                    }),
                    Id = b.Id

                }).FirstOrDefaultAsync();               
        }
        public async Task<IEnumerable<Box>> GetBoxesAsync()
        {
            return await context.Boxes
                .Select(b => new Box
                {
                    BoxName = b.BoxName,
                    BoxContents = b.BoxContents                    
                    .Select(bc => new BoxContent
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
