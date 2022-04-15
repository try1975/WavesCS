using WavesNft.Api.Utils;

namespace WavesNft.Api.Model;

public class DeedcoinTransferResponse
{
    public string AssetId { get; internal set; }
    public DeedcoinAsset DeedcoinAsset { get; internal set; }
    public string token { get; internal set; }

    public static DeedcoinTransferResponse Build(DeedcoinAsset? deedcoinAsset)
    {
        if (deedcoinAsset == null) return new DeedcoinTransferResponse();
        var deedcoinTransferResponse = new DeedcoinTransferResponse
        {
            AssetId = deedcoinAsset.Id,
            DeedcoinAsset = deedcoinAsset,
            token = deedcoinAsset.DeedcoinDescription.token
        };
        return deedcoinTransferResponse;
    }
}
