namespace WavesNft.Api.Utils
{
    public interface IWavesNftService
    {
        bool IsExists(string address, string key);
        string GetAssetId(string address, string key);
    }
}
