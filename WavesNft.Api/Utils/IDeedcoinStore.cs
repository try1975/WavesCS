using System.Collections.Concurrent;

namespace WavesNft.Api.Utils
{
    public interface IDeedcoinStore
    {
        ConcurrentDictionary<string, DeedcoinAsset> IssuedDeedcoins { get; }
        ConcurrentDictionary<string, DeedcoinAsset> AccountDeedcoins { get; }

        void RefreshIssuedDeedcoins();
        void RefreshAccountDeedcoins();
    }
}
