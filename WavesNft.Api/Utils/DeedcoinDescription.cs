namespace WavesNft.Api.Utils;
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
/// <summary>
/// Serialize to asset description
/// </summary>
public class DeedcoinDescription
{
    public int id { get; set; }
    public string type { get; set; } // "unique"
    public string url { get; set; }
    public int series { get; set; }
    public int number { get; set; }
    public string token { get; set; }
}
