using Boxes.Domain.AggregatesModel.BoxAggregate;
using Microsoft.VisualBasic;

namespace Boxes.API.Application.Queries;

public interface IBoxQueries
{
    Task<BoxVM> GetBoxAsync(Guid id);
    Task<BoxVM> GetBoxAsync(Guid id, Guid userId);
    public Task<IEnumerable<BoxVM>> GetBoxesAsync();
    public Task<IEnumerable<BoxVM>> GetUserBoxesAsync(Guid userId);

    //Task<IEnumerable<OrderSummary>> GetOrdersFromUserAsync(string userId);

    //Task<IEnumerable<CardType>> GetCardTypesAsync();
}
