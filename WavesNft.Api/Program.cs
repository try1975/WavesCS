using Waves.standard;
using WavesNft.Api.Options;
using WavesNft.Api.Utils;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.Configure<WavesSettings>(builder.Configuration.GetSection(nameof(WavesSettings)));
#region di
builder.Services.AddSingleton<WavesSettings>(services =>
{
    var wavesSettings = new WavesSettings();
    builder.Configuration.GetSection(nameof(WavesSettings)).Bind(wavesSettings);
    return wavesSettings;
});
builder.Services.AddSingleton<Node>(services =>
{
    var wavesSettings = services.GetRequiredService<WavesSettings>();
    return new Node(wavesSettings.NetChainId);
});
builder.Services.AddSingleton<PrivateKeyAccount>(services =>
{
    var wavesSettings = services.GetRequiredService<WavesSettings>();
    if (!string.IsNullOrEmpty(wavesSettings.Seed)) return PrivateKeyAccount.CreateFromSeed(wavesSettings.Seed, wavesSettings.NetChainId);
    return PrivateKeyAccount.CreateFromPrivateKey(wavesSettings.PrivateKey, wavesSettings.NetChainId);
});
builder.Services.AddSingleton<IDeedcoinService, DeedcoinService>();
#endregion di

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Version = "v1",
        Title="Deedcoin backend API",
        Description=".net core api"
    });
});



var app = builder.Build();

// warm up
app.Services.GetService<IDeedcoinService>();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
app.UseSwagger();
    app.UseSwaggerUI();
//}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
