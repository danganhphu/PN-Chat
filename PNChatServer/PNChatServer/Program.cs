using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using PNChatServer.Data;
using PNChatServer.Hubs;
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

#region EntityFramework Core
builder.Services.AddDbContext<DbChatContext>(option =>
{
    option.UseLazyLoadingProxies().UseSqlServer(EnviConfig.DbConnectionString);
});
#endregion

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
app.UseRouting();
app.UseCors(policy);

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
    endpoints.MapHub<ChatHub>("/chatHub");
});

//app.MapGet("/", () => "Hello World!");

app.Run();
