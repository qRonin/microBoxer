using MediatR;

namespace Boxes.API.Application.Commands.Box
{
    public record DeleteBoxWithContentCommand(Guid Id) : IRequest<bool>;

}
