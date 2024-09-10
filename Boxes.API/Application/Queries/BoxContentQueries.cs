
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
                    UserId = bc.UserId

                })
                .FirstOrDefaultAsync();         
        }

        public async Task<BoxContentVM> GetBoxContent(Guid contentId, Guid userId)
        {
            return await context.BoxContents.Where(bc => bc.Id == contentId &&
                bc.UserId == userId)
                .Select(bc => new BoxContentVM
                {
                    Id = bc.Id,
                    Name = bc.Name,
                    Description = bc.Description,
                    BoxId = bc.BoxId ?? bc.LastKnownBoxId,
                    UserId = bc.UserId

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
                    UserId = bc.UserId
                })
                .ToListAsync();
        }

        public async Task<IEnumerable<BoxContentVM>> GetBoxContents(Guid userId)
        {
            return await context.BoxContents.Where(bc => bc.UserId == userId)
                .Select(bc => new BoxContentVM
                {
                    Id = bc.Id,
                    Name = bc.Name,
                    Description = bc.Description,
                    BoxId = bc.BoxId ?? bc.LastKnownBoxId,
                    UserId = bc.UserId
                })
                .ToListAsync();
        }

        public async Task<IEnumerable<BoxContentVM>> GetBoxContentsByBoxId(Guid boxId)
        {
            return await context.BoxContents.Where(bc => bc.BoxId == boxId)
                .Select(bc => new BoxContentVM
                {
                    Id = bc.Id,
                    Name = bc.Name,
                    Description = bc.Description,
                    BoxId = bc.BoxId ?? bc.LastKnownBoxId,
                    UserId = bc.UserId
                })
                .ToListAsync();
        }
        public async Task<IEnumerable<BoxContentVM>> GetBoxContentsByBoxId(Guid boxId, Guid userId)
        {
            return await context.BoxContents.Where(bc => bc.BoxId == boxId && 
            bc.UserId == userId)
                .Select(bc => new BoxContentVM
                {
                    Id = bc.Id,
                    Name = bc.Name,
                    Description = bc.Description,
                    BoxId = bc.BoxId ?? bc.LastKnownBoxId,
                    UserId = bc.UserId
                })
                .ToListAsync();
        }
    }
}
