using Boxes.API.Apis;
using Boxes.API.Application.Commands;
using Boxes.API.Application.Commands.Box;
using Boxes.API.Application.Commands.BoxContent;
using Boxes.API.Application.Queries;
using Boxes.API.Infrastructure.Services;
using Boxes.Domain.AggregatesModel.BoxAggregate;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.SignalR.Protocol;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using NSubstitute.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Boxes.UnitTests.Application;


[TestClass]
public class BoxesWebApiTest
{
    private readonly IMediator _mediatorMock;
    private readonly IBoxQueries _boxesQueriesMock;
    private readonly IBoxContentQueries _boxContentsQueriesMock;
    private readonly IIdentityService _identityServiceMock;
    private readonly ILogger<BoxesServices> _loggerMock;

    public BoxesWebApiTest()
    {
        _mediatorMock = Substitute.For<IMediator>();
        _boxesQueriesMock = Substitute.For<IBoxQueries>();
        _boxContentsQueriesMock = Substitute.For<IBoxContentQueries>();
        _identityServiceMock = Substitute.For<IIdentityService>();
        _loggerMock = Substitute.For<ILogger<BoxesServices>>();


    }

    [TestMethod]
    public void Delete_box_with_command_boxId_success()
    {
        // Arrange
        _mediatorMock.Send(Arg.Any<DeleteBoxCommand>, default)
        .Returns(Task.FromResult(true));
        var BoxId = Guid.NewGuid();
        // Act
        var boxesServices = new BoxesServices(_mediatorMock, _boxesQueriesMock, _identityServiceMock, _loggerMock, _boxContentsQueriesMock);
        var result =  BoxesApi.DeleteBox(new DeleteBoxCommand(BoxId), boxesServices);
        // Assert
        Microsoft.VisualStudio.TestTools.UnitTesting.Assert.IsInstanceOfType<bool>(result.Result);
    }
    [TestMethod]
    public void Create_box_with_command_boxName_success()
    {
        // Arrange
        _mediatorMock.Send(Arg.Any<CreateBoxCommand>, default)
        .Returns(Task.FromResult(true));
        var BoxName = "new Box";
        var ownerId = Guid.NewGuid();
        // Act
        var boxesServices = new BoxesServices(_mediatorMock, _boxesQueriesMock, _identityServiceMock, _loggerMock, _boxContentsQueriesMock);
        var result =  BoxesApi.CreateBox(new CreateBoxCommand(BoxName, ownerId), boxesServices);
        // Assert
        Microsoft.VisualStudio.TestTools.UnitTesting.Assert.IsInstanceOfType<bool>(result.Result);
    }
    [TestMethod]
    public void Update_box_with_command_boxName_success()
    {
        // Arrange
        _mediatorMock.Send(Arg.Any<UpdateBoxCommand>, default)
        .Returns(Task.FromResult(true));
        var BoxName = "new BoxName";
        var BoxId = Guid.NewGuid();
        IEnumerable<API.Application.Models.BoxContent> contents =
            new List<API.Application.Models.BoxContent>();
        // Act
        var boxesServices = new BoxesServices(_mediatorMock, _boxesQueriesMock, _identityServiceMock, _loggerMock, _boxContentsQueriesMock);
        var result = BoxesApi.UpdateBox(new UpdateBoxCommand(BoxId, BoxName, contents), boxesServices);
        // Assert
        Microsoft.VisualStudio.TestTools.UnitTesting.Assert.IsInstanceOfType<bool>(result.Result);
    }
    [TestMethod]
    public async Task Delete_box_with_request_boxId_success()
    {
        // Arrange
        var requestId = Guid.NewGuid();
        var BoxId = Guid.NewGuid();
        var deleteBoxRequest = new DeleteBoxRequest(BoxId.ToString(), true);
        _mediatorMock.Send(Arg.Any<DeleteBoxCommand>, default)
        .Returns(Task.FromResult(true));
        var boxesServices = new BoxesServices(_mediatorMock, _boxesQueriesMock, _identityServiceMock, _loggerMock, _boxContentsQueriesMock);
        var deleteBoxContentCommand = new DeleteBoxContentCommand(Guid.Parse(deleteBoxRequest.Id));
        // Act
        var result =  await BoxesApi.DeleteBoxRequest(requestId, deleteBoxRequest, boxesServices);
        // Assert
        Microsoft.VisualStudio.TestTools.UnitTesting.Assert.IsNotNull(deleteBoxRequest);
        Microsoft.VisualStudio.TestTools.UnitTesting.Assert.IsInstanceOfType<Ok>(result.Result);
    }




}
