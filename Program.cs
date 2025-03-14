using CodeChallenging.ClientServices.Contracts;
using CodeChallenging.ClientServices.Interfaces;
using CodeChallenging.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.CircuitBreaker;
using Polly.Extensions.Http;

var builder = WebApplication.CreateBuilder(args);

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


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
