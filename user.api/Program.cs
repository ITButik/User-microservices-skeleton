using user.api.Endpoints;
using user.api.Extensions;
using user.api.Middlewares;


var builder = WebApplication.CreateBuilder(args);

// Add services
//builder.Services.AddControllers();
builder.Services.AddApplicationServices();       // MediatR, validation, etc.
builder.Services.AddInfrastructure(builder.Configuration); // DbContext, repos
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    //app.UseMiddleware<ErrorHandlerMiddleware>();
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
