namespace user.api.Extensions;

public static class ApplicationBuilderExtensions
{
    public static void UseGlobalExceptionHandler(this IApplicationBuilder app)
    {
        app.UseExceptionHandler(errorApp =>
        {
            errorApp.Run(async context =>
            {
                context.Response.StatusCode = 500;
                await context.Response.WriteAsync("Something went wrong.");
            });
        });
    }
}

