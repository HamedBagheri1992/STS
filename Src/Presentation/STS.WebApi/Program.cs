using STS.Common.ExceptionMiddlewareExtensions;
using STS.Interfaces.Contracts;
using STS.Services.Extensions;
using STS.Services.Impls;
using STS.WebApi.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().ConfigureApiBehaviorOptions((opt =>
{
    opt.ChangeModelStateInvalidModel();
}));

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddTransient<IApplicationService, ApplicationService>();
builder.Services.AddTransient<IRoleService, RoleService>();
builder.Services.AddTransient<IPermissionService, PermissionService>();

builder.Services.AddDbContext(builder.Configuration);

builder.Services.AddApiVersioning();


builder.Services.AddSwaggerGen();
builder.Services.AddLogging();
builder.Services.AddCors();

var app = builder.Build();

// Configure the HTTP request pipeline.

//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}

app.UseSwagger();
app.UseSwaggerUI();

app.ConfigureCustomExceptionMiddleware();


app.UseCors(options => options.WithOrigins(
    builder.Configuration.GetSection("AppSettings").GetValue<string>("AllowedOrigins")
    .Split(";")).AllowAnyMethod().AllowCredentials().AllowAnyHeader().AllowAnyMethod().Build());

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();