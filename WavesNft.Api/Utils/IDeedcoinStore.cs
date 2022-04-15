namespace WavesNft.Api.Utils;
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
public interface IDeedcoinStore
{
    bool IssuedDeedcoinsContainsKey(string token);
    DeedcoinAsset IssuedDeedcoinsValue(string token);
    bool IssuedDeedcoinsTryAdd(string token, DeedcoinAsset deedcoinAsset);

    bool AccountDeedcoinsContainsKey(string token);
    DeedcoinAsset AccountDeedcoinsValue(string token);
    bool AccountDeedcoinsTryAdd(string token, DeedcoinAsset deedcoinAsset);
    bool AccountDeedcoinsTryRemove(string token, out DeedcoinAsset deedcoinAsset);
}
