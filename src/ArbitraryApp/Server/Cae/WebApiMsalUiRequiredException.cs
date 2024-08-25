using System.Net;
using System.Net.Http.Headers;

namespace ArbitraryApp.Server.Cae;

/// <summary>
/// This exception class is used to pass HTTP CAE unauthorized responses from a HttpClient and 
/// return the WWW-Authenticate header with the required claims challenge. 
/// This is only required if using a downstream API
/// </summary>
public class WebApiMsalUiRequiredException : Exception
{
    private readonly HttpResponseMessage _httpResponseMessage;

    public WebApiMsalUiRequiredException(string message, HttpResponseMessage response)
        : base(message)
    {
        _httpResponseMessage = response;
    }

    public HttpStatusCode StatusCode => _httpResponseMessage.StatusCode;

    public HttpResponseHeaders Headers => _httpResponseMessage.Headers;

    public HttpResponseMessage HttpResponseMessage => _httpResponseMessage;
}