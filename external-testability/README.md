## Library Creation

There are various external libraries used throughout projects, but they are often used inconsistently and most of the implementations are not testable.

Below is an attempt to catalog library usage and places where there is duplication .

The following are areas of code that need to be explored:

* [HttpClient](#HttpClient)
* FileSystem
* Messaging
* Logging
    * BaseLog
    * [LogExceptionHandler](#LogExceptionHandler)
* EventLog
* Sql - `BaseRepository/IBaseRepository`
* [WebApi](#WebApi)
    * [CompressedContent class](#CompressedContent)
    

<a name="Sql"></a>
### Sql

There is a class used in 5 different places called `BaseRepository` with an interface `IBaseRepository`. This code is difficult to test as is, so if coverage is desired, it should be moved to one place and

*Usage*
*TODO*


<a name="HttpClient"></a>
### HttpClient 

There are:
* multiple styles of usage
* incorrect use of HttpClient (instatiating for each usage, which will cause issues in high volume systems)
* most usage prevents testability.

##### Use Cases

* The instantiated `HttpClient` should not allow users to add headers or change the base url, otherwise any other users of it may be negatively impacted. Example: Service1 class makes requests to `https://google.com` and Service2 class then tries to make a request to `https://yahoo.com`. There should be no conflicts in doing so.

##### Proposed Implementation

```csharp
public interface IHttpService
{
    Task<HttpResponseMessage> SendAsync(HttpRequestMessage req);
}

public class HttpService : IHttpService
{
    private static HttpClient _client;

    public HttpClient GetHttpClient()
    {
        if (_client == null)
        {
            _client = new HttpClient();
        }
        
        return _client;
    }

    public Task<HttpResponseMessage> SendAsync(HttpRequestMessage req)
    {
        return GetHttpClient().SendAsync(req);
    }
}
```

##### Usage

```csharp
/* This can be injected */
var _httpService = new HttpService();

/* Build the request object - this could be implemented in a helper method which can be tested */
var req = new HttpRequestMessage();
req.Method = HttpMethod.Post;
req.RequestUri = new Uri(new Uri(/* baseUrl */ ), new Uri(/* pathUrl */, UriKind.Relative));
req.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

string payload = JsonConvert.SerializeObject(/* some Object */)
req.Content = new StringContent(payload);

/* Execute the request and read the response */
HttpResponseMessage res = await _httpService.SendAsync(req);
string responseBody = await res.Content.ReadAsStringAsync();
```

### Logging


<a name="BaseLog"></a>
##### Base Log

<a name="LogExceptionHandler"></a>
##### LogExceptionHandler




<a name="WebApi"></a>
### Web API

There most likely are several components having to do with WebApi, which could be reused across projects.

<a name="CompressedContent"></a>
#### CompressedContent




