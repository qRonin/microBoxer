using MediatR;

namespace Boxes.API.Application.Commands.Box;

public record DeleteBoxCommand(Guid Id) : IRequest<bool>;

