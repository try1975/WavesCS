using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Waves.standard;
using WavesNft.Api.Model;
using WavesNft.Api.Options;

namespace WavesNft.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WavesNftController : ControllerBase
    {
        private readonly ILogger<WavesNftController> logger;
        private readonly WavesSettings wavesSettings;

        private readonly PrivateKeyAccount account;
        private readonly Node node;

        public WavesNftController(ILogger<WavesNftController> logger, IOptions<WavesSettings> wavesOptions, Node node)
        {
            this.logger = logger;
            this.node = node;
            wavesSettings = wavesOptions.Value;
            var netChainId = wavesSettings.GetNetChainId();
            //node = new Node(netChainId);
            if (!string.IsNullOrEmpty(wavesSettings.Seed)) account = PrivateKeyAccount.CreateFromSeed(wavesSettings.Seed, netChainId);
            if (!string.IsNullOrEmpty(wavesSettings.PrivateKey)) account = PrivateKeyAccount.CreateFromPrivateKey(wavesSettings.PrivateKey, netChainId);
        }

        //[HttpGet("WeatherForecast2", Name = nameof(GetWeatherForecast2))]
        //public IEnumerable<WeatherForecast> GetWeatherForecast2()
        //{
        //    return Enumerable.Range(1, 5).Select(index => new WeatherForecast
        //    {
        //        Date = DateTime.Now.AddDays(index),
        //        TemperatureC = Random.Shared.Next(-20, 55),
        //        Summary = Summaries[Random.Shared.Next(Summaries.Length)]
        //    })
        //    .ToArray();
        //}

        [HttpGet("balance", Name = nameof(GetBalance))]
        public ActionResult<decimal> GetBalance()
        {
            var accountBalance = node.GetBalance(account.Address);
            return Ok(accountBalance);
        }

        [HttpPost("mint", Name = nameof(Mint))]
        public async Task<ActionResult<WavesNftMintResponse>> Mint(WavesNftMintRequest wavesNftMintRequest)
        {
            var name = $"DeedCoin {wavesNftMintRequest.series}#{wavesNftMintRequest.number}";
            var deedCoinDescription = new DeedCoinDescription
            {
                id = wavesNftMintRequest.id,
                type = "unique",
                url = wavesNftMintRequest.seria.certificate,
                series = wavesNftMintRequest.series,
                number = wavesNftMintRequest.number,
                token = wavesNftMintRequest.token
            };
            var description = JsonConvert.SerializeObject(deedCoinDescription);
            try
            {
                //Asset asset = node.IssueAsset(account: account, name: name, description: description, quantity: 1, decimals: 0, reissuable: false, fee: 0.001m);
                Asset asset;
                var key = deedCoinDescription.token;
                var entries = node.GetAddressDataByKey(account.Address, key);
                string assetId = "";
                if (entries.TryGetValue(key, out var assetIdObject))
                {
                    if (assetIdObject != null) assetId = assetIdObject.ToString();
                    asset = node.GetAsset(assetId);
                }
                else
                {
                    asset = node.IssueAsset(account: account, name: name, description: description, quantity: 1, decimals: 0, reissuable: false, fee: 0.001m);
                    assetId = asset.Id;
                    node.WaitTransactionConfirmation(assetId);
                    entries.Add(key, assetId);
                    node.PutData(account, entries);
                }

                var wavesNftMintResponse = new WavesNftMintResponse
                {
                    AssetId = assetId
                };

                return Ok(wavesNftMintResponse);
            }
            catch (System.Net.WebException webException)
            {
                return Problem(
                     title: "Authenticated user is not authorized.",
                     detail: webException.ToString(),
                     statusCode: StatusCodes.Status404NotFound,
                     instance: HttpContext.Request.Path);
            }
            catch (Exception exception)
            {

                return Problem(
                     title: "Authenticated user is not authorized.",
                     detail: exception.ToString(),
                     statusCode: StatusCodes.Status417ExpectationFailed,
                     instance: HttpContext.Request.Path);
            }
        }
    }
}
