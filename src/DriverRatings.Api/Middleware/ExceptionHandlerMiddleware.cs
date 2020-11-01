using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace src.DriverRatings.Api.Middleware
{

  public static class MiddlewareExtensions
  {
    public static IApplicationBuilder UseExceptionMiddleware(this IApplicationBuilder builder)
    {
      return builder.UseMiddleware(typeof(ExceptionHandlerMiddleware));
    }
  }

  internal class ExceptionHandlerMiddleware
  {
    private readonly RequestDelegate _next;

    public ExceptionHandlerMiddleware(RequestDelegate next)
    {
      this._next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
      try
      {
        await _next(context);

      }
      catch (Exception ex)
      {
        await HandleExceptionAsync(context, ex);
      }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
      var response = new { code = "error", exception = exception.Message };
      var statusCode = HttpStatusCode.BadRequest;
      var exceptionType = exception.GetType();

      switch (exception)
      {
        case Exception ex when exceptionType == typeof(UnauthorizedAccessException):
          statusCode = HttpStatusCode.Unauthorized;
          break;

        default:
          statusCode = HttpStatusCode.InternalServerError;
          break;
      }

      var payload = JsonConvert.SerializeObject(response);
      context.Response.ContentType = "application/json";
      context.Response.StatusCode = (int)statusCode;
      return context.Response.WriteAsync(payload);
    }
  }
}