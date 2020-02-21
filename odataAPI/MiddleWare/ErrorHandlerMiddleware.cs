using System;
using System.IO;
using System.Net;
using System.Runtime.Serialization;
using System.Security.Authentication;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace odataAPI.MiddleWare
{
    /// <summary>
    /// Middleware to catch the application exceptions and Send the response to the frontend
    /// </summary>
    public class ErrorHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ErrorHandlerMiddleware> _logger;

        public ErrorHandlerMiddleware(RequestDelegate next, IConfiguration configuration, ILogger<ErrorHandlerMiddleware> logger)
        {
            _next = next;
            configuration.GetValue<string>("Logging:PathFormat");
            _logger = logger;
        }
        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (AggregateException ae)
            {
                foreach (var exception in ae.Flatten().InnerExceptions)
                {
                    _logger.LogError(exception.Message);
                    await HandleErrorAsync(context, exception);
                }
            }
            catch (Exception exception)
            {
                _logger.LogError(exception.Message);
                await HandleErrorAsync(context, exception);
            }
        }

        private static Task HandleErrorAsync(HttpContext context, Exception exception)
        {
            var response = ErrorValidator.Validator(exception);
            var payload = JsonConvert.SerializeObject(response);
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = response.Code;
            return context.Response.WriteAsync(payload);
        }
    }

    internal static class ErrorValidator
    {
        // https://docs.servicestack.net/error-handling

        public static ErrorResponse Validator(Exception exception)
        {
            switch (exception)
            {
                case ArgumentException _:
                    return new ErrorResponse(Convert.ToInt32(HttpStatusCode.BadRequest), "Bad Request, the request was invalid or cannot be served.");
                case SerializationException _:
                    return new ErrorResponse(Convert.ToInt32(HttpStatusCode.BadRequest), "Bad Request, the request was invalid or cannot be served.");
                case FormatException _:
                    return new ErrorResponse(Convert.ToInt32(HttpStatusCode.BadRequest), "Bad Request, the request was invalid or cannot be served.");
                case AuthenticationException _:
                    return new ErrorResponse(Convert.ToInt32(HttpStatusCode.Unauthorized), "Unauthorized, the request requires user authentication.");
                case UnauthorizedAccessException _:
                    return new ErrorResponse(Convert.ToInt32(HttpStatusCode.Forbidden), "Forbidden, the server understood the request, but is refusing it or the access is not allowed.");
                case FileNotFoundException _:
                    return new ErrorResponse(Convert.ToInt32(HttpStatusCode.NotFound), "NotFound, the requested resource could not be found but may be available in the future.");
                default:
                    return new ErrorResponse(Convert.ToInt32(HttpStatusCode.InternalServerError), "Internal Server Error");
            }
        }

        public static void Validator(HttpStatusCode httpStatusCode, string errorMessage)
        {
            switch (httpStatusCode)
            {
                case HttpStatusCode.BadRequest:
                    throw new ArgumentException(errorMessage);
                case HttpStatusCode.Unauthorized:
                    throw new AuthenticationException(errorMessage);
                case HttpStatusCode.Forbidden:
                    throw new UnauthorizedAccessException(errorMessage);
                case HttpStatusCode.NotFound:
                    throw new FileNotFoundException(errorMessage);
                case HttpStatusCode.Accepted:
                    break;
                case HttpStatusCode.AlreadyReported:
                    break;
                case HttpStatusCode.Ambiguous:
                    break;
                case HttpStatusCode.BadGateway:
                    break;
                case HttpStatusCode.Conflict:
                    break;
                case HttpStatusCode.Continue:
                    break;
                case HttpStatusCode.Created:
                    break;
                case HttpStatusCode.EarlyHints:
                    break;
                case HttpStatusCode.ExpectationFailed:
                    break;
                case HttpStatusCode.FailedDependency:
                    break;
                case HttpStatusCode.Found:
                    break;
                case HttpStatusCode.GatewayTimeout:
                    break;
                case HttpStatusCode.Gone:
                    break;
                case HttpStatusCode.HttpVersionNotSupported:
                    break;
                case HttpStatusCode.IMUsed:
                    break;
                case HttpStatusCode.InsufficientStorage:
                    break;
                case HttpStatusCode.InternalServerError:
                    break;
                case HttpStatusCode.LengthRequired:
                    break;
                case HttpStatusCode.Locked:
                    break;
                case HttpStatusCode.LoopDetected:
                    break;
                case HttpStatusCode.MethodNotAllowed:
                    break;
                case HttpStatusCode.MisdirectedRequest:
                    break;
                case HttpStatusCode.Moved:
                    break;
                case HttpStatusCode.MultiStatus:
                    break;
                case HttpStatusCode.NetworkAuthenticationRequired:
                    break;
                case HttpStatusCode.NoContent:
                    break;
                case HttpStatusCode.NonAuthoritativeInformation:
                    break;
                case HttpStatusCode.NotAcceptable:
                    break;
                case HttpStatusCode.NotExtended:
                    break;
                case HttpStatusCode.NotImplemented:
                    break;
                case HttpStatusCode.NotModified:
                    break;
                case HttpStatusCode.OK:
                    break;
                case HttpStatusCode.PartialContent:
                    break;
                case HttpStatusCode.PaymentRequired:
                    break;
                case HttpStatusCode.PermanentRedirect:
                    break;
                case HttpStatusCode.PreconditionFailed:
                    break;
                case HttpStatusCode.PreconditionRequired:
                    break;
                case HttpStatusCode.Processing:
                    break;
                case HttpStatusCode.ProxyAuthenticationRequired:
                    break;
                case HttpStatusCode.RedirectKeepVerb:
                    break;
                case HttpStatusCode.RedirectMethod:
                    break;
                case HttpStatusCode.RequestedRangeNotSatisfiable:
                    break;
                case HttpStatusCode.RequestEntityTooLarge:
                    break;
                case HttpStatusCode.RequestHeaderFieldsTooLarge:
                    break;
                case HttpStatusCode.RequestTimeout:
                    break;
                case HttpStatusCode.RequestUriTooLong:
                    break;
                case HttpStatusCode.ResetContent:
                    break;
                case HttpStatusCode.ServiceUnavailable:
                    break;
                case HttpStatusCode.SwitchingProtocols:
                    break;
                case HttpStatusCode.TooManyRequests:
                    break;
                case HttpStatusCode.UnavailableForLegalReasons:
                    break;
                case HttpStatusCode.UnprocessableEntity:
                    break;
                case HttpStatusCode.UnsupportedMediaType:
                    break;
                case HttpStatusCode.Unused:
                    break;
                case HttpStatusCode.UpgradeRequired:
                    break;
                case HttpStatusCode.UseProxy:
                    break;
                case HttpStatusCode.VariantAlsoNegotiates:
                    break;
                default:
                    throw new Exception(errorMessage);
            }
        }
    }
    internal sealed class ErrorResponse
    {
        public ErrorResponse(int code, string message)
        {
            Code = code;
            Message = message;
        }

        public int Code { get; set; }
        public string Message { get; set; }
    }
}
