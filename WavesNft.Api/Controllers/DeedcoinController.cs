﻿using Microsoft.AspNetCore.Mvc;
using WavesNft.Api.Model;
using WavesNft.Api.Utils;

namespace WavesNft.Api.Controllers;
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

[ApiController]
[Route("waves/[controller]")]
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
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status417ExpectationFailed)]
    public ActionResult DeedcoinMint(DeedcoinMintRequest deedcoinMintRequest)
    {
        try
        {
            var deedcoinDescription = DeedcoinDescriptionBuilder.Build(deedcoinMintRequest);
            (var deedcoinAsset, var message) = _deedcoinService.MintDeedcoin(deedcoinDescription);
            if (!string.IsNullOrEmpty(message) || deedcoinAsset == null)
            {
                return Problem(
                     title: message,
                     detail: "details",
                     statusCode: StatusCodes.Status409Conflict,
                     instance: HttpContext.Request.Path);
            }
            return Ok(DeedcoinMintResponse.Build(deedcoinAsset));
        }
        catch (System.Net.WebException webException)
        {
            return Problem(
                title: "Web problem",
                detail: webException.ToString(),
                statusCode: StatusCodes.Status404NotFound,
                instance: HttpContext.Request.Path);
        }
        catch (Exception exception)
        {

            return Problem(
                 title: "Unexpected problem",
                 detail: exception.ToString(),
                 statusCode: StatusCodes.Status417ExpectationFailed,
                 instance: HttpContext.Request.Path);
        }
    }

    /// <summary>
    /// Transfer deedcoin NFT to another waves account
    /// </summary>
    /// <param name="deedcoinTrasferRequest"></param>
    /// <returns></returns>
    [HttpPost("transfer", Name = nameof(DeedcoinTransfer))]
    [ProducesResponseType(typeof(DeedcoinTransferResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status417ExpectationFailed)]
    public ActionResult DeedcoinTransfer(DeedcoinTransferRequest deedcoinTrasferRequest)
    {
        try
        {
            var deedcoinDescription = DeedcoinDescriptionBuilder.Build(deedcoinTrasferRequest);
            (var deedcoinAsset, var message) = _deedcoinService.TransferDeedcoin(deedcoinTrasferRequest.recipient, deedcoinDescription);
            if (!string.IsNullOrEmpty(message) || deedcoinAsset == null)
            {
                return Problem(
                     title: message,
                     detail: "details",
                     statusCode: StatusCodes.Status409Conflict,
                     instance: HttpContext.Request.Path);
            }
            return Ok(DeedcoinTransferResponse.Build(deedcoinAsset));
        }
        catch (Exception exception)
        {

            return Problem(
                 title: "Unexpected problem",
                 detail: exception.ToString(),
                 statusCode: StatusCodes.Status417ExpectationFailed,
                 instance: HttpContext.Request.Path);
        }
    }
}
