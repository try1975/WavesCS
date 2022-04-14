using Microsoft.AspNetCore.Mvc;
using Waves.standard;
using WavesNft.Api.Model;
using WavesNft.Api.Utils;

namespace WavesNft.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DeedcoinController : ControllerBase
    {
        private readonly ILogger<DeedcoinController> logger;
        private readonly Node node;
        private readonly PrivateKeyAccount account;
        private readonly IWavesNftService _wavesNftService;
        private readonly IWavesApiService _wavesApiService;

        public DeedcoinController(ILogger<DeedcoinController> logger
            , Node node
            , PrivateKeyAccount account
            , IWavesNftService wavesNftService
            , IWavesApiService wavesApiService)
        {
            this.logger = logger;
            this.node = node;
            this.account = account;
            _wavesNftService = wavesNftService;
            _wavesApiService = wavesApiService;
        }

        [HttpGet("balance", Name = nameof(GetBalance))]
        public ActionResult<decimal> GetBalance()
        {
            var accountBalance = node.GetBalance(account.Address);
            return Ok(accountBalance);
        }

        /// <summary>
        /// Create deadcoin NFT
        /// </summary>
        /// <param name="deedcoinMintRequest"></param>
        /// <returns></returns>
        [HttpPost("mint", Name = nameof(DeedcoinMint))]
        [ProducesResponseType(typeof(DeedcoinMintResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status417ExpectationFailed)]
        public ActionResult DeedcoinMint(DeedcoinMintRequest deedcoinMintRequest)
        {
            try
            {
                var deedcoinDescription = DeedcoinDescriptionBuilder.Build(deedcoinMintRequest);
                var deedcoinAsset = _wavesApiService.MintDeedcoin(deedcoinDescription);

                var deedcoinMintResponse = new DeedcoinMintResponse
                {
                    AssetId = deedcoinAsset.Id,
                    DeedcoinAsset = deedcoinAsset,
                    token = deedcoinDescription.token

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

        [HttpPost("transfer", Name = nameof(DeedcoinTransfer))]
        [ProducesResponseType(typeof(DeedcoinTransferResponse), StatusCodes.Status200OK)]
        public ActionResult DeedcoinTransfer(DeedcoinTransferRequest deedcoinTrasferRequest)
        {
            return Ok();
        }
    }
}
