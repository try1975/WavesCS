using WavesNft.Api.Utils;

namespace WavesNft.Api.Model;
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

public class DeedcoinMintResponse
{
    public string AssetId { get; set; }
    public string token { get; set; }
    public DeedcoinAsset DeedcoinAsset { get; set; }

    public static DeedcoinMintResponse Build(DeedcoinAsset? deedcoinAsset)
    {
        if (deedcoinAsset == null) return new DeedcoinMintResponse();
        var deedcoinMintResponse = new DeedcoinMintResponse
        {
            AssetId = deedcoinAsset.Id,
            DeedcoinAsset = deedcoinAsset,
            token = deedcoinAsset.DeedcoinDescription.token
        };
        return deedcoinMintResponse;
    }
}
