using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Postera.WebApp.Data;

namespace Postera.WebApp
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        public ErrorHandlingMiddleware(RequestDelegate requestDelegate)
        {
            _next = requestDelegate;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (RequestException e) when (e.StatusCode == HttpStatusCode.Unauthorized)
            {
                await HandleAuthErrorAsync(context, e);
            }
            catch (RequestException e) when (e.StatusCode == HttpStatusCode.BadRequest)
            {
                await HandleValidationErrorAsync(context, e);
            }
            catch (Exception e)
            {
                await HandleErrorAsync(context, e);
            }
        }

        private async Task HandleErrorAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            var response = JsonConvert.SerializeObject(
                new { exception.Message, exception?.InnerException, exception.StackTrace });

            await context.Response.WriteAsync(response);
        }

        private async Task HandleAuthErrorAsync(HttpContext context, RequestException exception)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
            var response = JsonConvert.SerializeObject(new { exception.Message });

            await context.Response.WriteAsync(response);
        }

        private async Task HandleValidationErrorAsync(HttpContext context, RequestException exception)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            var exceptionMessage = new { exception.Message };
            var response = JsonConvert.SerializeObject(exceptionMessage);

            await context.Response.WriteAsync(response);
        }
    }
}