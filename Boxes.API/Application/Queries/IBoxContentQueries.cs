namespace Boxes.API.Application.Queries
{
    public interface IBoxContentQueries
    {
        public Task<BoxContentVM> GetBoxContent(Guid contentId);
        public Task<BoxContentVM> GetBoxContent(Guid contentId, Guid userId);
        public Task<IEnumerable<BoxContentVM>> GetBoxContentsByBoxId(Guid boxId);
        public Task<IEnumerable<BoxContentVM>> GetBoxContentsByBoxId(Guid boxId,Guid userId);
        public Task<IEnumerable<BoxContentVM>> GetBoxContents();
        public Task<IEnumerable<BoxContentVM>> GetBoxContents(Guid userId);
    }
}
