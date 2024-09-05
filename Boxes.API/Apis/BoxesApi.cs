using Boxes.API.Application.Commands;
using Boxes.API.Application.Commands.Box;
using Boxes.API.Application.Commands.BoxContent;
using Boxes.API.Application.Queries;
using MicroBoxer.ServiceDefaults;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Boxes.API.Apis
{
    public static class BoxesApi
    {
        public static RouteGroupBuilder MapBoxesApiV1(this IEndpointRouteBuilder app)
        {
            var api = app.MapGroup("api/boxesapi").HasApiVersion(1.0);
            api.MapGet("/box/", GetBoxes);
            api.MapGet("/box/{id:Guid}", GetBox);
            api.MapGet("/content/{id:Guid}", GetBoxContent);
            api.MapGet("/box/contents/{boxId:Guid}", GetBoxContents);
            api.MapPut("/box/", CreateBoxRequest);
            api.MapPut("/content/", CreateBoxContentRequest);
            api.MapPut("/box/update/", UpdateBoxRequest);
            api.MapPut("/content/update/", UpdateBoxContentRequest);
            api.MapPut("/box/delete/", DeleteBoxRequest);
            //api.MapPut("/box/delete/content", DeleteBoxWithContent);
            api.MapPut("/content/delete/", DeleteBoxContentRequest);

            return api;
        }
        public static async Task<Results<Ok<BoxContentVM>, NotFound>> GetBoxContent
            (Guid? id, [AsParameters] BoxContentsServices services)
        {
            try
            {
                if (id != null)
                {

                    var boxContent = await services.Queries.GetBoxContent((Guid)id);
                    return TypedResults.Ok(boxContent);
                }
                else return TypedResults.NotFound();

            }
            catch
            {
                return TypedResults.NotFound();
            }
        }
        public static async Task<Results<Ok<IEnumerable<BoxContentVM>>, NotFound>> GetBoxContents
            (Guid? boxId, [AsParameters] BoxContentsServices services)
        {
            try
            {
                if (boxId != null)
                {
                    var boxContents = await services.Queries.GetBoxContentsByBoxId((Guid)boxId);
                    return TypedResults.Ok(boxContents);
                }
                else return TypedResults.NotFound();

            }
            catch
            {
                return TypedResults.NotFound();
            }
        }
        public static async Task<Results<Ok<BoxVM>, NotFound>> GetBox
            (Guid? id, [AsParameters] BoxesServices services)
        {
            try
            {
                if (id!=null)
                {
                    
                    var box = await services.Queries.GetBoxAsync((Guid)id);
                    return TypedResults.Ok(box);
                }
                else return TypedResults.NotFound();

            }
            catch
            {
                return TypedResults.NotFound();
            }
        }
        public static async Task<Results<Ok<IEnumerable<BoxVM>>, NotFound>> GetBoxes
            ([AsParameters] BoxesServices services)
        {
            try
            {
                var boxes = await services.Queries.GetBoxesAsync();
                return TypedResults.Ok(boxes);
            }
            catch
            {
                return TypedResults.NotFound();
            }
        }
        public static async Task<BoxDTO> CreateBox(CreateBoxCommand command, [AsParameters] BoxesServices services)
        {
            return await services.Mediator.Send(command);
        }
        public static async Task<BoxContentDTO> CreateContent(CreateBoxContentCommand command, [AsParameters] BoxesServices services)
        {
            return await services.Mediator.Send(command);
        }
        public static async Task<bool> UpdateBox(UpdateBoxCommand command, [AsParameters] BoxesServices services)
        {
            return await services.Mediator.Send(command);
        }
        public static async Task<bool> UpdateContent(UpdateBoxContentCommand command, [AsParameters] BoxesServices services)
        {
            return await services.Mediator.Send(command);
        }
        public static async Task<bool> DeleteContent(DeleteBoxContentCommand command, [AsParameters] BoxesServices services)
        {
            return await services.Mediator.Send(command);
        }
        public static async Task<bool> DeleteBox(DeleteBoxCommand command, [AsParameters] BoxesServices services)
        {
            return await services.Mediator.Send(command);
        }
        public static async Task<bool> DeleteBoxWithContent
            (DeleteBoxWithContentCommand command, [AsParameters] BoxesServices services)
        {
            return await services.Mediator.Send(command);
        }
        public static async Task<Results<Ok, BadRequest<string>, ProblemHttpResult>> UpdateBoxRequest
            ([FromHeader(Name = "x-requestid")] Guid requestId, 
            UpdateBoxRequest request,
            [AsParameters] BoxesServices services)
        {
            if (requestId == Guid.Empty) return TypedResults.BadRequest("requestId cannot be Empty");

            var updateBoxCommand = new UpdateBoxCommand(
                Guid.Parse(request.id), request.boxName, request.boxContents
                );
            var result = await UpdateBox(updateBoxCommand, services);
            if (result)
            {
                services.Logger.LogInformation($"UpdateBoxCommand succeeded - RequestId: {requestId}");
            }
            else
            {
                services.Logger.LogWarning($"UpdateBoxCommand failed - RequestId: {requestId}");
            }
            return TypedResults.Ok();
        }
        public static async Task<Results<Ok<string>, BadRequest<string>, ProblemHttpResult>> CreateBoxRequest
            ([FromHeader(Name = "x-requestid")] Guid requestId, 
            CreateBoxRequest request,
            [AsParameters] BoxesServices services)
        {
            if (requestId == Guid.Empty) return TypedResults.BadRequest("requestId cannot be Empty");

            var createBoxCommand = new CreateBoxCommand(request.BoxName);

            var requestCreateBox = new IdentifiedCommand<CreateBoxCommand, BoxDTO>(createBoxCommand, requestId);

            //var result = await CreateBox(createBoxCommand, services);
            var result = await services.Mediator.Send(requestCreateBox);

            if (result != null)
            {
                services.Logger.LogInformation($"CreateBoxCommand succeeded - RequestId: {requestId}");
            }
            else
            {
                services.Logger.LogWarning($"CreateBoxCommand failed - RequestId: {requestId}");
            }

            var res = TypedResults.Ok<string>(result.Id.ToString());
            return res;

        }
        public static async Task<Results<Ok, BadRequest<string>, ProblemHttpResult>> DeleteBoxRequest
            ([FromHeader(Name = "x-requestid")] Guid requestId, 
            DeleteBoxRequest request,
            [AsParameters] BoxesServices services)
        {
            if (requestId == Guid.Empty) return TypedResults.BadRequest("requestId cannot be Empty");


            bool result;
            if (request.withContent)
            {
                var deleteBoxWithContentCommand = new DeleteBoxWithContentCommand(Guid.Parse(request.Id));
                result = await DeleteBoxWithContent(deleteBoxWithContentCommand, services);
            }
            else
            {
                var deleteBoxCommand = new DeleteBoxCommand(Guid.Parse(request.Id));
                result = await DeleteBox(deleteBoxCommand, services);
            }

            if (result)
            {
                services.Logger.LogInformation($"DeleteBoxCommand succeeded - RequestId: {requestId}");
            }
            else
            {
                services.Logger.LogWarning($"DeleteBoxCommand failed - RequestId: {requestId}");
            }

            return TypedResults.Ok();

        }
        public static async Task<Results<Ok, BadRequest<string>, ProblemHttpResult>> DeleteBoxContentRequest
            ([FromHeader(Name = "x-requestid")] Guid requestId, 
            DeleteBoxContentRequest request,
            [AsParameters] BoxesServices services)
        {
            if (requestId == Guid.Empty) return TypedResults.BadRequest("requestId cannot be Empty");

            var deleteBoxContentCommand = new DeleteBoxContentCommand(Guid.Parse(request.Id));

           var result = await DeleteContent(deleteBoxContentCommand, services);

            if (result)
            {
                services.Logger.LogInformation($"DeleteBoxContentCommand succeeded - RequestId: {requestId}");
            }
            else
            {
                services.Logger.LogWarning($"DeleteBoxContentCommand failed - RequestId: {requestId}");
            }

            return TypedResults.Ok();

        }
        public static async Task<Results<Ok, BadRequest<string>, ProblemHttpResult>> UpdateBoxContentRequest
            ([FromHeader(Name = "x-requestid")] Guid requestId, 
            UpdateBoxContentRequest request,
            [AsParameters] BoxesServices services)
        {
            if (requestId == Guid.Empty) return TypedResults.BadRequest("requestId cannot be Empty");

            var updateBoxContentCommand = new UpdateBoxContentCommand(request.Name, request.Description,
                 Guid.Parse(request.BoxId), Guid.Parse(request.Id));

            var result = await UpdateContent(updateBoxContentCommand, services);

            if (result)
            {
                services.Logger.LogInformation($"UpdateBoxContentCommand succeeded - RequestId: {requestId}");
            }
            else
            {
                services.Logger.LogWarning($"UpdateBoxContentCommand failed - RequestId: {requestId}");
            }

            return TypedResults.Ok();

        }
        public static async Task<Results<Ok<string>, BadRequest<string>, ProblemHttpResult>> CreateBoxContentRequest
            ([FromHeader(Name = "x-requestid")] Guid requestId,
            CreateBoxContentRequest request,
            [AsParameters] BoxesServices services)
        {
            if(requestId == Guid.Empty) return TypedResults.BadRequest("requestId cannot be Empty");

            var createBoxContentCommand = new CreateBoxContentCommand(request.Name, request.Description,
                 Guid.Parse(request.BoxId));

            var requestCreateBoxContent = new IdentifiedCommand<CreateBoxContentCommand, BoxContentDTO>(createBoxContentCommand, requestId);

            //var result = await CreateContent(createBoxContentCommand, services);
            var result = await services.Mediator.Send(requestCreateBoxContent);


            if (result != null)
            {
                services.Logger.LogInformation($"CreateBoxContentCommand succeeded - RequestId: {requestId}");
            }
            else
            {
                services.Logger.LogWarning($"CreateBoxContentCommand failed - RequestId: {requestId}");
            }
            return TypedResults.Ok(result.Id.ToString());
        }
    }
}

