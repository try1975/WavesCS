namespace WavesNft.Api.Utils;
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
public interface IDeedcoinService
{
    (DeedcoinAsset?, string message) MintDeedcoin(DeedcoinDescription deedcoinDescription);
    (DeedcoinAsset?, string message) TransferDeedcoin(string recipient, DeedcoinDescription deedcoinDescription);
}
