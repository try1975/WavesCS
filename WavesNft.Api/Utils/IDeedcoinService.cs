namespace WavesNft.Api.Utils;
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
public interface IDeedcoinService
{
    bool DeedcoinIssued(string token);
    bool DeedcoinNotTrasfered(string token);
    DeedcoinAsset? GetDeedcoinByToken(string token);
    DeedcoinAsset MintDeedcoin(DeedcoinDescription deedcoinDescription);
    DeedcoinAsset? TransferDeedcoin(string recipient, DeedcoinDescription deedcoinDescription);
}
