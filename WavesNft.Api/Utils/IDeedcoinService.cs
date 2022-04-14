using WavesNft.Api.Model;

namespace WavesNft.Api.Utils
{
    public interface IDeedcoinService
    {
        bool DeedcoinIssued(string token);
        bool DeedcoinNotTrasfered(string token);
        DeedcoinAsset? GetDeedcoinByToken(string token);
        DeedcoinAsset MintDeedcoin(DeedcoinDescription deedcoinDescription);
        DeedcoinAsset? TransferDeedcoin(string recipient, DeedcoinDescription deedcoinDescription);
    }
}
