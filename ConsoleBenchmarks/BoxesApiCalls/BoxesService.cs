using ConsoleBenchmarks.BoxesApiCalls;
using System.Net.Http.Json;
using System.Text.Json;

namespace ConsoleBenchmarks.BoxesApiCalls;

public class BoxesService(HttpClient httpClient)
{



    private readonly string remoteServiceBaseUrlBoxes = "https://localhost:7046/api/boxesapi/box/";
    private readonly string remoteServiceBaseUrlContents = "https://localhost:7046/api/boxesapi/content/";
    public Task<BoxRecord[]> GetBoxes()
    {


       var result = httpClient.GetFromJsonAsync<BoxRecord[]>(remoteServiceBaseUrlBoxes)!;
        Console.WriteLine(result);
        return result;
    }
    public Task<BoxRecord> GetBox(Guid id)
    {
        httpClient.DefaultRequestVersion = new Version(1, 0);
        var result = httpClient.GetFromJsonAsync<BoxRecord>(remoteServiceBaseUrlBoxes+id)!;
        Console.WriteLine(result);
        return result;
    }

    public async Task<Guid?> CreateBox(CreateBoxRequest request
        , Guid requestId
        )
    {
        httpClient.DefaultRequestVersion = new Version(1, 0);
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
        httpClient.DefaultRequestVersion = new Version(1, 0);
        var requestMessage = new HttpRequestMessage(HttpMethod.Put, remoteServiceBaseUrlContents);
        requestMessage.Headers.Add("x-requestid", requestId.ToString());
        requestMessage.Content = JsonContent.Create(request);
        return httpClient.SendAsync(requestMessage);
    }
    public Task<HttpResponseMessage> UpdateBox(UpdateBoxRequest request
    , Guid requestId
    )
    {
        httpClient.DefaultRequestVersion = new Version(1, 0);
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
        httpClient.DefaultRequestVersion = new Version(1, 0);
        var requestMessage = new HttpRequestMessage(HttpMethod.Put, remoteServiceBaseUrlContents+"update/");
        requestMessage.Headers.Add("x-requestid", requestId.ToString());
        requestMessage.Content = JsonContent.Create(request);
        return httpClient.SendAsync(requestMessage);
    }
    public Task<HttpResponseMessage> DeleteBox(DeleteBoxRequest request
    , Guid requestId
    )
    {
        httpClient.DefaultRequestVersion = new Version(1, 0);
        var requestMessage = new HttpRequestMessage(HttpMethod.Put, remoteServiceBaseUrlBoxes+"delete/");
        requestMessage.Headers.Add("x-requestid", requestId.ToString());
        requestMessage.Content = JsonContent.Create(request);
        return httpClient.SendAsync(requestMessage);
    }
    public Task<HttpResponseMessage> DeleteBoxContent(DeleteBoxContentRequest request
    , Guid requestId
    )
    {
        httpClient.DefaultRequestVersion = new Version(1, 0);
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