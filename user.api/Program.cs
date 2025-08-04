using user.api.Endpoints;
using user.api.Extensions;
using user.api.Middlewares;
using Serilog;


var builder = WebApplication.CreateBuilder(args);

// Configure Serilog from appsettings.json
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .CreateLogger();

builder.Host.UseSerilog();

// Add services
builder.Services.AddHttpClient(); // Register HttpClient for DI
builder.Services.AddApplicationServices();       // MediatR, validation, etc.
builder.Services.AddInfrastructure(builder.Configuration); // DbContext, repos
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    //app.UseDeveloperExceptionPage();
    app.UseMiddleware<ErrorHandlerMiddleware>();
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    // Add global exception handling here
    app.UseMiddleware<ErrorHandlerMiddleware>();
}

app.UseHttpsRedirection();

//app.UseAuthorization();
app.MapUserEndpoints();
//app.MapControllers();

app.Run();
