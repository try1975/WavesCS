using Waves.standard;

namespace WavesNft.Api.Options
{
    public class WavesSettings
    {
        public string Chain { get; set; }
        public string Seed { get; set; }
        public string PrivateKey { get; set; }


        public char GetNetChainId()
        {
            if (string.IsNullOrEmpty(Chain) || Chain.StartsWith("Test")) return Node.TestNetChainId;
            return Node.MainNetChainId;
        }
    }
}
