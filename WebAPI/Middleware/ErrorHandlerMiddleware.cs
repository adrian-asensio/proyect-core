using System;
using System.Threading.Tasks;
using Aplicacion.ErrorHandler;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Net;

namespace WebAPI.Middleware
{
    public class ErrorHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ErrorHandlerMiddleware> _logger;
        public ErrorHandlerMiddleware(RequestDelegate next, ILogger<ErrorHandlerMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch(Exception ex)
            {
                await ExceptionHandlerAsync(context, ex, _logger);
            }
        }

        private async Task ExceptionHandlerAsync(HttpContext context, Exception exception, ILogger<ErrorHandlerMiddleware> logger)
        {
            object errores = null;
            switch(exception)
            {
                case ExceptionHandler eh:
                    logger.LogError(exception, "Handler Error");
                    errores = eh.Errores;
                    context.Response.StatusCode = (int)eh.Codigo;
                    break;
                case Exception ex:
                    logger.LogError(exception, "Error de servidor");
                    errores = string.IsNullOrWhiteSpace(ex.Message) ? "Error" : ex.Message;
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    break;
            }
            context.Response.ContentType = "application/json";
            if(errores != null)
            {
                var resultados = JsonConvert.SerializeObject(new {errores});
                await context.Response.WriteAsync(resultados);
            }
        }
    }
}