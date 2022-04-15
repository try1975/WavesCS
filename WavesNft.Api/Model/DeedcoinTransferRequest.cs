namespace WavesNft.Api.Model;
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

public class DeedcoinTransferRequest
{
    /// <summary>
    /// waves account for recieve deedcoin NFT
    /// </summary>
    public string recipient { get; set; }

    public int id { get; set; }
    public int series { get; set; }
    public int number { get; set; }
    public string token { get; set; }
}
