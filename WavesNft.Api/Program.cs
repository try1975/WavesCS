using Waves.standard;
using WavesNft.Api.Options;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.Configure<WavesSettings>(builder.Configuration.GetSection(nameof(WavesSettings)));
builder.Services.AddSingleton(services =>
{
    var chain = builder.Configuration[$"{nameof(WavesSettings)}:{nameof(WavesSettings.Chain)}"];
    var nodeChainId = Node.MainNetChainId;
    if (string.IsNullOrEmpty(chain) || chain.StartsWith("Test")) nodeChainId = Node.TestNetChainId;
    return new Node(nodeChainId);
});

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
