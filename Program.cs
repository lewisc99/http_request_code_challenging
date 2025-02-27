using CodeChallenging.ClientServices.Contracts;
using CodeChallenging.ClientServices.Interfaces;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IFakeStoreService, FakeStoreService>();

builder.Services.AddHttpClient<IJsonplaceholderClientService,JsonplaceholderClientService>(client =>
{
    client.BaseAddress = new Uri(builder.Configuration["API"]!);
});

builder.Services.AddHttpClient("FakeStore", httpClient =>
{
    httpClient.BaseAddress = new Uri("https://fakestoreapi.com/");
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
