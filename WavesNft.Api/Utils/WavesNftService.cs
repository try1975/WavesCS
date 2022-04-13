using Waves.standard;

namespace WavesNft.Api.Utils
{
    public class WavesNftService : IWavesNftService
    {
        private readonly Node node;
        private readonly PrivateKeyAccount account;
        private readonly IWavesApiService wavesApiService;

        public WavesNftService(Node node, PrivateKeyAccount account, IWavesApiService wavesApiService)
        {
            this.node = node;
            this.account = account;
            this.wavesApiService = wavesApiService;
        }
        public bool IsExists(string address, string key)
        {
            return !string.IsNullOrEmpty(GetAssetId(address, key));
        }

        public string GetAssetId(string address, string key)
        {
            return GetAssetIdFromData(address, key);
        }

        private string GetAssetIdFromData(string address, string key)
        {
            var entries = node.GetAddressDataByKey(address, key);
            if (entries.TryGetValue(key, out var assetIdObject) && (assetIdObject != null)) return $"{assetIdObject}";
            return string.Empty;
        }

    }
}
