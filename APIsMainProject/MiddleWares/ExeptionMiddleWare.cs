using APIsMainProject.ResponseModule;
using System.Net;
using System.Text.Json;

namespace APIsMainProject.MiddleWares
{
    public class ExeptionMiddleWare
    {
        private readonly RequestDelegate Next;
        private readonly ILogger<ExeptionMiddleWare> logger;
        private readonly IHostEnvironment environment;

        public ExeptionMiddleWare(RequestDelegate next ,
                                  ILogger<ExeptionMiddleWare> logger,
                                  IHostEnvironment environment
                                  )
        {
            this.Next = next;
            this.logger = logger;
            this.environment = environment;
        }
        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await Next(httpContext);
            }
            catch (Exception ex)
            {
                logger.LogError(ex,ex.Message);
                httpContext.Response.ContentType = "Application/json";
                httpContext.Response.StatusCode =(int)HttpStatusCode.InternalServerError ;
                var response = environment.IsDevelopment()
                       ? new ApiExeption((int)HttpStatusCode.InternalServerError, ex.Message, ex.StackTrace.ToString())
                       : new ApiExeption((int)HttpStatusCode.InternalServerError);
                var json = JsonSerializer.Serialize(response);
                await httpContext.Response.WriteAsync(json);
            }
        }
    }
}
