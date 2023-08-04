using Coravel;
using CoravelDemo;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<UpdateCacheService>();
builder.Services.AddScheduler();
builder.Services.AddTransient<UpdateCacheJob>();

var app = builder.Build();

#region - method 1 - 
// var provider = app.Services;
// provider.UseScheduler(scheduler =>
// {
//     scheduler.Schedule<UpdateCacheJob>()
//         .Cron("0 0 * * *")
//         .Zoned( TimeZoneInfo.FindSystemTimeZoneById("Asia/Shanghai"))
//         .RunOnceAtStart();
// }).OnError((exception) => Console.WriteLine(exception.Message));
#endregion

#region - method 2 - 
var provider = app.Services;
provider.UseScheduler(scheduler =>
{
    scheduler.Schedule(
            async () =>
            {
                var updateCacheService = provider.GetService<UpdateCacheService>();
                if (updateCacheService!=null)
                {
                    await updateCacheService.GetAsync(new CancellationToken());
                }
                await Task.CompletedTask;
            }
        )
        .Cron("0 0 * * *")
        .Zoned( TimeZoneInfo.FindSystemTimeZoneById("Asia/Shanghai"))
        .RunOnceAtStart();
}).OnError((exception) => Console.WriteLine(exception.Message));
#endregion

app.MapGet("/", () => "Hello World!");

app.Run();