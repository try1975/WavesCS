using Microsoft.AspNetCore.Mvc;
using WavesNft.Api.Model;
using WavesNft.Api.Utils;

namespace WavesNft.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DeedcoinController : ControllerBase
    {
        private readonly ILogger<DeedcoinController> _logger;
        private readonly IDeedcoinService _deedcoinService;

        public DeedcoinController(ILogger<DeedcoinController> logger, IDeedcoinService deedcoinService)
        {
            _logger = logger;
            _deedcoinService = deedcoinService;
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
                var deedcoinAsset = _deedcoinService.MintDeedcoin(deedcoinDescription);

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
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status417ExpectationFailed)]
        public ActionResult DeedcoinTransfer(DeedcoinTransferRequest deedcoinTrasferRequest)
        {
            if (!_deedcoinService.DeedcoinIssued(deedcoinTrasferRequest.token))
            {
                return Problem(
                     title: "DeedCoin not minted",
                     detail: "details",
                     statusCode: StatusCodes.Status417ExpectationFailed,
                     instance: HttpContext.Request.Path);
            }
            var deedcoinAsset = _deedcoinService.TransferDeedcoin(deedcoinTrasferRequest.recipient, null);
            return Ok();
        }
    }
}
