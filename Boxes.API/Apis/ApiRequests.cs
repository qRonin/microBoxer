using Boxes.API.Application.Models;

namespace Boxes.API.Apis;
public record CreateBoxRequest(
    string Id,
    string BoxName,
    List<BoxContent> boxContents
    );
public record UpdateBoxRequest(
    string id,
    string boxName,
    List<BoxContent> boxContents
    );
public record DeleteBoxRequest(
    string Id
    ,bool withContent
    );

public record CreateBoxContentRequest(
    string? BoxId,
    string Name,
    string Description
    );
public record UpdateBoxContentRequest(
    string? BoxId,
    string Name,
    string Description,
    string Id
    );
public record DeleteBoxContentRequest(
    string Id
    );
