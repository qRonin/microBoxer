namespace Boxes.API.Application.Queries
{
    public interface IBoxContentQueries
    {
        public Task<BoxContent> GetBoxContent(Guid contentId);
        public Task<IEnumerable<BoxContent>> GetBoxContentsByBoxId(Guid boxId);
        public Task<IEnumerable<BoxContent>> GetBoxContents();
    }
}
