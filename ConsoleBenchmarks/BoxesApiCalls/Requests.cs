using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConsoleBenchmarks.BoxesApiCalls;
using ConsoleBenchmarks.BoxesApiCalls.Model;

namespace ConsoleBenchmarks.BoxesApiCalls;

public record CreateBoxRequest(
    string Id,
    string BoxName,
    IEnumerable<BoxContentRecord> BoxContents
    );
public record UpdateBoxRequest(
    string id,
    string boxName,
    IEnumerable<BoxContent> boxContents
    //IEnumerable<BoxContentRecord> boxContents
    );
public record DeleteBoxRequest(
    string Id
    , bool withContent
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
