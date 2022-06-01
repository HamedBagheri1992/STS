using STS.Common.Configuration;
using STS.Common.ExceptionMiddlewareExtensions;
using STS.Interfaces.Contracts;
using STS.Services.Extensions;
using STS.Services.Impls;
using STS.Services.ServiceSetups;
using STS.WebApi.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

#region Configuration

builder.Services.Configure<BearerTokensConfigurationModel>(builder.Configuration.GetSection(BearerTokensConfigurationModel.Name));

#endregion

builder.Services.AddControllers().ConfigureApiBehaviorOptions((opt =>
{
    opt.ChangeModelStateInvalidModel();
}));

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddTransient<IAccountService, AccountService>();
builder.Services.AddTransient<IApplicationService, ApplicationService>();
builder.Services.AddTransient<IRoleService, RoleService>();
builder.Services.AddTransient<IPermissionService, PermissionService>();
builder.Services.AddTransient<IUserService, UserService>();


builder.Services.AddDbContext(builder.Configuration);
builder.Services.AddApiVersioning();
builder.Services.ConfigureAuthentication(builder.Configuration);


builder.Services.ConfigureSwaggerGen();
builder.Services.AddLogging();

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


app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.MigrationDatabase().Run();