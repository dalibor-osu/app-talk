using System.Net;
using AppTalk.API;
using AppTalk.API.Hubs;
using AppTalk.Core.Extensions;
using AppTalk.Models.Database;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Configure();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider
        .GetRequiredService<AppTalkDatabaseContext>();

    await dbContext.Database.MigrateAsync();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapHub<MessageHub>("/message");

app.MapControllers();
await app.RunAsync();