using Microsoft.EntityFrameworkCore;
using PNChatServer.Data;
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

#region EntityFramework Core
builder.Services.AddDbContext<DbChatContext>(option =>
{
    option.UseLazyLoadingProxies().UseSqlServer(EnviConfig.DbConnectionString);
});
#endregion

var app = builder.Build();



app.MapGet("/", () => "Hello World!");

app.Run();
