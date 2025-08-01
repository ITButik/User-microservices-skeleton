namespace user.api.Middlewares;

public class ErrorHandlerMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ErrorHandlerMiddleware> _logger;

    public ErrorHandlerMiddleware(RequestDelegate next, ILogger<ErrorHandlerMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unhandled exception");

            context.Response.StatusCode = 500;
            context.Response.ContentType = "application/json";

            var error = new
            {
                Title = "Unexpected error",
                Detail = context.RequestServices.GetService<IWebHostEnvironment>()?.IsDevelopment() == true
                    ? ex.Message
                    : "Please contact support."
            };

            await context.Response.WriteAsJsonAsync(error);
        }
    }
}

