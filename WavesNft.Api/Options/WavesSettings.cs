using Waves.standard;

namespace WavesNft.Api.Options;
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

public class WavesSettings
{
    public string Chain { get; set; }
    public string Seed { get; set; }
    public string PrivateKey { get; set; }

    public char NetChainId => GetNetChainId();

    private char GetNetChainId()
    {
        if (string.IsNullOrEmpty(Chain)) return Node.TestNetChainId;
        if (!Chain.Equals("Main")) return Node.TestNetChainId;
        return Node.MainNetChainId;
    }
}
