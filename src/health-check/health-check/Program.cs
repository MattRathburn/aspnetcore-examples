using health_check;
using health_check.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpContextAccessor();
builder.Services.AddHttpClient<OKHttpStatusCodeHealthCheck>();
builder.Services.AddHttpClient<Error2HttpStatusCodeHealthCheck>();
builder.Services.AddSingleton<StatusOK>().AddSingleton<StatusBadRequest>();

builder.Services.AddHealthChecks()
    .AddCheck<OKHttpStatusCodeHealthCheck>("OK Status Check")
    .AddCheck<Error2HttpStatusCodeHealthCheck>("Error Status 2 Check");

builder.Services.AddControllersWithViews();


var app = builder.Build();

app.MapHealthChecks("/HealthCheck", new Microsoft.AspNetCore.Diagnostics.HealthChecks.HealthCheckOptions
{
    ResponseWriter = async (context, health) =>
    {
        context.Response.Headers.Add("content-type", "text/plain");

        if (health.Status == Microsoft.Extensions.Diagnostics.HealthChecks.HealthStatus.Healthy)
            await context.Response.WriteAsync("Everything is good");
        else
        {
            foreach (var h in health.Entries)
            {
                await context.Response.WriteAsync($"Key = {h.Key} :: Description = {h.Value.Description} :: Status = {h.Value.Status} \n");
            }

            await context.Response.WriteAsync($"\n\n Overall Status: {health.Status}");
        }
    }
});

app.MapDefaultControllerRoute();

app.Run();
