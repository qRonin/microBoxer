
using Boxes.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Boxes.API.Application.Queries
{
    public class BoxContentQueries(BoxesContext context) : IBoxContentQueries
    {
        public async Task<BoxContentVM> GetBoxContent(Guid contentId)
        {
            return await context.BoxContents.Where(b=>b.Id == contentId)
                .Select( bc => new BoxContentVM
                {
                    Id = bc.Id,
                    Name = bc.Name,
                    Description = bc.Description,
                    BoxId = bc.BoxId ?? bc.LastKnownBoxId,
                })
                .FirstOrDefaultAsync();         
        }

        public async Task<IEnumerable<BoxContentVM>> GetBoxContents()
        {
            return await context.BoxContents
                .Select(bc => new BoxContentVM
                {
                    Id = bc.Id,
                    Name = bc.Name,
                    Description = bc.Description,
                    BoxId = bc.BoxId ?? bc.LastKnownBoxId,
                })
                .ToListAsync();
        }

        public async Task<IEnumerable<BoxContentVM>> GetBoxContentsByBoxId(Guid boxId)
        {
            return await context.BoxContents.Where(b => b.BoxId == boxId)
                .Select(bc => new BoxContentVM
                {
                    Id = bc.Id,
                    Name = bc.Name,
                    Description = bc.Description,
                    BoxId = bc.BoxId ?? bc.LastKnownBoxId,
                })
                .ToListAsync();
        }
    }
}
