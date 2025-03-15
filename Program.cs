using CodeChallenging.ClientServices.Contracts;
using CodeChallenging.ClientServices.Interfaces;
using CodeChallenging.Database;
using CodeChallenging.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Polly;
using Polly.Extensions.Http; 

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHealthChecks()
    // URL health check for FakeStore API
    .AddUrlGroup(new Uri(builder.Configuration["API:FakeStore"]),
        name: "FakeStore API",
        failureStatus: HealthStatus.Unhealthy)

    // URL health check for JsonPlaceholder API
    .AddUrlGroup(new Uri(builder.Configuration["API:JsonPlaceholder"]),
        name: "JsonPlaceholder API",
        failureStatus: HealthStatus.Unhealthy)

    // Custom SQL Server health check
    .AddCheck(
        "OrderingDB-check",
        new SqlConnectionHealthCheck(builder.Configuration.GetConnectionString("DefaultConnection")),
        HealthStatus.Unhealthy,
        new[] { "HttpClientDb" });

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IFakeStoreService, FakeStoreService>();

string fakeStoreApiUrl = builder.Configuration.GetValue<string>("API:FakeStore");
var circuitBreakerPolicy = GetCircuitBreakerPolicy();

builder.Services.AddHttpClient<IFakeStoreService, FakeStoreService>(client =>
{
    client.BaseAddress = new Uri(fakeStoreApiUrl);
})
.SetHandlerLifetime(TimeSpan.FromMinutes(5))  // Sample: default lifetime is 2 minutes
// Old code without Jitter
//.AddPolicyHandler(PollyExtensions.GetRetryPolicy());
.AddPolicyHandler(PollyExtensions.GetJitterRetryPolicy())
.AddPolicyHandler(circuitBreakerPolicy);

static IAsyncPolicy<HttpResponseMessage> GetCircuitBreakerPolicy()
{
    return HttpPolicyExtensions
        .HandleTransientHttpError()
          .CircuitBreakerAsync(
        5, // Number of failures before breaking the circuit
        TimeSpan.FromSeconds(30) // Duration of break
    );
}

string jsonPlaceholderUrl = builder.Configuration.GetValue<string>("API:JsonPlaceholder");

builder.Services.AddHttpClient<IJsonplaceholderClientService, JsonplaceholderClientService>(client =>
{
    client.BaseAddress = new Uri(jsonPlaceholderUrl);
});

builder.Services.AddDbContext<HealthCheckDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"),
    sqlOptions => sqlOptions.MigrationsAssembly("CodeChallenging")));

builder.Services
    .AddHealthChecksUI(settings =>
    {
        settings.SetEvaluationTimeInSeconds(5); // How often to check health status
        settings.SetMinimumSecondsBetweenFailureNotifications(60); // Minimum time between failure notifications
        settings.MaximumHistoryEntriesPerEndpoint(50); // Number of history entries to keep
        
    })
      .AddSqlServerStorage(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"),
        sqlOptions => sqlOptions.MigrationsAssembly("CodeChallenging"))
);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapHealthChecks("/hc");
app.UseHealthChecksUI(config => config.UIPath = "/hc-ui");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
