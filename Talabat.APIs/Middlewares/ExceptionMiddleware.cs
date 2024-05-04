using System.Net;
using System.Text.Json;
using Talabat.APIs.Errors;

namespace Talabat.APIs.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate next;
        private readonly ILogger<ExceptionMiddleware> logger;
        private readonly IHostEnvironment env;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger, IHostEnvironment env)
        {
            this.next = next;
            this.logger = logger;
            this.env = env;
        }
        public async Task Invoke(HttpContext context)
        {
            try
            {
                await next.Invoke(context);
            }
            catch (Exception ex)
            {

                logger.LogError(ex, ex.Message);

                context.Response.ContentType = "application/json";
                var statusCode = context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                var options = new JsonSerializerOptions()
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                };

                var response = env.IsDevelopment() ?
                    new ApiExceptionResponse((int)statusCode, ex.Message, ex.StackTrace.ToString())
                    :
                    new ApiExceptionResponse((int)statusCode, ex.Message);

                var json = JsonSerializer.Serialize(response, options);

                await context.Response.WriteAsync(json);
            }
        }
    }
}
