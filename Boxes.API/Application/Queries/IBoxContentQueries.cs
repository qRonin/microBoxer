namespace Boxes.API.Application.Queries
{
    public interface IBoxContentQueries
    {
        public Task<BoxContentVM> GetBoxContent(Guid contentId);
        public Task<IEnumerable<BoxContentVM>> GetBoxContentsByBoxId(Guid boxId);
        public Task<IEnumerable<BoxContentVM>> GetBoxContents();
    }
}
