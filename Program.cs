using CodeChallenging.ClientServices.Contracts;
using CodeChallenging.ClientServices.Interfaces;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IFakeStoreService, FakeStoreService>();


string fakeStoreApiUrl = builder.Configuration.GetValue<string>("API:FakeStore");

builder.Services.AddHttpClient<IFakeStoreService, FakeStoreService>(client =>
{
    client.BaseAddress = new Uri(fakeStoreApiUrl);
});

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
