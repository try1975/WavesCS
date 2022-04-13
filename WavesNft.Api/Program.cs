using Waves.standard;
using WavesNft.Api.Options;
using WavesNft.Api.Utils;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.Configure<WavesSettings>(builder.Configuration.GetSection(nameof(WavesSettings)));
//var wavesSettings = new WavesSettings();
//builder.Configuration.GetSection(nameof(WavesSettings)).Bind(wavesSettings);
builder.Services.AddSingleton(services =>
{
    var wavesSettings = new WavesSettings();
    builder.Configuration.GetSection(nameof(WavesSettings)).Bind(wavesSettings);
    return wavesSettings;
});
builder.Services.AddSingleton(services =>
{
    //var wavesSettings = new WavesSettings();
    //builder.Configuration.GetSection(nameof(WavesSettings)).Bind(wavesSettings);
    var wavesSettings = services.GetRequiredService<WavesSettings>();
    return new Node(wavesSettings.NetChainId);
});
builder.Services.AddSingleton(services =>
{
    //var wavesSettings = new WavesSettings();
    //builder.Configuration.GetSection(nameof(WavesSettings)).Bind(wavesSettings);
    var wavesSettings = services.GetRequiredService<WavesSettings>();
    if (!string.IsNullOrEmpty(wavesSettings.Seed)) return PrivateKeyAccount.CreateFromSeed(wavesSettings.Seed, wavesSettings.NetChainId);
    return PrivateKeyAccount.CreateFromPrivateKey(wavesSettings.PrivateKey, wavesSettings.NetChainId);
});
builder.Services.AddScoped<IWavesNftService, WavesNftService>();
builder.Services.AddSingleton<IWavesApiService, WavesApiService>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
