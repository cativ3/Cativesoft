using Cativesoft.Utilities.Responses;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using System;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Cativesoft.Core.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                switch (ex)
                {
                    case ValidationException error:
                        await ExceptionHandlerAsync(httpContext, error.Errors.ToList()[0].ErrorMessage);
                        break;
                    default:
                        await ExceptionHandlerAsync(httpContext, ex.Message);
                        break;
                }
            }
        }

        public async Task ExceptionHandlerAsync(HttpContext httpContext, string errorMessage)
        {
            var response = new ApiResponse(false, errorMessage, errorMessage);

            httpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            httpContext.Response.ContentType = "application/json";

            var jsonResponse = JsonSerializer.Serialize(response);

            await httpContext.Response.WriteAsync(jsonResponse);
        }
    }
}
