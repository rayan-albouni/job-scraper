using System.Net;

namespace JobScraper.Tests.Mocks;

public class HttpMessageHandlerMock : HttpMessageHandler
{
    private HttpResponseMessage _response;
    private Queue<HttpResponseMessage> _responseQueue;

    public HttpMessageHandlerMock()
    {
        _responseQueue = new Queue<HttpResponseMessage>();
        _response = new HttpResponseMessage(HttpStatusCode.OK);
    }

    public void SetupResponse(HttpStatusCode statusCode, string content = "")
    {
        _response = new HttpResponseMessage(statusCode)
        {
            Content = new StringContent(content)
        };
    }

    public void SetupSequenceResponse(params HttpResponseMessage[] responses)
    {
        _responseQueue = new Queue<HttpResponseMessage>(responses);
    }

    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        if (_responseQueue.Count > 0)
        {
            return Task.FromResult(_responseQueue.Dequeue());
        }

        return Task.FromResult(_response);
    }
}
