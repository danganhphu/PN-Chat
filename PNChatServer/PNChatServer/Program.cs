using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using PNChatServer.Data;
using PNChatServer.Hubs;
using PNChatServer.Repository;
using PNChatServer.Service;
using PNChatServer.Utils;


var policy = "_anyCorsPolicy";

var builder = WebApplication.CreateBuilder(args);

EnviConfig.Config(builder.Configuration);
builder.Services.AddCors(options =>
{
    options.AddPolicy(policy,
        builder =>
        {
            builder.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
        });
});

builder.Services.AddSignalR();
builder.Services.AddControllers();

#region jwt
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
#endregion

#region EntityFramework Core
builder.Services.AddDbContext<DbChatContext>(option =>
{
    option.UseLazyLoadingProxies().UseSqlServer(EnviConfig.DbConnectionString);
});
#endregion

#region dependency injection
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<ICallService, CallService>();
builder.Services.AddScoped<IChatBoardService, ChatBoardService>();
builder.Services.AddScoped<IUserService, UserService>();
#endregion

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
app.UseRouting();
app.UseCors(policy);
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
    endpoints.MapHub<ChatHub>("/chatHub");
});

//app.MapGet("/", () => "Hello World!");

app.Run();
