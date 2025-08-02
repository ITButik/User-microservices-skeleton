using user.application.Exceptions;

namespace user.api.Middlewares;

public class ErrorHandlerMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ErrorHandlerMiddleware> _logger;
    private readonly IHostEnvironment _env;

    public ErrorHandlerMiddleware(RequestDelegate next, ILogger<ErrorHandlerMiddleware> logger, IHostEnvironment env)
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
        //catch (DomainException ex)
        //{
        //    _logger.LogWarning(ex, "Domain error");
        //    context.Response.StatusCode = 400;
        //    await context.Response.WriteAsJsonAsync(getErrorObject("Domain error", ex));
        //}
        catch (ExternalServiceException ex)
        {
            _logger.LogError(ex, "External Service error");
            context.Response.StatusCode = 500;
            await context.Response.WriteAsJsonAsync(getErrorObject("External Service error", ex));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unhandled exception");
            context.Response.StatusCode = 500;
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsJsonAsync(getErrorObject("Exception", ex));
        }
    }

    private string GetOriginalExceptionMessage(Exception ex)
    {
        if (ex == null) return string.Empty;

        while (ex.InnerException != null)
        {
            ex = ex.InnerException;
        }

        return ex.Message;
    }

    private object getErrorObject(string title, Exception ex)
    {
        var error = new
        {
            Title = title,
            Detail = _env.IsProduction()? ex.Message : GetOriginalExceptionMessage(ex)  
        };
        return error;
    }

}

