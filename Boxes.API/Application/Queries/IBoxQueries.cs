using Boxes.Domain.AggregatesModel.BoxAggregate;
using Microsoft.VisualBasic;

namespace Boxes.API.Application.Queries;

public interface IBoxQueries
{
    Task<Box> GetBoxAsync(Guid id);
    public Task<IEnumerable<Box>> GetBoxesAsync();


    //Task<IEnumerable<OrderSummary>> GetOrdersFromUserAsync(string userId);

    //Task<IEnumerable<CardType>> GetCardTypesAsync();
}
