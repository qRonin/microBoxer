using MicroBoxer.Web.Services.ViewModel;
using System.Text.Json;

namespace MicroBoxer.Web.Services;

public class BoxesService(HttpClient httpClient)
{
    private readonly string remoteServiceBaseUrlBoxes = "/api/boxesapi/box/";
    private readonly string remoteServiceBaseUrlContents = "/api/boxesapi/content/";
    public Task<BoxRecord[]> GetBoxes()
    {
       var result = httpClient.GetFromJsonAsync<BoxRecord[]>(remoteServiceBaseUrlBoxes)!;
        return result;
    }
    public Task<BoxRecord> GetBox(Guid id)
    {
        var result = httpClient.GetFromJsonAsync<BoxRecord>(remoteServiceBaseUrlBoxes+id)!;
        return result;
    }
    public Task<BoxContentRecord[]> GetBoxContents(Guid id)
    {
        var result = httpClient.GetFromJsonAsync<BoxContentRecord[]>($"/api/boxesapi/box/contents/{id}")!;
        return result;
    }
    public async Task<Guid?> CreateBox(CreateBoxRequest request
        , Guid requestId
        )
    {
        var requestMessage = new HttpRequestMessage(HttpMethod.Put, remoteServiceBaseUrlBoxes);
        requestMessage.Headers.Add("x-requestid", requestId.ToString());
        requestMessage.Content = JsonContent.Create(request);
        var response = await httpClient.SendAsync(requestMessage);
        var result = JsonSerializer.Deserialize(response.Content.ReadAsStream(), typeof(Guid?)) as Guid?;

        if (result != null)
            return (Guid)result;
            else return null;
    }
    public Task<HttpResponseMessage> CreateBoxContent(CreateBoxContentRequest request
    , Guid requestId
    )
    {
        var requestMessage = new HttpRequestMessage(HttpMethod.Put, remoteServiceBaseUrlContents);
        requestMessage.Headers.Add("x-requestid", requestId.ToString());
        requestMessage.Content = JsonContent.Create(request);
        return httpClient.SendAsync(requestMessage);
    }
    public Task<HttpResponseMessage> UpdateBox(UpdateBoxRequest request
    , Guid requestId
    )
    {
        // new() { PropertyNameCaseInsensitive = true,WriteIndented = true  }
        var serializedRequest = JsonSerializer.Serialize(request);
        var requestMessage = new HttpRequestMessage(HttpMethod.Put, remoteServiceBaseUrlBoxes+"update/");
        requestMessage.Headers.Add("x-requestid", requestId.ToString());
        requestMessage.Content = JsonContent.Create(request);
        //requestMessage.Content =  JsonContent.Create(serializedRequest);
        return httpClient.SendAsync(requestMessage);
    }
    public Task<HttpResponseMessage> UpdateBoxContent(UpdateBoxContentRequest request
    , Guid requestId
    )
    {
        var requestMessage = new HttpRequestMessage(HttpMethod.Put, remoteServiceBaseUrlContents+"update/");
        requestMessage.Headers.Add("x-requestid", requestId.ToString());
        requestMessage.Content = JsonContent.Create(request);
        return httpClient.SendAsync(requestMessage);
    }
    public Task<HttpResponseMessage> DeleteBox(DeleteBoxRequest request
    , Guid requestId
    )
    {
        var requestMessage = new HttpRequestMessage(HttpMethod.Put, remoteServiceBaseUrlBoxes+"delete/");
        requestMessage.Headers.Add("x-requestid", requestId.ToString());
        requestMessage.Content = JsonContent.Create(request);
        return httpClient.SendAsync(requestMessage);
    }
    public Task<HttpResponseMessage> DeleteBoxContent(DeleteBoxContentRequest request
    , Guid requestId
    )
    {
        var requestMessage = new HttpRequestMessage(HttpMethod.Put, remoteServiceBaseUrlContents+"delete/");
        requestMessage.Headers.Add("x-requestid", requestId.ToString());
        requestMessage.Content = JsonContent.Create(request);
        return httpClient.SendAsync(requestMessage);
    }

}
public record BoxRecord(
    string BoxName,
    string Id,
    BoxContentRecord[] BoxContents
    );


public record BoxContentRecord(
    string Name,
    string BoxId,
    string Description,
    string Id
    );