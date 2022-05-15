using STS.Common.ExceptionMiddlewareExtensions;
using STS.Interfaces.Contracts;
using STS.Services.Impls;
using STS.WebApi.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().ConfigureApiBehaviorOptions((opt =>
{
    opt.ChangeModelStateInvalidModel();
}));

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddTransient<IPermissionService, PermissionService>();
builder.Services.AddTransient<IRoleService, RoleService>();


builder.Services.AddApiVersioning();


builder.Services.AddSwaggerGen();
builder.Services.AddLogging();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.ConfigureCustomExceptionMiddleware();


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();