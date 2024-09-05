using Boxes.Domain.AggregatesModel.BoxAggregate;
using Microsoft.VisualBasic;

namespace Boxes.API.Application.Queries;

public interface IBoxQueries
{
    Task<BoxVM> GetBoxAsync(Guid id);
    public Task<IEnumerable<BoxVM>> GetBoxesAsync();


    //Task<IEnumerable<OrderSummary>> GetOrdersFromUserAsync(string userId);

    //Task<IEnumerable<CardType>> GetCardTypesAsync();
}
