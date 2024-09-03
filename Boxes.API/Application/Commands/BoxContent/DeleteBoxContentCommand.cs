using MediatR;

namespace Boxes.API.Application.Commands.BoxContent;

public record DeleteBoxContentCommand(Guid Id) : IRequest<bool>;

