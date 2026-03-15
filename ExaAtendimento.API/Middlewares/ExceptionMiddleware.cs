using System.Net;
using System.Text.Json;
using ExaAtendimento.Application.Exceptions;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ExaAtendimento.API.Middlewares;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware> _logger;
    private readonly IHostEnvironment _env;

    public ExceptionMiddleware(RequestDelegate next,
                               ILogger<ExceptionMiddleware> logger,
                               IHostEnvironment env)
    {
        _next = next;
        _logger = logger;
        _env = env;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ocorreu um erro inesperado.");
            await HandleExceptionAsync(context, ex);
        }
    }

    private Task HandleExceptionAsync(HttpContext context, Exception ex)
    {
        context.Response.ContentType = "application/json";

        int statusCode = (int)HttpStatusCode.InternalServerError;
        object response;

        if (ex is ApiValidationException validationEx)
        {
            statusCode = (int)HttpStatusCode.BadRequest;

            response = new
            {
                statusCode,
                message = "Erros de validação.",
                errors = validationEx.Errors
            };
        }
        else if (ex is BusinessException businessEx)
        {
            statusCode = (int)HttpStatusCode.BadRequest;

            response = new
            {
                statusCode,
                message = businessEx.Message
            };
        }
        else if (_env.IsDevelopment())
        {
            response = new
            {
                statusCode,
                message = ex.Message,
                stackTrace = ex.StackTrace
            };
        }
        else
        {
            response = new
            {
                statusCode,
                message = "Ocorreu um erro inesperado."
            };
        }

        context.Response.StatusCode = statusCode;

        var json = JsonSerializer.Serialize(response, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = true
        });

        return context.Response.WriteAsync(json);
    }
}
