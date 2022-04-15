namespace WavesNft.Api.Utils
{
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
}
