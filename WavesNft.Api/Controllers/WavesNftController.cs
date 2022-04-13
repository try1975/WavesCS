using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Waves.standard;
using WavesNft.Api.Model;
using WavesNft.Api.Utils;

namespace WavesNft.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WavesNftController : ControllerBase
    {
        private readonly ILogger<WavesNftController> logger;
        private readonly Node node;
        private readonly PrivateKeyAccount account;
        private readonly IWavesNftService wavesNftService;

        public WavesNftController(ILogger<WavesNftController> logger
            , Node node
            , PrivateKeyAccount account
            , IWavesNftService wavesNftService)
        {
            this.logger = logger;
            this.node = node;
            this.account = account;
        }

        [HttpGet("balance", Name = nameof(GetBalance))]
        public ActionResult<decimal> GetBalance()
        {
            var accountBalance = node.GetBalance(account.Address);
            return Ok(accountBalance);
        }

        [HttpPost("mint", Name = nameof(DeedcoinMint))]
        [ProducesResponseType(typeof(DeedcoinMintResponse), StatusCodes.Status200OK)]
        public async Task<ActionResult> DeedcoinMint(DeedcoinMintRequest deedcoinMintRequest)
        {
            var name = $"DeedCoin {deedcoinMintRequest.series}#{deedcoinMintRequest.number}";
            var deedcoinDescription = new DeedcoinDescription
            {
                id = deedcoinMintRequest.id,
                type = "unique",
                url = deedcoinMintRequest.certificate_url,
                series = deedcoinMintRequest.series,
                number = deedcoinMintRequest.number,
                token = deedcoinMintRequest.token
            };
            var description = JsonConvert.SerializeObject(deedcoinDescription);
            try
            {
                var key = deedcoinDescription.token;
                Asset asset;
                string assetId = wavesNftService.GetAssetId(account.Address, key);
                if (!string.IsNullOrEmpty(assetId))
                {
                    asset = node.GetAsset(assetId);
                }
                else
                {
                    asset = node.IssueAsset(account: account, name: name, description: description, quantity: 1, decimals: 0, reissuable: false, fee: 0.001m);
                    assetId = asset.Id;
                    //node.WaitTransactionConfirmation(assetId);
                    node.PutData(account, new Dictionary<string, object>() { { key, assetId } });
                }

                var deedcoinMintResponse = new DeedcoinMintResponse
                {
                    AssetId = assetId
                };

                return Ok(deedcoinMintResponse);
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

        [HttpPost("trasfer", Name = nameof(DeedcoinTransfer))]
        [ProducesResponseType(typeof(DeedcoinTransferResponse), StatusCodes.Status200OK)]
        public async Task<ActionResult> DeedcoinTransfer(DeedcoinTransferRequest deedcoinTrasferRequest)
        {
            return Ok();
        }
    }
}
